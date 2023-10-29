﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceVisCore.AST.Dependencies;

namespace SourceVisCore.AST.ParserPatterns;

public class GenericsAnalysis : DependencyStrategy<GenericNameSyntax>
{
    protected override IEnumerable<AnalysisResult> Analyze(GenericNameSyntax node, SemanticModel model)
    {
        var typeInfo = model.GetTypeInfo(node);

        if (typeInfo.Type is not INamedTypeSymbol {IsGenericType: true} constructedType) yield break;

        foreach (var typeArgument in constructedType.TypeArguments)
        {
            yield return new AnalysisResult(true, DependencyType.GenericClass,
                typeArgument.ToDisplayString());
        }
    }
}