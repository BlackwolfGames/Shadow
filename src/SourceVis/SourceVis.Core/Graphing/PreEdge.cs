﻿using SourceVisCore.AST;

namespace SourceVisCore.Graphing;

public class PreEdge : IEdge
{
    public PreEdge(string lhs, KeyValuePair<string, Dependency> pairValue)
    {
        foreach (var dependencyType in Enum.GetValues<DependencyType>())
        {
            _weights.Add(dependencyType, pairValue.Value[dependencyType]);
        }

        LhsName = lhs;
        RhsName = pairValue.Key;
    }

    public INode from => throw new NotSupportedException("resolve edges first");
    public INode to => throw new NotSupportedException("resolve edges first");

    public string LhsName { get; }
    public string RhsName { get; }


    public int this[DependencyType type] => _weights[type];
    private readonly Dictionary<DependencyType, int> _weights = new();
}