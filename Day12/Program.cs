using System;
using System.IO;

namespace Day12
{
    enum Direction
    {
        North = 0,
        West = 1,
        South = 2,
        East = 3
    }

    class Vehicle
    {
        public Direction Direction;

        public int X;
        public int Y;

        public Vehicle()
        {
            X = Y = 0;
            Direction = Direction.East;
        }

        public void Move(string instruction)
        {
            int param = int.Parse(instruction.Substring(1));
            switch(instruction[0])
            {
                case 'N':
                    Y += param;
                    break;
                case 'S':
                    Y -= param;
                    break;
                case 'E':
                    X += param;
                    break;
                case 'W':
                    X -= param;
                    break;
                case 'L':
                    Rotate(param);
                    break;
                case 'R':
                    Rotate(-param);
                    break;
                case 'F':
                    Forward(param);
                    break;
            }
        }

        private void Forward(int param)
        {
            switch(Direction)
            {
                case Direction.North:
                    Move($"N{param}"); 
                    break;
                case Direction.East:
                    Move($"E{param}"); 
                    break;
                case Direction.West:
                    Move($"W{param}");
                    break; 
                case Direction.South:
                    Move($"S{param}");
                    break; 
            }
        }

        private void Rotate(int degrees)
        {
            int times = degrees / 90 + 4;
            Direction = (Direction)(((int)Direction+times) % 4);
        } 
    }

    class VehicleWithWayPoint
    {
        public int VehicleX;
        public int VehicleY;
        public int WayPointX;
        public int WayPointY;

        public VehicleWithWayPoint()
        {
            VehicleX = VehicleY = 0;
            WayPointX = 10;
            WayPointY = 1;
        }

        public void Move(string instruction)
        {
            int param = int.Parse(instruction.Substring(1));
            switch(instruction[0])
            {
                case 'N':
                    WayPointY += param;
                    break;
                case 'S':
                    WayPointY -= param;
                    break;
                case 'E':
                    WayPointX += param;
                    break;
                case 'W':
                    WayPointX -= param;
                    break;
                case 'L':
                    Rotate(param);
                    break;
                case 'R':
                    Rotate(-param);
                    break;
                case 'F':
                    Forward(param);
                    break;
            }
        }

        private void Forward(int param)
        {
            int x = WayPointX - VehicleX;
            int y = WayPointY - VehicleY;
            VehicleX += param * x;
            WayPointX += param * x;
            VehicleY += param * y;
            WayPointY += param * y;
        }

        private void Rotate(int param)
        {
            int times = param / 90 + 4;
            while(times > 0)
            {
                int x = WayPointX - VehicleX;
                int y = WayPointY - VehicleY;
                WayPointX = VehicleX - y;
                WayPointY = VehicleY + x;
                times--;
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt");
            Console.WriteLine(Part1(lines));
            Console.WriteLine(Part2(lines));
        }

        static int Part1(string[] lines)
        {
            Vehicle v = new Vehicle();
            foreach(var instr in lines)
            {
                v.Move(instr);
            }
            return Math.Abs(v.X) + Math.Abs(v.Y);
        }

        static int Part2(string[] lines)
        {
            VehicleWithWayPoint v = new VehicleWithWayPoint();
            foreach(var instr in lines)
            {
                v.Move(instr);
            }
            return Math.Abs(v.VehicleX) + Math.Abs(v.VehicleY);
        }
    }
}
