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
            _results.UpdateWith(_strategies
                .Where(strategy => strategy.ShouldAnalyze(node))
                .SelectMany(strategy => strategy.Analyze(node, _model)));

        base.Visit(node);
    }
}