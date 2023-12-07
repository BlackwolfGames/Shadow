namespace SourceVisCore.AST.Analysis;

public sealed record AnalysisResult(bool Success = false, DependencyType Dependency = DependencyType.Invalid, string ClassName = "");