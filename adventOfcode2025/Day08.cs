using System.Collections;

namespace adventOfcode2025;

class DSU
{
    public List<int> parents;
    public int[] size;
    public int NumSets;
    public DSU(int n)
    {
        parents= Enumerable.Repeat(0, n).ToList();
        size= Enumerable.Repeat(0, n).ToArray();
        NumSets=n;
    }

    public void make_set(int v)
    {
        parents[v]=v;
        size[v]=1;
    }

    public void union_set(int a, int b)
    {
        a = find_set(a);
        b = find_set(b);
        if (a == b) return;
        if (size[a] < size[b]) (a,b)=(b,a);
        parents[b] = a;
        size[a] += size[b];
        NumSets--;
    }

    public int find_set(int v)
    {
        if (v == parents[v]) return v;
        return parents[v] = find_set(parents[v]);
    }
    static public void PrintDSUStructure(DSU dsu, Dictionary<int, Point3D> idToCoords)
    {
        // 1. Group all nodes by their ultimate root
        var sets = new Dictionary<int, List<int>>();
        
        // Skip index 0 as your IDs start at 1
        for (int i = 1; i < dsu.parents.Count; i++)
        {
            // find_set ensures path compression, so we get the true root
            int root = dsu.find_set(i);
            
            if (!sets.ContainsKey(root))
            {
                sets[root] = new List<int>();
            }
            sets[root].Add(i);
        }

        Console.WriteLine("\n--- DSU Structure Visualization ---");
        Console.WriteLine($"Total Disjoint Sets: {sets.Count}");

        foreach (var set in sets)
        {
            int rootId = set.Key;
            List<int> members = set.Value;
            int setSize = dsu.size[rootId];
            
            Console.WriteLine($"Set Root ID: {rootId} (Size: {setSize})");
            Console.WriteLine("  Members:");
            
            foreach (var memberId in members)
            {
                string isRoot = memberId == rootId ? " [ROOT]" : "";
                // Retrieve coordinates if available, otherwise just print ID
                string coords = idToCoords.ContainsKey(memberId) ? idToCoords[memberId].ToString() : "Unknown";
                Console.WriteLine($"    - ID {memberId}: {coords}{isRoot}");
            }
            Console.WriteLine();
        }
        Console.WriteLine("-----------------------------------");
    }
}

public struct Point3D
{
    public int X, Y, Z;
    public Point3D(int x, int y, int z) { X = x; Y = y; Z = z; }
    public override string ToString() => $"({X},{Y},{Z})";
}

struct Edge
{
    public int U; // ID of first point
    public int V; // ID of second point
    public long DistSq; // Squared distance
}
public class Day08:ISolver
{
    public static Point3D GetClosestPoint(List<Point3D> points, Point3D query)
    {
        int closestIndex = -1;
        long minSqDist = long.MaxValue;

        for (int i = 0; i < points.Count; i++)
        {
            long dx = points[i].X - query.X;
            long dy = points[i].Y - query.Y;
            long dz = points[i].Z - query.Z;

            long currentSqDist = dx * dx + dy * dy + dz * dz;

            if (currentSqDist < minSqDist && currentSqDist > 0)
            {
                minSqDist = currentSqDist;
                closestIndex = i;
            }
        }
        return points[closestIndex];
    }
    public object Part1(string[] input)
    {
        DSU dsu = new(input.Length);
        Dictionary<Point3D,int> coordsToId = new();
        Dictionary<int, Point3D> idToCoords = new();
        List<Point3D> cdx= new List<Point3D>();
        int Id=0;
        foreach (var coords in input)
        {
            var co = coords.Split(',').Select(int.Parse).ToArray();
            dsu.make_set(Id);
            var aux = new Point3D(co[0], co[1], co[2]);
            coordsToId.Add(aux,Id);
            idToCoords.Add(Id, aux); 
            cdx.Add(aux);
            Id++;
        }
        // ho capito male il problema lol
        // foreach (var coords in cdx)
        // {
        //     dsu.union_set(coordsToId[coords], coordsToId[GetClosestPoint(cdx, coords)
        // ]);
        // }
        int n = cdx.Count;
        List<Edge> allEdges = new List<Edge>();
        for (int i = 0; i < n; i++)
        {
            for (int j = i + 1; j < n; j++)
            {
                long dx = cdx[i].X - cdx[j].X;
                long dy = cdx[i].Y - cdx[j].Y;
                long dz = cdx[i].Z - cdx[j].Z;
                long distSq = dx*dx + dy*dy + dz*dz;

                allEdges.Add(new Edge { U = i, V = j, DistSq = distSq });
            }
        }
        allEdges.Sort((a, b) => a.DistSq.CompareTo(b.DistSq));
        int limit = 1000;//10 for test case
        for (int i = 0; i < limit; i++)
        {
            var edge = allEdges[i];
            dsu.union_set(edge.U, edge.V);
        }
        List<int> Candidate=new List<int>();
        for (int i = 0; i < Id; i++)
        {
            if (dsu.parents[i]==i)Candidate.Add(dsu.size[i]);
        }
    //    DSU.PrintDSUStructure(dsu, idToCoords);
        Candidate.Sort();
        return Candidate[Candidate.Count-1]*Candidate[Candidate.Count-2]*Candidate[Candidate.Count-3];

    }

    public object Part2(string[] input)
    {
        DSU dsu = new(input.Length);
        Dictionary<Point3D,int> coordsToId = new();
        Dictionary<int, Point3D> idToCoords = new();
        List<Point3D> cdx= new List<Point3D>();
        int Id=0;
        foreach (var coords in input)
        {
            var co = coords.Split(',').Select(int.Parse).ToArray();
            dsu.make_set(Id);
            var aux = new Point3D(co[0], co[1], co[2]);
            coordsToId.Add(aux,Id);
            idToCoords.Add(Id, aux); 
            cdx.Add(aux);
            Id++;
        }
        // ho capito male il problema lol
        // foreach (var coords in cdx)
        // {
        //     dsu.union_set(coordsToId[coords], coordsToId[GetClosestPoint(cdx, coords)
        // ]);
        // }
        int n = cdx.Count;
        List<Edge> allEdges = new List<Edge>();
        for (int i = 0; i < n; i++)
        {
            for (int j = i + 1; j < n; j++)
            {
                long dx = cdx[i].X - cdx[j].X;
                long dy = cdx[i].Y - cdx[j].Y;
                long dz = cdx[i].Z - cdx[j].Z;
                long distSq = dx*dx + dy*dy + dz*dz;

                allEdges.Add(new Edge { U = i, V = j, DistSq = distSq });
            }
        }
        allEdges.Sort((a, b) => a.DistSq.CompareTo(b.DistSq));
        int limit = allEdges.Count;
        for (int i = 0; i < limit; i++)
        {
            var edge = allEdges[i];
            dsu.union_set(edge.U, edge.V);
            if (dsu.NumSets==1) {long result = (long)cdx[edge.U].X * cdx[edge.V].X;
                return result;};
        }
        DSU.PrintDSUStructure(dsu, idToCoords);
        // Console.WriteLine(cdx[allEdges[limit].U].X+ " " + cdx[allEdges[limit].V].X);
        return "Something went wrong";

    }
}