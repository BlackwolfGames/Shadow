using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using SourceVisCore.AST.Dependencies;

namespace SourceVisCore.AST.ParserPatterns;

public class DependencyAnalysisVisitor : CSharpSyntaxWalker
{
    private readonly SemanticModel _model;
    private readonly IEnumerable<IAnalysisStrategy> _strategies;
    private readonly Class _results;

    public DependencyAnalysisVisitor(SemanticModel model, IEnumerable<IAnalysisStrategy> strategies, Class targetClass)
    {
        _model = model;
        _strategies = strategies;
        _results = targetClass;
    }

    public override void Visit(SyntaxNode? node)
    {
        if (node != null)
        {
            var validStrategies = _strategies
                .Where(strategy => strategy.ShouldAnalyze(node));
            var analysisStrategies = validStrategies as IAnalysisStrategy[] ?? validStrategies.ToArray();
            if (!analysisStrategies.Any())
                Console.WriteLine($"No handler for {node.GetType().Name}, please add one or explicitly ignore it");
            _results.UpdateWith(analysisStrategies
                .SelectMany(strategy => strategy.Analyze(node, _model)));
        }


        base.Visit(node);
    }
}