using System;
using System.Linq;
using System.Collections.Generic;

public class Graph<VertexT,EdgeT> where EdgeT : IEdge<VertexT>
{
    public HashSet<VertexT> Vertices { get; private set; }
    public HashSet<EdgeT> Edges { get; private set; }
    public int VertexCount { get { return Vertices.Count; } }
    public int EdgeCount { get { return Edges.Count; } }

    public Dictionary<VertexT, HashSet<VertexT>> Neighbours { get; private set; }
    public bool IsUndirected { get; private set; }

    public Func<IEdge<VertexT>, double> CostFunction;
    public Func<IVertex, double> HeuristicFunction;

    public Graph(bool isUndirected)
    {
        Vertices = new HashSet<VertexT>();
        Edges = new HashSet<EdgeT>();
        Neighbours = new Dictionary<VertexT, HashSet<VertexT>>();
        IsUndirected = isUndirected;
    }

    public void AddVertex(VertexT vertex)
    {
        Vertices.Add(vertex);
        Neighbours.Add(vertex, new HashSet<VertexT>());
    }

    public void RemoveVertex(VertexT vertex)
    {
        Vertices.Remove(vertex);
        Neighbours.Remove(vertex);
        Edges.RemoveWhere((edge) => edge.Source.Equals(vertex) || edge.Target.Equals(vertex));
    }

    public void AddEdge(EdgeT edge) {
        if (!Vertices.Contains(edge.Source)) { this.AddVertex(edge.Source); }
        if (!Vertices.Contains(edge.Target)) { this.AddVertex(edge.Target); }
        Edges.Add(edge);
        Neighbours[edge.Source].Add(edge.Target);
        if (IsUndirected) {
            Neighbours[edge.Target].Add(edge.Source);
        }
    }

    public void RemoveEdge(EdgeT edge) {
        Edges.Remove(edge);
        Neighbours[edge.Source].Remove(edge.Target);
        if (IsUndirected) {
            Neighbours[edge.Target].Remove(edge.Source);
        }
    }

    public IVertex[] DijkstraShortestPath(VertexT source, VertexT target) {
        HashSet<VertexT> visited = new HashSet<VertexT>();
        Queue<VertexT> toVisit = new Queue<VertexT> ();
        toVisit.Enqueue(target);

        while (toVisit.Count > 0) {
            VertexT nextNode = toVisit.Dequeue();

            foreach (var neigh in Neighbours[nextNode]) {
                
            }
        }
        throw new NotImplementedException();
    }

    public List<List<VertexT>> AllPathsUndirected(VertexT source, VertexT target) {
        var allPaths = new List<List<VertexT>>();    
        var visitedStack = new Stack<VertexT>();

        FindAllPaths(source,target,visitedStack,allPaths);
     
        return allPaths;
    }

    private void FindAllPaths(VertexT current, VertexT target, Stack<VertexT> currentPath, List<List<VertexT>> allPaths) {
        if (current.Equals(target)) {
            var path = currentPath.ToList();
            path.Reverse();
            allPaths.Add(path);
        }
        else
        {
            var toVisit = new List<VertexT>();
            foreach (var neigh in Neighbours[current])
            {
                if (!currentPath.Contains(neigh)) { toVisit.Add(neigh); }
            }
            foreach(var vertex in toVisit)
            {
                currentPath.Push(vertex);
                FindAllPaths(vertex, target, currentPath, allPaths);
                currentPath.Pop();
            }
        }
    }

    public IVertex[] DijkstraAllPaths(IVertex source, IVertex target)
    {
        throw new NotImplementedException();
    }

    public IVertex[] AStarShortestPath() {
        throw new NotImplementedException();
    }
}

public interface IVertex {}

public interface IEdge<VertexT>{
    public VertexT Source { get; }
    public VertexT Target { get; }
}