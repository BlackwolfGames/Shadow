using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceVisCore.AST.Dependencies;

namespace SourceVisCore.AST.ParserPatterns;

public class ParameterAnalysis : DependencyStrategy<ParameterSyntax>
{
    protected override IEnumerable<AnalysisResult> Analyze(ParameterSyntax invocation, SemanticModel model)
    {
        var parameterSymbol = model.GetDeclaredSymbol(invocation);
        if (parameterSymbol is IParameterSymbol paramSymbol)
        {
            var type = paramSymbol.Type; // This will give you the type of the parameter.
            return new[]
            {
                new AnalysisResult(true, DependencyType.ParameterInjection,
                    type.ToDisplayString())
            };
        }
        else
        {
            // Handle the case where type information could not be obtained.
            return new[]
            {
                new AnalysisResult(false, DependencyType.Invalid, "Type information not available")
            };
        }
    }
}