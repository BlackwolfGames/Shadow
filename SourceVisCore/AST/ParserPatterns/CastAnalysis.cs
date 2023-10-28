using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceVisCore.AST.Dependencies;

namespace SourceVisCore.AST.ParserPatterns;

public class CastAnalysis : DependencyStrategy<CastExpressionSyntax>
{
    protected override IEnumerable<AnalysisResult> Analyze(CastExpressionSyntax invocation, SemanticModel model)
    {
        var symbol = model.GetSymbolInfo(invocation.Type);

        return new[]
        {
            new AnalysisResult(true, DependencyType.Typecast,
                symbol.Symbol?.ToDisplayString() ?? "BROKEN <ParameterInjection>")
        };
    }
}