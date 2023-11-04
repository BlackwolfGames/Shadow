using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceVisCore.AST.Dependencies;

namespace SourceVisCore.AST.ParserPatterns;

public class FieldAnalysis : DependencyStrategy<VariableDeclarationSyntax>
{
    protected override IEnumerable<AnalysisResult> Analyze(VariableDeclarationSyntax invocation, SemanticModel model)
    {

        var symbol = model.GetSymbolInfo(invocation.Type);

        return new[]
        {
            new AnalysisResult(true, DependencyType.VariableDeclaration,
                symbol.Symbol?.ToDisplayString() ?? "BROKEN <ParameterInjection>")
        };
    }
}