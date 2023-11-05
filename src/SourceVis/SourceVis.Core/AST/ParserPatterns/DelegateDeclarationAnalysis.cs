using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceVisCore.AST.Dependencies;

namespace SourceVisCore.AST.ParserPatterns;

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