using System;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using VisualDebugging;
using CustomAiNavigation.Zone.Data;
using Debug = UnityEngine.Debug;


public partial class NavigationZone : MonoBehaviour
{
    public Bounds WorldBounds { get; private set; }
    public float VoxelSize = 1f;

    public GameObject WorldRoot;
    public bool ShowAreas = true;
    public Graph<WalkableArea, AreaConnection> AreaTopologyGraph { get; private set; }

    public event Action InputLayersUpdated;
    public NavigationData NavData { get; private set; }

    private void Awake()
    {
        VisualDebugger.ClearAll(this);
        Debug.Assert(LayerMask.NameToLayer("Walkable") > 0, "In order to function the script needs the the \"Walkable\" layer");
        Debug.Assert(LayerMask.NameToLayer("World") > 0, "In order to function the script needs the the \"World\" layer");

        WorldBounds = GetEnvironmentBounds(WorldRoot);

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        GeneratePathfindingData();
        stopwatch.Stop();
        Debug.Log("Graph construction time: " + stopwatch.Elapsed);
    }
    public void Start()
    {
        NavigationZoneManager.Instance.SubscribeNavigationZone(this);
    }
    private Bounds GetEnvironmentBounds(GameObject root)
    {
        var bounds = new Bounds();
        var center = Vector3.zero;
        var extent = Vector3.zero;

        Vector2 xRange = new Vector2(Mathf.Infinity, -Mathf.Infinity);
        Vector2 yRange = new Vector2(Mathf.Infinity, -Mathf.Infinity);
        Vector2 zRange = new Vector2(Mathf.Infinity, -Mathf.Infinity);
        var meshRends = root.GetComponentsInChildren<MeshRenderer>();
        foreach (var mr in meshRends)
        {
            var c0 = mr.bounds.center - mr.bounds.extents;
            var c1 = mr.bounds.center + mr.bounds.extents;

            if (xRange[0] > c0[0]) { xRange[0] = c0[0]; }
            if (yRange[0] > c0[1]) { yRange[0] = c0[1]; }
            if (zRange[0] > c0[2]) { zRange[0] = c0[2]; }
            if (xRange[1] < c1[0]) { xRange[1] = c1[0]; }
            if (yRange[1] < c1[1]) { yRange[1] = c1[1]; }
            if (zRange[1] < c1[2]) { zRange[1] = c1[2]; }
        }

        center.x = (xRange[0] + xRange[1]) / 2f;
        center.y = (yRange[0] + yRange[1]) / 2f;
        center.z = (zRange[0] + zRange[1]) / 2f;

        extent.x = xRange[1] - center.x;
        extent.y = yRange[1] - center.y;
        extent.z = zRange[1] - center.z;

        bounds.center = center;
        bounds.extents = extent;
        bounds.size = extent * 2;

        return bounds;
    }

    private List<GridArea> CalculateAreas(Sample[,] sampleGrid)
    {
        List<GridArea> areas = new List<GridArea>();
        List<GridArea> nextAreas = new List<GridArea>();
        //Setup
        for (int x = 0; x < sampleGrid.GetLength(0); x++)
        {
            for (int y = 0; y < sampleGrid.GetLength(1); y++)
            {
                var node = sampleGrid[x, y];
                if (node != null)
                {
                    var area = new GridArea(new Vector2Int(x, y), Vector2Int.one);
                    areas.Add(area);
                }
            }
        }

        //Y Pass
        var groupedAreasY = GridArea.GroupAreasBySizeAlignmentY(areas);
        foreach (var bucket in groupedAreasY)
        {
            if (bucket.Count == 1)
            {
                nextAreas.Add(bucket[0]);
                continue;
            }

            GridArea lastMerged = bucket[0];
            bucket.Sort((a, b) => a.Anchor.x - b.Anchor.x);
            for (int i = 1; i < bucket.Count; i++)
            {
                var area = bucket[i];
                var merged = GridArea.MergeOrNull(lastMerged, area);
                if (merged != null)
                {
                    if (i == bucket.Count - 1)
                    {
                        nextAreas.Add(merged);
                    }
                    lastMerged = merged;
                }
                else
                {
                    nextAreas.Add(lastMerged);
                    lastMerged = area;
                }
            }
        }
        areas.Clear();
        areas.AddRange(nextAreas);
        nextAreas.Clear();

        // X Pass
        var groupedAreasX = GridArea.GroupAreasBySizeAlignmentX(areas);
        foreach (var bucket in groupedAreasX)
        {
            if (bucket.Count == 1)
            {
                nextAreas.Add(bucket[0]);
                continue;
            }

            GridArea lastMerged = bucket[0];
            bucket.Sort((a, b) => a.Anchor.y - b.Anchor.y);
            for (int i = 1; i < bucket.Count; i++)
            {
                var area = bucket[i];
                var merged = GridArea.MergeOrNull(lastMerged, area);
                if (merged != null)
                {
                    if (i == bucket.Count - 1)
                    {
                        nextAreas.Add(merged);
                    }
                    lastMerged = merged;
                }
                else
                {
                    nextAreas.Add(lastMerged);
                    lastMerged = area;
                }
            }
        }
        areas.Clear();
        areas.AddRange(nextAreas);
        nextAreas.Clear();

        return areas;
    }

    private Graph<WalkableArea, AreaConnection> GenerateAreaGraph(List<WalkableArea> walkableAreas)
    {
        var graph = new Graph<WalkableArea, AreaConnection>(isUndirected: true);

        foreach (var area in walkableAreas)
        {
            graph.AddVertex(area);
        }
        foreach (var area0 in walkableAreas)
        {
            foreach (var area1 in walkableAreas)
            {
                if (area0.Equals(area1)) { continue; }
                if (area0.GridArea.IsAdjacent(area1.GridArea))
                {
                    graph.AddEdge(new AreaConnection(area0, area1));
                }
            }
        }
        return graph;
    }
    public Dictionary<int, List<NavigationSample>> DiscretizeNavigationMesh(float delta)
    {
        var ret = new Dictionary<int, List<NavigationSample>>();
        var navMesh = NavMesh.CalculateTriangulation();
        var tris = navMesh.indices;
        var positions = navMesh.vertices;

        VisualDebugger.AddMesh(this, positions, tris).WithColor(Color.cyan);
        int lastIndex = 0;
        for (int i = 0; i < tris.Length; i += 3)
        {
            var Aind = tris[i];
            var Bind = tris[i + 1];
            var Cind = tris[i + 2];

            var Apos = positions[Aind];
            var Bpos = positions[Bind];
            var Cpos = positions[Cind];

            var v = Bpos - Apos;
            var w = Cpos - Apos;

            var trisNormal = Vector3.Cross(v, w);
            var vertices = new List<Vector3>() { Apos, Bpos, Cpos };

            var minIterX = (int)(vertices.Min((pos) => pos.x) / delta);
            var maxIterX = (int)(vertices.Max((pos) => pos.x) / delta);

            var minIterZ = (int)(vertices.Min((pos) => pos.z) / delta);
            var maxIterZ = (int)(vertices.Max((pos) => pos.z) / delta);

            var iterX = maxIterX - minIterX;
            var iterZ = maxIterZ - minIterZ;

            var sampleCount = 0;
            var area = Mathf.Abs(v.magnitude * w.magnitude * Mathf.Sin(Vector3.Angle(w, v) * Mathf.Deg2Rad) * .5f);

            var eps = 0.001f;
            var tolerance = 0.05f;

            for (int stepZ = 0; stepZ <= iterZ; stepZ++)
            {
                for (int stepX = 0; stepX <= iterX; stepX++)
                {
                    var globIterX = minIterX + stepX;
                    var globIterZ = minIterZ + stepZ;

                    //Calculate current position to evaluate. This position lies on the plane of the triangle but might not be inside it.
                    var pos = new Vector3();
                    pos.x = globIterX * delta;
                    pos.z = globIterZ * delta;
                    var k = (pos.x - Apos.x) / (v.x + eps);
                    var g = w.x / (v.x + eps);
                    var t = (pos.z - Apos.z - k * v.z) / (w.z - g * v.z + eps);
                    var s = k - g * t;

                    pos.y = Apos.y + s * v.y + t * w.y;

                    //Check if it's inside triangle
                    var e1 = Bpos - Apos;
                    var d1 = pos - Apos;
                    var area1 = Mathf.Abs(e1.magnitude * d1.magnitude * Mathf.Sin(Vector3.Angle(e1, d1) * Mathf.Deg2Rad) * .5f);
                    var e2 = Cpos - Bpos;
                    var d2 = pos - Bpos;
                    var area2 = Mathf.Abs(e2.magnitude * d2.magnitude * Mathf.Sin(Vector3.Angle(e2, d2) * Mathf.Deg2Rad) * .5f);
                    var e3 = Apos - Cpos;
                    var d3 = pos - Cpos;
                    var area3 = Mathf.Abs(e3.magnitude * d3.magnitude * Mathf.Sin(Vector3.Angle(e3, d3) * Mathf.Deg2Rad) * .5f);

                    var areaSum = area1 + area2 + area3;

                    if (Mathf.Abs(areaSum - area) < tolerance)
                    {
                        sampleCount++;
                        int hash = 19 * globIterX + 23 * globIterZ;
                        var sample = new NavigationSample();
                        sample.id = lastIndex;
                        sample.position = pos;
                        sample.normal = trisNormal;
                        sample.gridPosition.x = globIterX;
                        sample.gridPosition.z = globIterZ;
                        lastIndex++;
                        if (!ret.ContainsKey(hash))
                            ret[hash] = new List<NavigationSample>();
                        ret[hash].Add(sample);
                    }
                }
            }
        }

        foreach (var bucket in ret.Keys)
        {
            var list = ret[bucket];
            list.Sort((s0, s1) =>
            {
                if ((s0.position.y - s1.position.y) > 0f)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            });
            for (int i = 0; i < list.Count; i++)
            {
                list[i].gridPosition.y = i;
            }
        }

        return ret;
    }       

    public void GeneratePathfindingData()
    {
        var samples = DiscretizeNavigationMesh(VoxelSize);
        var flatSamples = samples.Values.SelectMany(l => l).ToList();
        NavData = new NavigationData(this,flatSamples);
        NavData.ComputeVisibilityData();
        NavData.ComputeCoverData();

        var fromSample = flatSamples[0];
        foreach (var sample in flatSamples)
        {
            var dPoint = VisualDebugger.AddPoint(this,sample.position);
            dPoint.color = Color.green;
            if (!NavData.HeadLevelVisibilityLayer[fromSample,sample]) {
                dPoint.color = Color.red;
            }
        }
    }

    public bool Contains(Vector3 point)
    {
        return WorldBounds.Contains(point);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(WorldBounds.center, WorldBounds.size);
    }

    private string visibleLayerName = "none";
    void OnDrawGizmosSelected()
    {
        VisualDebugger.Render(this);
    }
}


//if (ShowGraph && Graph != null && Graph.VertexCount > 0)
//{
//    foreach (Node n in Graph.Vertices)
//    {
//        //nodePositions[i] = n.position;
//        if (!visibleLayerName.Equals("none")) {
//            var input = CostInputLayers[visibleLayerName];
//            var range = input.MaxValue - input.MinValue;
//            Gizmos.color = Color.Lerp(Color.blue, Color.red, (float)((input.NodeValues[n] - input.MinValue) / range));
//        }
//        else
//            Gizmos.color = Color.red;
//        Gizmos.DrawWireCube(n.position + Vector3.up * .5f * VoxelSize, Vector3.one * VoxelSize / 3f);
//    }
//    foreach (Edge e in Graph.Edges)
//    {
//        if (!visibleLayerName.Equals("none"))
//        {
//            var input = CostInputLayers[visibleLayerName];
//            var range = input.MaxValue - input.MinValue;
//            Gizmos.color = Color.Lerp(Color.blue, Color.red, (float)((input.NodeValues[e.Source] + input.NodeValues[e.Target] - 2 * input.MinValue) / (2 * range)));
//        }
//        else
//            Gizmos.color = Color.yellow;
//        Gizmos.DrawLine(e.Source.position + Vector3.up * .5f * VoxelSize, e.Target.position + Vector3.up * .5f * VoxelSize);
//    }
//}


////Linking nodes close together    
//foreach (Node n in Graph.Vertices)
//{
//    nodeKdTree.Add(new[] { n.position.x, n.position.y, n.position.z }, n);
//}
//foreach (Node n in Graph.Vertices)
//{
//    var neighbours = nodeKdTree.GetNearestNeighbours(new[] { n.position.x, n.position.y, n.position.z }, 8);
//    foreach (var neigh in neighbours)
//    {
//        //Visibility check
//        bool obstructed = Physics.Raycast(n.position + Vector3.up
//            , neigh.Value.position - n.position
//            , (neigh.Value.position - n.position).magnitude
//            , LayerMask.GetMask("World", "Walkable"));
//        if (!obstructed)
//        {
//            Graph.AddEdge(new Edge(n, neigh.Value));
//        }
//    }
//}


//public Node GetNearestNodeToPoint(Vector3 point)
//{
//    var neigh = nodeKdTree.GetNearestNeighbours(new[] { point.x, point.y, point.z }, 1);
//    if (neigh == null || neigh.Length < 1)
//    {
//        return null;
//    }
//    return neigh[0].Value;
//}
//public Node[] GetPath(Vector3 start, Vector3 target, Dictionary<string,double> inputWeights, float maxError = 10f)
//{
//    var nearestNodeToStart = GetNearestNodeToPoint(start); //Get the nearest node to the starting position
//    var nearestNodeToTarget = GetNearestNodeToPoint(target); //Get the nearest node to the target position
//    if (Vector3.Distance(target, nearestNodeToTarget.position) > maxError) { return null; } //Check if target node is sufficiently close to the target position, otherwise
//                                                                                            //this might mean that the target position is out of bounds

//    Func<Edge, double> edgeCostFunc = (e) => { //The function which calculates the edge cost for the dijkstra shortest path algorithm
//        inputWeights.TryGetValue("distance", out var dw);
//        double cost = dw * Vector3.Distance(e.Target.position, e.Source.position); //The cost depends on the weighted distance

//        foreach(var input in CostInputLayers)
//        {
//            inputWeights.TryGetValue(input.Key, out var w);
//            if(w > 0d)
//                cost += w * (input.Value.NodeValues[e.Source] + input.Value.NodeValues[e.Target]); //The cost is calculated as the combination of the single weighted input layers 
//        }
//        return cost;
//    };

//    Stack<Node> path = new Stack<Node>();
//    path.Push(nearestNodeToTarget);
//    dijkstra = new UndirectedDijkstraShortestPathAlgorithm<Node, Edge>(Graph,edgeCostFunc);
//    dijkstra.Compute(nearestNodeToStart);
//    Node currNode = nearestNodeToTarget;

//    if (dijkstra.GetDistance(nearestNodeToTarget) >= Mathf.Infinity) return null;
//    //By default quikgraph returns the path as a sequence of edges
//    while (currNode != nearestNodeToStart)//This cycle builds the path backwards(starting from target node) as a sequence of nodes
//    {
//        Node minDistNode = null;
//        float minDist = Mathf.Infinity;

//        foreach (Node n in Graph.AdjacentVertices(currNode)) {
//            if (n == nearestNodeToStart) 
//            { 
//                minDistNode = n;
//                break;
//            }

//            double dist = dijkstra.GetDistance(n);
//            if (dist < minDist)
//            {
//                minDistNode = n;
//                minDist = (float)dist;
//            }
//        }
//        if(minDistNode == null) { return null; }

//        currNode = minDistNode;
//        path.Push(currNode);
//    }
//    return path.ToArray();      
//}


//public void CalculateNodeCostsInputs()
//{
//    if (CostInputLayers.TryGetValue(LOSInputLayer.DefaultIdentifier, out var losInput))
//    {
//        losInput.CalculateNodeValues(new object[] { EnemyAgentTransforms });
//    }
//    if (CostInputLayers.TryGetValue(VisibilityInputLayer.DefaultIdentifier, out var visInput)) {
//        VisibilityConeChecker[] checkers = new VisibilityConeChecker[EnemyAgentTransforms.Length];
//        for (int i = 0; i < checkers.Length; i++)
//        {
//            checkers[i] = new VisibilityConeChecker(EnemyAgentTransforms[i],30f);
//        }
//        visInput.CalculateNodeValues(new object[] { checkers });
//    }                
//    InputLayersUpdated?.Invoke();
//}        
//private int[,] convolution2D(Sample[,] grid, int[,] kernel, int stridex = 1, int stridey = 1)
//{
//    if(kernel.GetLength(0) % 2 == 0 || kernel.GetLength(1) % 2 == 0) { throw new ArgumentException("Kernel must have odd dimensions."); }
//    int[,] convMap = new int[grid.GetLength(0), grid.GetLength(1)];
//    for (int i = 0; i < grid.GetLength(0); i += stridex)
//    {
//        for (int j = 0; j < grid.GetLength(1); j += stridey)
//        {
//            convMap[i,j] = convolution2DSample(grid, kernel, i, j);
//        }
//    }
//    return convMap;
//}
//private int convolution2DSample(Sample[,] grid, int[,] kernel, int i, int j)
//{               
//    int klx = kernel.GetLength(0);
//    int kly = kernel.GetLength(1);
//    int val = 0;


//    for(int kx = -klx/2; kx <= klx / 2; kx++)
//    {
//        for(int ky = -kly/2; ky <= kly / 2; ky++)
//        {
//            if(i + kx < 0 || i + kx > grid.GetLength(0) - 1){ continue; }
//            if(j + ky < 0 || j + ky > grid.GetLength(1) - 1){ continue; }

//            if (grid[i + kx, j + ky] != null) {
//                val += 1 * kernel[kx+klx/2, ky+kly/2];
//            }
//        }
//    }        

//    return val;
//}



//dPoints.ToList().ForEach((dPoint) => {
//    bool visible = navigationData.HeadLevelVisibilityLayer[flatSamples[100],dPointSample[dPoint]];
//    if (visible)
//        dPoint.color = Color.green;
//    else
//        dPoint.color = Color.red;
//});

//WorldBounds = GetEnvironmentBounds(WorldRoot);

//Vector3?[,] fineSamples = sampleWorld(VoxelSize);
//Sample[,] sampleGrid = new Sample[fineSamples.GetLength(0), fineSamples.GetLength(1)];

////Environment Sampling
//for (int i = 0; i < fineSamples.GetLength(0); i++)
//{
//    for (int j = 0; j < fineSamples.GetLength(1); j++)
//    {
//        var s = fineSamples[i, j];
//        if (s != null)
//        {
//            sampleGrid[i, j] = new Sample(j + i * fineSamples.GetLength(1)).SetPosition((Vector3)s);
//        }
//    }
//}

////Calculating Areas
//List<WalkableArea> walkableAreas = new List<WalkableArea>();
//var areas = CalculateAreas(sampleGrid);
//foreach (var gArea in areas)
//{
//    var wArea = gArea.ToWalkableArea(sampleGrid, VoxelSize);
//    walkableAreas.Add(wArea);
//}
//this.AreaTopologyGraph = GenerateAreaGraph(walkableAreas);


//private Vector3?[,] sampleWorld(float voxelSize)
//{
//    //Spatial Bounds
//    float maxy = WorldBounds.center.y + WorldBounds.extents.y; //Volume max height
//    float minx = WorldBounds.center.x - WorldBounds.extents.x;
//    float maxx = WorldBounds.center.x + WorldBounds.extents.x;
//    float minz = WorldBounds.center.z - WorldBounds.extents.z;
//    float maxz = WorldBounds.center.z + WorldBounds.extents.z;

//    int iterx = Mathf.FloorToInt((maxx - minx) / voxelSize);
//    int iterz = Mathf.FloorToInt((maxz - minz) / voxelSize);

//    int layerId = LayerMask.NameToLayer("Walkable");
//    int layerMask = LayerMask.GetMask("Walkable", "World");
//    RaycastHit hit;
//    Vector3?[,] grid = new Vector3?[iterx, iterz];

//    //World Sampling and Vertex Generation
//    for (int i = 0; i < iterx; i++)
//    {
//        for (int j = 0; j < iterz; j++)
//        {
//            var rayOrigin = new Vector3(minx + i * voxelSize, maxy + Mathf.Abs(maxy), minz + j * voxelSize);
//            if (Physics.SphereCast(rayOrigin, voxelSize / 2f, -Vector3.up, out hit, Mathf.Infinity, layerMask))
//            {
//                if (hit.collider.gameObject.layer.Equals(layerId))
//                {
//                    grid[i, j] = hit.point;
//                }
//            }
//        }
//    }
//    return grid;
//}