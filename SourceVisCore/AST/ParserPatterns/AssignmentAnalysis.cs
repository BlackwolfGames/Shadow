using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceVisCore.AST.Dependencies;

namespace SourceVisCore.AST.ParserPatterns;

public class DelegateAssignmentAnalysis : DependencyStrategy<AssignmentExpressionSyntax>
{
    protected override IEnumerable<AnalysisResult> Analyze(AssignmentExpressionSyntax assignment, SemanticModel model)
    {
        if (assignment.Kind() != SyntaxKind.AddAssignmentExpression &&
            assignment.Kind() != SyntaxKind.SubtractAssignmentExpression) yield break;

        var leftSymbolInfo = model.GetSymbolInfo(assignment.Left);
        var leftType = (leftSymbolInfo.Symbol as IPropertySymbol)?.Type ??
                       (leftSymbolInfo.Symbol as IFieldSymbol)?.Type ??
                       (leftSymbolInfo.Symbol as ILocalSymbol)?.Type ??
                       (leftSymbolInfo.Symbol as IEventSymbol)?.Type;

        if (leftType?.TypeKind != TypeKind.Delegate) yield break; // Checking if it's a delegate

        // Distinguish between an event and a delegate
        var isEvent = (leftSymbolInfo.Symbol is IEventSymbol);
        DependencyType dependencyType; // Replace with whatever DependencyType you have for normal delegates
        if (isEvent)
            dependencyType = assignment.Kind() == SyntaxKind.AddAssignmentExpression
                ? DependencyType.SubscribesToEvent
                : DependencyType.UnsubscribesFromEvent;
        else
            dependencyType = assignment.Kind() == SyntaxKind.AddAssignmentExpression
                ? DependencyType.SubscribesToDelegate
                : DependencyType.UnsubscribesFromDelegate;

        yield return new AnalysisResult(true, dependencyType, leftType.ToDisplayString());
    }
}