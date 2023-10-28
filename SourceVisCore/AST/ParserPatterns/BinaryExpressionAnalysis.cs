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
        if (kind != SyntaxKind.AsExpression) throw new EvaluateException("Unknown binary expression kind: " + kind);

        var symbol = ModelExtensions.GetSymbolInfo(model, invocation.Right);

        return new[]
        {
            new AnalysisResult(true, DependencyType.SafeCast,
                symbol.Symbol?.ToDisplayString() ?? "BROKEN <ParameterInjection>")
        };
    }
}