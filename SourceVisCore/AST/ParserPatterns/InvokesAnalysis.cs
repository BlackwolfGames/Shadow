using System.Data;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SourceVisCore.AST.Dependencies;

public class InvokesAnalysis : DependencyStrategy<InvocationExpressionSyntax>
{
    protected override IEnumerable<AnalysisResult> Analyze(InvocationExpressionSyntax invocation, SemanticModel model)
    {
        var symbolInfo = model.GetSymbolInfo(invocation);

        if (symbolInfo.Symbol is not IMethodSymbol methodSymbol)
        {
            // Handle the special case or error
            yield break;
        }

        var fullyQualifiedClassName = methodSymbol.ContainingType.ToDisplayString();
    
        // Check if the method belongs to a delegate type
        if (methodSymbol.ContainingType.TypeKind == TypeKind.Delegate)
        {
            yield return new AnalysisResult(true, DependencyType.DelegateInvocation, fullyQualifiedClassName);
        }

        // Analyzing the generic type arguments if the method is generic
        if (methodSymbol.IsGenericMethod)
        {
            foreach (var typeArgument in methodSymbol.TypeArguments)
            {
                yield return new AnalysisResult(true, DependencyType.GenericMethod,
                    typeArgument.ToDisplayString());
            }
        }

        // Continue with other analysis steps, such as detecting whether it's a static or instance invocation.
        var type = methodSymbol.IsStatic ? DependencyType.StaticInvocation : DependencyType.InstanceInvocation;
        yield return new AnalysisResult(true, type, fullyQualifiedClassName);
    }
}