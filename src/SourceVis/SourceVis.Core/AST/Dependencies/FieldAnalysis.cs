using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceVisCore.AST.Analysis;

namespace SourceVisCore.AST.Dependencies;

public class FieldAnalysis : DependencyStrategy<VariableDeclarationSyntax>
{
  protected override IEnumerable<AnalysisResult> Analyze(VariableDeclarationSyntax invocation, SemanticModel model)
  {
    SymbolInfo symbol = model.GetSymbolInfo(invocation.Type);

    return new[]
    {
      new AnalysisResult(true, DependencyType.VariableDeclaration, symbol.Symbol?.ToDisplayString() ?? "BROKEN <ParameterInjection>")
    };
  }
}