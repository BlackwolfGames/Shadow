using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceVisCore.AST.Analysis;

namespace SourceVisCore.AST.Dependencies;

public class NestingAnalysis : DependencyStrategy<ClassDeclarationSyntax>
{
  protected override IEnumerable<AnalysisResult> Analyze(ClassDeclarationSyntax classSyntax, SemanticModel model)
  {
    if (classSyntax.Parent is not ClassDeclarationSyntax parentClass) yield break;

    ISymbol? parentType = model.GetDeclaredSymbol(parentClass);
    ISymbol? nestedType = model.GetDeclaredSymbol(classSyntax);

    if (parentType == null || nestedType == null || SymbolEqualityComparer.Default.Equals(parentType, nestedType)) yield break;

    yield return new AnalysisResult(true, DependencyType.Nesting, nestedType.ToDisplayString());
  }
}