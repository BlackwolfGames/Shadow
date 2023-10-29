using System.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceVisCore.AST.Dependencies;

namespace SourceVisCore.AST.ParserPatterns;

    public class CatchAnalysis : DependencyStrategy<CatchClauseSyntax>
    {
        protected override IEnumerable<AnalysisResult> Analyze(CatchClauseSyntax invocation, SemanticModel model)
        {
            if (invocation.Declaration == null)
            {
                // implicit silencing exception; cannot determine type.
                return new[]
                {
                    new AnalysisResult(true, DependencyType.CaughtException, "Hidden")
                };
            }

            var typeInfo = model.GetTypeInfo(invocation.Declaration.Type);
            var exceptionType = typeInfo.Type;

            return new[]
            {
                new AnalysisResult(true, DependencyType.CaughtException,
                    exceptionType?.ToDisplayString() ?? "BROKEN <ExceptionHandling>")
            };
        }
    }