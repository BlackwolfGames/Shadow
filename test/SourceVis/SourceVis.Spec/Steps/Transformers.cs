﻿using SourceVisCore.Graphing;

namespace SourceVis.Spec.Steps;

[Binding]
public class NodeTransformations
{
    private readonly ScenarioContext _scenarioContext;

    public NodeTransformations(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }
    [StepArgumentTransformation("(is|is not)")]
    public static bool IsToBool(string input) => input == "is";

    [StepArgumentTransformation("node '(.*)'")]
    public INode NameToNode(string input) => 
        _scenarioContext.Get<IDependencyGraph>()[input]
        ?? throw new KeyNotFoundException("Node not found: " + input);
    
    
    [StepArgumentTransformation("edge from '(.*)' to '(.*)'")]
    public IEdge NameToNode(string lhs, string rhs) => 
        _scenarioContext.Get<IDependencyGraph>()[lhs]?[rhs]
        ?? throw new KeyNotFoundException($"edge not found: {lhs} -> {rhs}");
}