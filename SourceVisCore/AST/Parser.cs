using System.Data;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;

namespace SourceVisCore.AST;

public static class Parser
{
    public static async Task<Solution> Parse(string solutionPath)
    {
        var instances = MSBuildLocator.QueryVisualStudioInstances().ToArray();
        var instance = instances.First(studioInstance =>
            studioInstance.Version.Major == instances.Max(inst => inst.Version.Major) &&
            studioInstance.Version.Minor == instances.Max(inst => inst.Version.Minor));
        MSBuildLocator.RegisterInstance(instance);
        var workspace = MSBuildWorkspace.Create();
        var solution = await workspace.OpenSolutionAsync(solutionPath);
        var parsedSolution = new Solution();
        foreach (var project in solution.Projects)
        {
            var parsedProject = parsedSolution.AddProject(project.Name);
            foreach (var document in project.Documents)
            {
                var tree = await document.GetSyntaxTreeAsync();
                if (tree == null) continue;

                var root = tree.GetCompilationUnitRoot();

                var model = document.GetSemanticModelAsync().Result;
                ParseFile(root, model, parsedProject);
            }
        }

        return parsedSolution;
    }

    public static async Task<Project> ParseFromSource(string sourceCode)
    {
        var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
        var references = loadedAssemblies
            .Where(assembly => !assembly.IsDynamic) // Filter out dynamic assemblies
            .Select(assembly => MetadataReference.CreateFromFile(assembly.Location))
            .ToList();

        var compilation = CSharpCompilation.Create("MyCompilation")
            .AddReferences(references)
            .AddSyntaxTrees(CSharpSyntaxTree.ParseText(sourceCode));

        var semanticModel = compilation.GetSemanticModel(compilation.SyntaxTrees.First());
        var returned = new Project();
        ParseFile(compilation.SyntaxTrees.First().GetCompilationUnitRoot(), semanticModel, returned);
        return returned;
    }

    private static void ParseFile(CompilationUnitSyntax root, SemanticModel? model, Project parsedProject)
    {
        foreach (var classDeclaration in root.DescendantNodes().OfType<ClassDeclarationSyntax>())
        {
            var symbol = model.GetDeclaredSymbol(classDeclaration);
            var fullyQualifiedClassName = symbol?.ToDisplayString();
            var parsedClass = parsedProject.AddClass(fullyQualifiedClassName ?? classDeclaration.Identifier.ValueText);

            // Identify method calls
            ParseMethod(classDeclaration, model, parsedClass);

            // Identify object creations
            ParseNew(classDeclaration, model, parsedClass);

            // Identify base classes and interfaces
            ParseInheritance(classDeclaration, model, parsedClass);
        }
    }

    private static void ParseInheritance(ClassDeclarationSyntax classDeclaration, SemanticModel? model,
        Class parsedClass)
    {
        if (classDeclaration.BaseList == null) return;

        foreach (var fullyQualifiedClassName in
                 classDeclaration
                     .BaseList
                     .Types
                     .Select(baseTypeSyntax => model.GetTypeInfo(baseTypeSyntax.Type).Type)
                )
        {
            var type = fullyQualifiedClassName?.IsAbstract ?? false
                ? DependencyType.Extension
                : DependencyType.Implementation;
            parsedClass.AddDependency(type, GetDisplayString(fullyQualifiedClassName) ?? "BROKEN <inheritance>");
        }
    }

    private static string? GetDisplayString(ISymbol? baseSymbol) => baseSymbol?.ToDisplayString();

    private static string? GetContainedDisplayString(SymbolInfo symbolInfo) =>
        GetDisplayString(symbolInfo.Symbol?.ContainingType);

    private static void ParseNew(ClassDeclarationSyntax classDeclaration, SemanticModel? model, Class parsedClass)
    {
        foreach (var fullyQualifiedClassName in classDeclaration
                     .DescendantNodes()
                     .OfType<ObjectCreationExpressionSyntax>()
                     .Select(objectCreation => model.GetSymbolInfo(objectCreation))
                     .Select(GetContainedDisplayString)
                )
        {
            parsedClass.AddDependency(DependencyType.DirectInstantiation,
                fullyQualifiedClassName ?? "BROKEN <allocation>");
        }
    }


    private static void ParseMethod(ClassDeclarationSyntax classDeclaration, SemanticModel? model, Class parsedClass)
    {
        foreach (var invocation in classDeclaration.DescendantNodes().OfType<InvocationExpressionSyntax>())
        {
            var symbolInfo = model.GetSymbolInfo(invocation);
            string? fullyQualifiedClassName;
            var type = symbolInfo.Symbol?.IsStatic ?? false
                ? DependencyType.StaticInvocation
                : DependencyType.InstanceInvocation;
            if (symbolInfo.Symbol == null)
            {
                type = DependencyType.Special;
                symbolInfo = model.GetSymbolInfo(invocation.ArgumentList.Arguments.First().Expression);
                fullyQualifiedClassName = symbolInfo.Symbol?.ToDisplayString();
            }
            else
            {
                fullyQualifiedClassName = GetContainedDisplayString(symbolInfo);
            }

            if (fullyQualifiedClassName == null)
            {
                throw new SyntaxErrorException(
                    $"Class name not resolved for {invocation.Expression.ToString()}, did you forget a 'using'?");
            }

            parsedClass.AddDependency(type, fullyQualifiedClassName);
        }
    }
}