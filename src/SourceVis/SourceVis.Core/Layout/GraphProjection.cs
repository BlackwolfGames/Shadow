using System.Numerics;
using System.Reflection;
using SourceVisCore.Graphing;

namespace SourceVisCore.Layout;

public class GraphProjection
{
    public static GraphProjection FromGraph(IDependencyGraph graph)
    {
        return new GraphProjection(
            graph.AllNodes.Select(node => new ProjectedNode(node)
                {
                    Position = Vector2.Normalize(new Vector2(Random.Shared.NextSingle() - 0.5f,
                        Random.Shared.NextSingle() - 0.5f)) * 10f
                })
                .ToArray()
        );
    }

    private GraphProjection(ProjectedNode[] nodes) => _nodes = nodes;
    private readonly ProjectedNode[] _nodes;

    public ProjectedNode this[string name]
    {
        get
        {
            try
            {
                return _nodes.SingleOrDefault(node => node.Held.Name.EndsWith(name)) ??
                       throw new KeyNotFoundException(
                           $"No node found with name {name}");
            }
            catch
            {
                throw new AmbiguousMatchException(
                    $"multiple nodes found ending with {name}, consider prefixing with '.'");
            }
        }
    }
}