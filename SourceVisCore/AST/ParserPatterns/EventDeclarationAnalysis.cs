using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceVisCore.AST.Dependencies;

namespace SourceVisCore.AST.ParserPatterns;

public class EventDeclarationAnalysis : DependencyStrategy<EventDeclarationSyntax>
{
    protected override IEnumerable<AnalysisResult> Analyze(EventDeclarationSyntax eventDeclaration, SemanticModel model)
    {
        var eventType = model.GetDeclaredSymbol(eventDeclaration);
        
        return new[]
        {
            new AnalysisResult(true, DependencyType.EventDeclaration,
                eventType?.ToDisplayString() ?? "BROKEN <DeclaresEvent>")
        };
    }
}
public class DelegateDeclarationAnalysis : DependencyStrategy<DelegateDeclarationSyntax>
{
    protected override IEnumerable<AnalysisResult> Analyze(DelegateDeclarationSyntax delegateDeclaration, SemanticModel model)
    {
        var delegateType = model.GetDeclaredSymbol(delegateDeclaration);
        
        return new[]
        {
            new AnalysisResult(true, DependencyType.DelegateDeclaration,
                delegateType?.ToDisplayString() ?? "BROKEN <DeclaresEvent>")
        };
    }
}