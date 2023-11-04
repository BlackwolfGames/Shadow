using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SourceVisCore.AST.Dependencies;

public class InstantiatesAnalysis : DependencyStrategy<ObjectCreationExpressionSyntax>
{
    protected override IEnumerable<AnalysisResult> Analyze(ObjectCreationExpressionSyntax invocation, SemanticModel model)
    {
        var symbol = model.GetSymbolInfo(invocation.Type);
        return new[]
        {
            new AnalysisResult(true, DependencyType.DirectInstantiation,
                symbol.Symbol?.ToDisplayString() ?? "BROKEN <instantiation>")
        };
    }
}