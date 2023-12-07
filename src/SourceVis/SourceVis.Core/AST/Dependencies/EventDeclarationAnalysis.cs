using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceVisCore.AST.Analysis;

namespace SourceVisCore.AST.Dependencies;

public class EventDeclarationAnalysis : DependencyStrategy<EventDeclarationSyntax>
{
  protected override IEnumerable<AnalysisResult> Analyze(EventDeclarationSyntax eventDeclaration, SemanticModel model)
  {
    ISymbol? eventType = model.GetDeclaredSymbol(eventDeclaration);

    return new[]
    {
      new AnalysisResult(true, DependencyType.EventDeclaration, eventType?.ToDisplayString() ?? "BROKEN <DeclaresEvent>")
    };
  }
}