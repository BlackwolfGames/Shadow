using System.Data;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SourceVisCore.AST.Dependencies;

public class InvokesAnalysis : SubStrategy<InvocationExpressionSyntax>
{
    protected override AnalysisResult[] Analyze(InvocationExpressionSyntax invocation, SemanticModel model)
    {
        var symbolInfo = model.GetSymbolInfo(invocation);
        string? fullyQualifiedClassName;
        var type = symbolInfo.Symbol?.IsStatic ?? false
            ? DependencyType.StaticInvocation
            : DependencyType.InstanceInvocation;
        if (symbolInfo.Symbol == null)
        {
            type = DependencyType.Special;
            symbolInfo = model.GetSymbolInfo(invocation.ArgumentList.Arguments.First().Expression);
            fullyQualifiedClassName = symbolInfo.Symbol?.ToDisplayString();
        }
        else
        {
            fullyQualifiedClassName = symbolInfo.Symbol?.ContainingType.ToDisplayString();
        }

        if (fullyQualifiedClassName == null)
        {
            return new[]
            {
                new AnalysisResult(false, DependencyType.Invalid,
                    $"Class name not resolved for {invocation.Expression.ToString()}, did you forget a 'using'?")
            };
        }

        return new[] {new AnalysisResult(true, type, fullyQualifiedClassName)};
    }
}