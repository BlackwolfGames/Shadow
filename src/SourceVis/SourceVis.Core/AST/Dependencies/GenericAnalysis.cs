using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceVisCore.AST.Analysis;

namespace SourceVisCore.AST.Dependencies;

public class GenericsAnalysis : DependencyStrategy<GenericNameSyntax>
{
  protected override IEnumerable<AnalysisResult> Analyze(GenericNameSyntax node, SemanticModel model)
  {
    TypeInfo typeInfo = model.GetTypeInfo(node);

    var constructedType = typeInfo.Type as INamedTypeSymbol;
    if (constructedType is not { IsGenericType: true }) yield break;

    foreach (ITypeSymbol typeArgument in constructedType.TypeArguments) yield return new AnalysisResult(true, DependencyType.GenericClass, typeArgument.ToDisplayString());
  }
}