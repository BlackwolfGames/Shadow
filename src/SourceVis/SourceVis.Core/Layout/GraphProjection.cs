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

    private GraphProjection(ProjectedNode[] nodes) => Nodes = nodes;
    public ProjectedNode[] Nodes { get; }

    public IEnumerable<ProjectedEdge> Edges => Nodes
        .SelectMany(node => node.Held.AllEdges
            .Select(edge => 
                new ProjectedEdge(
                    node, 
                    Nodes.Single(node => node.Held == edge.to))));

    public ProjectedNode this[string name]
    {
        get
        {
            try
            {
                return Nodes.SingleOrDefault(node => node.Held.Name.EndsWith(name)) ??
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

    public void Print(TextWriter? stream = null)
    {
        stream ??= Console.Out;
        foreach (ProjectedNode node in Nodes)
        {
            node.Print(stream);
        }
    }
}