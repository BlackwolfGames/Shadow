using System.Data;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;
using SourceVisCore.AST.Dependencies;

namespace SourceVisCore.AST;

public static class Parser
{
    private static readonly IAnalysisStrategy[] _strategies = {
        new InvokesAnalysis(),
        new InstantiatesAnalysis(),
        new InheritAnalysis()
    };

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
            .AddReferences(references)
            .AddSyntaxTrees(CSharpSyntaxTree.ParseText(sourceCode));

        var semanticModel = compilation.GetSemanticModel(compilation.SyntaxTrees.First());
        var returned = new Project();
        ParseFile(compilation.SyntaxTrees.First().GetCompilationUnitRoot(), semanticModel, returned);
        return returned;
    }

    private static void ParseFile(CompilationUnitSyntax root, SemanticModel model, Project parsedProject)
    {
        foreach (var classDeclaration in root.DescendantNodes().OfType<TypeDeclarationSyntax>())
        {
            var symbol = model.GetDeclaredSymbol(classDeclaration);
            var fullyQualifiedClassName = symbol?.ToDisplayString();
            var parsedClass = parsedProject.AddClass(fullyQualifiedClassName ?? classDeclaration.Identifier.ValueText);
            AnalyzeNode(classDeclaration, model, parsedClass);
        }
        
    }
    private static void AnalyzeNode(SyntaxNode node, SemanticModel model, Class parsedClass)
    {
        foreach (var result in node.DescendantNodes()
                     .SelectMany(childNode => _strategies.SelectMany(strategy => strategy.Analyze(childNode, model)))
                     .Where(result => result.Success))
        {
            parsedClass.AddDependency(result.Dependency, result.ClassName);
        }

        foreach (var lambda in node.DescendantNodes().OfType<LambdaExpressionSyntax>())
        {
            AnalyzeNode(lambda.Body, model, parsedClass);
        }

        foreach (var localFunction in node.DescendantNodes().OfType<LocalFunctionStatementSyntax>()
                     .Where(localFunction => localFunction.Body != null))
        {
            AnalyzeNode(localFunction.Body!, model, parsedClass);
        }
    }
}