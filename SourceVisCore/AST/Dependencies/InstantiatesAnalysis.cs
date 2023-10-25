using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SourceVisCore.AST.Dependencies;

public class InstantiatesAnalysis : SubStrategy<ObjectCreationExpressionSyntax>
{
    protected override AnalysisResult[] Analyze(ObjectCreationExpressionSyntax invocation, SemanticModel model)
    {
        var symbol = model.GetSymbolInfo(invocation);
        return new[]
        {
            new AnalysisResult(true, DependencyType.DirectInstantiation,
                symbol.Symbol?.ToDisplayString() ?? "BROKEN <instantiation>")
        };
    }
}