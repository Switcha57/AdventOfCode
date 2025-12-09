namespace adventOfcode2025;

using Dumpify;

public class Day09 : ISolver
{
    record Points(long x, long y);

    record Segment(Points p1, Points p2);

    public object Part1(string[] input)
    {
        List<Points> angoli = new List<Points>();
        foreach (var line in input)
        {
            var co = line.Split(',').Select(int.Parse).ToArray();
            angoli.Add(new Points(co[0], co[1]));
        }

        long maxArea = 0;

        for (int i = 0; i < angoli.Count; i++)
        {
            for (int j = i + 1; j < angoli.Count; j++)
            {
                Points a = angoli[i];
                Points b = angoli[j];
                long Area = ((Math.Abs(a.x - b.x) + 1) * (Math.Abs(a.y - b.y) + 1));
                if (Area > maxArea)
                {
                    maxArea = Area;
                }
            }
        }

        // Console.WriteLine($"{bestP1.x},{bestP1.y} {bestP2.x},{bestP2.y}");
        return maxArea;
    }

//https://training.olinfo.it/task/ois_polygon/ ma solo muri orizonatali e veritcali
// Non si puo creare esplicitamente la griglia sad..
    public object Part2(string[] input)
    {
        List<Points> angoli = new List<Points>();
        foreach (var line in input)
        {
            var co = line.Split(',').Select(int.Parse).ToArray();
            angoli.Add(new Points(co[1], co[0]));
        }

        // long minX = angoli.Min(p => p.x);
        // long maxX = angoli.Max(p => p.x);
        // long minY = angoli.Min(p => p.y);
        // long maxY = angoli.Max(p => p.y);
        //
        // // Mi assicuro che 0,0 sia libero
        // long width = maxX + 2;
        // long height = maxY + 2;
        //
        // long[,] grid = new long[width, height];
        // for (int i = 0; i < angoli.Count; i++)
        // {
        //     Points p1 = angoli[i];
        //     Points p2 = angoli[(i + 1) % angoli.Count]; // Wrap around to first point
        //
        //     // Draw line between p1 and p2
        //     if (p1.x == p2.x) // Vertical
        //     {
        //         long start = Math.Min(p1.y, p2.y);
        //         long end = Math.Max(p1.y, p2.y);
        //         for (var y = start; y <= end; y++) grid[p1.x, y] = 1;
        //     }
        //     else // Horizontal
        //     {
        //         var start = Math.Min(p1.x, p2.x);
        //         var end = Math.Max(p1.x, p2.x);
        //         for (var x = start; x <= end; x++) grid[x, p1.y] = 1;
        //     }
        // }
        //
        //
        // Queue<Points> q = new Queue<Points>();
        // q.Enqueue(new Points(0, 0));
        // grid[0, 0] = -1; // esterno
        // while (q.Count > 0)
        // {
        //     Points p = q.Dequeue();
        //     int[] dx = { 0, 0, 1, -1 };
        //     int[] dy = { 1, -1, 0, 0 };
        //     for (int k = 0; k < 4; k++)
        //     {
        //         long nx = p.x + dx[k];
        //         long ny = p.y + dy[k];
        //
        //         if (nx >= 0 && nx < width && ny >= 0 && ny < height)
        //         {
        //             if (grid[nx, ny] == 0)
        //             {
        //                 grid[nx, ny] = -1;
        //                 q.Enqueue(new Points(nx, ny));
        //             }
        //         }
        //     }
        // }
        //
        // Console.WriteLine("Grid:");
        // grid.Dump();
        // // prefix sum 2d
        // long[,] prefix_sum = new long[width + 1, height + 1];
        // Func<long, long, long, long, long> getSum = (x1, y1, x2, y2) =>
        // {
        //     long xMin = Math.Min(x1, x2);
        //     long xMax = Math.Max(x1, x2);
        //     long yMin = Math.Min(y1, y2);
        //     long yMax = Math.Max(y1, y2);
        //     
        //     return prefix_sum[xMax + 1, yMax + 1] 
        //            - prefix_sum[xMin, yMax + 1] 
        //            - prefix_sum[xMax + 1, yMin] 
        //            + prefix_sum[xMin, yMin];
        // };
        //
        //
        // for (int x = 0; x < width; x++)
        // {
        //     for (int y = 0; y < height; y++)
        //     {
        //         // A tile is valid (1) if it is NOT outside (2). 
        //         // So (Border or Interior) counts as 1.
        //         int isTileValid = (grid[x, y] != -1? 1 : 0);
        //
        //         prefix_sum[x + 1, y + 1] = isTileValid 
        //                                  + prefix_sum[x, y + 1] 
        //                                  + prefix_sum[x + 1, y] 
        //                                  - prefix_sum[x, y];
        //     }
        // }
        //
        long maxArea = 0;
        Points bestP1 = null;
        Points bestP2 = null;
        List<Segment> perimeter = new List<Segment>();
        for (int i = 0; i < angoli.Count; i++)
        {
            perimeter.Add(new Segment(angoli[i], angoli[(i + 1) % angoli.Count]));
        }

        for (int i = 0; i < angoli.Count; i++)
        {
            for (int j = i + 1; j < angoli.Count; j++)
            {
                Points p1 = angoli[i];
                Points p2 = angoli[j];

                long minX = Math.Min(p1.x, p2.x);
                long maxX = Math.Max(p1.x, p2.x);
                long minY = Math.Min(p1.y, p2.y);
                long maxY = Math.Max(p1.y, p2.y);

                long width = (maxX - minX) + 1;
                long height = (maxY - minY) + 1;
                long area = width * height;

                if (area <= maxArea) continue;

                if (!IsCenterInPolygon(p1, p2, perimeter))
                {
                    continue;
                }

                if (DoesPolygonCutRect(minX, maxX, minY, maxY, perimeter))
                {
                    continue;
                }

                maxArea = area;
                bestP1 = p1;
                bestP2 = p2;
            }
        }

        // Console.WriteLine($"Area Massima: {maxArea}");
        // if (bestP1 != null)
        // {
        //     Console.WriteLine($"Coordinate: ({bestP1.x},{bestP1.y}) - ({bestP2.x},{bestP2.y})");
        // }
        return maxArea;
    }

    // logica semplificata del problema di olinfo
    static bool IsCenterInPolygon(Points p1, Points p2, List<Segment> segments)
    {
        long cx2 = p1.x + p2.x;
        long cy2 = p1.y + p2.y;

        int intersections = 0;

        foreach (var seg in segments)
        {
            long s1x2 = seg.p1.x * 2;
            long s1y2 = seg.p1.y * 2;
            long s2x2 = seg.p2.x * 2;
            long s2y2 = seg.p2.y * 2;

            if (s1y2 == s2y2) continue;

            bool isBetweenY = (s1y2 > cy2) != (s2y2 > cy2);

            if (isBetweenY)
            {
                if (s1x2 > cx2)
                {
                    intersections++;
                }
            }
        }

        return (intersections % 2) != 0;
    }


    static bool DoesPolygonCutRect(long rMinX, long rMaxX, long rMinY, long rMaxY, List<Segment> segments)
    {
        foreach (var seg in segments)
        {
            if (seg.p1.x == seg.p2.x)
            {
                long x = seg.p1.x;
                long yStart = Math.Min(seg.p1.y, seg.p2.y);
                long yEnd = Math.Max(seg.p1.y, seg.p2.y);

                if (x > rMinX && x < rMaxX)
                {
                    if (Math.Max(yStart, rMinY) < Math.Min(yEnd, rMaxY)) return true;
                }
            }
            else
            {
                long y = seg.p1.y;
                long xStart = Math.Min(seg.p1.x, seg.p2.x);
                long xEnd = Math.Max(seg.p1.x, seg.p2.x);

                if (y > rMinY && y < rMaxY)
                {
                    if (Math.Max(xStart, rMinX) < Math.Min(xEnd, rMaxX)) return true;
                }
            }
        }

        return false;
    }
}