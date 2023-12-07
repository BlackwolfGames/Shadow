using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceVisCore.AST.Analysis;

namespace SourceVisCore.AST.Dependencies;

public class BinaryExpressionAnalysis : DependencyStrategy<BinaryExpressionSyntax>
{
  protected override IEnumerable<AnalysisResult> Analyze(BinaryExpressionSyntax invocation, SemanticModel model)
  {
    SyntaxKind kind = invocation.Kind();
    switch (kind)
    {
      case SyntaxKind.AsExpression:
        SymbolInfo symbol = ModelExtensions.GetSymbolInfo(model, invocation.Right);

        return new[]
        {
          new AnalysisResult(true, DependencyType.SafeCast, symbol.Symbol?.ToDisplayString() ?? "BROKEN <SafeCast>")
        };
      default:
        Console.WriteLine("Unhandled binary expression: " + kind);
        return Enumerable.Empty<AnalysisResult>();
    }
  }
}