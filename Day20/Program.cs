using System;
using System.Collections.Generic;
using System.IO;

namespace Day20
{
    class Tile
    {
        public int Id;
        public int Index;
        public bool[,] Puzzle;
    }

    class TileContainer
    {
        public Tile Tile;
        public int ConfigurationNumber;
        public bool[,] CurrentConfiguration;
    }

    enum State
    {
        None,
        Checked,
        Marked,
    }

    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt");
            var tiles = Parse(lines);
            Console.WriteLine(Part1(tiles));
            Console.WriteLine(Part2(tiles));
        }

        static List<Tile> Parse(string[] lines)
        {
            var tiles = new List<Tile>();
            int ind = 0;
            Tile tile = null;
            int tind = 0;
            while(ind < lines.Length)
            {
                if(lines[ind].StartsWith("Tile"))
                {    
                    tile = new Tile() { Id = int.Parse(lines[ind].Substring(5).Trim(':')), Index = tind++};
                    tiles.Add(tile);
                }
                else
                {
                    while(ind < lines.Length && !string.IsNullOrWhiteSpace(lines[ind]))
                    {
                        var n = lines[ind].Trim().Length;
                        tile.Puzzle = new bool[n,n];
                        for(int i = 0; i < n; i++, ind++)
                        {
                            for(int j = 0; j < n; j++)
                                tile.Puzzle[i,j] = lines[ind][j] == '#';
                        }
                    }
                }
                ind++;
            }
            return tiles;
        }

        static bool[,] GetConfiguration(Tile tile, int configNum)
        {
            int size = tile.Puzzle.GetLength(0);
            bool[,] puzzle = new bool[size, size];
            switch(configNum)
            {
                case 0: //normal
                    for(int i = 0; i < size; i++)
                        for(int j = 0; j < size; j++)
                            puzzle[i,j] = tile.Puzzle[i,j];
                    break;
                case 1: //rotated 90 deg counterclockwise TODO
                    for(int i = 0; i < size; i++)
                        for(int j = 0; j < size; j++)
                            puzzle[size-1-j, i] = tile.Puzzle[i,j];
                    break;
                case 2: //rotated 180 deg counterclockwise TODO
                    for(int i = 0; i < size; i++)
                        for(int j = 0; j < size; j++)
                            puzzle[size-1-i, size-1-j] = tile.Puzzle[i,j];
                    break;
                case 3: //rotated 270 deg counterclockwise TODO
                    for(int i = 0; i < size; i++)
                        for(int j = 0; j < size; j++)
                            puzzle[j, size-1-i] = tile.Puzzle[i,j];
                    break;
                case 4: //flipped, no rotation TODO
                    for(int i = 0; i < size; i++)
                        for(int j = 0; j < size; j++)
                            puzzle[i,size-1-j] = tile.Puzzle[i,j];
                    break;
                case 5: //flipped, rotated 90 deg counterclockwise TODO
                    for(int i = 0; i < size; i++)
                        for(int j = 0; j < size; j++)
                            puzzle[j,i] = tile.Puzzle[i,j];
                    break;
                case 6: //flipped, rotated 180 deg counterclockwise TODO
                    for(int i = 0; i < size; i++)
                        for(int j = 0; j < size; j++)
                            puzzle[size-1-i,j] = tile.Puzzle[i,j];
                    break;
                case 7: //filipped, rotated 270 deg counterclockwise TODO
                    for(int i = 0; i < size; i++)
                        for(int j = 0; j < size; j++)
                            puzzle[size-1-j,size-1-i] = tile.Puzzle[i,j];
                    break;
            }
            return puzzle;
        }

        static bool IsConfigurationPossibleInPlace(TileContainer[,] tiles, int currentX, int currentY, int imageSize, bool[,] currentTile)
        {
            if(currentX > 0)
            {
                var left = tiles[currentY, currentX-1];
                for(int i = 0; i < imageSize; i++)
                {
                    if(left.CurrentConfiguration[i, imageSize-1] != currentTile[i, 0])
                        return false;
                }
            }
            if(currentY > 0)
            {
                var up = tiles[currentY-1, currentX];
                for(int i = 0; i < imageSize; i++)
                {
                    if(up.CurrentConfiguration[imageSize-1, i] != currentTile[0, i])
                        return false;
                }
            }
            return true;
        }

        static bool FindTileConfiguration(List<Tile> allTiles, bool[] visited, int imageSize, int currentY, int currentX, TileContainer[,] tiles, int count = 0, bool found = false)
        {
            if(found) return true; 
            if(count == imageSize * imageSize)
            {
                return true;
            }
            for(int i = 0; i < allTiles.Count; i++)
            {
                if(!visited[allTiles[i].Index])
                {
                    for(int c = 0; c < 8; c++)
                    {
                        bool[,] puzzle = GetConfiguration(allTiles[i], c);
                        if(IsConfigurationPossibleInPlace(tiles, currentX, currentY, puzzle.GetLength(0), puzzle))
                        {
                            tiles[currentY, currentX] = new TileContainer()
                            {   
                                Tile = allTiles[i], ConfigurationNumber = c, CurrentConfiguration = puzzle
                            };
                            visited[i] = true;
                            if(currentX < imageSize - 1)
                                found = FindTileConfiguration(allTiles, visited, imageSize, currentY, currentX + 1, tiles, count+1, found);
                            else
                                found = FindTileConfiguration(allTiles, visited, imageSize, currentY+1, 0, tiles, count+1, found);
                            visited[i] = false;
                            if(found) return found;
                        }
                    }
                }
            }
            return found;
        }

        static long Part1(List<Tile> allTiles)
        {
            int imageSize = (int)Math.Sqrt(allTiles.Count);
            var visited = new bool[allTiles.Count];
            var tiles = new TileContainer[imageSize,imageSize];
            FindTileConfiguration(allTiles, visited, imageSize, 0, 0, tiles, 0);
            return (long)tiles[0,0].Tile.Id * (long)tiles[imageSize-1,0].Tile.Id * (long)tiles[0, imageSize-1].Tile.Id * (long)tiles[imageSize-1, imageSize-1].Tile.Id;
        }

        static State[,] PrepareImage(TileContainer[,] tiles, int imageSize)
        {
            int tileSize = tiles[0,0].CurrentConfiguration.GetLength(0);
            State[,] image = new State[imageSize*tileSize - 2 * imageSize, imageSize*tileSize - 2* imageSize];
            for(int i = 0; i < imageSize; i++)
                for(int j = 0; j < imageSize; j++)
                    for(int ii = 1; ii < tileSize-1; ii++)
                        for(int jj = 1; jj < tileSize-1; jj++)
                            if(tiles[i,j].CurrentConfiguration[ii, jj])
                                image[i*(tileSize-2)+ii-1,j*(tileSize-2)+jj-1] = State.Checked; 
            return image; 
        }

        static State[,] GetConfiguration(State[,] image, int number)
        {
            int size = image.GetLength(0);
            State[,] tab = new State[size, size];
            switch(number)
            {
                case 0: //normal
                    for(int i = 0; i < size; i++)
                        for(int j = 0; j < size; j++)
                            tab[i,j] = image[i,j];
                    break;
                case 1: //rotated 90 deg counterclockwise TODO
                    for(int i = 0; i < size; i++)
                        for(int j = 0; j < size; j++)
                            tab[size-1-j, i] = image[i,j];
                    break;
                case 2: //rotated 180 deg counterclockwise TODO
                    for(int i = 0; i < size; i++)
                        for(int j = 0; j < size; j++)
                            tab[size-1-i, size-1-j] = image[i,j];
                    break;
                case 3: //rotated 270 deg counterclockwise TODO
                    for(int i = 0; i < size; i++)
                        for(int j = 0; j < size; j++)
                            tab[j, size-1-i] = image[i,j];
                    break;
                case 4: //flipped, no rotation TODO
                    for(int i = 0; i < size; i++)
                        for(int j = 0; j < size; j++)
                            tab[i,size-1-j] = image[i,j];
                    break;
                case 5: //flipped, rotated 90 deg counterclockwise TODO
                    for(int i = 0; i < size; i++)
                        for(int j = 0; j < size; j++)
                            tab[j,i] = image[i,j];
                    break;
                case 6: //flipped, rotated 180 deg counterclockwise TODO
                    for(int i = 0; i < size; i++)
                        for(int j = 0; j < size; j++)
                            tab[size-1-i,j] = image[i,j];
                    break;
                case 7: //filipped, rotated 270 deg counterclockwise TODO
                    for(int i = 0; i < size; i++)
                        for(int j = 0; j < size; j++)
                            tab[size-1-j,size-1-i] = image[i,j];
                    break;
            }
            return tab;
        }

        static bool[,] LoadMonster()
        {
            var monsterLines = File.ReadAllLines("monster.txt");
            var monster = new bool[monsterLines.Length, monsterLines[0].Length];
            for(int i = 0; i < monsterLines.Length; i++)
                for(int j = 0; j < monsterLines[i].Length; j++)
                    monster[i,j] = monsterLines[i][j] == '#';
            return monster;
        }

        static int CalculateWaterRoughtness(State[,] image)
        {
            int c = 0;
            for(int i = 0; i < image.GetLength(0); i++)
                for(int j = 0; j < image.GetLength(0); j++)
                    if(image[i,j] == State.Checked)
                        c++;
            return c;
        }

        static bool CheckMonster(State[,] image, bool[,] monster, int x, int y, bool mark = false)
        {
            for(int i = 0; i < monster.GetLength(0); i++)
                for(int j = 0; j < monster.GetLength(1); j++)
                    if(monster[i,j] && image[y+i, x+j] == State.None)
                        return false;
                    else if(monster[i,j] && image[y+i, x+j] != State.None && mark)
                        image[y+i, x+j] = State.Marked;
            return true;       
        }

        static int FindMonsters(State[,] imageConf, bool[,] monster)
        {
            for(int i = 0; i <= imageConf.GetLength(0) - monster.GetLength(0); i++)
                for(int j = 0; j <= imageConf.GetLength(1) - monster.GetLength(1); j++)
                {
                    if(CheckMonster(imageConf, monster, j, i, false))
                        CheckMonster(imageConf, monster, j, i, true);
                }
            return CalculateWaterRoughtness(imageConf);
        }

        static long Part2(List<Tile> allTiles)
        {
            int imageSize = (int)Math.Sqrt(allTiles.Count);
            var visited = new bool[allTiles.Count];
            var tiles = new TileContainer[imageSize,imageSize];
            FindTileConfiguration(allTiles, visited, imageSize, 0, 0, tiles, 0);
            var image = PrepareImage(tiles, imageSize);
            var monster = LoadMonster();
            var minRoughtness = int.MaxValue;
            for(int c = 0; c < 8; c++)
            {
                var imageConf = GetConfiguration(image, c);
                var roughtness = FindMonsters(imageConf, monster);
                if(roughtness < minRoughtness)
                    minRoughtness = roughtness;
            }
            return minRoughtness;
        }
    }
}
