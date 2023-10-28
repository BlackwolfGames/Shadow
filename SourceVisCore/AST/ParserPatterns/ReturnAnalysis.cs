using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceVisCore.AST.Dependencies;

namespace SourceVisCore.AST.ParserPatterns;

public class ReturnAnalysis : DependencyStrategy<MethodDeclarationSyntax>
{
    protected override IEnumerable<AnalysisResult> Analyze(MethodDeclarationSyntax invocation, SemanticModel model)
    {
        if (invocation.ReturnType is PredefinedTypeSyntax)
            return Array.Empty<AnalysisResult>();

        var symbol = model.GetSymbolInfo(invocation.ReturnType);

        return new[]
        {
            new AnalysisResult(true, DependencyType.ReturnType,
                symbol.Symbol?.ToDisplayString() ?? "BROKEN <ParameterInjection>")
        };
    }
}