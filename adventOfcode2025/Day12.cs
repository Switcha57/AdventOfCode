using System;
using System.Collections.Generic;
using System.Linq;

namespace adventOfcode2025
{
    public class Day12 : ISolver
    {
        public object Part1(string[] input) // NOT FUN
        {
            List<List<(int r, int c)>> rawShapes = new();
            List<string> currentShapeLines = new();
            List<(int W, int H, List<int> ShapeIds)> regions = new();

            bool parsingShapes = true;

            for (int i = 0; i < input.Length; i++)
            {
                string line = input[i].Trim();
                if (string.IsNullOrWhiteSpace(line)) continue;

                if (line.Contains("x") && line.Contains(":"))
                {
                    if (currentShapeLines.Count > 0)
                    {
                        rawShapes.Add(ParseShapeGrid(currentShapeLines));
                        currentShapeLines.Clear();
                    }
                    parsingShapes = false;
                    
                    var parts = line.Split(':');
                    var dims = parts[0].Split('x');
                    int w = int.Parse(dims[0]);
                    int h = int.Parse(dims[1]);
                    
                    var counts = parts[1].Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
                    
                    List<int> shapeIds = new();
                    for (int id = 0; id < counts.Length; id++)
                    {
                        for (int k = 0; k < counts[id]; k++) shapeIds.Add(id);
                    }
                    regions.Add((w, h, shapeIds));
                }
                else if (parsingShapes)
                {
                    if (char.IsDigit(line[0]) && line.Contains(":"))
                    {
                        if (currentShapeLines.Count > 0)
                        {
                            rawShapes.Add(ParseShapeGrid(currentShapeLines));
                            currentShapeLines.Clear();
                        }
                    }
                    else
                    {
                        currentShapeLines.Add(line);
                    }
                }
            }
            if (currentShapeLines.Count > 0) rawShapes.Add(ParseShapeGrid(currentShapeLines));

            var shapeVariations = new List<List<List<(int r, int c)>>>();

            foreach (var baseShape in rawShapes)
            {
                var distinctVars = new List<List<(int r, int c)>>();
                var currentVar = baseShape;

                for (int r = 0; r < 4; r++)
                {
                    AddDistinct(distinctVars, currentVar);
                    AddDistinct(distinctVars, Flip(currentVar));
                    currentVar = Rotate90(currentVar);
                }
                shapeVariations.Add(distinctVars);
            }

            int solvedCount = 0;

            foreach (var region in regions)
            {
                var sortedShapeIds = region.ShapeIds
                    .OrderByDescending(id => rawShapes[id].Count)
                    .ToList();

                bool[,] grid = new bool[region.H, region.W]; 
                
                int totalArea = sortedShapeIds.Sum(id => rawShapes[id].Count);
                if (totalArea > region.W * region.H) continue; 

                if (Solve(grid, sortedShapeIds, 0))
                {
                    solvedCount++;
                }
            }

            return solvedCount;


            bool Solve(bool[,] grid, List<int> shapesToPlace, int index)
            {
                if (index >= shapesToPlace.Count) return true;

                int shapeId = shapesToPlace[index];
                var variations = shapeVariations[shapeId];
                int H = grid.GetLength(0);
                int W = grid.GetLength(1);

                // Optimization: Scan linearly for placement
                for (int r = 0; r < H; r++)
                {
                    for (int c = 0; c < W; c++)
                    {
                        foreach (var shape in variations)
                        {
                            if (CanPlace(grid, shape, r, c))
                            {
                                Place(grid, shape, r, c, true);
                                if (Solve(grid, shapesToPlace, index + 1)) return true;
                                Place(grid, shape, r, c, false);
                            }
                        }
                    }
                }
                return false;
            }

            bool CanPlace(bool[,] grid, List<(int r, int c)> shape, int rOff, int cOff)
            {
                int H = grid.GetLength(0);
                int W = grid.GetLength(1);

                foreach (var p in shape)
                {
                    int nr = rOff + p.r;
                    int nc = cOff + p.c;
                    if (nr < 0 || nr >= H || nc < 0 || nc >= W) return false;
                    if (grid[nr, nc]) return false;
                }
                return true;
            }

            void Place(bool[,] grid, List<(int r, int c)> shape, int rOff, int cOff, bool val)
            {
                foreach (var p in shape)
                {
                    grid[rOff + p.r, cOff + p.c] = val;
                }
            }

            List<(int r, int c)> ParseShapeGrid(List<string> lines)
            {
                var points = new List<(int r, int c)>();
                for (int r = 0; r < lines.Count; r++)
                {
                    for (int c = 0; c < lines[r].Length; c++)
                    {
                        if (lines[r][c] == '#') points.Add((r, c));
                    }
                }
                return Normalize(points);
            }

            List<(int r, int c)> Normalize(List<(int r, int c)> input)
            {
                if (input.Count == 0) return input;
                int minR = input.Min(p => p.r);
                int minC = input.Min(p => p.c);
                
                return input.Select(p => (r: p.r - minR, c: p.c - minC)) 
                            .OrderBy(p => p.r).ThenBy(p => p.c)
                            .ToList();
            }

            List<(int r, int c)> Rotate90(List<(int r, int c)> input)
            {
                return Normalize(input.Select(p => (r: p.c, c: -p.r)).ToList());
            }

            List<(int r, int c)> Flip(List<(int r, int c)> input)
            {
                return Normalize(input.Select(p => (r: p.r, c: -p.c)).ToList());
            }

            void AddDistinct(List<List<(int r, int c)>> distinctList, List<(int r, int c)> candidate)
            {
                foreach (var existing in distinctList)
                {
                    if (existing.Count != candidate.Count) continue;
                    bool match = true;
                    for (int i = 0; i < existing.Count; i++)
                    {
                        if (existing[i].r != candidate[i].r || existing[i].c != candidate[i].c) 
                        { 
                            match = false; 
                            break; 
                        }
                    }
                    if (match) return;
                }
                distinctList.Add(candidate);
            }
        }

        public object Part2(string[] input)
        {
            throw new NotImplementedException();
        }
    }
}