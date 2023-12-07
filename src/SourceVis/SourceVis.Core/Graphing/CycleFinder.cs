namespace SourceVisCore.Graphing;

public static class CycleFinder
{
  public static List<INode[]> FindCycles(this List<INode> nodes)
  {
    var cycles = new List<INode[]>();
    var visited = new HashSet<INode>();
    var stack = new List<INode>();
    var cycleSet = new HashSet<string>(); // Used to check for existing cycles

    foreach (INode node in nodes.Where(node => !visited.Contains(node))) DFS(node, visited, stack, cycleSet, cycles);

    return cycles;
  }

  private static void DFS(
    INode node,
    HashSet<INode> visited,
    List<INode> stack,
    HashSet<string> cycleSet,
    List<INode[]> cycles)
  {
    if (stack.Contains(node))
    {
      // Extract the cycle
      var cycle = ExtractCycle(node, stack);
      var normalizedCycle = NormalizeCycle(cycle);
      var cycleKey = string.Join("-", normalizedCycle.Select(n => n.Name)); // Assume each INode has an Identifier

      // Add cycle if not already present
      if (cycleSet.Add(cycleKey)) cycles.Add(normalizedCycle);
      return;
    }

    if (visited.Contains(node))
    {
      // Node has been visited on the current path, so skip.
      return;
    }

    visited.Add(node);
    stack.Add(node);

    // Iterate over all outgoing edges.
    foreach (IEdge edge in node.AllEdges) DFS(edge.to, visited, stack, cycleSet, cycles);

    // Remove the node from the stack and visited set when backtracking.
    stack.Remove(node);
    visited.Remove(node);
  }

  private static INode[] ExtractCycle(INode node, List<INode> stack)
  {
    // Find the index of the current node in the stack, which marks the beginning of the cycle.
    var startIndex = stack.IndexOf(node);
    // Calculate the length of the cycle.
    var cycleLength = stack.Count - startIndex;
    // Allocate an array to hold the nodes forming the cycle.
    var cycle = new INode[cycleLength];
    // Copy the nodes from the stack to the cycle array.
    for (var i = 0; i < cycleLength; i++) cycle[i] = stack[startIndex + i];
    return cycle;
  }

  private static INode[] NormalizeCycle(INode[] cycle)
  {
    // Assuming that 'INode' has a 'Name' property that returns the class name as a string.
    // Find the lexicographically smallest node's name in the cycle
    var minNodeIndex = 0;
    for (var i = 1; i < cycle.Length; i++)
    {
      if (string.Compare(cycle[i].Name, cycle[minNodeIndex].Name) < 0)
        minNodeIndex = i;
    }

    // Rotate the cycle array so that it starts with the lexicographically smallest node
    var normalizedCycle = new INode[cycle.Length];
    for (var i = 0; i < cycle.Length; i++) normalizedCycle[i] = cycle[(minNodeIndex + i) % cycle.Length];
    return normalizedCycle;
  }
}