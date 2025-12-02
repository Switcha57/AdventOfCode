namespace adventOfcode2025;

public class Day01 : ISolver
{
    private class Dial
    {
        private int _position = 0;
        private int _count = 0;
        public void SetCurrentPosition(int position) => _position = position;
        public void IncrementCount() => _count++;
        public int GetCurrentPosition() => _position;
        public int GetCount() => _count;
        public int TurnRight(int v) => (_position + v) % 100;
        public int TurnLeft(int v) => (_position - v + 100) % 100;
    }

    public object Part1(string[] input)
    {
        Dial lockeDial = new();
        lockeDial.SetCurrentPosition(50);

        foreach (var line in input)
        {
            lockeDial.SetCurrentPosition(line.StartsWith("L")
                ? lockeDial.TurnLeft(int.Parse(line.Substring(1)))
                : lockeDial.TurnRight(int.Parse(line.Substring(1))));

            if (lockeDial.GetCurrentPosition() == 0) lockeDial.IncrementCount();
        }

        //Console.WriteLine(lockeDial.getCount());
        return lockeDial.GetCount();
    }

    private class Dial2
    {
        private int _position = 0;
        private int _count = 0;
        public void setCurrentPosition(int position) => _position = position;
        private void incrementCount(int v) => _count += v;
        private int getCurrentPosition() => _position;
        public int getCount() => _count;
        private int turnRight(int v) => (_position + v) % 100;
        private int turnLeft(int v) => (_position - v + 100) % 100;

        public void CheckTurn(char turn, int v)
        {
            int old = getCurrentPosition();
            int numberOfFullRot = v / 100;
            incrementCount(numberOfFullRot);
            v -= numberOfFullRot * 100;
            if (v == 0) return;
            if (turn == 'R')
            {
                setCurrentPosition(turnRight(v));
                if (old > getCurrentPosition()|| getCurrentPosition() == 0)
                {
                    incrementCount(1);
                }
            }
            else
            {
                setCurrentPosition(turnLeft(v));
                if (old!=0&&(old < getCurrentPosition()|| getCurrentPosition() == 0))
                {
                    incrementCount(1);
                }
            }

            return;
        }
    }

    public object Part2(string[] input)
    {

        Dial2 lockeDial = new();
        lockeDial.setCurrentPosition(50);

        foreach (var line in input)
        {
            lockeDial.CheckTurn(line[0], int.Parse(line.Substring(1)));
        }

        // Console.WriteLine(lockeDial.getCount());
        return lockeDial.getCount();
    }
}
