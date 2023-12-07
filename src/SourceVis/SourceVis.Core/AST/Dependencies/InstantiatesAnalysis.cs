using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceVisCore.AST.Analysis;

namespace SourceVisCore.AST.Dependencies;

public class InstantiatesAnalysis : DependencyStrategy<ObjectCreationExpressionSyntax>
{
  protected override IEnumerable<AnalysisResult> Analyze(ObjectCreationExpressionSyntax invocation, SemanticModel model)
  {
    SymbolInfo symbol = model.GetSymbolInfo(invocation.Type);
    return new[]
    {
      new AnalysisResult(true, DependencyType.DirectInstantiation, symbol.Symbol?.ToDisplayString() ?? "BROKEN <instantiation>")
    };
  }
}