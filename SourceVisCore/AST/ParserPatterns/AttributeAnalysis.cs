using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceVisCore.AST.Dependencies;

namespace SourceVisCore.AST.ParserPatterns;

public class AttributeAnalysis : DependencyStrategy<AttributeSyntax>
{
    protected override IEnumerable<AnalysisResult> Analyze(AttributeSyntax attributeSyntax, SemanticModel model)
    {
        var symbolInfo = model.GetSymbolInfo(attributeSyntax);
        if (symbolInfo.Symbol is not IMethodSymbol attributeConstructor) yield break;

        var attributeType = attributeConstructor.ContainingType;
        yield return new AnalysisResult(true, DependencyType.Attribute, attributeType?.ToDisplayString() ?? "Unknown Attribute");
    }
}