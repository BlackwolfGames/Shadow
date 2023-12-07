using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceVisCore.AST.Analysis;

namespace SourceVisCore.AST.Dependencies;

public class AttributeAnalysis : DependencyStrategy<AttributeSyntax>
{
  protected override IEnumerable<AnalysisResult> Analyze(AttributeSyntax attributeSyntax, SemanticModel model)
  {
    SymbolInfo symbolInfo = model.GetSymbolInfo(attributeSyntax);
    if (symbolInfo.Symbol is not IMethodSymbol attributeConstructor) yield break;

    INamedTypeSymbol? attributeType = attributeConstructor.ContainingType;
    yield return new AnalysisResult(true, DependencyType.Attribute, attributeType?.ToDisplayString() ?? "Unknown Attribute");
  }
}