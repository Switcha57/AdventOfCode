namespace adventOfcode2025;

public class Day04 : ISolver
{
    private bool SurroundingCells(int row, int col)
    {
        int rows = _board.GetLength(0);
        int cols = _board.GetLength(1);
        int count = 0;
        int[] dRow = { -1, -1, -1, 0, 0, 1, 1, 1 };
        int[] dCol = { -1, 0, 1, -1, 1, -1, 0, 1 };
        
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

    public object Part2(string[] input)
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
                if (_board[i, j] && SurroundingCells(i, j))
                {
                    ans++;
                    _board[i, j] = false;
                    goto Inizia;
                }
            }
        }
        return ans;
    }
}