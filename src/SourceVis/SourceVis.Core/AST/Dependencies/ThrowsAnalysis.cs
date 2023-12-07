using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceVisCore.AST.Analysis;

namespace SourceVisCore.AST.Dependencies;

public class ThrowsAnalysis : DependencyStrategy<ThrowStatementSyntax>
{
  protected override IEnumerable<AnalysisResult> Analyze(ThrowStatementSyntax invocation, SemanticModel model)
  {
    if (invocation.Expression == null)
    {
      // Re-throwing the current exception; cannot determine type.
      return new[]
      {
        new AnalysisResult(true, DependencyType.ThrownException, "Hidden")
      };
    }

    TypeInfo typeInfo = model.GetTypeInfo(invocation.Expression);
    ITypeSymbol? exceptionType = typeInfo.Type;

    return new[]
    {
      new AnalysisResult(true, DependencyType.ThrownException, exceptionType?.ToDisplayString() ?? "BROKEN <ParameterInjection>")
    };
  }
}