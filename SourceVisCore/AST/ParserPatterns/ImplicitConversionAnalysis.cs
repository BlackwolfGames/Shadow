using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceVisCore.AST.Dependencies;

namespace SourceVisCore.AST.ParserPatterns;

public class ImplicitConversionAnalysis : DependencyStrategy<EqualsValueClauseSyntax>
{
    protected override IEnumerable<AnalysisResult> Analyze(EqualsValueClauseSyntax invocation, SemanticModel model)
    {
        var rhsType = model.GetTypeInfo(invocation.Value).Type; // Type of right-hand side
        var variableDeclarator = invocation.Parent as VariableDeclaratorSyntax;
        var variableDeclaration = variableDeclarator?.Parent as VariableDeclarationSyntax;

        // Ensure the parent is a VariableDeclaratorSyntax and then get the type info
        if (variableDeclaration == null)
            return Array.Empty<AnalysisResult>();

        var lhsType = model.GetTypeInfo(variableDeclaration.Type).Type;  // Type of left-hand side

        if (lhsType?.ToString() == rhsType?.ToString())
            return Array.Empty<AnalysisResult>();

        return new[]
        {
            new AnalysisResult(true, DependencyType.ImplicitConversion,
                rhsType?.ToDisplayString() ?? "BROKEN <ParameterInjection>")
        };
    }
}