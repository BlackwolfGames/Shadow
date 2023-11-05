using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceVisCore.AST.Dependencies;

namespace SourceVisCore.AST.ParserPatterns;

public class PropertyAnalysis : DependencyStrategy<PropertyDeclarationSyntax>
{
    protected override IEnumerable<AnalysisResult> Analyze(PropertyDeclarationSyntax invocation, SemanticModel model)
    {
        var symbol = model.GetSymbolInfo(invocation.Type);
        return new[]
        {
            new AnalysisResult(true, DependencyType.Property,
                symbol.Symbol?.ToDisplayString() ?? "BROKEN <Property>")
        };
    }
}