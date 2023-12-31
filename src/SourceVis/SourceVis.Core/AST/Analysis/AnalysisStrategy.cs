﻿using Microsoft.CodeAnalysis;

namespace SourceVisCore.AST.Analysis;

public abstract class DependencyStrategy<T> : IAnalysisStrategy where T : SyntaxNode
{
  public IEnumerable<AnalysisResult> Analyze(SyntaxNode node, SemanticModel model) => node is T invocation
    ? Analyze(invocation, model)
    : throw new InvalidOperationException($"{GetType().FullName}, " + $"intended for {typeof(T).FullName} " + $"can't handle {node.GetType().FullName}," + $" check with ShouldAnalyze");

  protected abstract IEnumerable<AnalysisResult> Analyze(T invocation, SemanticModel model);

  public bool ShouldAnalyze(SyntaxNode node) => node is T;
}