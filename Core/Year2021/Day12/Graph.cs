namespace AdventOfCode.Core.Year2021.Day12;

public class Graph
{
    public Dictionary<string, HashSet<string>> Edges { get; private set; }

    public Graph()
    {
        Edges = new Dictionary<string, HashSet<string>>();
    }

    public void AddEdge(string u, string v)
    {
        if (Edges.ContainsKey(u))
        {
            Edges[u].Add(v);
        }
        else
        {
            Edges.Add(u, new HashSet<string>() { v });
        }


        if (Edges.ContainsKey(v))
        {
            Edges[v].Add(u);
        }
        else
        {
            Edges.Add(v, new HashSet<string>() { u });
        }
    }
}
