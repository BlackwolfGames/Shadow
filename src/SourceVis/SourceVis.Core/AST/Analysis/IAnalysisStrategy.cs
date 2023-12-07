using Microsoft.CodeAnalysis;

namespace SourceVisCore.AST.Analysis;

public interface IAnalysisStrategy
{
  bool ShouldAnalyze(SyntaxNode node);
  IEnumerable<AnalysisResult> Analyze(SyntaxNode node, SemanticModel model);
}