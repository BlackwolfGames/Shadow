using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SourceVisCore.AST.Dependencies;

public class InheritAnalysis : SubStrategy<ClassDeclarationSyntax>
{
    protected override AnalysisResult[] Analyze(ClassDeclarationSyntax invocation, SemanticModel model)
    {
        return invocation.BaseList == null
            ? new[] {new AnalysisResult()}
            : (from fullyQualifiedClassName in invocation.BaseList.Types
                    .Select(baseTypeSyntax => model.GetTypeInfo(baseTypeSyntax.Type).Type)
                select new AnalysisResult(Success: true,
                    Dependency: DependencyTypeOf(fullyQualifiedClassName),
                    ClassName: fullyQualifiedClassName?.ToDisplayString() ?? "BROKEN <Inheritance>"))
            .ToArray();
    }

    private static DependencyType DependencyTypeOf(ISymbol fullyQualifiedClassName) =>
        fullyQualifiedClassName.IsAbstract
            ? DependencyType.Extension
            : DependencyType.Implementation;
}