namespace AdventOfCode.Core.Year2021.Day12;

//public class Graph
//{
//    public HashSet<Cave> Caves { get; private set; }
//    public List<Path> Paths { get; private set; }

//    public Graph(string[] inputLines)
//    {
//        Caves = new HashSet<Cave>();
//        Paths = new List<Path>();

//        foreach (var line in inputLines)
//        {
//            var lineParts = line.Split('-');
//            var start = new Cave(lineParts[0]);
//            var end = new Cave(lineParts[1]);

//            Caves.Add(start);
//            Caves.Add(end);

//            Paths.Add(new Path(start, end));
//        }
//    }
//}

public class Graph
{
    //public List<LinkedList<string>> Edges { get; private set; }
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
        } else
        {
            Edges.Add(u, new HashSet<string>() { v});
        }


        if (Edges.ContainsKey(v))
        {
            Edges[v].Add(u);
        }
        else
        {
            Edges.Add(v, new HashSet<string>() { u });
        }

        //if (Edg.Contains(u))
        //{

        //}
    }
}
