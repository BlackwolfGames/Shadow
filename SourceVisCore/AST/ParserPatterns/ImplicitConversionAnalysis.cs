using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceVisCore.AST.Dependencies;

namespace SourceVisCore.AST.ParserPatterns;

public class ImplicitConversionAnalysis : DependencyStrategy<EqualsValueClauseSyntax>
{
    protected override IEnumerable<AnalysisResult> Analyze(EqualsValueClauseSyntax invocation, SemanticModel model)
    {
        if (invocation.Value.IsKind(SyntaxKind.SimpleLambdaExpression) || invocation.Value.IsKind(SyntaxKind.ParenthesizedLambdaExpression))
        {
            // This is a lambda expression, skip processing
            return Array.Empty<AnalysisResult>();
        }
    
        var rhsType = ModelExtensions.GetTypeInfo(model, invocation.Value).Type; // Type of right-hand side
        var variableDeclarator = invocation.Parent as VariableDeclaratorSyntax;
        var variableDeclaration = variableDeclarator?.Parent as VariableDeclarationSyntax;

        // Ensure the parent is a VariableDeclaratorSyntax and then get the type info
        if (variableDeclaration == null)
            return Array.Empty<AnalysisResult>();

        var lhsType = ModelExtensions.GetTypeInfo(model, variableDeclaration.Type).Type;  // Type of left-hand side

        if (lhsType?.BaseType?.Name == "MulticastDelegate")
        {
            return new[]
            {
                new AnalysisResult(true, DependencyType.SubscribesToDelegate,
                    lhsType.ToDisplayString())
            };
        }

        if (lhsType?.ToString() == rhsType?.ToString())
            return Array.Empty<AnalysisResult>();

        return new[]
        {
            new AnalysisResult(true, DependencyType.ImplicitConversion,
                rhsType?.ToDisplayString() ?? "BROKEN <ImplicitConversion>")
        };
    }

}