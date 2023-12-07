using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceVisCore.AST.Analysis;

namespace SourceVisCore.AST.Dependencies;

public class DelegateDeclarationAnalysis : DependencyStrategy<DelegateDeclarationSyntax>
{
  protected override IEnumerable<AnalysisResult> Analyze(DelegateDeclarationSyntax delegateDeclaration, SemanticModel model)
  {
    ISymbol? delegateType = model.GetDeclaredSymbol(delegateDeclaration);

    return new[]
    {
      new AnalysisResult(true, DependencyType.DelegateDeclaration, delegateType?.ToDisplayString() ?? "BROKEN <DeclaresEvent>")
    };
  }
}