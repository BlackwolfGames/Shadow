using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceVisCore.AST.Dependencies;

namespace SourceVisCore.AST.ParserPatterns;

public class ParameterAnalysis : DependencyStrategy<ParameterSyntax>
{
    protected override IEnumerable<AnalysisResult> Analyze(ParameterSyntax invocation, SemanticModel model)
    {
        var symbol = model.GetSymbolInfo(invocation.Type);
        return new[]
        {
            new AnalysisResult(true, DependencyType.ParameterInjection,
                symbol.Symbol?.ToDisplayString() ?? "BROKEN <ParameterInjection>")
        };
    }
}