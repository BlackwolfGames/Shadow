using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceVisCore.AST.Analysis;

namespace SourceVisCore.AST.Dependencies;

public class CatchAnalysis : DependencyStrategy<CatchClauseSyntax>
{
  protected override IEnumerable<AnalysisResult> Analyze(CatchClauseSyntax invocation, SemanticModel model)
  {
    if (invocation.Declaration == null)
    {
      // implicit silencing exception; cannot determine type.
      return new[]
      {
        new AnalysisResult(true, DependencyType.CaughtException, "Hidden")
      };
    }

    TypeInfo typeInfo = model.GetTypeInfo(invocation.Declaration.Type);
    ITypeSymbol? exceptionType = typeInfo.Type;

    return new[]
    {
      new AnalysisResult(true, DependencyType.CaughtException, exceptionType?.ToDisplayString() ?? "BROKEN <ExceptionHandling>")
    };
  }
}