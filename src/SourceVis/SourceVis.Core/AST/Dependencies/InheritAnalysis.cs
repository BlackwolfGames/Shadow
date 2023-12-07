using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceVisCore.AST.Analysis;

namespace SourceVisCore.AST.Dependencies;

public class InheritAnalysis : DependencyStrategy<ClassDeclarationSyntax>
{
  protected override IEnumerable<AnalysisResult> Analyze(ClassDeclarationSyntax invocation, SemanticModel model)
  {
    return invocation.BaseList == null
      ? new[] { new AnalysisResult() }
      : (
        from fullyQualifiedClassName in invocation.BaseList.Types.Select(baseTypeSyntax => model.GetTypeInfo(baseTypeSyntax.Type).Type)
        select new AnalysisResult(true, DependencyTypeOf(fullyQualifiedClassName), fullyQualifiedClassName?.ToDisplayString() ?? "BROKEN <Inheritance>")).ToArray();
  }

  private static DependencyType DependencyTypeOf(ITypeSymbol fullyQualifiedClassName)
  {
    if (fullyQualifiedClassName is not INamedTypeSymbol namedTypeSymbol)
      return DependencyType.Invalid; // Assume you have an "Unknown" enum value or similar

    return namedTypeSymbol.TypeKind switch
    {
      TypeKind.Interface => DependencyType.Implementation,
      TypeKind.Class => DependencyType.Extension,
      _ => DependencyType.Invalid
    };
  }
}