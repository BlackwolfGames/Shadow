using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceVisCore.AST.Analysis;

namespace SourceVisCore.AST.Dependencies;

public class ReturnAnalysis : DependencyStrategy<MethodDeclarationSyntax>
{
  protected override IEnumerable<AnalysisResult> Analyze(MethodDeclarationSyntax invocation, SemanticModel model)
  {
    if (invocation.ReturnType is PredefinedTypeSyntax)
      return Array.Empty<AnalysisResult>();

    SymbolInfo symbol = model.GetSymbolInfo(invocation.ReturnType);

    return new[]
    {
      new AnalysisResult(true, DependencyType.ReturnType, symbol.Symbol?.ToDisplayString() ?? "BROKEN <ParameterInjection>")
    };
  }
}