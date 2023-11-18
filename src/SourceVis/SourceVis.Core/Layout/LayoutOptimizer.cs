using SourceVisCore.Layout.Forces;

namespace SourceVisCore.Layout;

public class LayoutOptimizer
{
    public List<IForce> Forces { get; } = new(); 
    public void Optimize(GraphProjection get)
    {
        Step++;
    }

    public int Step { get; private set; }

    public void AddForce(Forces.Forces forceType)
    {
        Forces.Add(ForceMapping.Get[forceType]);
    }

    public void RemoveForce(Forces.Forces forceType)
    {
        Forces.Remove(ForceMapping.Get[forceType]);
    }
}