using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;
using SourceVisCore.AST.Dependencies;
using SourceVisCore.AST.ParserPatterns;

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

                var model = await document.GetSemanticModelAsync();
                Debug.Assert(model != null, nameof(model) + " != null");
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
            .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
            .AddReferences(references)
            .AddSyntaxTrees(CSharpSyntaxTree.ParseText(sourceCode));

        var diagnostics = compilation.GetDiagnostics();
        if (diagnostics.Any())
            throw new ValidationException(diagnostics
                .Select(diagnostic => diagnostic.ToString())
                .Aggregate((s1, s2) => s1 + "\n-=-\n" + s2));
        
        var semanticModel = compilation.GetSemanticModel(compilation.SyntaxTrees.First());
        var returned = new Project();
        ParseFile(compilation.SyntaxTrees.First().GetCompilationUnitRoot(), semanticModel, returned);
        return returned;
    }

    private static IEnumerable<IAnalysisStrategy> GatherStrategies()
    {
        var type = typeof(IAnalysisStrategy);
        return Assembly.GetExecutingAssembly().GetTypes()
            .Where(p => type.IsAssignableFrom(p) && !p.IsInterface && !p.IsAbstract)
            .Select(Activator.CreateInstance)
            .Cast<IAnalysisStrategy>();
    }

    private static void ParseFile(SyntaxNode root, SemanticModel model, Project parsedProject)
    {
        var strategies = GatherStrategies().ToArray();
        foreach (var node in root.DescendantNodes())
        {
            if (node is not TypeDeclarationSyntax classDeclaration) continue;

            var symbol = model.GetDeclaredSymbol(classDeclaration);
            var fullyQualifiedClassName = symbol?.ToDisplayString();
            var parsedClass = parsedProject.AddClass(fullyQualifiedClassName ?? classDeclaration.Identifier.ValueText);

            var visitor = new DependencyAnalysisVisitor(model, strategies, parsedClass);
            visitor.Visit(classDeclaration);
        }
    }
}