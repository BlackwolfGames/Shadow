using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SourceVisCore.AST.Dependencies;

public record AnalysisResult(
    bool Success = false,
    DependencyType Dependency = DependencyType.Invalid,
    string ClassName = "");

public interface IAnalysisStrategy
{
    IEnumerable<AnalysisResult> Analyze(SyntaxNode node, SemanticModel model);
}

public abstract class SubStrategy<T> : IAnalysisStrategy where T : SyntaxNode
{
    public IEnumerable<AnalysisResult> Analyze(SyntaxNode node, SemanticModel model) =>
        node is T invocation ? Analyze(invocation, model) : Array.Empty<AnalysisResult>();

    protected abstract AnalysisResult[] Analyze(T invocation, SemanticModel model);
}