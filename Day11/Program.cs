using System;
using System.IO;

namespace Day11
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt");
            Console.WriteLine(Part1(lines));
            Console.WriteLine(Part2(lines));
        }

        static int[,] PrepareSeats(string[] lines)
        {
            int[,] seats = new int[lines.Length, lines[0].Length];
            for(int i = 0; i < lines.Length; i++)
            {
                for(int j = 0; j < lines[i].Length; j++)
                {
                    if(lines[i][j] == 'L') seats[i,j] = -1;
                    if(lines[i][j] == '#') seats[i,j] = 1;
                }
            }
            return seats; 
        }

        static int CheckAdjacency(int[,] seats, int i, int j, int s)
        {
            int taken = 0;
            if(i > 0) taken += (seats[i-1,j] == s) ? 1 : 0;
            if(i > 0 && j > 0) taken += (seats[i-1,j-1] == s) ? 1 : 0;
            if(i > 0 && j < seats.GetLength(1) - 1) taken += (seats[i-1,j+1] == s) ? 1 : 0;
            if(j > 0) taken += (seats[i,j-1] == s) ? 1 : 0;
            if(j < seats.GetLength(1) - 1) taken += (seats[i,j+1] == s) ? 1 : 0;
            if(i < seats.GetLength(0) - 1) taken += (seats[i+1,j] == s) ? 1 : 0;
            if(i < seats.GetLength(0) - 1 && j > 0) taken += (seats[i+1,j-1] == s) ? 1 : 0;
            if(i < seats.GetLength(0) - 1 && j < seats.GetLength(1)-1) taken += (seats[i+1,j+1] == s) ? 1 : 0;
            return taken;
        }

        static bool CheckInDirection(int[,] seats, int i, int j, int s, Func<int,int, (int,int)> moveIndexesFunc)
        {
            (int ii, int jj) = moveIndexesFunc(i,j);
            while(ii >= 0 && ii < seats.GetLength(0) && jj >= 0 && jj < seats.GetLength(1))
            {
                if(seats[ii,jj] != 0)
                {
                    return seats[ii,jj] == s;
                }
                (ii,jj) = moveIndexesFunc(ii,jj);
            }
            return false;
        }

        static int CheckAdjacency2(int[,] seats, int i, int j, int s)
        {
            int taken = 0;
            taken += CheckInDirection(seats, i, j, s, (x, y) => (x-1, y)) ? 1 : 0;
            taken += CheckInDirection(seats, i, j, s, (x, y) => (x-1, y-1)) ? 1 : 0;
            taken += CheckInDirection(seats, i, j, s, (x, y) => (x-1, y+1)) ? 1 : 0;
            taken += CheckInDirection(seats, i, j, s, (x, y) => (x, y-1)) ? 1 : 0;
            taken += CheckInDirection(seats, i, j, s, (x, y) => (x, y+1)) ? 1 : 0;
            taken += CheckInDirection(seats, i, j, s, (x, y) => (x+1, y-1)) ? 1 : 0;
            taken += CheckInDirection(seats, i, j, s, (x, y) => (x+1, y)) ? 1 : 0;
            taken += CheckInDirection(seats, i, j, s, (x, y) => (x+1, y+1)) ? 1 : 0;
            return taken;
        }

        static (int[,], int) TakeSeats(int[,] seats, Func<int[,], int, int, int, int> checkAdjacency)
        {
            int changes = 0;
            int[,] newSeats = new int[seats.GetLength(0), seats.GetLength(1)];
            for(int i = 0; i < seats.GetLength(0); i++)
            {
                for(int j = 0; j < seats.GetLength(1); j++)
                {
                    newSeats[i,j] = seats[i,j];
                    if(seats[i,j] == -1 && checkAdjacency(seats, i, j, 1) == 0)
                    {
                        newSeats[i,j] = 1;
                        changes++;
                    }
                }
            }
            return (newSeats, changes);
        }

        static (int[,], int) LeaveSeats(int[,] seats, int tollerance, Func<int[,], int, int, int, int> checkAdjacency)
        {
            int changes = 0;
            int[,] newSeats = new int[seats.GetLength(0), seats.GetLength(1)];
            for(int i = 0; i < seats.GetLength(0); i++)
            {
                for(int j = 0; j < seats.GetLength(1); j++)
                {
                    newSeats[i,j] = seats[i,j];
                    if(seats[i,j] == 1 && checkAdjacency(seats, i, j, 1) >= tollerance)
                    {
                        newSeats[i,j] = -1;
                        changes++;
                    }
                }
            }
            return (newSeats, changes);
        }

        static int NumberOfTaken(int[,] seats)
        {
            int s = 0;
            for(int i = 0; i < seats.GetLength(0); i++)
            {
                for(int j = 0; j < seats.GetLength(1); j++)
                {
                    if(seats[i,j] == 1) s++;
                }
            }
            return s;
        }

        static int Part1(string[] lines)
        {
            int changes = 0;
            var seats = PrepareSeats(lines);
            do
            {
                (seats, changes) = TakeSeats(seats, CheckAdjacency);
                (seats, changes) = LeaveSeats(seats, 4, CheckAdjacency);
            } while(changes != 0);
            return NumberOfTaken(seats);
        }

        static int Part2(string[] lines)
        {
            int changes = 0;
            var seats = PrepareSeats(lines);
            do
            {
                (seats, changes) = TakeSeats(seats, CheckAdjacency2);
                (seats, changes) = LeaveSeats(seats, 5, CheckAdjacency2);
            } while(changes != 0);
            return NumberOfTaken(seats);
        }
    }
}
