using System.Data;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceVisCore.AST.Dependencies;

namespace SourceVisCore.AST.ParserPatterns;

public class BinaryExpressionAnalysis : DependencyStrategy<BinaryExpressionSyntax>
{
    protected override IEnumerable<AnalysisResult> Analyze(BinaryExpressionSyntax invocation, SemanticModel model)
    {
        var kind = invocation.Kind();
        switch (kind)
        {
            case SyntaxKind.AsExpression:
                var symbol = ModelExtensions.GetSymbolInfo(model, invocation.Right);

                return new[]
                {
                    new AnalysisResult(true, DependencyType.SafeCast,
                        symbol.Symbol?.ToDisplayString() ?? "BROKEN <SafeCast>")
                };
            default:
                Console.WriteLine("Unhandled binary expression: " + kind);
                return Enumerable.Empty<AnalysisResult>();
        }
    }
}