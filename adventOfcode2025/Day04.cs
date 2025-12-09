namespace adventOfcode2025;

public class Day04 : ISolver
{
    int[] dRow = { -1, -1, -1, 0, 0, 1, 1, 1 };
    int[] dCol = { -1, 0, 1, -1, 1, -1, 0, 1 };
    private bool SurroundingCells(int row, int col)
    {
        int rows = _board.GetLength(0);
        int cols = _board.GetLength(1);
        int count = 0;
      
        
        for (int i = 0; i < 8; i++)
        {
            int newRow = row + dRow[i];
            int newCol = col + dCol[i];
            if (newRow >= 0 && newRow < rows && newCol >= 0 && newCol < cols)
            {
                if (_board[newRow, newCol])
                {
                    count++;
                }
            }
        }
    
        return count < 4;
    }
    bool[,] _board;
    public object Part1(string[] input)
    {
        _board = new bool[input.Length,input[0].Length];
        for(int i = 0; i < input.Length; i++)
        for (int j = 0; j< input[i].Length; j++)
        {
            if (input[i][j] == '@') _board[i,j] = true;
            else _board[i,j] = false;
        }

        int ans = 0;
        Inizia:
        for (int i = 0; i < _board.GetLength(0); i++)
        {
            for (int j = 0; j < _board.GetLength(1); j++)
            {
                if (_board[i,j] && SurroundingCells(i,j)) ans++;
            }
        }
        return ans;
    }

    // public object Part2(string[] input) goto solve
    // {
    //     _board = new bool[input.Length,input[0].Length];
    //     for(int i = 0; i < input.Length; i++)
    //     for (int j = 0; j< input[i].Length; j++)
    //     {
    //         if (input[i][j] == '@') _board[i,j] = true;
    //         else _board[i,j] = false;
    //     }
    //
    //     int ans = 0;
    //     Inizia:
    //     for (int i = 0; i < _board.GetLength(0); i++)
    //     {
    //         for (int j = 0; j < _board.GetLength(1); j++)
    //         {
    //             if (_board[i, j] && SurroundingCells(i, j))
    //             {
    //                 ans++;
    //                 _board[i, j] = false;
    //                 goto Inizia;
    //             }
    //         }
    //     }
    //     return ans;
    // }
    public object Part2(string[] input)
    {
        _board = new bool[input.Length, input[0].Length];
        var queue = new Queue<(int row, int col)>();

        // Initialize board and queue
        for (int i = 0; i < input.Length; i++)
        {
            for (int j = 0; j < input[i].Length; j++)
            {
                _board[i, j] = input[i][j] == '@';
                if (_board[i, j])
                {
                    queue.Enqueue((i, j));
                }
            }
        }

        int ans = 0;
        var processed = new HashSet<(int, int)>();

        while (queue.Count > 0)
        {
            var (row, col) = queue.Dequeue();

            if (!_board[row, col] || processed.Contains((row, col)))
                continue;

            if (SurroundingCells(row, col))
            {
                ans++;
                _board[row, col] = false;
                processed.Add((row, col));

                // Add all 8 neighbors to queue for potential processing
                for (int i = 0; i < 8; i++)
                {
                    int newRow = row + dRow[i];
                    int newCol = col + dCol[i];
                    if (newRow >= 0 && newRow < _board.GetLength(0) &&
                        newCol >= 0 && newCol < _board.GetLength(1))
                    {
                        queue.Enqueue((newRow, newCol));
                    }
                }
            }
        }

        return ans;
    }

}