using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapGenerator
{
    public enum Direction
    {
        error = 0,
        NORTH = 1,
        EAST = 2,
        SOUTH = 4,
        WEST = 8,
    }

    class Level
    {
        public Bitmap tex;
        private int[,] _map;
        private Vector2i _startPoint;
        private int _mazeRows;
        public int MazeRows
        {
            get { return _mazeRows; }
            set { _mazeRows = value; }
        }
        private int _mazeColumns;
        public int MazeColumns
        {
            get { return _mazeColumns; }
            set { _mazeColumns = value; }
        }
        private int _deathLimit;
        public int DeathLimit
        {
            get { return _deathLimit; }
            set { _deathLimit = value; }
        }
        private int _birthLimit;
        public int BirthLimit
        {
            get { return _birthLimit; }
            set { _birthLimit = value; }
        }
        private int _chanceToStartAlive;
        public int ChanceToStartAlive
        {
            get { return _chanceToStartAlive; }
            set { _chanceToStartAlive = value; }
        }
        //private int _diagonalChance;
        private int _pathWidth;
        public int PathWidth
        {
            get { return _pathWidth; }
            set { _pathWidth = value; }
        }
        private int _mapHeight;
        public int MapHeight
        {
            get { return _mapHeight; }
            set { _mapHeight = value; }
        }
        private int _mapWidth;
        public int MapWidth
        {
            get { return _mapWidth; }
            set { _mapWidth = value; }
        }
        private int _numberOfSteps;
        public int NumberOfSteps
        {
            get { return _numberOfSteps; }
            set { _numberOfSteps = value; }
        }
        private int _lineSize;
        public int LineSize
        {
            get { return _lineSize; }
            set { _lineSize = value; }
        }
        private int _pixelsPerMapUnit;
        public int PixelsPerMapUnit
        {
            get { return _pixelsPerMapUnit; }
            set { _pixelsPerMapUnit = value; }
        }
        private bool _drawSmooth;
        public bool DrawSmooth
        {
            get { return _drawSmooth; }
            set { _drawSmooth = value; }
        }
        private bool _drawGridLines;
        public bool DrawGridLines
        {
            get { return _drawGridLines; }
            set { _drawGridLines = value; }
        }
        private int _gridLineWidth;
        public int GridLineWidth
        {
            get { return _gridLineWidth; }
            set { _gridLineWidth = value; }
        }
        private int _gridLineHeight;
        public int GridLineHeight
        {
            get { return _gridLineHeight; }
            set { _gridLineHeight = value; }
        }

        private int _rngSeed;
        public int RngSeed
        {
            set { _rngSeed = value; }
            get { return _rngSeed; }
        }
        private System.Random _rng;

        private int hardPath = 2; // Value used in mazes to force connectivity along the path
        private int softPath = 1; // Value used in mazes to indicate area to be seeded for random generation
        
        public delegate bool LevelRegeneratedHandler();
        public event LevelRegeneratedHandler OnLevelRegenerated;

        public Level()
        {
            //RegenerateLevel();

            //Vector2 tileSize = wall.GetComponent<BoxCollider2D>().size;
            //for (int i = 0; i < map.GetLength(0); i++)
            //{
            //    for (int j = 0; j < map.GetLength(1); j++)
            //    {
            //        if (map[i, j] == 0 && CountNeighborsNotOfValue(map, i, j, 0) > 0)
            //        {
            //            if (!IndexOnEdge(map, i, j) && map[i + 1, j] == 0 && map[i, j - 1] == 0 && map[i - 1, j] != 0 && map[i, j + 1] != 0)
            //                Instantiate(corner, new Vector3(i * tileSize.x, j * tileSize.y), Quaternion.Euler(0, 0, 0));
            //            else if (!IndexOnEdge(map, i, j) && map[i + 1, j] == 0 && map[i, j + 1] == 0 && map[i - 1, j] != 0 && map[i, j - 1] != 0)
            //                Instantiate(corner, new Vector3(i * tileSize.x, j * tileSize.y), Quaternion.Euler(0, 0, 90));
            //            else if (!IndexOnEdge(map, i, j) && map[i - 1, j] == 0 && map[i, j + 1] == 0 && map[i + 1, j] != 0 && map[i, j - 1] != 0)
            //                Instantiate(corner, new Vector3(i * tileSize.x, j * tileSize.y), Quaternion.Euler(0, 0, 180));
            //            else if (!IndexOnEdge(map, i, j) && map[i - 1, j] == 0 && map[i, j - 1] == 0 && map[i + 1, j] != 0 && map[i, j + 1] != 0)
            //                Instantiate(corner, new Vector3(i * tileSize.x, j * tileSize.y), Quaternion.Euler(0, 0, 270));
            //            else if (!IndexOnEdge(map, i, j) && map[i, j - 1] == 0 && map[i + 1, j] != 0 && map[i - 1, j] != 0 && map[i, j + 1] != 0)
            //                Instantiate(smallCorner, new Vector3(i * tileSize.x, j * tileSize.y), Quaternion.Euler(0, 0, 0));
            //            else if (!IndexOnEdge(map, i, j) && map[i + 1, j] == 0 && map[i - 1, j] != 0 && map[i, j + 1] != 0 && map[i, j - 1] != 0)
            //                Instantiate(smallCorner, new Vector3(i * tileSize.x, j * tileSize.y), Quaternion.Euler(0, 0, 90));
            //            else if (!IndexOnEdge(map, i, j) && map[i, j + 1] == 0 && map[i + 1, j] != 0 && map[i - 1, j] != 0 && map[i, j - 1] != 0)
            //                Instantiate(smallCorner, new Vector3(i * tileSize.x, j * tileSize.y), Quaternion.Euler(0, 0, 180));
            //            else if (!IndexOnEdge(map, i, j) && map[i - 1, j] == 0 && map[i + 1, j] != 0 && map[i, j + 1] != 0 && map[i, j - 1] != 0)
            //                Instantiate(smallCorner, new Vector3(i * tileSize.x, j * tileSize.y), Quaternion.Euler(0, 0, 270));
            //            else
            //                Instantiate(wall, new Vector3(i * tileSize.x, j * tileSize.y), Quaternion.identity);
            //        }
            //        else if (map[i, j] == 0)
            //            Instantiate(noColliderWall, new Vector3(i * tileSize.x, j * tileSize.y), Quaternion.identity);
            //    }
            //}
        }

        public async void RegenerateLevel()
        {
            _rng = new System.Random(_rngSeed);
            await Task.Run(() =>
            {
                
                    GenerateRandomWalkLevel();
                //try
                {
                    if (DrawSmooth)
                    {
                        WriteToTexture(_map, _pixelsPerMapUnit, _pixelsPerMapUnit, true);
                    }
                    else
                    {
                        //WriteToTexture(_map, _pixelsPerMapUnit, _pixelsPerMapUnit, false);
                    }
                }
                //catch (Exception ex)
                //{
                //    Console.WriteLine(ex.ToString());
                //    tex = new Bitmap(600, 600);
                //    Graphics g = Graphics.FromImage(tex);
                //    g.FillRectangle(System.Drawing.Brushes.White, 0, 0, 600, 600);
                //    g.DrawImage(SystemIcons.Error.ToBitmap(), 300, 300, SystemIcons.Error.ToBitmap().Width * 2, SystemIcons.Error.ToBitmap().Height * 2);
                //    Font stringFont = new Font(FontFamily.GenericMonospace, 36, FontStyle.Regular);
                //    //g.DrawString("Something has gone wrong", stringFont, Brushes.Black, MapWidth * _pixelsPerMapUnit / 2 - 340, MapHeight * _pixelsPerMapUnit / 2 - 75);
                //    g.DrawString("Invalid Parameters", stringFont, Brushes.Black, 45, 375);
                //}

                OnLevelRegenerated?.Invoke();
            });
        }

        public int[,] GenerateRandomWalkLevel()
        {
            _startPoint = GetRandomStartSide(_mazeRows, _mazeColumns);

            _map = GenerateRandomWalkMaze(ref _startPoint, _mazeRows, _mazeColumns, _mapWidth, _mapHeight);

            //startPoint.x = startPoint.x * map.GetLength(0) / mazeRows + map.GetLength(0) / (2 * mazeRows);
            //startPoint.y = startPoint.y * map.GetLength(1) / mazeColumns + map.GetLength(1) / (2 * mazeColumns);

            //WriteToTexture(map, 4, 4);

            InitializeMap(_map);

            //WriteToTexture(map, 4, 4);

            for (int i = 0; i < _numberOfSteps; i++)
                _map = DoSimulationStep(_map);

            _map = CleanupMap(_map, _startPoint);

            //WriteToTexture(map, 4, 4);
            //spriteRenderer.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));

            return _map;
        }

        /// <summary>
        /// Initializes the entire map with random alive or dead cells
        /// </summary>
        /// <param name="mapWidth">Map width in cells</param>
        /// <param name="mapHeight">Map height in cells</param>
        /// <param name="chanceToStartAlive">Chance that each cell starts alive from 0 to 99</param>
        private void InitializeMap(int mapWidth, int mapHeight, int chanceToStartAlive)
        {
            _map = new int[mapWidth, mapHeight];

            for (int i = 0; i < mapWidth; i++)
            {
                for (int j = 0; j < mapHeight; j++)
                {
                    //if (UnityEngine.Random.Range(0, 100) < chanceToStartAlive)
                    if (_rng.Next(0, 100) < chanceToStartAlive)
                    {
                        _map[i, j] = 1;
                    }
                }
            }
        }

        /// <summary>
        /// Initializes the map along a path with random alive or dead cells
        /// </summary>
        /// <param name="startingPath"></param>
        private void InitializeMap(int[,] startingPath)
        {
            _map = new int[startingPath.GetLength(0), startingPath.GetLength(1)];

            // Mark each cell that is within one pathwidth of the initial path
            for (int i = 0; i < startingPath.GetLength(0); i++)
            {
                for (int j = 0; j < startingPath.GetLength(1); j++)
                {
                    // Check if the current point is on the existing path, then mark all points in the radius
                    if (startingPath[i, j] >= hardPath)
                    {
                        for (int k = 0; k < _pathWidth; k++)
                        {
                            for (int k2 = 0; k2 < (_pathWidth - k); k2++)
                            {
                                // Skip the starting point since we know it is a hard path
                                if (k == 0 && k2 == 0)
                                    continue;

                                if (i + k < startingPath.GetLength(0) && j + k2 < startingPath.GetLength(1) && startingPath[i + k, j + k2] < hardPath)
                                    startingPath[i + k, j + k2] = softPath;
                                if (i - k > 0 && j + k2 < startingPath.GetLength(1) && startingPath[i - k, j + k2] < hardPath)
                                    startingPath[i - k, j + k2] = softPath;
                                if (i + k < startingPath.GetLength(0) && j - k2 > 0 && startingPath[i + k, j - k2] < hardPath)
                                    startingPath[i + k, j - k2] = softPath;
                                if (i - k > 0 && j - k2 > 0 && startingPath[i - k, j - k2] < hardPath)
                                    startingPath[i - k, j - k2] = softPath;
                            }
                        }
                    }
                }
            }

            // Seed the map with random living cells in areas determined by the maze
            for (int i = 0; i < _map.GetLength(0); i++)
            {
                for (int j = 0; j < _map.GetLength(1); j++)
                {
                    if (startingPath[i, j] >= hardPath)
                        _map[i, j] = startingPath[i, j];
                    //else if (startingPath[i, j] == softPath && UnityEngine.Random.Range(0, 100) < chanceToStartAlive)
                    else if (startingPath[i, j] == softPath && _rng.Next(0, 100) < _chanceToStartAlive)
                        _map[i, j] = 1;
                }
            }
        }

        private int CountAliveNeighbors(int[,] map, int x, int y)
        {
            int count = 0;

            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    int checkX = x + i;
                    int checkY = y + j;
                    if (i == 0 && j == 0)
                    {
                        continue;
                    }
                    else if (checkX < 0 || checkX >= map.GetLength(0) || checkY < 0 || checkY >= map.GetLength(1))
                    {
                        //Count the borders as live neighbors
                        //count++;
                        //Count the borders as dead neighbors
                        continue;
                    }
                    else
                    {
                        count += map[checkX, checkY];
                    }
                }
            }

            return count;
        }

        private int CountNeighborsNotOfValue(int[,] map, int x, int y, int countIfNot)
        {
            int count = 0;

            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    int checkX = x + i;
                    int checkY = y + j;
                    if (i == 0 && j == 0)
                    {
                        continue;
                    }
                    else if (checkX < 0 || checkX >= map.GetLength(0) || checkY < 0 || checkY >= map.GetLength(1))
                    {
                        //Count the borders as live neighbors
                        //count++;
                        //Count the borders as dead neighbors
                        continue;
                    }
                    else if (map[checkX, checkY] != countIfNot)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private int CountAdjacentNeighborsNotOfValue(int[,] map, int x, int y, int countIfNot)
        {
            int count = 0;
            
            if (x - 1 >= 0 && map[x - 1, y] != countIfNot)
            {
                count++;
            }
            if (x + 1 < map.GetLength(0) && map[x + 1, y] != countIfNot)
            {
                count++;
            }
            if (y - 1 >= 0 && map[x, y - 1] != countIfNot)
            {
                count++;
            }
            if (y + 1 < map.GetLength(1) && map[x, y + 1] != countIfNot)
            {
                count++;
            }

            return count;
        }

        private int CountNeighborsInSet(Hashtable set, int[,] map, int x, int y)
        {
            int count = 0;

            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    int checkX = x + i;
                    int checkY = y + j;
                    if (i == 0 && j == 0)
                    {
                        continue;
                    }
                    else if (checkX < 0 || checkX >= map.GetLength(0) || checkY < 0 || checkY >= map.GetLength(1))
                    {
                        //Count the borders as live neighbors
                        //count++;
                        //Count the borders as dead neighbors
                        continue;
                    }
                    else if (set.Contains(UniquePointHashCode(new Point(checkX, checkY), map)))
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private bool IndexOnEdge(int[,] map, int xIndex, int yIndex)
        {
            return xIndex == map.GetLength(0) - 1 || xIndex == 0 || yIndex == map.GetLength(1) - 1 || yIndex == 0;
        }

        private int GetAdjacentWalls(int[,] map, int xIndex, int yIndex, int wallNumber)
        {
            int result = 0;
            if (xIndex == map.GetLength(0) - 1 || map[xIndex + 1, yIndex] == wallNumber)
            {
                result += (int)Direction.EAST;
            }
            if (xIndex == 0 || map[xIndex - 1, yIndex] == wallNumber)
            {
                result += (int)Direction.WEST;
            }
            if (yIndex == map.GetLength(1) - 1 || map[xIndex, yIndex + 1] == wallNumber)
            {
                result += (int)Direction.NORTH;
            }
            if (yIndex == 0 || map[xIndex, yIndex - 1] == wallNumber)
            {
                result += (int)Direction.SOUTH;
            }

            return result;
        }

        /// <summary>
        /// Perform a Conway's Game of Life simulation step (without overpopulation) 
        /// </summary>
        /// <param name="oldMap"></param>
        /// <returns></returns>
        private int[,] DoSimulationStep(int[,] oldMap)
        {
            int[,] newMap = new int[oldMap.GetLength(0), oldMap.GetLength(1)];

            for (int i = 0; i < oldMap.GetLength(0); i++)
            {
                for (int j = 0; j < oldMap.GetLength(1); j++)
                {
                    int neighbors = CountAliveNeighbors(oldMap, i, j);

                    if (oldMap[i, j] > 0)
                    {
                        if (neighbors < _deathLimit)
                            newMap[i, j] = 0;
                        else
                            newMap[i, j] = 1;
                    }
                    else
                    {
                        if (neighbors > _birthLimit)
                            newMap[i, j] = 1;
                        else
                            newMap[i, j] = 0;
                    }
                }
            }

            return newMap;
        }

        /// <summary>
        /// Perform a flood fill on the map to remove isolated cells
        /// </summary>
        /// <param name="oldMap"></param>
        /// <param name="startPoint"></param>
        /// <returns></returns>
        private int[,] CleanupMap(int[,] oldMap, Vector2i startPoint)
        {
            int[,] newMap = new int[oldMap.GetLength(0), oldMap.GetLength(1)];

            List<Vector2i> pointsToCheck = new List<Vector2i>
            {
                startPoint
            };
            newMap[startPoint.x, startPoint.y] = oldMap[startPoint.x, startPoint.y];
            //newMap[startPoint.x, startPoint.y] = 2;

            while (pointsToCheck.Count > 0)
            {
                Vector2i currentPoint = pointsToCheck[pointsToCheck.Count - 1];

                pointsToCheck.RemoveAt(pointsToCheck.Count - 1);

                if (currentPoint.y + 1 < oldMap.GetLength(1) && oldMap[currentPoint.x, currentPoint.y + 1] > 0 && newMap[currentPoint.x, currentPoint.y + 1] == 0) // Check the north point
                {
                    pointsToCheck.Add(new Vector2i(currentPoint.x, currentPoint.y + 1));
                    newMap[currentPoint.x, currentPoint.y + 1] = oldMap[currentPoint.x, currentPoint.y + 1];
                }
                if (currentPoint.x + 1 < oldMap.GetLength(0) && oldMap[currentPoint.x + 1, currentPoint.y] > 0 && newMap[currentPoint.x + 1, currentPoint.y] == 0) // Check the east point
                {
                    pointsToCheck.Add(new Vector2i(currentPoint.x + 1, currentPoint.y));
                    newMap[currentPoint.x + 1, currentPoint.y] = oldMap[currentPoint.x + 1, currentPoint.y];
                }
                if (currentPoint.y - 1 >= 0 && oldMap[currentPoint.x, currentPoint.y - 1] > 0 && newMap[currentPoint.x, currentPoint.y - 1] == 0) // Check the south point
                {
                    pointsToCheck.Add(new Vector2i(currentPoint.x, currentPoint.y - 1));
                    newMap[currentPoint.x, currentPoint.y - 1] = oldMap[currentPoint.x, currentPoint.y - 1];
                }
                if (currentPoint.x - 1 >= 0 && oldMap[currentPoint.x - 1, currentPoint.y] > 0 && newMap[currentPoint.x - 1, currentPoint.y] == 0) // Check the west point
                {
                    pointsToCheck.Add(new Vector2i(currentPoint.x - 1, currentPoint.y));
                    newMap[currentPoint.x - 1, currentPoint.y] = oldMap[currentPoint.x - 1, currentPoint.y];
                }
            }

            Point startPointMazeCell = new Point(startPoint.x * MazeColumns / MapWidth, startPoint.y * MazeRows / MapHeight);
            for (int i = 0; i < newMap.GetLength(0); i++)
            {
                bool onMazeX = (int)(i * MazeColumns / MapWidth) == startPointMazeCell.X;
                
                if (newMap[i, 0] != 0 && !(startPointMazeCell.Y == 0 && onMazeX))
                    newMap[i, 0] = 0;
                if (newMap[i, newMap.GetLength(1) - 1] != 0 && !(startPointMazeCell.Y == MazeRows - 1 && onMazeX))
                    newMap[i, newMap.GetLength(1) - 1] = 0;
            }
            for (int i = 0; i < newMap.GetLength(1); i++)
            {
                bool onMazeY = (int)(i * MazeRows / MapWidth) == startPointMazeCell.Y;

                if (newMap[0, i] != 0 && !(startPointMazeCell.X == 0 && onMazeY))
                    newMap[0, i] = 0;
                if (newMap[newMap.GetLength(0) - 1, i] != 0 && !(startPointMazeCell.X == MazeColumns - 1 && onMazeY))
                    newMap[newMap.GetLength(0) - 1, i] = 0;
            }

            return newMap;
        }

        private void DebugWriteToTexture(int [,] map, int horizontalScale, int verticalScale)
        {
            tex = new Bitmap(map.GetLength(0) * horizontalScale, map.GetLength(1) * verticalScale);
            Graphics g = Graphics.FromImage(tex);
            g.FillRectangle(System.Drawing.Brushes.White, 0, 0, map.GetLength(0) * horizontalScale, map.GetLength(1) * verticalScale);

            Pen linePen = new Pen(Color.Black, LineSize);
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (map[i, j] != 0)
                    {
                        g.FillRectangle(Brushes.Black, i * horizontalScale, j * verticalScale, horizontalScale, verticalScale);
                    }
                }
            }

            g.Dispose();
        }

        private void WriteToTextureBad(int[,] map, int horizontalScale, int verticalScale)
        {
            tex = new Bitmap(map.GetLength(0) * horizontalScale, map.GetLength(1) * verticalScale);
            Graphics g = Graphics.FromImage(tex);
            g.FillRectangle(System.Drawing.Brushes.White, 0, 0, map.GetLength(0) * horizontalScale, map.GetLength(1) * verticalScale);
            
            Pen linePen = new Pen(Color.Black, LineSize);
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (map[i, j] == 0 && CountNeighborsNotOfValue(map, i, j, 0) > 0)
                    {
                        bool onEdge = IndexOnEdge(map, i, j);
                        int wallDirections = GetAdjacentWalls(map, i, j, 0);
                        int halfHorizontal = (int)(0.5f * horizontalScale);
                        int halfVertical = (int)(0.5f * verticalScale);
                        int x = i * horizontalScale;
                        int y = j * verticalScale;

                        bool northWall = (wallDirections & (int)Direction.NORTH) > 0;
                        bool eastWall = (wallDirections & (int)Direction.EAST) > 0;
                        bool southWall = (wallDirections & (int)Direction.SOUTH) > 0;
                        bool westWall = (wallDirections & (int)Direction.WEST) > 0;
                        
                        if (!northWall && eastWall && southWall && !westWall)
                        {
                            // Wall right and below, so make a corner
                            g.DrawLine(linePen, new Point(x, y), new Point(x + horizontalScale, y + verticalScale));
                            g.FillEllipse(Brushes.Black, x - LineSize / 2.0f, y - LineSize / 2.0f, LineSize, LineSize);
                            g.FillEllipse(Brushes.Black, x + horizontalScale - LineSize / 2.0f, y + verticalScale - LineSize / 2.0f, LineSize, LineSize);
                        }
                        else if (northWall && eastWall && !southWall && !westWall)
                        {
                            // Wall right and above, so make a corner
                            g.DrawLine(linePen, new Point(x, y + verticalScale), new Point(x + horizontalScale, y));
                            g.FillEllipse(Brushes.Black, x - LineSize / 2.0f, y + verticalScale - LineSize / 2.0f, LineSize, LineSize);
                            g.FillEllipse(Brushes.Black, x + horizontalScale - LineSize / 2.0f, y - LineSize / 2.0f, LineSize, LineSize);
                        }
                        else if (northWall && !eastWall && !southWall && westWall)
                        {
                            // Wall left and above, so make a corner
                            g.DrawLine(linePen, new Point(x, y), new Point(x + horizontalScale, y + verticalScale));
                            g.FillEllipse(Brushes.Black, x - LineSize / 2.0f, y - LineSize / 2.0f, LineSize, LineSize);
                            g.FillEllipse(Brushes.Black, x + horizontalScale - LineSize / 2.0f, y + verticalScale - LineSize / 2.0f, LineSize, LineSize);
                        }
                        else if (!northWall && !eastWall && southWall && westWall)
                        {
                            // Wall left and below, so make a corner
                            g.DrawLine(linePen, new Point(x, y + verticalScale), new Point(x + horizontalScale, y));
                            g.FillEllipse(Brushes.Black, x - LineSize / 2.0f, y + verticalScale - LineSize / 2.0f, LineSize, LineSize);
                            g.FillEllipse(Brushes.Black, x + horizontalScale - LineSize / 2.0f, y - LineSize / 2.0f, LineSize, LineSize);
                        }
                        else if (!northWall && !eastWall && southWall && !westWall)
                        {
                            // Wall only below, make a small corner
                            g.DrawLine(linePen, new Point(x, y), new Point(x + halfHorizontal, y + halfVertical));
                            g.DrawLine(linePen, new Point(x + horizontalScale, y), new Point(x + halfHorizontal, y + halfVertical));
                            g.FillEllipse(Brushes.Black, x + halfHorizontal - LineSize / 2.0f, y + halfVertical - LineSize / 2.0f, LineSize, LineSize);
                        }
                        else if (!northWall && eastWall && !southWall && !westWall)
                        {
                            // Wall only right, make a small corner
                            g.DrawLine(linePen, new Point(x + horizontalScale, y), new Point(x + halfHorizontal, y + halfVertical));
                            g.DrawLine(linePen, new Point(x + horizontalScale, y + verticalScale), new Point(x + halfHorizontal, y + halfVertical));
                            g.FillEllipse(Brushes.Black, x + halfHorizontal - LineSize / 2.0f, y + halfVertical - LineSize / 2.0f, LineSize, LineSize);
                        }
                        else if (northWall && !eastWall && !southWall && !westWall)
                        {
                            // Wall only above, make a small corner
                            g.DrawLine(linePen, new Point(x, y + verticalScale), new Point(x + halfHorizontal, y + halfVertical));
                            g.DrawLine(linePen, new Point(x + horizontalScale, y + verticalScale), new Point(x + halfHorizontal, y + halfVertical));
                            g.FillEllipse(Brushes.Black, x + halfHorizontal - LineSize / 2.0f, y + halfVertical - LineSize / 2.0f, LineSize, LineSize);
                        }
                        else if (!northWall && !eastWall && !southWall && westWall)
                        {
                            // Wall only left, make a small corner
                            g.DrawLine(linePen, new Point(x, y), new Point(x + halfHorizontal, y + halfVertical));
                            g.DrawLine(linePen, new Point(x, y + verticalScale), new Point(x + halfHorizontal, y + halfVertical));
                            g.FillEllipse(Brushes.Black, x + halfHorizontal - LineSize / 2.0f, y + halfVertical - LineSize / 2.0f, LineSize, LineSize);
                        }
                        else if (!northWall && eastWall && southWall && westWall)
                        {
                            // No wall above, make a straight line
                            g.DrawLine(linePen, new Point(x, y + verticalScale), new Point(x + horizontalScale, y + verticalScale));
                            g.FillEllipse(Brushes.Black, x - LineSize / 2.0f, y + verticalScale - LineSize / 2.0f, LineSize, LineSize);
                            g.FillEllipse(Brushes.Black, x + horizontalScale - LineSize / 2.0f, y + verticalScale - LineSize / 2.0f, LineSize, LineSize);
                        }
                        else if (northWall && eastWall && !southWall && westWall)
                        {
                            // No wall below, make a straight line
                            g.DrawLine(linePen, new Point(x, y), new Point(x + horizontalScale, y));
                            g.FillEllipse(Brushes.Black, x - LineSize / 2.0f, y - LineSize / 2.0f, LineSize, LineSize);
                            g.FillEllipse(Brushes.Black, x + horizontalScale - LineSize / 2.0f, y - LineSize / 2.0f, LineSize, LineSize);
                        }
                        else if (northWall && eastWall && southWall && !westWall)
                        {
                            // No Wall left, make a straight line
                            g.DrawLine(linePen, new Point(x, y), new Point(x, y + verticalScale));
                            g.FillEllipse(Brushes.Black, x - LineSize / 2.0f, y - LineSize / 2.0f, LineSize, LineSize);
                            g.FillEllipse(Brushes.Black, x - LineSize / 2.0f, y + verticalScale - LineSize / 2.0f, LineSize, LineSize);
                        }
                        else if (northWall && !eastWall && southWall && westWall)
                        {
                            // No wall right, make a straight line
                            g.DrawLine(linePen, new Point(x + horizontalScale, y), new Point(x + horizontalScale, y + verticalScale));
                            g.FillEllipse(Brushes.Black, x + horizontalScale - LineSize / 2.0f, y - LineSize / 2.0f, LineSize, LineSize);
                            g.FillEllipse(Brushes.Black, x + horizontalScale - LineSize / 2.0f, y + verticalScale - LineSize / 2.0f, LineSize, LineSize);
                        }
                        else if (northWall && !eastWall && southWall && !westWall)
                        {
                            // Walls only above and below, make a straight line right and left
                            g.DrawLine(linePen, new Point(x, y), new Point(x, y + verticalScale));
                            g.DrawLine(linePen, new Point(x + horizontalScale, y), new Point(x + horizontalScale, y + verticalScale));
                            g.FillEllipse(Brushes.Black, x - LineSize / 2.0f, y - LineSize / 2.0f, LineSize, LineSize);
                            g.FillEllipse(Brushes.Black, x - LineSize / 2.0f, y + verticalScale - LineSize / 2.0f, LineSize, LineSize);
                            g.FillEllipse(Brushes.Black, x + horizontalScale - LineSize / 2.0f, y - LineSize / 2.0f, LineSize, LineSize);
                            g.FillEllipse(Brushes.Black, x + horizontalScale - LineSize / 2.0f, y + verticalScale - LineSize / 2.0f, LineSize, LineSize);
                        }
                        else if (!northWall && eastWall && !southWall && westWall)
                        {
                            // Walls only right and left, make a straight line above and below
                            g.DrawLine(linePen, new Point(x, y), new Point(x + horizontalScale, y));
                            g.DrawLine(linePen, new Point(x, y + verticalScale), new Point(x + horizontalScale, y + verticalScale));
                            g.FillEllipse(Brushes.Black, x - LineSize / 2.0f, y - LineSize / 2.0f, LineSize, LineSize);
                            g.FillEllipse(Brushes.Black, x - LineSize / 2.0f, y + verticalScale - LineSize / 2.0f, LineSize, LineSize);
                            g.FillEllipse(Brushes.Black, x + horizontalScale - LineSize / 2.0f, y - LineSize / 2.0f, LineSize, LineSize);
                            g.FillEllipse(Brushes.Black, x + horizontalScale - LineSize / 2.0f, y + verticalScale - LineSize / 2.0f, LineSize, LineSize);
                        }
                    }
                }
            }
            
            g.Dispose();
        }

        private void WriteToTexture(int[,] map, int horizontalScale, int verticalScale, bool drawSmooth)
        {
            tex = new Bitmap(map.GetLength(0) * horizontalScale, map.GetLength(1) * verticalScale);
            Graphics g = Graphics.FromImage(tex);
            g.FillRectangle(System.Drawing.Brushes.White, 0, 0, map.GetLength(0) * horizontalScale, map.GetLength(1) * verticalScale);

            Pen linePen = new Pen(Color.Black, LineSize);

            Hashtable openSet = new Hashtable();
            List<Point> singleNeighbor = new List<Point>();

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    int aliveNeighbors = CountNeighborsNotOfValue(map, i, j, 0);
                    if (map[i, j] == 0 && aliveNeighbors > 0 && CountAdjacentNeighborsNotOfValue(map, i, j, 0) >= 1) // if the current point is a wall and there are non-wall points adajacent
                    {
                        Point currentPoint = new Point(i, j);
                        openSet.Add(UniquePointHashCode(currentPoint, map), currentPoint);
                        //g.DrawEllipse(linePen, i * horizontalScale, j * verticalScale, horizontalScale / 4, verticalScale / 4);
                        if (IndexOnEdge(map, i, j) && CountAdjacentNeighborsNotOfValue(map, i, j, 0) >= 1)
                        {
                            singleNeighbor.Add(currentPoint);
                            //g.DrawEllipse(linePen, i * horizontalScale, j * verticalScale, horizontalScale * 4, verticalScale * 4);
                        }
                    }
                }
            }

            List<Point> connectedPoints = new List<Point>();

            while (singleNeighbor.Count > 0)
            {
                Point currentPoint = singleNeighbor[singleNeighbor.Count - 1];
                Dictionary<int, Point> doubleSidedPoints = new Dictionary<int, Point>();
                if (!openSet.Contains(UniquePointHashCode(currentPoint, map)))
                {
                    singleNeighbor.Remove(currentPoint);
                    continue;
                }
                Point lastVertex = GetSingleNeighborStartPoint(currentPoint, map, horizontalScale, verticalScale);
                //lastVertex = new Point(currentPoint.X, currentPoint.Y);
                //if (currentPoint.X < 1)
                //{
                //    lastVertex = new Point(0, currentPoint.Y * verticalScale);
                //}
                //else if (currentPoint.X > map.GetLength(0) - 2)
                //{
                //    lastVertex = new Point(map.GetLength(0) * horizontalScale, currentPoint.Y * verticalScale);
                //}
                //else if (currentPoint.Y < 1)
                //{
                //    lastVertex = new Point(currentPoint.X * horizontalScale, 0);
                //}
                //else
                //{
                //    lastVertex = new Point(currentPoint.X * horizontalScale, map.GetLength(1) * verticalScale);
                //}
                connectedPoints.Add(lastVertex);
                connectedPoints.AddRange(GetPoints(currentPoint, lastVertex, map, horizontalScale, verticalScale));
                lastVertex = connectedPoints[connectedPoints.Count - 1];

                singleNeighbor.Remove(currentPoint);
                if (!IsDoubleSidedPoint(currentPoint, map))
                {
                    openSet.Remove(UniquePointHashCode(currentPoint, map));
                }
                else if (doubleSidedPoints.ContainsKey(UniquePointHashCode(currentPoint, map)))
                {
                    openSet.Remove(UniquePointHashCode(currentPoint, map));
                    doubleSidedPoints.Remove(UniquePointHashCode(currentPoint, map));
                }
                else
                {
                    doubleSidedPoints.Add(UniquePointHashCode(currentPoint, map), currentPoint);
                }

                while (FindNextPointInSet(currentPoint, out Point nextPoint, lastVertex, openSet, map, horizontalScale, verticalScale))
                {
                    currentPoint = nextPoint;
                    connectedPoints.AddRange(GetPoints(currentPoint, lastVertex, map, horizontalScale, verticalScale));

                    lastVertex = connectedPoints[connectedPoints.Count - 1];

                    if (singleNeighbor.Contains(currentPoint))
                        singleNeighbor.Remove(currentPoint);

                    if (!IsDoubleSidedPoint(currentPoint, map))
                    {
                        openSet.Remove(UniquePointHashCode(currentPoint, map));
                    }
                    else if (doubleSidedPoints.ContainsKey(UniquePointHashCode(currentPoint, map)))
                    {
                        openSet.Remove(UniquePointHashCode(currentPoint, map));
                        doubleSidedPoints.Remove(UniquePointHashCode(currentPoint, map));
                    }
                    else
                    {
                        doubleSidedPoints.Add(UniquePointHashCode(currentPoint, map), currentPoint);
                    }
                }
                
                if (drawSmooth)
                {
                    while (connectedPoints.Count % 3 != 1)
                        connectedPoints.Add(connectedPoints[connectedPoints.Count - 1]);

                    g.DrawBeziers(linePen, connectedPoints.ToArray());
                }
                else
                {
                    if (connectedPoints.Count % 2 > 0)
                        connectedPoints.Add(connectedPoints[connectedPoints.Count - 1]);

                    g.DrawLines(linePen, connectedPoints.ToArray());
                }

                connectedPoints.Clear();
            }

            while (openSet.Count > 0)
            //while (false)
            {
                Dictionary<int, Point> doubleSidedPoints = new Dictionary<int, Point>();
                Point currentPoint = new Point();
                Point nextPoint = new Point();
                Point lastVertex = new Point();
                Point[] allPoints = openSet.Values.OfType<Point>().ToArray<Point>();
                bool foundStart = false;
                int count = 0;
                while (!foundStart && count < allPoints.Count())
                {
                    currentPoint = allPoints[allPoints.Count() - count - 1];

                    FindNextPointInSet(currentPoint, out nextPoint, lastVertex, openSet, map, horizontalScale, verticalScale);

                    foundStart = GetStartPoint(currentPoint, nextPoint, out lastVertex, map, horizontalScale, verticalScale);
                    count++;
                }

                if (!foundStart)
                {
                    break;
                }
                
                connectedPoints.AddRange(GetPoints(currentPoint, lastVertex, map, horizontalScale, verticalScale));
                lastVertex = connectedPoints[connectedPoints.Count - 1];
                if (!IsDoubleSidedPoint(currentPoint, map))
                {
                    openSet.Remove(UniquePointHashCode(currentPoint, map));
                }
                else if (doubleSidedPoints.ContainsKey(UniquePointHashCode(currentPoint, map)))
                {
                    openSet.Remove(UniquePointHashCode(currentPoint, map));
                    doubleSidedPoints.Remove(UniquePointHashCode(currentPoint, map));
                }
                else
                {
                    doubleSidedPoints.Add(UniquePointHashCode(currentPoint, map), currentPoint);
                }
                
                while (FindNextPointInSet(currentPoint, out nextPoint, lastVertex, openSet, map, horizontalScale, verticalScale))
                {
                    currentPoint = nextPoint;
                    connectedPoints.AddRange(GetPoints(currentPoint, lastVertex, map, horizontalScale, verticalScale));
                    lastVertex = connectedPoints[connectedPoints.Count - 1];

                    if (!IsDoubleSidedPoint(currentPoint, map))
                    {
                        openSet.Remove(UniquePointHashCode(currentPoint, map));
                    }
                    else if (doubleSidedPoints.ContainsKey(UniquePointHashCode(currentPoint, map)))
                    {
                        openSet.Remove(UniquePointHashCode(currentPoint, map));
                        doubleSidedPoints.Remove(UniquePointHashCode(currentPoint, map));
                    }
                    else
                    {
                        doubleSidedPoints.Add(UniquePointHashCode(currentPoint, map), currentPoint);
                    }
                }

                connectedPoints.Add(new Point(connectedPoints[0].X, connectedPoints[0].Y));

                if (drawSmooth)
                {
                    while (connectedPoints.Count % 3 != 1)
                        connectedPoints.Add(connectedPoints[connectedPoints.Count - 1]);

                    if (connectedPoints.Count > 4)
                        g.DrawBeziers(linePen, connectedPoints.ToArray());
                }
                else
                {
                    if (connectedPoints.Count % 2 > 0)
                        connectedPoints.Add(connectedPoints[connectedPoints.Count - 1]);

                    g.DrawLines(linePen, connectedPoints.ToArray());
                }
                
                connectedPoints.Clear();
            }
            
            if (DrawGridLines && MapWidth > GridLineWidth && MapHeight > GridLineHeight)
            {
                Pen gridPen = new Pen(Color.Black, linePen.Width / 2);
                int[,] aliveGridCells = new int[Convert.ToInt32(1.0f * MapWidth / GridLineWidth), Convert.ToInt32(1.0f * MapHeight / GridLineHeight)];
                for (int i = 0; i < map.GetLength(0); i++)
                {
                    for (int j = 0; j < map.GetLength(1); j++)
                    {
                        if (map[i,j] == 1)
                        {
                            aliveGridCells[i / GridLineWidth, j / GridLineHeight] = 1;
                        }
                    }
                }
                int gridWidthScaled = GridLineWidth * horizontalScale;
                int gridHeightScaled = GridLineHeight * verticalScale;
                for (int i = 0; i < aliveGridCells.GetLength(0); i++)
                {
                    for (int j = 0; j < aliveGridCells.GetLength(1); j++)
                    {
                        if (i * gridWidthScaled + gridWidthScaled == MapWidth * horizontalScale)
                        {
                            g.DrawLine(gridPen, i * gridWidthScaled + gridWidthScaled - 1, j * gridHeightScaled, i * gridWidthScaled + gridWidthScaled - 1, j * gridHeightScaled + gridHeightScaled);
                        }
                        else
                        {
                            g.DrawLine(gridPen, i * gridWidthScaled + gridWidthScaled, j * gridHeightScaled, i * gridWidthScaled + gridWidthScaled, j * gridHeightScaled + gridHeightScaled);
                        }
                        if (j * gridHeightScaled + gridHeightScaled == MapHeight * verticalScale)
                        {
                            g.DrawLine(gridPen, i * gridWidthScaled, j * gridHeightScaled + gridHeightScaled - 1, i * gridWidthScaled + gridWidthScaled, j * gridHeightScaled + gridHeightScaled - 1);
                        }
                        else
                        {
                            g.DrawLine(gridPen, i * gridWidthScaled, j * gridHeightScaled + gridHeightScaled, i * gridWidthScaled + gridWidthScaled, j * gridHeightScaled + gridHeightScaled);
                        }
                        g.DrawLine(gridPen, i * gridWidthScaled, j * gridHeightScaled, i * gridWidthScaled + gridWidthScaled, j * gridHeightScaled);
                        g.DrawLine(gridPen, i * gridWidthScaled, j * gridHeightScaled, i * gridWidthScaled, j * gridHeightScaled + gridHeightScaled);
                    }
                }
            }

            g.Dispose();
        }

        private bool FindAdjacentPointInSet(Point currentPoint, out Point adjacentPoint, Hashtable openSet, int[,] map)
        {
            Point testAdjacentPoint = new Point(currentPoint.X + 1, currentPoint.Y);
            Point smallestPoint = new Point();
            Point smallestNonZeroPoint = new Point();
            int smallestNeighbors = 10;
            bool foundPoint = false;
            bool foundNonZero = false;
            int neighborCount = CountNeighborsInSet(openSet, map, testAdjacentPoint.X, testAdjacentPoint.Y);
            if (openSet.Contains(UniquePointHashCode(testAdjacentPoint, map)))
            {
                if (neighborCount == 0)
                {
                    smallestPoint = testAdjacentPoint;
                }
                else if (neighborCount < smallestNeighbors)
                {
                    smallestNonZeroPoint = testAdjacentPoint;
                    smallestNeighbors = neighborCount;
                    foundNonZero = true;
                }
                foundPoint = true;
                //return true;
            }
            testAdjacentPoint = new Point(currentPoint.X - 1, currentPoint.Y);
            neighborCount = CountNeighborsInSet(openSet, map, testAdjacentPoint.X, testAdjacentPoint.Y);
            if (openSet.Contains(UniquePointHashCode(testAdjacentPoint, map)))
            {
                if (neighborCount == 0)
                {
                    smallestPoint = testAdjacentPoint;
                }
                else if (neighborCount < smallestNeighbors)
                {
                    smallestNonZeroPoint = testAdjacentPoint;
                    smallestNeighbors = neighborCount;
                    foundNonZero = true;
                }
                foundPoint = true;
                //return true;
            }
            testAdjacentPoint = new Point(currentPoint.X, currentPoint.Y + 1);
            neighborCount = CountNeighborsInSet(openSet, map, testAdjacentPoint.X, testAdjacentPoint.Y);
            if (openSet.Contains(UniquePointHashCode(testAdjacentPoint, map)))
            {
                if (neighborCount == 0)
                {
                    smallestPoint = testAdjacentPoint;
                }
                else if (neighborCount < smallestNeighbors)
                {
                    smallestNonZeroPoint = testAdjacentPoint;
                    smallestNeighbors = neighborCount;
                    foundNonZero = true;
                }
                foundPoint = true;
                //return true;
            }
            testAdjacentPoint = new Point(currentPoint.X, currentPoint.Y - 1);
            neighborCount = CountNeighborsInSet(openSet, map, testAdjacentPoint.X, testAdjacentPoint.Y);
            if (openSet.Contains(UniquePointHashCode(testAdjacentPoint, map)))
            {
                if (neighborCount == 0)
                {
                    smallestPoint = testAdjacentPoint;
                }
                else if (neighborCount < smallestNeighbors)
                {
                    smallestNonZeroPoint = testAdjacentPoint;
                    smallestNeighbors = neighborCount;
                    foundNonZero = true;
                }
                foundPoint = true;
                //return true;
            }
            // Have to check diagonals if we haven't found a point yet
            testAdjacentPoint = new Point(currentPoint.X + 1, currentPoint.Y + 1);
            neighborCount = CountNeighborsInSet(openSet, map, testAdjacentPoint.X, testAdjacentPoint.Y);
            if (openSet.Contains(UniquePointHashCode(testAdjacentPoint, map)))
            {
                if (neighborCount == 0)
                {
                    smallestPoint = testAdjacentPoint;
                }
                else if (neighborCount < smallestNeighbors)
                {
                    smallestNonZeroPoint = testAdjacentPoint;
                    smallestNeighbors = neighborCount;
                    foundNonZero = true;
                }
                foundPoint = true;
                //return true;
            }
            testAdjacentPoint = new Point(currentPoint.X - 1, currentPoint.Y + 1);
            neighborCount = CountNeighborsInSet(openSet, map, testAdjacentPoint.X, testAdjacentPoint.Y);
            if (openSet.Contains(UniquePointHashCode(testAdjacentPoint, map)))
            {
                if (neighborCount == 0)
                {
                    smallestPoint = testAdjacentPoint;
                }
                else if (neighborCount < smallestNeighbors)
                {
                    smallestNonZeroPoint = testAdjacentPoint;
                    smallestNeighbors = neighborCount;
                    foundNonZero = true;
                }
                foundPoint = true;
                //return true;
            }
            testAdjacentPoint = new Point(currentPoint.X + 1, currentPoint.Y - 1);
            neighborCount = CountNeighborsInSet(openSet, map, testAdjacentPoint.X, testAdjacentPoint.Y);
            if (openSet.Contains(UniquePointHashCode(testAdjacentPoint, map)))
            {
                if (neighborCount == 0)
                {
                    smallestPoint = testAdjacentPoint;
                }
                else if (neighborCount < smallestNeighbors)
                {
                    smallestNonZeroPoint = testAdjacentPoint;
                    smallestNeighbors = neighborCount;
                    foundNonZero = true;
                }
                foundPoint = true;
                //return true;
            }
            testAdjacentPoint = new Point(currentPoint.X - 1, currentPoint.Y - 1);
            neighborCount = CountNeighborsInSet(openSet, map, testAdjacentPoint.X, testAdjacentPoint.Y);
            if (openSet.Contains(UniquePointHashCode(testAdjacentPoint, map)))
            {
                if (neighborCount == 0)
                {
                    smallestPoint = testAdjacentPoint;
                }
                else if (neighborCount < smallestNeighbors)
                {
                    smallestNonZeroPoint = testAdjacentPoint;
                    smallestNeighbors = neighborCount;
                    foundNonZero = true;
                }
                foundPoint = true;
                //return true;
            }

            adjacentPoint = testAdjacentPoint;
            if (foundNonZero)
                adjacentPoint = smallestNonZeroPoint;
            else
                adjacentPoint = smallestPoint;
            
            return foundPoint;
            //return false;
        }

        private bool FindNextPointInSet(Point currentPoint, out Point nextPoint, Point lastVertex, Hashtable openSet, int[,] map, int horizontalScale, int verticalScale)
        {
            int x = currentPoint.X * horizontalScale;
            int y = currentPoint.Y * verticalScale;

            int wallDirections = GetAdjacentWalls(map, currentPoint.X, currentPoint.Y, 0);
            
            bool northWall = (wallDirections & (int)Direction.NORTH) > 0;
            bool eastWall = (wallDirections & (int)Direction.EAST) > 0;
            bool southWall = (wallDirections & (int)Direction.SOUTH) > 0;
            bool westWall = (wallDirections & (int)Direction.WEST) > 0;
            
            if (!northWall && eastWall && southWall && !westWall)
            {
                // Wall right and below, so this point is a corner
                if (lastVertex.X == x + horizontalScale && lastVertex.Y == y + verticalScale) // Last vertex is the final vert of the current point
                {
                    if (currentPoint.X + 1 < map.GetLength(0) && currentPoint.Y + 1 < map.GetLength(1) && openSet.Contains(UniquePointHashCode(new Point(currentPoint.X + 1, currentPoint.Y + 1), map)))
                    {
                        nextPoint = new Point(currentPoint.X + 1, currentPoint.Y + 1);
                        return true;
                    }
                    else if (currentPoint.X + 1 < map.GetLength(0) && openSet.Contains(UniquePointHashCode(new Point(currentPoint.X + 1, currentPoint.Y), map)))
                    {
                        nextPoint = new Point(currentPoint.X + 1, currentPoint.Y);
                        return true;
                    }
                    else
                    {
                        nextPoint = new Point();
                        return false;
                    }
                }
                else
                {
                    if (currentPoint.X - 1 >= 0 && currentPoint.Y - 1 >= 0 && openSet.Contains(UniquePointHashCode(new Point(currentPoint.X - 1, currentPoint.Y - 1), map)))
                    {
                        nextPoint = new Point(currentPoint.X - 1, currentPoint.Y - 1);
                        return true;
                    }
                    else if (currentPoint.Y - 1 >= 0 && openSet.Contains(UniquePointHashCode(new Point(currentPoint.X, currentPoint.Y - 1), map)))
                    {
                        nextPoint = new Point(currentPoint.X, currentPoint.Y - 1);
                        return true;
                    }
                    else
                    {
                        nextPoint = new Point();
                        return false;
                    }
                }
            }
            else if (northWall && eastWall && !southWall && !westWall)
            {
                // Wall right and above, so this point is a corner
                if (lastVertex.X == x && lastVertex.Y == y + verticalScale) // Last vertex is the final vert of the current point
                {
                    if (currentPoint.X - 1 >= 0 && currentPoint.Y + 1 < map.GetLength(1) && openSet.Contains(UniquePointHashCode(new Point(currentPoint.X - 1, currentPoint.Y + 1), map)))
                    {
                        nextPoint = new Point(currentPoint.X - 1, currentPoint.Y + 1);
                        return true;
                    }
                    else if (currentPoint.Y + 1 < map.GetLength(1) && openSet.Contains(UniquePointHashCode(new Point(currentPoint.X, currentPoint.Y + 1), map)))
                    {
                        nextPoint = new Point(currentPoint.X, currentPoint.Y + 1);
                        return true;
                    }
                    else
                    {
                        nextPoint = new Point();
                        return false;
                    }
                }
                else
                {
                    if (currentPoint.X + 1 < map.GetLength(0) && currentPoint.Y - 1 >= 0 && openSet.Contains(UniquePointHashCode(new Point(currentPoint.X + 1, currentPoint.Y - 1), map)))
                    {
                        nextPoint = new Point(currentPoint.X + 1, currentPoint.Y - 1);
                        return true;
                    }
                    else if (currentPoint.X + 1 < map.GetLength(0) && openSet.Contains(UniquePointHashCode(new Point(currentPoint.X + 1, currentPoint.Y), map)))
                    {
                        nextPoint = new Point(currentPoint.X + 1, currentPoint.Y);
                        return true;
                    }
                    else
                    {
                        nextPoint = new Point();
                        return false;
                    }
                }
            }
            else if (northWall && !eastWall && !southWall && westWall)
            {
                // Wall left and above, so this point is a corner
                if (lastVertex.X == x + horizontalScale && lastVertex.Y == y + verticalScale) // Last vertex is the final vert of the current point
                {
                    if (currentPoint.X + 1 < map.GetLength(0) && currentPoint.Y + 1 < map.GetLength(1) && openSet.Contains(UniquePointHashCode(new Point(currentPoint.X + 1, currentPoint.Y + 1), map)))
                    {
                        nextPoint = new Point(currentPoint.X + 1, currentPoint.Y + 1);
                        return true;
                    }
                    else if (currentPoint.Y + 1 < map.GetLength(1) && openSet.Contains(UniquePointHashCode(new Point(currentPoint.X, currentPoint.Y + 1), map)))
                    {
                        nextPoint = new Point(currentPoint.X, currentPoint.Y + 1);
                        return true;
                    }
                    else
                    {
                        nextPoint = new Point();
                        return false;
                    }
                }
                else
                {
                    if (currentPoint.X - 1 >= 0 && currentPoint.Y - 1 >= 0 && openSet.Contains(UniquePointHashCode(new Point(currentPoint.X - 1, currentPoint.Y - 1), map)))
                    {
                        nextPoint = new Point(currentPoint.X - 1, currentPoint.Y - 1);
                        return true;
                    }
                    else if (currentPoint.X - 1 >= 0 && openSet.Contains(UniquePointHashCode(new Point(currentPoint.X - 1, currentPoint.Y), map)))
                    {
                        nextPoint = new Point(currentPoint.X - 1, currentPoint.Y);
                        return true;
                    }
                    else
                    {
                        nextPoint = new Point();
                        return false;
                    }
                }
            }
            else if (!northWall && !eastWall && southWall && westWall)
            {
                // Wall left and below, so this point is a corner
                if (lastVertex.X == x && lastVertex.Y == y + verticalScale) // Last vertex is the final vert of the current point
                {
                    if (currentPoint.X - 1 >= 0 && currentPoint.Y + 1 < map.GetLength(1) && openSet.Contains(UniquePointHashCode(new Point(currentPoint.X - 1, currentPoint.Y + 1), map)))
                    {
                        nextPoint = new Point(currentPoint.X - 1, currentPoint.Y + 1);
                        return true;
                    }
                    else if (currentPoint.X - 1 >= 0 && openSet.Contains(UniquePointHashCode(new Point(currentPoint.X - 1, currentPoint.Y), map)))
                    {
                        nextPoint = new Point(currentPoint.X - 1, currentPoint.Y);
                        return true;
                    }
                    else
                    {
                        nextPoint = new Point();
                        return false;
                    }
                }
                else
                {
                    if (currentPoint.X + 1 < map.GetLength(0) && currentPoint.Y - 1 >= 0 && openSet.Contains(UniquePointHashCode(new Point(currentPoint.X + 1, currentPoint.Y - 1), map)))
                    {
                        nextPoint = new Point(currentPoint.X + 1, currentPoint.Y - 1);
                        return true;
                    }
                    else if (currentPoint.Y - 1 >= 0 && openSet.Contains(UniquePointHashCode(new Point(currentPoint.X, currentPoint.Y - 1), map)))
                    {
                        nextPoint = new Point(currentPoint.X, currentPoint.Y - 1);
                        return true;
                    }
                    else
                    {
                        nextPoint = new Point();
                        return false;
                    }
                }
            }
            else if (!northWall && !eastWall && southWall && !westWall)
            {
                // Wall only below, so this point is a small corner
                if (lastVertex.X == x && lastVertex.Y == y) // Last vertex is the final vert of the current point
                {
                    if (currentPoint.X - 1 >= 0 && currentPoint.Y - 1 >= 0 && openSet.Contains(UniquePointHashCode(new Point(currentPoint.X - 1, currentPoint.Y - 1), map)))
                    {
                        nextPoint = new Point(currentPoint.X - 1, currentPoint.Y - 1);
                        return true;
                    }
                    else if (currentPoint.Y - 1 >= 0 && openSet.Contains(UniquePointHashCode(new Point(currentPoint.X, currentPoint.Y - 1), map)))
                    {
                        nextPoint = new Point(currentPoint.X, currentPoint.Y - 1);
                        return true;
                    }
                    else
                    {
                        nextPoint = new Point();
                        return false;
                    }
                }
                else
                {
                    if (currentPoint.X + 1 < map.GetLength(0) && currentPoint.Y - 1 >= 0 && openSet.Contains(UniquePointHashCode(new Point(currentPoint.X + 1, currentPoint.Y - 1), map)))
                    {
                        nextPoint = new Point(currentPoint.X + 1, currentPoint.Y - 1);
                        return true;
                    }
                    else if (currentPoint.Y - 1 >= 0 && openSet.Contains(UniquePointHashCode(new Point(currentPoint.X, currentPoint.Y - 1), map)))
                    {
                        nextPoint = new Point(currentPoint.X, currentPoint.Y - 1);
                        return true;
                    }
                    else
                    {
                        nextPoint = new Point();
                        return false;
                    }
                }
            }
            else if (!northWall && eastWall && !southWall && !westWall)
            {
                // Wall only right, so this point is a small corner
                if (lastVertex.X == x + horizontalScale && lastVertex.Y == y) // Last vertex is the final vert of the current point
                {
                    if (currentPoint.X + 1 < map.GetLength(0) && currentPoint.Y - 1 >= 0 && openSet.Contains(UniquePointHashCode(new Point(currentPoint.X + 1, currentPoint.Y - 1), map)))
                    {
                        nextPoint = new Point(currentPoint.X + 1, currentPoint.Y - 1);
                        return true;
                    }
                    else if (currentPoint.X + 1 < map.GetLength(0) && openSet.Contains(UniquePointHashCode(new Point(currentPoint.X + 1, currentPoint.Y), map)))
                    {
                        nextPoint = new Point(currentPoint.X + 1, currentPoint.Y);
                        return true;
                    }
                    else
                    {
                        nextPoint = new Point();
                        return false;
                    }
                }
                else
                {
                    if (currentPoint.X + 1 < map.GetLength(0) && currentPoint.Y + 1 < map.GetLength(1) && openSet.Contains(UniquePointHashCode(new Point(currentPoint.X + 1, currentPoint.Y + 1), map)))
                    {
                        nextPoint = new Point(currentPoint.X + 1, currentPoint.Y + 1);
                        return true;
                    }
                    else if (currentPoint.X + 1 < map.GetLength(0) && openSet.Contains(UniquePointHashCode(new Point(currentPoint.X + 1, currentPoint.Y), map)))
                    {
                        nextPoint = new Point(currentPoint.X + 1, currentPoint.Y);
                        return true;
                    }
                    else
                    {
                        nextPoint = new Point();
                        return false;
                    }
                }
            }
            else if (northWall && !eastWall && !southWall && !westWall)
            {
                // Wall only above, so this point is a small corner
                if (lastVertex.X == x && lastVertex.Y == y + verticalScale) // Last vertex is the final vert of the current point
                {
                    if (currentPoint.X - 1 >= 0 && currentPoint.Y + 1 < map.GetLength(1) && openSet.Contains(UniquePointHashCode(new Point(currentPoint.X - 1, currentPoint.Y + 1), map)))
                    {
                        nextPoint = new Point(currentPoint.X - 1, currentPoint.Y + 1);
                        return true;
                    }
                    else if (currentPoint.Y + 1 < map.GetLength(1) && openSet.Contains(UniquePointHashCode(new Point(currentPoint.X, currentPoint.Y + 1), map)))
                    {
                        nextPoint = new Point(currentPoint.X, currentPoint.Y + 1);
                        return true;
                    }
                    else
                    {
                        nextPoint = new Point();
                        return false;
                    }
                }
                else
                {
                    if (currentPoint.X + 1 < map.GetLength(0) && currentPoint.Y + 1 < map.GetLength(1) && openSet.Contains(UniquePointHashCode(new Point(currentPoint.X + 1, currentPoint.Y + 1), map)))
                    {
                        nextPoint = new Point(currentPoint.X + 1, currentPoint.Y + 1);
                        return true;
                    }
                    else if (currentPoint.Y + 1 < map.GetLength(1) && openSet.Contains(UniquePointHashCode(new Point(currentPoint.X, currentPoint.Y + 1), map)))
                    {
                        nextPoint = new Point(currentPoint.X, currentPoint.Y + 1);
                        return true;
                    }
                    else
                    {
                        nextPoint = new Point();
                        return false;
                    }
                }
            }
            else if (!northWall && !eastWall && !southWall && westWall)
            {
                // Wall only left, so this point is a small corner
                if (lastVertex.X == x && lastVertex.Y == y) // Last vertex is the final vert of the current point
                {
                    if (currentPoint.X - 1 >= 0 && currentPoint.Y - 1 >= 0 && openSet.Contains(UniquePointHashCode(new Point(currentPoint.X - 1, currentPoint.Y - 1), map)))
                    {
                        nextPoint = new Point(currentPoint.X - 1, currentPoint.Y - 1);
                        return true;
                    }
                    else if (currentPoint.X - 1 >= 0 && openSet.Contains(UniquePointHashCode(new Point(currentPoint.X - 1, currentPoint.Y), map)))
                    {
                        nextPoint = new Point(currentPoint.X - 1, currentPoint.Y);
                        return true;
                    }
                    else
                    {
                        nextPoint = new Point();
                        return false;
                    }
                }
                else
                {
                    if (currentPoint.X - 1 >= 0 && currentPoint.Y + 1 < map.GetLength(1) && openSet.Contains(UniquePointHashCode(new Point(currentPoint.X - 1, currentPoint.Y + 1), map)))
                    {
                        nextPoint = new Point(currentPoint.X - 1, currentPoint.Y + 1);
                        return true;
                    }
                    else if (currentPoint.X - 1 >= 0 && openSet.Contains(UniquePointHashCode(new Point(currentPoint.X - 1, currentPoint.Y), map)))
                    {
                        nextPoint = new Point(currentPoint.X - 1, currentPoint.Y);
                        return true;
                    }
                    else
                    {
                        nextPoint = new Point();
                        return false;
                    }
                }
            }
            else if (!northWall && eastWall && southWall && westWall)
            {
                // No wall above, so this point is a straight line
                if (lastVertex.X == x && lastVertex.Y == y + verticalScale)
                {
                    if (currentPoint.X - 1 >= 0 && currentPoint.Y + 1 < map.GetLength(1) && openSet.Contains(UniquePointHashCode(new Point(currentPoint.X - 1, currentPoint.Y + 1), map)))
                    {
                        nextPoint = new Point(currentPoint.X - 1, currentPoint.Y + 1);
                        return true;
                    }
                    else if (currentPoint.X - 1 >= 0 && openSet.Contains(UniquePointHashCode(new Point(currentPoint.X - 1, currentPoint.Y), map)))
                    {
                        nextPoint = new Point(currentPoint.X - 1, currentPoint.Y);
                        return true;
                    }
                    else
                    {
                        nextPoint = new Point();
                        return false;
                    }
                }
                else
                {
                    if (currentPoint.X + 1 < map.GetLength(0) && currentPoint.Y + 1 < map.GetLength(1) && openSet.Contains(UniquePointHashCode(new Point(currentPoint.X + 1, currentPoint.Y + 1), map)))
                    {
                        nextPoint = new Point(currentPoint.X + 1, currentPoint.Y + 1);
                        return true;
                    }
                    else if (currentPoint.X + 1 < map.GetLength(0) && openSet.Contains(UniquePointHashCode(new Point(currentPoint.X + 1, currentPoint.Y), map)))
                    {
                        nextPoint = new Point(currentPoint.X + 1, currentPoint.Y);
                        return true;
                    }
                    else
                    {
                        nextPoint = new Point();
                        return false;
                    }
                }
            }
            else if (northWall && eastWall && !southWall && westWall)
            {
                // No wall below, so this point is a straight line
                if (lastVertex.X == x && lastVertex.Y == y)
                {
                    if (currentPoint.X - 1 >= 0 && currentPoint.Y - 1 >= 0 && openSet.Contains(UniquePointHashCode(new Point(currentPoint.X - 1, currentPoint.Y - 1), map)))
                    {
                        nextPoint = new Point(currentPoint.X - 1, currentPoint.Y - 1);
                        return true;
                    }
                    else if (currentPoint.X - 1 >= 0 && openSet.Contains(UniquePointHashCode(new Point(currentPoint.X - 1, currentPoint.Y), map)))
                    {
                        nextPoint = new Point(currentPoint.X - 1, currentPoint.Y);
                        return true;
                    }
                    else
                    {
                        nextPoint = new Point();
                        return false;
                    }
                }
                else
                {
                    if (currentPoint.X + 1 < map.GetLength(0) && currentPoint.Y - 1 >= 0 && openSet.Contains(UniquePointHashCode(new Point(currentPoint.X + 1, currentPoint.Y - 1), map)))
                    {
                        nextPoint = new Point(currentPoint.X + 1, currentPoint.Y - 1);
                        return true;
                    }
                    else if (currentPoint.X + 1 < map.GetLength(0) && openSet.Contains(UniquePointHashCode(new Point(currentPoint.X + 1, currentPoint.Y), map)))
                    {
                        nextPoint = new Point(currentPoint.X + 1, currentPoint.Y);
                        return true;
                    }
                    else
                    {
                        nextPoint = new Point();
                        return false;
                    }
                }
            }
            else if (northWall && eastWall && southWall && !westWall)
            {
                // No Wall left, so this point is a straight line
                if (lastVertex.X == x && lastVertex.Y == y)
                {
                    if (currentPoint.X - 1 >= 0 && currentPoint.Y - 1 >= 0 && openSet.Contains(UniquePointHashCode(new Point(currentPoint.X - 1, currentPoint.Y - 1), map)))
                    {
                        nextPoint = new Point(currentPoint.X - 1, currentPoint.Y - 1);
                        return true;
                    }
                    else if (currentPoint.Y - 1 >= 0 && openSet.Contains(UniquePointHashCode(new Point(currentPoint.X, currentPoint.Y - 1), map)))
                    {
                        nextPoint = new Point(currentPoint.X, currentPoint.Y - 1);
                        return true;
                    }
                    else
                    {
                        nextPoint = new Point();
                        return false;
                    }
                }
                else
                {
                    if (currentPoint.X - 1 >= 0 && currentPoint.Y + 1 < map.GetLength(1) && openSet.Contains(UniquePointHashCode(new Point(currentPoint.X - 1, currentPoint.Y + 1), map)))
                    {
                        nextPoint = new Point(currentPoint.X - 1, currentPoint.Y + 1);
                        return true;
                    }
                    else if (currentPoint.Y + 1 < map.GetLength(1) && openSet.Contains(UniquePointHashCode(new Point(currentPoint.X, currentPoint.Y + 1), map)))
                    {
                        nextPoint = new Point(currentPoint.X, currentPoint.Y + 1);
                        return true;
                    }
                    else
                    {
                        nextPoint = new Point();
                        return false;
                    }
                }
            }
            else if (northWall && !eastWall && southWall && westWall)
            {
                // No wall right, so this point is a straight line
                if (lastVertex.X == x + horizontalScale && lastVertex.Y == y)
                {
                    if (currentPoint.X + 1 < map.GetLength(0) && currentPoint.Y - 1 >= 0 && openSet.Contains(UniquePointHashCode(new Point(currentPoint.X + 1, currentPoint.Y - 1), map)))
                    {
                        nextPoint = new Point(currentPoint.X + 1, currentPoint.Y - 1);
                        return true;
                    }
                    else if (currentPoint.Y - 1 >= 0 && openSet.Contains(UniquePointHashCode(new Point(currentPoint.X, currentPoint.Y - 1), map)))
                    {
                        nextPoint = new Point(currentPoint.X, currentPoint.Y - 1);
                        return true;
                    }
                    else
                    {
                        nextPoint = new Point();
                        return false;
                    }
                }
                else
                {
                    if (currentPoint.X + 1 < map.GetLength(0) && currentPoint.Y + 1 < map.GetLength(1) && openSet.Contains(UniquePointHashCode(new Point(currentPoint.X + 1, currentPoint.Y + 1), map)))
                    {
                        nextPoint = new Point(currentPoint.X + 1, currentPoint.Y + 1);
                        return true;
                    }
                    else if (currentPoint.Y + 1 < map.GetLength(1) && openSet.Contains(UniquePointHashCode(new Point(currentPoint.X, currentPoint.Y + 1), map)))
                    {
                        nextPoint = new Point(currentPoint.X, currentPoint.Y + 1);
                        return true;
                    }
                    else
                    {
                        nextPoint = new Point();
                        return false;
                    }
                }
            }
            else if (northWall && !eastWall && southWall && !westWall)
            {
                // Walls only above and below, so this point is a straight line right and left
                if (lastVertex.X == x && lastVertex.Y == y)
                {
                    if (currentPoint.X - 1 >= 0 && currentPoint.Y - 1 >= 0 && openSet.Contains(UniquePointHashCode(new Point(currentPoint.X - 1, currentPoint.Y - 1), map)))
                    {
                        nextPoint = new Point(currentPoint.X - 1, currentPoint.Y - 1);
                        return true;
                    }
                    else if (currentPoint.Y - 1 >= 0 && openSet.Contains(UniquePointHashCode(new Point(currentPoint.X, currentPoint.Y - 1), map)))
                    {
                        nextPoint = new Point(currentPoint.X, currentPoint.Y - 1);
                        return true;
                    }
                    else
                    {
                        nextPoint = new Point();
                        return false;
                    }
                }
                else if (lastVertex.X == x && lastVertex.Y == y + verticalScale)
                {
                    if (currentPoint.X - 1 >= 0 && currentPoint.Y + 1 < map.GetLength(1) && openSet.Contains(UniquePointHashCode(new Point(currentPoint.X - 1, currentPoint.Y + 1), map)))
                    {
                        nextPoint = new Point(currentPoint.X - 1, currentPoint.Y + 1);
                        return true;
                    }
                    else if (currentPoint.Y + 1 < map.GetLength(1) && openSet.Contains(UniquePointHashCode(new Point(currentPoint.X, currentPoint.Y + 1), map)))
                    {
                        nextPoint = new Point(currentPoint.X, currentPoint.Y + 1);
                        return true;
                    }
                    else
                    {
                        nextPoint = new Point();
                        return false;
                    }
                }
                else if (lastVertex.X == x + horizontalScale && lastVertex.Y == y)
                {
                    if (currentPoint.X + 1 < map.GetLength(0) && currentPoint.Y - 1 >= 0 && openSet.Contains(UniquePointHashCode(new Point(currentPoint.X + 1, currentPoint.Y - 1), map)))
                    {
                        nextPoint = new Point(currentPoint.X + 1, currentPoint.Y - 1);
                        return true;
                    }
                    else if (currentPoint.Y - 1 >= 0 && openSet.Contains(UniquePointHashCode(new Point(currentPoint.X, currentPoint.Y - 1), map)))
                    {
                        nextPoint = new Point(currentPoint.X, currentPoint.Y - 1);
                        return true;
                    }
                    else
                    {
                        nextPoint = new Point();
                        return false;
                    }
                }
                else
                {
                    if (currentPoint.X + 1 < map.GetLength(0) && currentPoint.Y + 1 < map.GetLength(1) && openSet.Contains(UniquePointHashCode(new Point(currentPoint.X + 1, currentPoint.Y + 1), map)))
                    {
                        nextPoint = new Point(currentPoint.X + 1, currentPoint.Y + 1);
                        return true;
                    }
                    else if (currentPoint.Y + 1 < map.GetLength(1) && openSet.Contains(UniquePointHashCode(new Point(currentPoint.X, currentPoint.Y + 1), map)))
                    {
                        nextPoint = new Point(currentPoint.X, currentPoint.Y + 1);
                        return true;
                    }
                    else
                    {
                        nextPoint = new Point();
                        return false;
                    }
                }
            }
            else if (!northWall && eastWall && !southWall && westWall)
            {
                // Walls only right and left, so this point is a straight line above and below
                if (lastVertex.X == x && lastVertex.Y == y)
                {
                    if (currentPoint.X - 1 >= 0 && currentPoint.Y - 1 >= 0 && openSet.Contains(UniquePointHashCode(new Point(currentPoint.X - 1, currentPoint.Y - 1), map)))
                    {
                        nextPoint = new Point(currentPoint.X - 1, currentPoint.Y - 1);
                        return true;
                    }
                    else if (currentPoint.X - 1 >= 0 && openSet.Contains(UniquePointHashCode(new Point(currentPoint.X - 1, currentPoint.Y), map)))
                    {
                        nextPoint = new Point(currentPoint.X - 1, currentPoint.Y);
                        return true;
                    }
                    else
                    {
                        nextPoint = new Point();
                        return false;
                    }
                }
                else if (lastVertex.X == x + horizontalScale && lastVertex.Y == y)
                {
                    if (currentPoint.X + 1 < map.GetLength(0) && currentPoint.Y - 1 >= 0 && openSet.Contains(UniquePointHashCode(new Point(currentPoint.X + 1, currentPoint.Y - 1), map)))
                    {
                        nextPoint = new Point(currentPoint.X + 1, currentPoint.Y - 1);
                        return true;
                    }
                    else if (currentPoint.X + 1 < map.GetLength(0) && openSet.Contains(UniquePointHashCode(new Point(currentPoint.X + 1, currentPoint.Y), map)))
                    {
                        nextPoint = new Point(currentPoint.X + 1, currentPoint.Y);
                        return true;
                    }
                    else
                    {
                        nextPoint = new Point();
                        return false;
                    }
                }
                else if (lastVertex.X == x && lastVertex.Y == y + verticalScale)
                {
                    if (currentPoint.X - 1 >= 0 && currentPoint.Y + 1 < map.GetLength(1) && openSet.Contains(UniquePointHashCode(new Point(currentPoint.X - 1, currentPoint.Y + 1), map)))
                    {
                        nextPoint = new Point(currentPoint.X - 1, currentPoint.Y + 1);
                        return true;
                    }
                    else if (currentPoint.X - 1 >= 0 && openSet.Contains(UniquePointHashCode(new Point(currentPoint.X - 1, currentPoint.Y), map)))
                    {
                        nextPoint = new Point(currentPoint.X - 1, currentPoint.Y);
                        return true;
                    }
                    else
                    {
                        nextPoint = new Point();
                        return false;
                    }
                }
                else
                {
                    if (currentPoint.X + 1 < map.GetLength(0) && currentPoint.Y + 1 < map.GetLength(1) && openSet.Contains(UniquePointHashCode(new Point(currentPoint.X + 1, currentPoint.Y + 1), map)))
                    {
                        nextPoint = new Point(currentPoint.X + 1, currentPoint.Y + 1);
                        return true;
                    }
                    else if (currentPoint.X + 1 < map.GetLength(0) && openSet.Contains(UniquePointHashCode(new Point(currentPoint.X + 1, currentPoint.Y), map)))
                    {
                        nextPoint = new Point(currentPoint.X + 1, currentPoint.Y);
                        return true;
                    }
                    else
                    {
                        nextPoint = new Point();
                        return false;
                    }
                }
            }
            else if (northWall && eastWall && southWall && westWall)
            {
                nextPoint = new Point(currentPoint.X, currentPoint.Y);
                return true;
            }
            else
            {
                nextPoint = new Point();
                return false;
            }
        }

        private bool IsDoubleSidedPoint(Point currentPoint, int[,] map)
        {
            int wallDirections = GetAdjacentWalls(map, currentPoint.X, currentPoint.Y, 0);

            bool northWall = (wallDirections & (int)Direction.NORTH) > 0;
            bool eastWall = (wallDirections & (int)Direction.EAST) > 0;
            bool southWall = (wallDirections & (int)Direction.SOUTH) > 0;
            bool westWall = (wallDirections & (int)Direction.WEST) > 0;

            if ((northWall && !eastWall && southWall && !westWall) || (!northWall && eastWall && !southWall && westWall))
                return true;
            else
                return false;
        }

        private int UniquePointHashCode(Point pointToHash, int[,] map)
        {
            int hash = 0;
            hash += pointToHash.X;
            hash += pointToHash.Y * map.GetLength(0);

            return hash;
        }

        private bool GetStartPoint(Point currentPoint, Point nextPoint, out Point startPoint, int[,] map, int horizontalScale, int verticalScale)
        {
            int x = currentPoint.X * horizontalScale;
            int y = currentPoint.Y * verticalScale;
            int wallDirections = GetAdjacentWalls(map, currentPoint.X, currentPoint.Y, 0);

            int nextDirection = 0;
            if (currentPoint.X > nextPoint.X)
                nextDirection += (int)Direction.WEST;
            else if (currentPoint.X < nextPoint.X)
                nextDirection += (int)Direction.EAST;

            if (currentPoint.Y > nextPoint.Y)
                nextDirection += (int)Direction.SOUTH;
            else
                nextDirection += (int)Direction.NORTH;
            
            bool northWall = (wallDirections & (int)Direction.NORTH) > 0;
            bool eastWall = (wallDirections & (int)Direction.EAST) > 0;
            bool southWall = (wallDirections & (int)Direction.SOUTH) > 0;
            bool westWall = (wallDirections & (int)Direction.WEST) > 0;

            if (!northWall && eastWall && southWall && !westWall)
            {
                // Wall right and below, so make a corner
                if ((nextDirection & (int)Direction.NORTH) > 0 || (nextDirection & (int)Direction.EAST) > 0)
                {
                    startPoint = new Point(x, y);
                    return true;
                }
                else
                {
                    startPoint = new Point(x + horizontalScale, y + verticalScale);
                    return true;
                }
            }
            else if (northWall && eastWall && !southWall && !westWall)
            {
                // Wall right and above, so make a corner
                if ((nextDirection & (int)Direction.NORTH) > 0 || (nextDirection & (int)Direction.WEST) > 0)
                {
                    startPoint = new Point(x + horizontalScale, y);
                    return true;
                }
                else
                {
                    startPoint = new Point(x, y + verticalScale);
                    return true;
                }
            }
            else if (northWall && !eastWall && !southWall && westWall)
            {
                // Wall left and above, so make a corner
                if ((nextDirection & (int)Direction.NORTH) > 0 || (nextDirection & (int)Direction.EAST) > 0)
                {
                    startPoint = new Point(x, y);
                    return true;
                }
                else
                {
                    startPoint = new Point(x + horizontalScale, y + verticalScale);
                    return true;
                }
            }
            else if (!northWall && !eastWall && southWall && westWall)
            {
                // Wall left and below, so make a corner
                if ((nextDirection & (int)Direction.NORTH) > 0 || (nextDirection & (int)Direction.WEST) > 0)
                {
                    startPoint = new Point(x + horizontalScale, y);
                    return true;
                }
                else
                {
                    startPoint = new Point(x, y + verticalScale);
                    return true;
                }
            }
            else if (!northWall && !eastWall && southWall && !westWall)
            {
                // Wall only below, make a small corner
                if ((nextDirection & (int)Direction.WEST) > 0)
                {
                    startPoint = new Point(x + horizontalScale, y);
                    return true;
                }
                else if ((nextDirection & (int)Direction.EAST) > 0)
                {
                    startPoint = new Point(x, y);
                    return true;
                }
                else // can't guarantee which point to start from
                {
                    startPoint = new Point();
                    return false;
                }
            }
            else if (!northWall && eastWall && !southWall && !westWall)
            {
                // Wall only right, make a small corner
                if ((nextDirection & (int)Direction.NORTH) > 0)
                {
                    startPoint = new Point(x + horizontalScale, y + verticalScale);
                    return true;
                }
                else if ((nextDirection & (int)Direction.SOUTH) > 0)
                {
                    startPoint = new Point(x + horizontalScale, y);
                    return true;
                }
                else // can't guarantee which point to start from
                {
                    startPoint = new Point();
                    return false;
                }
            }
            else if (northWall && !eastWall && !southWall && !westWall)
            {
                // Wall only above, make a small corner
                if ((nextDirection & (int)Direction.WEST) > 0)
                {
                    startPoint = new Point(x + horizontalScale, y + verticalScale);
                    return true;
                }
                else if ((nextDirection & (int)Direction.EAST) > 0)
                {
                    startPoint = new Point(x, y + verticalScale);
                    return true;
                }
                else // can't guarantee which point to start from
                {
                    startPoint = new Point();
                    return false;
                }
            }
            else if (!northWall && !eastWall && !southWall && westWall)
            {
                // Wall only left, make a small corner
                if ((nextDirection & (int)Direction.NORTH) > 0)
                {
                    startPoint = new Point(x, y + verticalScale);
                    return true;
                }
                else if ((nextDirection & (int)Direction.SOUTH) > 0)
                {
                    startPoint = new Point(x, y);
                    return true;
                }
                else // can't guarantee which point to start from
                {
                    startPoint = new Point();
                    return false;
                }
            }
            else if (!northWall && eastWall && southWall && westWall)
            {
                // No wall above, make a straight line
                if ((nextDirection & (int)Direction.WEST) > 0)
                {
                    startPoint = new Point(x + horizontalScale, y + verticalScale);
                    return true;
                }
                else if ((nextDirection & (int)Direction.EAST) > 0)
                {
                    startPoint = new Point(x, y + verticalScale);
                    return true;
                }
                else // can't guarantee which point to start from
                {
                    startPoint = new Point();
                    return false;
                }
            }
            else if (northWall && eastWall && !southWall && westWall)
            {
                // No wall below, make a straight line
                if ((nextDirection & (int)Direction.WEST) > 0)
                {
                    startPoint = new Point(x + horizontalScale, y);
                    return true;
                }
                else if ((nextDirection & (int)Direction.EAST) > 0)
                {
                    startPoint = new Point(x, y);
                    return true;
                }
                else // can't guarantee which point to start from
                {
                    startPoint = new Point();
                    return false;
                }
            }
            else if (northWall && eastWall && southWall && !westWall)
            {
                // No Wall left, make a straight line
                if ((nextDirection & (int)Direction.NORTH) > 0)
                {
                    startPoint = new Point(x, y);
                    return true;
                }
                else if ((nextDirection & (int)Direction.SOUTH) > 0)
                {
                    startPoint = new Point(x, y + verticalScale);
                    return true;
                }
                else
                {
                    startPoint = new Point();
                    return false;
                }
            }
            else if (northWall && !eastWall && southWall && westWall)
            {
                // No wall right, make a straight line
                if ((nextDirection & (int)Direction.NORTH) > 0)
                {
                    startPoint = new Point(x + horizontalScale, y);
                    return true;
                }
                else if ((nextDirection & (int)Direction.SOUTH) > 0)
                {
                    startPoint = new Point(x + horizontalScale, y + verticalScale);
                    return true;
                }
                else
                {
                    startPoint = new Point();
                    return false;
                }
            }
            else if (northWall && !eastWall && southWall && !westWall)
            {
                // Walls only above and below, make a straight line right and left
                if ((nextDirection & (int)Direction.NORTH & (int)Direction.WEST) > 0)
                {
                    startPoint = new Point(x, y);
                    return true;
                }
                else if ((nextDirection & (int)Direction.NORTH & (int)Direction.EAST) > 0)
                {
                    startPoint = new Point(x + horizontalScale, y);
                    return true;
                }
                else if ((nextDirection & (int)Direction.SOUTH & (int)Direction.WEST) > 0)
                {
                    startPoint = new Point(x, y + verticalScale);
                    return true;
                }
                else if ((nextDirection & (int)Direction.SOUTH & (int)Direction.EAST) > 0)
                {
                    startPoint = new Point(x + horizontalScale, y + verticalScale);
                    return true;
                }
                else
                {
                    startPoint = new Point();
                    return false;
                }
            }
            else if (!northWall && eastWall && !southWall && westWall)
            {
                // Walls only right and left, make a straight line above and below
                if ((nextDirection & (int)Direction.NORTH & (int)Direction.WEST) > 0)
                {
                    startPoint = new Point(x + horizontalScale, y + verticalScale);
                    return true;
                }
                else if ((nextDirection & (int)Direction.NORTH & (int)Direction.EAST) > 0)
                {
                    startPoint = new Point(x, y + verticalScale);
                    return true;
                }
                else if ((nextDirection & (int)Direction.SOUTH & (int)Direction.WEST) > 0)
                {
                    startPoint = new Point(x + horizontalScale, y);
                    return true;
                }
                else if ((nextDirection & (int)Direction.SOUTH & (int)Direction.EAST) > 0)
                {
                    startPoint = new Point(x, y);
                    return true;
                }
                else
                {
                    startPoint = new Point();
                    return false;
                }
            }
            else
            {
                startPoint = new Point();
                return false;
            }
        }

        private Point GetSingleNeighborStartPoint(Point currentPoint, int[,] map, int horizontalScale, int verticalScale)
        {
            int halfHorizontal = (int)(0.5f * horizontalScale);
            int halfVertical = (int)(0.5f * verticalScale);
            int x = currentPoint.X * horizontalScale;
            int y = currentPoint.Y * verticalScale;
            int wallDirections = GetAdjacentWalls(map, currentPoint.X, currentPoint.Y, 0);

            bool northWall = (wallDirections & (int)Direction.NORTH) > 0;
            bool eastWall = (wallDirections & (int)Direction.EAST) > 0;
            bool southWall = (wallDirections & (int)Direction.SOUTH) > 0;
            bool westWall = (wallDirections & (int)Direction.WEST) > 0;

            if (!northWall && eastWall && southWall && !westWall)
            {
                // Wall right and below, so make a corner
                if (currentPoint.X == 0 || currentPoint.Y == 0)
                    return new Point(x, y);
                else
                    return new Point(x + horizontalScale, y + verticalScale);
            }
            else if (northWall && eastWall && !southWall && !westWall)
            {
                // Wall right and above, so make a corner
                if (currentPoint.X == 0)
                    return new Point(x, y + verticalScale);
                else
                    return new Point(x + horizontalScale, y);
            }
            else if (northWall && !eastWall && !southWall && westWall)
            {
                // Wall left and above, so make a corner
                if (currentPoint.X == 0 || currentPoint.Y == 0)
                    return new Point(x, y);
                else
                    return new Point(x + horizontalScale, y + verticalScale);
            }
            else if (!northWall && !eastWall && southWall && westWall)
            {
                // Wall left and below, so make a corner
                if (currentPoint.X == 0)
                    return new Point(x, y + verticalScale);
                else
                    return new Point(x + horizontalScale, y);
            }
            else if (!northWall && !eastWall && southWall && !westWall)
            {
                // Wall only below, make a small corner
                //if (currentPoint.Y == 0)
                //    return new Point(x + horizontalScale, y);
                //else
                //    return new Point(x, y);
                Console.WriteLine("Error in start vertex");
                return new Point();
            }
            else if (!northWall && eastWall && !southWall && !westWall)
            {
                // Wall only right, make a small corner
                //if (currentPoint.X == 0)
                //    return new Point(x + horizontalScale, y + verticalScale);
                //else
                //    return new Point(x + horizontalScale, y);
                Console.WriteLine("Error in start vertex");
                return new Point();
            }
            else if (northWall && !eastWall && !southWall && !westWall)
            {
                // Wall only above, make a small corner
                //if (lastVertex.X == x && lastVertex.Y == y + verticalScale)
                //    return new Point[] { new Point(x + halfHorizontal, y + halfVertical), new Point(x + horizontalScale, y + verticalScale) };
                //else
                //    return new Point[] { new Point(x + halfHorizontal, y + halfVertical), new Point(x, y + verticalScale) };
                Console.WriteLine("Error in start vertex");
                return new Point();
            }
            else if (!northWall && !eastWall && !southWall && westWall)
            {
                // Wall only left, make a small corner
                //if (lastVertex.X == x && lastVertex.Y == y)
                //    return new Point[] { new Point(x + halfHorizontal, y + halfVertical), new Point(x, y + verticalScale) };
                //else
                //    return new Point[] { new Point(x + halfHorizontal, y + halfVertical), new Point(x, y) };
                Console.WriteLine("Error in start vertex");
                return new Point();
            }
            else if (!northWall && eastWall && southWall && westWall)
            {
                // No wall above, make a straight line
                if (currentPoint.X == 0)
                    return new Point(x, y + verticalScale);
                else
                    return new Point(x + horizontalScale, y + verticalScale);
            }
            else if (northWall && eastWall && !southWall && westWall)
            {
                // No wall below, make a straight line
                if (currentPoint.X == 0)
                    return new Point(x, y);
                else
                    return new Point(x + horizontalScale, y);
            }
            else if (northWall && eastWall && southWall && !westWall)
            {
                // No Wall left, make a straight line
                if (currentPoint.Y == 0)
                    return new Point(x, y);
                else
                    return new Point(x, y + verticalScale);
            }
            else if (northWall && !eastWall && southWall && westWall)
            {
                // No wall right, make a straight line
                if (currentPoint.Y == 0)
                    return new Point(x + horizontalScale, y);
                else
                    return new Point(x + horizontalScale, y + verticalScale);
            }
            else if (northWall && !eastWall && southWall && !westWall)
            {
                // Walls only above and below, make a straight line right and left
                //if (currentPoint.X)
                //    return new Point[] { new Point(x, y + verticalScale) };
                //else if (lastVertex.X == x && lastVertex.Y == y + verticalScale)
                //    return new Point[] { new Point(x, y) };
                //else if (lastVertex.X == x + horizontalScale && lastVertex.Y == y)
                //    return new Point[] { new Point(x + horizontalScale, y + verticalScale) };
                //else
                //    return new Point[] { new Point(x + horizontalScale, y) };
                Console.WriteLine("Error in start vertex");
                return new Point();
            }
            else if (!northWall && eastWall && !southWall && westWall)
            {
                // Walls only right and left, make a straight line above and below
                //if (cameFrom == Direction.NORTH || cameFrom == Direction.WEST)
                //if (lastVertex.X == x && lastVertex.Y == y)
                //    return new Point[] { new Point(x + horizontalScale, y) };
                //else if (lastVertex.X == x + horizontalScale && lastVertex.Y == y)
                //    return new Point[] { new Point(x, y) };
                //else if (lastVertex.X == x && lastVertex.Y == y + verticalScale)
                //    return new Point[] { new Point(x + horizontalScale, y + verticalScale) };
                //else
                //    return new Point[] { new Point(x, y + verticalScale) };
                Console.WriteLine("Error in start vertex");
                return new Point();
            }
            else
            {
                Console.WriteLine("Error in start vertex");
                return new Point();
            }
        }

        private Point[] GetPoints(Point currentPoint, Point lastVertex, int[,] map, int horizontalScale, int verticalScale)
        {
            //Direction cameFrom;
            //if (currentPoint.X > lastPoint.X)
            //    cameFrom = Direction.WEST;
            //else if (currentPoint.X < lastPoint.X)
            //    cameFrom = Direction.EAST;
            //else if (currentPoint.Y > lastPoint.Y)
            //    cameFrom = Direction.SOUTH;
            //else //if (currentPoint.Y < lastPoint.Y)
            //    cameFrom = Direction.NORTH;

            int halfHorizontal = (int)(0.5f * horizontalScale);
            int halfVertical = (int)(0.5f * verticalScale);
            int x = currentPoint.X * horizontalScale;
            int y = currentPoint.Y * verticalScale;
            int wallDirections = GetAdjacentWalls(map, currentPoint.X, currentPoint.Y, 0);

            bool northWall = (wallDirections & (int)Direction.NORTH) > 0;
            bool eastWall = (wallDirections & (int)Direction.EAST) > 0;
            bool southWall = (wallDirections & (int)Direction.SOUTH) > 0;
            bool westWall = (wallDirections & (int)Direction.WEST) > 0;

            if (!northWall && eastWall && southWall && !westWall)
            {
                // Wall right and below, so make a corner
                //if (cameFrom == Direction.NORTH || cameFrom == Direction.EAST)
                if (lastVertex.X == x && lastVertex.Y == y)
                    return new Point[] { new Point(x + horizontalScale, y + verticalScale) };
                else
                    return new Point[] { new Point(x, y) };
            }
            else if (northWall && eastWall && !southWall && !westWall)
            {
                // Wall right and above, so make a corner
                //if (cameFrom == Direction.NORTH || cameFrom == Direction.WEST)
                if (lastVertex.X == x && lastVertex.Y == y + verticalScale)
                    return new Point[] { new Point(x + horizontalScale, y) };
                else
                    return new Point[] { new Point(x, y + verticalScale) };
            }
            else if (northWall && !eastWall && !southWall && westWall)
            {
                // Wall left and above, so make a corner
                //if (cameFrom == Direction.NORTH || cameFrom == Direction.EAST)
                if (lastVertex.X == x && lastVertex.Y == y)
                    return new Point[] { new Point(x + horizontalScale, y + verticalScale) };
                else
                    return new Point[] { new Point(x, y) };
            }
            else if (!northWall && !eastWall && southWall && westWall)
            {
                // Wall left and below, so make a corner
                //if (cameFrom == Direction.NORTH || cameFrom == Direction.WEST)
                if (lastVertex.X == x && lastVertex.Y == y + verticalScale)
                    return new Point[] { new Point(x + horizontalScale, y) };
                else
                    return new Point[] { new Point(x, y + verticalScale) };
            }
            else if (!northWall && !eastWall && southWall && !westWall)
            {
                // Wall only below, make a small corner
                //if (cameFrom == Direction.NORTH || cameFrom == Direction.WEST)
                if (lastVertex.X == x && lastVertex.Y == y)
                    return new Point[] { new Point(x + halfHorizontal, y + halfVertical), new Point(x + horizontalScale, y) };
                else
                    return new Point[] { new Point(x + halfHorizontal, y + halfVertical), new Point(x, y) };
            }
            else if (!northWall && eastWall && !southWall && !westWall)
            {
                // Wall only right, make a small corner
                //if (cameFrom == Direction.NORTH || cameFrom == Direction.WEST)
                if (lastVertex.X == x + horizontalScale && lastVertex.Y == y)
                    return new Point[] { new Point(x + halfHorizontal, y + halfVertical), new Point(x + horizontalScale, y + verticalScale) };
                else
                    return new Point[] { new Point(x + halfHorizontal, y + halfVertical), new Point(x + horizontalScale, y)};
            }
            else if (northWall && !eastWall && !southWall && !westWall)
            {
                // Wall only above, make a small corner
                //if (cameFrom == Direction.NORTH || cameFrom == Direction.WEST)
                if (lastVertex.X == x && lastVertex.Y == y + verticalScale)
                    return new Point[] { new Point(x + halfHorizontal, y + halfVertical), new Point(x + horizontalScale, y + verticalScale) };
                else
                    return new Point[] { new Point(x + halfHorizontal, y + halfVertical), new Point(x, y + verticalScale) };
            }
            else if (!northWall && !eastWall && !southWall && westWall)
            {
                // Wall only left, make a small corner
                //if (cameFrom == Direction.NORTH || cameFrom == Direction.WEST)
                if (lastVertex.X == x && lastVertex.Y == y)
                    return new Point[] { new Point(x + halfHorizontal, y + halfVertical), new Point(x, y + verticalScale) };
                else
                    return new Point[] { new Point(x + halfHorizontal, y + halfVertical), new Point(x, y) };
            }
            else if (!northWall && eastWall && southWall && westWall)
            {
                // No wall above, make a straight line
                //if (cameFrom == Direction.NORTH || cameFrom == Direction.WEST)
                if (lastVertex.X == x && lastVertex.Y == y + verticalScale)
                    return new Point[] { new Point(x + horizontalScale, y + verticalScale) };
                else
                    return new Point[] { new Point(x, y + verticalScale) };
            }
            else if (northWall && eastWall && !southWall && westWall)
            {
                // No wall below, make a straight line
                //if (cameFrom == Direction.NORTH || cameFrom == Direction.WEST)
                if (lastVertex.X == x && lastVertex.Y == y)
                    return new Point[] { new Point(x + horizontalScale, y) };
                else
                    return new Point[] { new Point(x, y) };
            }
            else if (northWall && eastWall && southWall && !westWall)
            {
                // No Wall left, make a straight line
                //if (cameFrom == Direction.NORTH || cameFrom == Direction.WEST)
                if (lastVertex.X == x && lastVertex.Y == y)
                    return new Point[] { new Point(x, y + verticalScale) };
                else
                    return new Point[] { new Point(x, y) };
            }
            else if (northWall && !eastWall && southWall && westWall)
            {
                // No wall right, make a straight line
                //if (cameFrom == Direction.NORTH || cameFrom == Direction.WEST)
                if (lastVertex.X == x + horizontalScale && lastVertex.Y == y + verticalScale)
                    return new Point[] { new Point(x + horizontalScale, y) };
                else
                    return new Point[] { new Point(x + horizontalScale, y + verticalScale) };
            }
            else if (northWall && !eastWall && southWall && !westWall)
            {
                // Walls only above and below, make a straight line right and left
                //if (cameFrom == Direction.NORTH || cameFrom == Direction.WEST)
                if (lastVertex.X == x && lastVertex.Y == y)
                    return new Point[] { new Point(x, y + verticalScale) };
                else if (lastVertex.X == x && lastVertex.Y == y + verticalScale)
                    return new Point[] { new Point(x, y) };
                else if (lastVertex.X == x + horizontalScale && lastVertex.Y == y)
                    return new Point[] { new Point(x + horizontalScale, y + verticalScale) };
                else
                    return new Point[] { new Point(x + horizontalScale, y) };
            }
            else if (!northWall && eastWall && !southWall && westWall)
            {
                // Walls only right and left, make a straight line above and below
                //if (cameFrom == Direction.NORTH || cameFrom == Direction.WEST)
                if (lastVertex.X == x && lastVertex.Y == y)
                    return new Point[] { new Point(x + horizontalScale, y) };
                else if (lastVertex.X == x + horizontalScale && lastVertex.Y == y)
                    return new Point[] { new Point(x, y) };
                else if (lastVertex.X == x && lastVertex.Y == y + verticalScale)
                    return new Point[] { new Point(x + horizontalScale, y + verticalScale) };
                else
                    return new Point[] { new Point(x, y + verticalScale) };
            }
            else if (northWall && eastWall && southWall && westWall)
            {
                return new Point[] { new Point(lastVertex.X, lastVertex.Y) };
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Creates a maze twice the given dimensions, to allow for the walls to be empty cells between the living passage cells
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        private int[,] GenerateMaze(int rows, int columns)
        {
            int[,] maze = new int[rows * 2, columns * 2];
            List<Vector2i> activeList = new List<Vector2i>();
            Direction lastDirection = Direction.NORTH;
            Direction currentDirection = Direction.NORTH;

            //int startSide = UnityEngine.Random.Range(0, 4);

            Vector2i startingCell = GetRandomStartSide(rows, columns);
            
            startingCell = startingCell * 2;
            activeList.Add(startingCell);
            maze[startingCell.x, startingCell.y] = 2;

            while (activeList.Count > 0)
            {
                int index = ChooseIndex(activeList.Count);

                Direction[] directions = GetDirections(); //_diagonalChance);

                for (int i = 0; i < directions.Length; i++)
                {
                    Vector2i movement = GetDirectionMovement(directions[i]);
                    Vector2i newCell = activeList[index] + (movement * 2);

                    // If we are in the maze and the new cell has not been visited yet
                    if (newCell.x >= 0 && newCell.y >= 0 && newCell.x < maze.GetLength(0) && newCell.y < maze.GetLength(1) && maze[newCell.x, newCell.y] == 0)
                    {
                        // Add the cell between the new cell and the old cell if we can
                        Vector2i oddCell = newCell - movement;
                        if (oddCell.x >= 0 && oddCell.y >= 0 && oddCell.x < maze.GetLength(0) && oddCell.y < maze.GetLength(1))
                        {
                            maze[oddCell.x, oddCell.y] |= (int)directions[i];
                            maze[oddCell.x, oddCell.y] |= (int)GetInverseDirection(directions[i]);
                        }

                        // Add the new cell to the maze and the active list
                        maze[newCell.x, newCell.y] |= (int)directions[i];
                        maze[activeList[index].x, activeList[index].y] |= (int)GetInverseDirection(directions[i]);
                        activeList.Add(newCell);

                        if (currentDirection != directions[i])
                        {
                            lastDirection = currentDirection;
                            currentDirection = directions[i];
                        }

                        break;
                    }
                    // Remove the current node from the active list if we checked all directions and couldn't find any available neighbors
                    else if (i == directions.Length - 1)
                    {
                        // Attempt to connect the dead end to the rest of the maze
                        Vector2i fuseMovement = GetDirectionMovement(lastDirection);

                        Vector2i oddCell = activeList[index] - fuseMovement;

                        if (CountAliveNeighbors(maze, activeList[index].x, activeList[index].y) <= 2)
                        {
                            if (oddCell.x >= 0 && oddCell.y >= 0 && oddCell.x < maze.GetLength(0) - 1 && oddCell.y < maze.GetLength(1) - 1)
                            {
                                maze[oddCell.x, oddCell.y] |= (int)lastDirection;
                                maze[oddCell.x, oddCell.y] |= (int)GetInverseDirection(lastDirection);
                            }
                        }

                        activeList.RemoveAt(index);
                    }
                }
            }

            return maze;
        }

        private int[,] GenerateRandomWalkMaze(ref Vector2i startingPoint, int mazeRows, int mazeColumns, int mapWidth, int mapHeight)
        {
            MazeNode[,] mazeNodes = GenerateMazeNodes(startingPoint, mazeRows, mazeColumns);
            int[,] maze = new int[mapWidth, mapHeight];
            int xEdgeBuffer = PathWidth * mapWidth / (20 * (mazeColumns + 1));
            int yEdgeBuffer = PathWidth * mapHeight / (20 * (mazeRows + 1));
            int maxXPerturb = PathWidth * mapWidth / (10 * (mazeColumns + 1));
            int maxYPerturb = PathWidth * mapHeight / (10 * (mazeRows + 1));
            int cellHalfWidth = mapWidth / (2 * mazeRows);
            int cellHalfHeight = mapHeight / (2 * mazeColumns);
            int[,] test = new int[mapWidth, mapHeight];
            for (int i = 0; i < mazeNodes.GetLength(0); i++)
            {
                for (int j = 0; j < mazeNodes.GetLength(1); j++)
                {
                    foreach (Direction d in mazeNodes[i, j].connections.Keys)
                    {
                        // Skip connections to the north or west, so we don't duplicate our paths
                        if (d == Direction.SOUTH || d == Direction.EAST)
                        {
                            bool horizontal = d == Direction.EAST;
                            int cellSize = horizontal ? cellHalfWidth * 2 : cellHalfHeight * 2;
                            for (int k = 0; k < cellSize; k++)
                            {
                                if (horizontal)
                                    test[i * cellHalfWidth * 2 + k, j * cellHalfHeight * 2] = 1;
                                else
                                    test[i * cellHalfWidth * 2, j * cellHalfHeight * 2 + k] = 1;
                            }
                        }
                    }
                }
            }
            DebugWriteToTexture(test, _pixelsPerMapUnit, _pixelsPerMapUnit);
            ScaleMazeNodes(ref startingPoint, mazeNodes, mapWidth, mapHeight);
            PerturbNodes(ref startingPoint, mazeNodes, maxXPerturb, maxYPerturb, mapWidth, mapHeight, xEdgeBuffer, yEdgeBuffer, cellHalfWidth, cellHalfHeight);

            for (int i = 0; i < mazeNodes.GetLength(0); i++)
            {
                for (int j = 0; j < mazeNodes.GetLength(1); j++)
                {
                    // Add the entrance
                    if (mazeNodes[i, j].distanceFromStart == 0)
                    {
                        // If we are on the west side
                        if (i == 0)
                            AddRandomWalkPath(maze, mazeNodes[i, j].position, new Vector2i(0, mazeNodes[i, j].position.y), i, j, cellHalfWidth * 2, xEdgeBuffer, yEdgeBuffer, true);
                        else if (i == mazeNodes.GetLength(0) - 1) // East side
                            AddRandomWalkPath(maze, mazeNodes[i, j].position, new Vector2i(maze.GetLength(0) - 1, mazeNodes[i, j].position.y), i, j, cellHalfWidth * 2, xEdgeBuffer, yEdgeBuffer, true);
                        else if (j == 0) // North side
                            AddRandomWalkPath(maze, mazeNodes[i, j].position, new Vector2i(mazeNodes[i, j].position.x, 0), i, j, cellHalfHeight * 2, xEdgeBuffer, yEdgeBuffer, false);
                        else // South side
                            AddRandomWalkPath(maze, mazeNodes[i, j].position, new Vector2i(mazeNodes[i, j].position.x, maze.GetLength(1) - 1), i, j, cellHalfHeight * 2, xEdgeBuffer, yEdgeBuffer, false);
                    }

                    foreach (Direction d in mazeNodes[i, j].connections.Keys)
                    {
                        // Skip connections to the north or west, so we don't duplicate our paths
                        if (d == Direction.SOUTH || d == Direction.EAST)
                        {
                            bool horizontal = d == Direction.EAST;
                            int cellSize = horizontal ? cellHalfWidth * 2 : cellHalfHeight * 2;
                            AddRandomWalkPath(maze, mazeNodes[i, j].position, mazeNodes[i, j].connections[d].position, i, j, cellSize, xEdgeBuffer, yEdgeBuffer, horizontal);
                        }
                    }

                    // Add extra path to make bigger rooms
                    Vector2i scale = new Vector2i(mapWidth / mazeNodes.GetLength(0), mapHeight / mazeNodes.GetLength(1));

                    AddRoomFromNode(ref maze, mazeNodes[i, j], scale, _pathWidth * 2);
                }
            }
            
            return maze;
        }

        /// <summary>
        /// Creates a maze twice the given dimensions, to allow for the walls to be empty cells between the living passage cells
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        private MazeNode[,] GenerateMazeNodes(Vector2i startingCell, int rows, int columns)
        {
            bool[,] visited = new bool[rows, columns];
            MazeNode[,] mazeNodes = new MazeNode[rows, columns];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    mazeNodes[i, j] = new MazeNode(i, j);
                }
            }
            List<MazeNode> activeList = new List<MazeNode>();
            
            //Vector2i startingCell;
            MazeNode startingNode;
            
            startingNode = mazeNodes[startingCell.x, startingCell.y];

            startingNode.distanceFromStart = 0;
            activeList.Add(startingNode);
            visited[startingCell.x, startingCell.y] = true;

            while (activeList.Count > 0)
            {
                int index = ChooseIndex(activeList.Count);

                Direction[] directions = GetDirections();// _diagonalChance);

                for (int i = 0; i < directions.Length; i++)
                {
                    Vector2i movement = GetDirectionMovement(directions[i]);
                    MazeNode activeNode = activeList[index];

                    // If we are in the maze and the new cell has not been visited yet
                    if (PositionInRange(activeNode.position + movement, mazeNodes.GetLength(0), mazeNodes.GetLength(1)) &&
                        !visited[activeNode.position.x + movement.x, activeNode.position.y + movement.y])
                    {
                        // Add the new node to the active list
                        MazeNode newNode = mazeNodes[activeList[index].position.x + movement.x, activeList[index].position.y + movement.y];
                        activeList.Add(newNode);

                        // Add the connections between the new node and the active node
                        activeNode.connections.Add(directions[i], newNode);
                        newNode.connections.Add(GetInverseDirection(directions[i]), activeNode);

                        visited[newNode.position.x, newNode.position.y] = true;

                        // End the for loop since we added a connection
                        break;
                    }
                    // Remove the current node from the active list if we checked all directions and couldn't find any available neighbors
                    else if (i == directions.Length - 1)
                    {
                        FuseDeadEnds(activeNode, mazeNodes);

                        activeList.RemoveAt(index);
                    }
                }
            }

            // Go through all the nodes and get their distance from the starting node
            int currentDistance = 0;
            List<MazeNode> nextNodes = new List<MazeNode>();
            activeList.Add(startingNode);
            while (activeList.Count > 0)
            {
                currentDistance++;

                for (int i = 0; i < activeList.Count; i++)
                {
                    foreach (MazeNode node in activeList[i].connections.Values)
                    {
                        if (node.distanceFromStart < 0)
                        {
                            node.distanceFromStart = currentDistance;
                            nextNodes.Add(node);
                        }
                    }
                }

                activeList = new List<MazeNode>(nextNodes);
                nextNodes.Clear();
            }

            return mazeNodes;
        }

        private Vector2i GetRandomStartSide(int rows, int columns)
        {
            Vector2i startingCell;
            int startSide = _rng.Next(0, 4);

            if (startSide == 0)//Start on the West
            {
                startingCell = new Vector2i(0, _rng.Next(0, columns));
            }
            else if (startSide == 1) // Start on the North
            {
                startingCell = new Vector2i(_rng.Next(0, rows), columns - 1);
            }
            else if (startSide == 2) // Start on the East
            {
                startingCell = new Vector2i(rows - 1, _rng.Next(0, columns));
            }
            else // Start on the South
            {
                startingCell = new Vector2i(_rng.Next(0, rows), 0);
            }

            return startingCell;
        }

        private void FuseDeadEnds(MazeNode activeNode, MazeNode[,] mazeNodes)
        {
            // Attempt to connect the dead end to the rest of the maze
            if (activeNode.connections.Count < 2)
            {
                Direction lastDirection = Direction.NORTH;
                Direction oppositeDirection = Direction.NORTH;
                Direction currentDirection = Direction.NORTH;
                Vector2i lastDirectionPos = new Vector2i();
                Vector2i oppositeDirectionPos = new Vector2i();
                Vector2i currentDirectionPos = new Vector2i();

                // Get the most recently changed direction
                foreach (Direction d in activeNode.connections.Keys)
                {
                    lastDirection = activeNode.LastDirection(null, d);
                    if (lastDirection == Direction.error)
                    {
                        continue;
                    }
                    oppositeDirection = GetInverseDirection(lastDirection);
                    currentDirection = d;
                    lastDirectionPos = activeNode.position + GetDirectionMovement(lastDirection);
                    oppositeDirectionPos = activeNode.position + GetDirectionMovement(oppositeDirection);
                    currentDirectionPos = activeNode.position + GetDirectionMovement(currentDirection);
                }

                // Attempt to fuse in the opposite direction of the most recently changed direction, for the longest loop
                if (PositionInRange(oppositeDirectionPos, mazeNodes.GetLength(0), mazeNodes.GetLength(1)))
                {
                    MazeNode newNode = mazeNodes[oppositeDirectionPos.x, oppositeDirectionPos.y];
                    if (!activeNode.connections.ContainsKey(oppositeDirection))
                    {
                        if (!activeNode.connections.ContainsKey(oppositeDirection))
                        {
                            activeNode.connections.Add(oppositeDirection, newNode);
                        }
                        if (!newNode.connections.ContainsKey(lastDirection))
                        {
                            newNode.connections.Add(lastDirection, activeNode);
                        }
                    }
                }
                // If we can't go in the opposite direction, the current direction is the next longest loop
                else if (PositionInRange(currentDirectionPos, mazeNodes.GetLength(0), mazeNodes.GetLength(1)) &&
                         activeNode.GetDistance(null, mazeNodes[currentDirectionPos.x, currentDirectionPos.y], 1) > 5)
                {
                    MazeNode newNode = mazeNodes[currentDirectionPos.x, currentDirectionPos.y];
                    if (!activeNode.connections.ContainsKey(currentDirection))
                    {
                        activeNode.connections.Add(currentDirection, newNode);
                    }
                    if (!newNode.connections.ContainsKey(GetInverseDirection(currentDirection)))
                    {
                        newNode.connections.Add(GetInverseDirection(currentDirection), activeNode);
                    }
                }
                // If we can't go in the opposite or current direction, try the most recently changed direction
                else if (PositionInRange(lastDirectionPos, mazeNodes.GetLength(0), mazeNodes.GetLength(1)) &&
                         activeNode.GetDistance(null, mazeNodes[lastDirectionPos.x, lastDirectionPos.y], 1) > 5)
                {
                    MazeNode newNode = mazeNodes[lastDirectionPos.x, lastDirectionPos.y];
                    if (!activeNode.connections.ContainsKey(lastDirection))
                    {
                        activeNode.connections.Add(lastDirection, newNode);
                    }
                    if (!newNode.connections.ContainsKey(oppositeDirection))
                    {
                        newNode.connections.Add(oppositeDirection, activeNode);
                    }
                }
            }
        }

        private bool PositionInRange(Vector2i newPosition, int xLimit, int yLimit)
        {
            return (newPosition.x >= 0 && newPosition.y >= 0 && newPosition.x < xLimit && newPosition.y < yLimit);
        }

        private int[,] ScaleMap(int[,] oldMap, int newWidth, int newHeight)
        {
            int[,] newMap = new int[newWidth, newHeight];

            Vector2i scale = new Vector2i(newWidth / oldMap.GetLength(0), newHeight / oldMap.GetLength(1));

            for (int i = 0; i < oldMap.GetLength(0); i++)
            {
                for (int j = 0; j < oldMap.GetLength(1); j++)
                {
                    if (oldMap[i, j] > 0)
                    {
                        newMap[i * scale.x + scale.x, j * scale.y + scale.y] = oldMap[i, j];
                        
                        if (j < oldMap.GetLength(1) && (oldMap[i, j] & (int)Direction.SOUTH) != 0)
                        {
                            for (int k = 1; k <= scale.y; k++)
                            {
                                newMap[i * scale.x + scale.x, j * scale.y + k + scale.y] = oldMap[i, j];
                            }
                        }
                        if (i < oldMap.GetLength(0) && (oldMap[i, j] & (int)Direction.WEST) != 0)
                        {
                            for (int k = 1; k <= scale.x; k++)
                            {
                                newMap[i * scale.x + k + scale.x, j * scale.y + scale.y] = oldMap[i, j];
                            }
                        }
                    }
                }
            }

            return newMap;
        }

        private void ScaleMazeNodes(ref Vector2i startingPoint, MazeNode[,] mazeNodes, int newWidth, int newHeight)
        {
            Vector2i scale = new Vector2i(newWidth / mazeNodes.GetLength(0), newHeight / mazeNodes.GetLength(1));
            bool scaledStartPoint = false;

            for (int i = 0; i < mazeNodes.GetLength(0); i++)
            {
                for (int j = 0; j < mazeNodes.GetLength(1); j++)
                {
                    mazeNodes[i, j].position.x = mazeNodes[i, j].position.x * scale.x + scale.x / 2;
                    mazeNodes[i, j].position.y = mazeNodes[i, j].position.y * scale.y + scale.y / 2;

                    if (!scaledStartPoint && startingPoint.x == i && startingPoint.y == j)
                    {
                        startingPoint.x = mazeNodes[i, j].position.x;
                        startingPoint.y = mazeNodes[i, j].position.y;
                        scaledStartPoint = true;
                    }
                }
            }
        }

        private void PerturbNodes(ref Vector2i startingPoint, MazeNode[,] nodes, int maxXPerturb, int maxYPerturb, int maxWidth, int maxHeight, int xEdgeBuffer, int yEdgeBuffer, int cellHalfWidth, int cellHalfHeight)
        {
            bool perturbedStartPoint = false;

            for (int i = 0; i < nodes.GetLength(0); i++)
            {
                for (int j = 0; j < nodes.GetLength(1); j++)
                {
                    //Vector2i perturbAmount = new Vector2i(UnityEngine.Random.Range(-maxPerturb, maxPerturb), UnityEngine.Random.Range(-maxPerturb, maxPerturb));
                    Vector2i perturbAmount = new Vector2i(_rng.Next(-maxXPerturb, maxXPerturb), _rng.Next(-maxYPerturb, maxYPerturb));
                    Vector2i oldPosition = new Vector2i(nodes[i, j].position.x, nodes[i, j].position.y);
                    nodes[i, j].position += perturbAmount;

                    // Clamp node position
                    if (nodes[i, j].position.x > maxWidth - xEdgeBuffer)
                        nodes[i, j].position.x = maxWidth - xEdgeBuffer;
                    else if (nodes[i, j].position.x < xEdgeBuffer)
                        nodes[i, j].position.x = xEdgeBuffer;
                    else if (cellHalfWidth > xEdgeBuffer && nodes[i, j].position.x > oldPosition.x + cellHalfWidth - xEdgeBuffer)
                        nodes[i, j].position.x = oldPosition.x + cellHalfWidth - xEdgeBuffer;
                    else if (cellHalfWidth > xEdgeBuffer && nodes[i, j].position.x < oldPosition.x - cellHalfWidth + xEdgeBuffer)
                        nodes[i, j].position.x = oldPosition.x - cellHalfWidth + xEdgeBuffer;
                    if (nodes[i, j].position.y > maxHeight - yEdgeBuffer)
                        nodes[i, j].position.y = maxHeight - yEdgeBuffer;
                    else if (nodes[i, j].position.y < yEdgeBuffer)
                        nodes[i, j].position.y = yEdgeBuffer;
                    else if (cellHalfHeight > yEdgeBuffer && nodes[i, j].position.y > oldPosition.y + cellHalfHeight - yEdgeBuffer)
                        nodes[i, j].position.y = oldPosition.y + cellHalfHeight - yEdgeBuffer;
                    else if (cellHalfHeight > yEdgeBuffer && nodes[i, j].position.y < oldPosition.y - cellHalfHeight + yEdgeBuffer)
                        nodes[i, j].position.y = oldPosition.y - cellHalfHeight + yEdgeBuffer;

                    if (!perturbedStartPoint && startingPoint.x == oldPosition.x && startingPoint.y == oldPosition.y)
                    {
                        startingPoint.x = nodes[i, j].position.x;
                        startingPoint.y = nodes[i, j].position.y;
                        perturbedStartPoint = true;
                    }
                }
            }
        }

        private void AddRoomFromNode(ref int[,] map, MazeNode node, Vector2i scale, int size)
        {
            Vector2i nodeCenter = new Vector2i(node.gridIndex.x * scale.x + scale.x / 2, node.gridIndex.y * scale.y + scale.y / 2);
            Vector2i distance = node.position - nodeCenter;
            int roomX;
            int roomY;
            float distanceMag = (float)Math.Sqrt(distance.x * distance.x + distance.y * distance.y);
            float ratio = distanceMag / size;
            if (ratio < 1)
                ratio = 1;
            Vector2i distanceReduced = new Vector2i((int)(distance.x / ratio), (int)(distance.y / ratio));
            roomX = node.position.x - distanceReduced.x;
            roomY = node.position.y - distanceReduced.y;

            if (distance.x > 0)
            {
                roomX = Math.Max(roomX, nodeCenter.x);
            }
            else
            {
                roomX = Math.Min(roomX, nodeCenter.x);
            }
            if (distance.y > 0)
            {
                roomY = Math.Max(roomY, nodeCenter.y);
            }
            else
            {
                roomY = Math.Min(roomY, nodeCenter.y);
            }

            int rngRoll = _rng.Next(0, 100);
            if (rngRoll < 17)
            {
                AddCRoom(ref map, nodeCenter, node.position, new Vector2i(roomX, roomY), scale, size);
            }
            else if (rngRoll < 34)
            {
                AddIRoom(ref map, nodeCenter, node.position, new Vector2i(roomX, roomY), scale, size);
            }
            else if (rngRoll < 51)
            {
                AddLRoom(ref map, nodeCenter, node.position, new Vector2i(roomX, roomY), scale, size);
            }
            else if (rngRoll < 68)
            {
                AddORoom(ref map, nodeCenter, node.position, new Vector2i(roomX, roomY), scale, size);
            }
            else if (rngRoll < 85)
            {
                AddSolidORoom(ref map, nodeCenter, node.position, new Vector2i(roomX, roomY), scale, size);
            }
            else
            {
                AddXRoom(ref map, nodeCenter, node.position, new Vector2i(roomX, roomY), scale, size);
            }
        }

        private void AddCRoom(ref int[,] map, Vector2i nodeCenter, Vector2i nodePosition, Vector2i adjustedNodePosition, Vector2i scale, int size)
        {
            Vector2 distance = new Vector2(nodePosition.x - adjustedNodePosition.x, nodePosition.y - adjustedNodePosition.y);
            Vector2 normalizedDistance = distance / (float)Math.Sqrt(distance.x * distance.x + distance.y * distance.y);
            Vector2 rotatedNormalizedDistance = new Vector2(normalizedDistance.y, -normalizedDistance.x);
            float openSideAngle = (float)Math.Atan2(rotatedNormalizedDistance.y, rotatedNormalizedDistance.x);
            float openSideAngleHigh = openSideAngle + (float)Math.PI / 4.0f;
            float openSideAngleLow = openSideAngle - (float)Math.PI / 4.0f;

            // Rotate counterclockwise
            if (_rng.Next(0, 2) > 0)
            {
                rotatedNormalizedDistance = new Vector2(-normalizedDistance.y, normalizedDistance.x);
            }
            else // Rotate clockwise
            {
                rotatedNormalizedDistance = new Vector2(normalizedDistance.y, -normalizedDistance.x);
            }

            for (int i = -size; i < size; i++)
            {
                for (int j = -size; j < size; j++)
                {
                    float mag = (float)Math.Sqrt(i * i + j * j);
                    //if (i != 0)
                    //{
                    float angle = (float)Math.Atan2(j, i);
                    if (adjustedNodePosition.x + i < 0 || adjustedNodePosition.x + i > map.GetLength(0) - 1 || adjustedNodePosition.y + j < 0 || adjustedNodePosition.y + j > map.GetLength(1) - 1 || mag > size || mag < size - 1 || (angle < openSideAngleHigh && angle > openSideAngleLow))
                    {
                        continue;
                    }
                    else
                    {
                        map[adjustedNodePosition.x + i, adjustedNodePosition.y + j] = 2;
                    }
                }
            }
        }

        private void AddIRoom(ref int[,] map, Vector2i nodeCenter, Vector2i nodePosition, Vector2i adjustedNodePosition, Vector2i scale, int size)
        {
            Vector2 distance = new Vector2(nodePosition.x - adjustedNodePosition.x, nodePosition.y - adjustedNodePosition.y);
            Vector2 normalizedDistance = distance / (float)Math.Sqrt(distance.x * distance.x + distance.y * distance.y);
            Vector2 rotatedNormalizedDistance;
            // Rotate counterclockwise
            if (_rng.Next(0, 2) > 0)
            {
                rotatedNormalizedDistance = new Vector2(-normalizedDistance.y, normalizedDistance.x);
            }
            else // Rotate clockwise
            {
                rotatedNormalizedDistance = new Vector2(normalizedDistance.y, -normalizedDistance.x);
            }

            for (int i = -size; i < size; i++)
            {
                // Add center of I
                int xPos = (int)(adjustedNodePosition.x + (i * normalizedDistance.x));
                int yPos = (int)(adjustedNodePosition.y + (i * normalizedDistance.y));
                if (xPos < 0 || xPos > map.GetLength(0) - 1 || yPos < 0 || yPos > map.GetLength(1) - 1)
                {
                    continue;
                }
                else
                {
                    map[xPos, yPos] = 2;
                }

                // Add other parts of I
                if (i == -size || i == size - 1)
                {
                    for (int j = -size; j < size; j++)
                    {
                        int x2Pos = (int)(xPos + (j * rotatedNormalizedDistance.x));
                        int y2Pos = (int)(yPos + (j * rotatedNormalizedDistance.y));

                        if (x2Pos < 0 || x2Pos > map.GetLength(0) - 1 || y2Pos < 0 || y2Pos > map.GetLength(1) - 1)
                        {
                            continue;
                        }
                        else
                        {
                            map[x2Pos, y2Pos] = 2;
                        }
                    }
                }
            }
        }

        private void AddLRoom(ref int[,] map, Vector2i nodeCenter, Vector2i nodePosition, Vector2i adjustedNodePosition, Vector2i scale, int size)
        {
            Vector2 distance = new Vector2(nodePosition.x - adjustedNodePosition.x, nodePosition.y - adjustedNodePosition.y);
            Vector2 normalizedDistance = distance / (float)Math.Sqrt(distance.x * distance.x + distance.y * distance.y);
            Vector2 rotatedNormalizedDistance;
            // Rotate counterclockwise
            if (_rng.Next(0, 2) > 0)
            {
                rotatedNormalizedDistance = new Vector2(-normalizedDistance.y, normalizedDistance.x);
            }
            else // Rotate clockwise
            {
                rotatedNormalizedDistance = new Vector2(normalizedDistance.y, -normalizedDistance.x);
            }

            for (int i = 0; i < 2 * size; i++)
            {
                // Add one part of the L
                int xPos = (int)(adjustedNodePosition.x + (i * normalizedDistance.x));
                int yPos = (int)(adjustedNodePosition.y + (i * normalizedDistance.y));
                if (xPos < 0 || xPos > map.GetLength(0) - 1 || yPos < 0 || yPos > map.GetLength(1) - 1)
                {
                    continue;
                }
                else
                {
                    map[xPos, yPos] = 2;
                }
                // Add the other part of the L
                xPos = (int)(adjustedNodePosition.x + (i * rotatedNormalizedDistance.x));
                yPos = (int)(adjustedNodePosition.y + (i * rotatedNormalizedDistance.y));
                if (xPos < 0 || xPos > map.GetLength(0) - 1 || yPos < 0 || yPos > map.GetLength(1) - 1)
                {
                    continue;
                }
                else
                {
                    map[xPos, yPos] = 2;
                }
            }
        }

        private void AddORoom(ref int[,] map, Vector2i nodeCenter, Vector2i nodePosition, Vector2i adjustedNodePosition, Vector2i scale, int size)
        {
            for (int i = -size; i < size; i++)
            {
                for (int j = -size; j < size; j++)
                {
                    float mag = (float)Math.Sqrt(i * i + j * j);
                    if (adjustedNodePosition.x + i < 0 || adjustedNodePosition.x + i > map.GetLength(0) - 1 || adjustedNodePosition.y + j < 0 || adjustedNodePosition.y + j > map.GetLength(1) - 1 || mag > size || mag < size - 1)
                    {
                        continue;
                    }
                    else
                    {
                        map[adjustedNodePosition.x + i, adjustedNodePosition.y + j] = 2;
                    }
                }
            }
        }

        private void AddSolidORoom(ref int[,] map, Vector2i nodeCenter, Vector2i nodePosition, Vector2i adjustedNodePosition, Vector2i scale, int size)
        {
            for (int i = -size; i < size; i++)
            {
                for (int j = -size; j < size; j++)
                {
                    if (adjustedNodePosition.x + i < 0 || adjustedNodePosition.x + i > map.GetLength(0) - 1 || adjustedNodePosition.y + j < 0 || adjustedNodePosition.y + j > map.GetLength(1) - 1 || Math.Sqrt(i * i + j * j) > size)
                    {
                        continue;
                    }
                    else
                    {
                        map[adjustedNodePosition.x + i, adjustedNodePosition.y + j] = 2;
                    }
                }
            }
        }

        private void AddXRoom(ref int[,] map, Vector2i nodeCenter, Vector2i nodePosition, Vector2i adjustedNodePosition, Vector2i scale, int size)
        {
            Vector2 distance = new Vector2(nodePosition.x - adjustedNodePosition.x, nodePosition.y - adjustedNodePosition.y);
            Vector2 normalizedDistance = distance / (float)Math.Sqrt(distance.x * distance.x + distance.y * distance.y);
            Vector2 rotatedNormalizedDistance;
            // Rotate counterclockwise
            if (_rng.Next(0, 2) > 0)
            {
                rotatedNormalizedDistance = new Vector2(-normalizedDistance.y, normalizedDistance.x);
            }
            else // Rotate clockwise
            {
                rotatedNormalizedDistance = new Vector2(normalizedDistance.y, -normalizedDistance.x);
            }

            for (int i = -size; i < size; i++)
            {
                // Add one part of the X
                int xPos = (int)(adjustedNodePosition.x + (i * normalizedDistance.x));
                int yPos = (int)(adjustedNodePosition.y + (i * normalizedDistance.y));
                if (xPos < 0 || xPos > map.GetLength(0) - 1 || yPos < 0 || yPos > map.GetLength(1) - 1)
                {
                    continue;
                }
                else
                {
                    map[xPos, yPos] = 2;
                }
                // Add the other part of the X
                xPos = (int)(adjustedNodePosition.x + (i * rotatedNormalizedDistance.x));
                yPos = (int)(adjustedNodePosition.y + (i * rotatedNormalizedDistance.y));
                if (xPos < 0 || xPos > map.GetLength(0) - 1 || yPos < 0 || yPos > map.GetLength(1) - 1)
                {
                    continue;
                }
                else
                {
                    map[xPos, yPos] = 2;
                }
            }
        }

        private int ChooseIndex(int activeCellCount)
        {
            return activeCellCount - 1;
        }

        private Vector2i GetDirectionMovement(Direction direction)
        {
            if (direction == Direction.NORTH)
                return new Vector2i(0, 1);
            //else if (direction == Direction.NorthEast)
            //    return new Vector2i(1, 1);
            else if (direction == Direction.EAST)
                return new Vector2i(1, 0);
            //else if (direction == Direction.SouthEast)
            //    return new Vector2i(1, -1);
            else if (direction == Direction.SOUTH)
                return new Vector2i(0, -1);
            //else if (direction == Direction.SouthWest)
            //    return new Vector2i(-1, -1);
            else //if (direction == Direction.West)
                return new Vector2i(-1, 0);
            //else //direction == Direction.NorthWest
            //return new Vector2i(-1, 1);
        }

        private Direction GetInverseDirection(Direction direction)
        {
            if (direction == Direction.NORTH)
                return Direction.SOUTH;
            //else if (direction == Direction.NorthEast)
            //    return Direction.SouthWest;
            else if (direction == Direction.EAST)
                return Direction.WEST;
            //else if (direction == Direction.SouthEast)
            //    return Direction.NorthWest;
            else if (direction == Direction.SOUTH)
                return Direction.NORTH;
            //else if (direction == Direction.SouthWest)
            //    return Direction.NorthEast;
            else //if (direction == Direction.West)
                return Direction.EAST;
            //else //direction == Direction.NorthWest
            //    return Direction.SouthEast;
        }

        private Direction[] GetDirections()//int diagonalChance)
        {
            Direction[] directions = new Direction[4];

            Array array = Enum.GetValues(typeof(Direction));
            //List<Direction> directionsToGet = new List<Direction>();
            List<Direction> cardinalDirectionsToGet = new List<Direction>();
            //List<Direction> diagonalDirectionsToGet = new List<Direction>();

            for (int i = 0; i < array.Length; i++)
            {
                Direction cur = (Direction)array.GetValue(i);
                //directionsToGet.Add(cur);
                //if (cur == Direction.North || cur == Direction.East || cur == Direction.South || cur == Direction.West)
                cardinalDirectionsToGet.Add(cur);
                cardinalDirectionsToGet.Remove(Direction.error);
                //else
                //    diagonalDirectionsToGet.Add(cur);

            }

            for (int i = 0; i < directions.Length; i++)
            {
                //int index = UnityEngine.Random.Range(0, directionsToGet.Count);
                //directions[i] = directionsToGet[index];
                //directionsToGet.RemoveAt(index);
                //if (diagonalDirectionsToGet.Count > 0 && (cardinalDirectionsToGet.Count == 0 || UnityEngine.Random.Range(0, 100) < diagonalChance))
                //{
                //    int index = UnityEngine.Random.Range(0, diagonalDirectionsToGet.Count);
                //    directions[i] = diagonalDirectionsToGet[index];
                //    diagonalDirectionsToGet.RemoveAt(index);
                //}
                //else
                //{
                //int index = UnityEngine.Random.Range(0, cardinalDirectionsToGet.Count);
                int index = _rng.Next(0, cardinalDirectionsToGet.Count);
                directions[i] = cardinalDirectionsToGet[index];
                cardinalDirectionsToGet.RemoveAt(index);
                //}
            }

            return directions;
        }

        /// <summary>
        /// Get a random walk path by giving each direction a percentage chance of occuring
        /// </summary>
        /// <param name="mapWidth"></param>
        /// <param name="mapHeight"></param>
        /// <param name="startingPoint"></param>
        /// <param name="pathLength"></param>
        /// <param name="northChance"></param>
        /// <param name="eastChance"></param>
        /// <param name="southChance"></param>
        /// <param name="westChance"></param>
        /// <returns></returns>
        private int[,] GetRandomWalkPath(int mapWidth, int mapHeight, Vector2i startingPoint, int pathLength, int northChance, int eastChance, int southChance, int westChance)
        {
            int[,] map = new int[mapWidth, mapHeight];

            map[startingPoint.x, startingPoint.y] = 2;

            int count = 0;
            Vector2i lastPoint = new Vector2i(startingPoint.x, startingPoint.y);

            while (count < pathLength)
            {
                Direction direction = GetRandomDirection(northChance, eastChance, southChance, westChance);

                Vector2i newPoint = lastPoint + GetDirectionMovement(direction);

                if (newPoint.x < map.GetLength(0) && newPoint.x > 0 && newPoint.y < map.GetLength(1) && newPoint.y > 0)
                {
                    map[newPoint.x, newPoint.y] = 2;
                    lastPoint = newPoint;
                }

                count++;
            }

            return map;
        }

        /// <summary>
        /// Get random walk path by giving a start and an end point
        /// </summary>
        /// <param name="mapWidth"></param>
        /// <param name="mapHeight"></param>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        private int[,] GetRandomWalkPath(int mapWidth, int mapHeight, Vector2i startPoint, Vector2i endPoint)
        {
            int[,] map = new int[mapWidth, mapHeight];

            int count = 0;

            map[startPoint.x, startPoint.y] = 2;
            map[endPoint.x, endPoint.y] = 2;

            Vector2i prevStartPoint = new Vector2i(startPoint.x, startPoint.y);
            Vector2i prevEndPoint = new Vector2i(endPoint.x, endPoint.y);
            float startDistance = (float)Math.Sqrt((endPoint.x - startPoint.x) * (endPoint.x - startPoint.x) + (endPoint.y - startPoint.y) * (endPoint.y - startPoint.y));

            while ((prevStartPoint.x != prevEndPoint.x || prevStartPoint.y != prevEndPoint.y) && count < 10000)
            {
                Direction direction = GetRandomDirection(prevStartPoint, prevEndPoint, startDistance);

                Vector2i newStartPoint = prevStartPoint + GetDirectionMovement(direction);

                if (newStartPoint.x < map.GetLength(0) && newStartPoint.x > 0 && newStartPoint.y < map.GetLength(1) && newStartPoint.y > 0)
                {
                    map[newStartPoint.x, newStartPoint.y] = 2;
                    prevStartPoint = newStartPoint;
                }

                if (prevStartPoint.x == prevEndPoint.x && prevStartPoint.y == prevEndPoint.y)
                    break;

                direction = GetRandomDirection(prevEndPoint, prevStartPoint, startDistance);

                Vector2i newEndPoint = prevEndPoint + GetDirectionMovement(direction);

                if (newEndPoint.x < map.GetLength(0) && newEndPoint.x > 0 && newEndPoint.y < map.GetLength(1) && newEndPoint.y > 0)
                {
                    map[newEndPoint.x, newEndPoint.y] = 2;
                    prevEndPoint = newEndPoint;
                }

                count++;
            }

            return map;
        }

        /// <summary>
        /// Add a random walk path to an existing map by giving a start and an end point
        /// </summary>
        /// <param name="map"></param>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        private int[,] AddRandomWalkPath(int[,] map, Vector2i startPoint, Vector2i endPoint, int columnNumber, int rowNumber, int cellSize, int xEdgeBuffer, int yEdgeBuffer, bool horizontalMovement)
        {
            int count = 0;

            map[startPoint.x, startPoint.y] = 2;
            map[endPoint.x, endPoint.y] = 2;

            int maxEdgeDistance = 0;
            int minEdgeDistance = 0;

            if (horizontalMovement)
            {
                maxEdgeDistance = rowNumber * cellSize + cellSize - xEdgeBuffer;
                minEdgeDistance = rowNumber * cellSize + xEdgeBuffer;
            }
            else
            {
                maxEdgeDistance = columnNumber * cellSize + cellSize - yEdgeBuffer;
                minEdgeDistance = columnNumber * cellSize + yEdgeBuffer;
            }

            Vector2i prevStartPoint = new Vector2i(startPoint.x, startPoint.y);
            Vector2i prevEndPoint = new Vector2i(endPoint.x, endPoint.y);
            float startDistance = (float)Math.Sqrt((endPoint.x - startPoint.x) * (endPoint.x - startPoint.x) + (endPoint.y - startPoint.y) * (endPoint.y - startPoint.y));

            while ((prevStartPoint.x != prevEndPoint.x || prevStartPoint.y != prevEndPoint.y) && count < 10000)
            {
                Direction direction = GetRandomDirection(prevStartPoint, prevEndPoint, startDistance);

                Vector2i newStartPoint = prevStartPoint + GetDirectionMovement(direction);

                if (newStartPoint.x < map.GetLength(0) && newStartPoint.x > 0 && newStartPoint.y < map.GetLength(1) && newStartPoint.y > 0)
                {
                    if (horizontalMovement && (newStartPoint.y < maxEdgeDistance && newStartPoint.y > minEdgeDistance) ||
                        !horizontalMovement && (newStartPoint.x < maxEdgeDistance && newStartPoint.x > minEdgeDistance))
                    {
                        map[newStartPoint.x, newStartPoint.y] = 2;
                        prevStartPoint = newStartPoint;
                    }
                }

                if (prevStartPoint.x == prevEndPoint.x && prevStartPoint.y == prevEndPoint.y)
                    break;

                direction = GetRandomDirection(prevEndPoint, prevStartPoint, startDistance);

                Vector2i newEndPoint = prevEndPoint + GetDirectionMovement(direction);

                if (newEndPoint.x < map.GetLength(0) && newEndPoint.x > 0 && newEndPoint.y < map.GetLength(1) && newEndPoint.y > 0)
                {
                    map[newEndPoint.x, newEndPoint.y] = 2;
                    prevEndPoint = newEndPoint;
                }

                count++;
            }

            return map;
        }

        private Direction GetRandomDirection(int northChance, int eastChance, int southChance, int westChance)
        {
            if (northChance < 0 || eastChance < 0 || southChance < 0 || westChance < 0)
                throw new ArgumentException("Chances cannot be negative");

            //int roll = UnityEngine.Random.Range(0, 100);
            int roll = _rng.Next(0, 100);

            if (roll < northChance)
            {
                return Direction.NORTH;
            }
            else if (roll < northChance + eastChance)
            {
                return Direction.EAST;
            }
            else if (roll < northChance + eastChance + southChance)
            {
                return Direction.SOUTH;
            }
            else //if (roll < northChance + eastChance + southChance + westChance)
            {
                return Direction.WEST;
            }
        }

        private Direction GetRandomDirection(Vector2i currentPoint, Vector2i targetPoint, float startDistance)
        {
            int northChance = 0;
            int eastChance = 0;
            int southChance = 0;
            int westChance = 0;

            // Allow slightly more drift in one coordinate if the other coordinate has a much greater distance
            Vector2i distance = currentPoint - targetPoint;
            float distanceMag = (float)Math.Sqrt(distance.x * distance.x + distance.y * distance.y);
            float distanceRatio = (float)Math.Min(1, distanceMag / startDistance);
            int yOverX;
            int xOverY;
            int driftAmount = 20;
            int convergeAmount = 20;

            // Clamp the drift to a maximum, and take the maximum if we would divide by zero
            //yOverX = Mathf.Abs(distance.x) > 0 ? Mathf.Min(driftAmount, Mathf.Abs(Mathf.RoundToInt(driftAmount * distance.y / distance.x))) : driftAmount;
            //xOverY = Mathf.Abs(distance.y) > 0 ? Mathf.Min(driftAmount, Mathf.Abs(Mathf.RoundToInt(driftAmount * distance.x / distance.y))) : driftAmount;
            yOverX = Math.Abs(distance.x) > 0 ? Math.Min(driftAmount, Math.Abs((int)Math.Round((float)distance.y / distance.x))) : driftAmount;
            xOverY = Math.Abs(distance.y) > 0 ? Math.Min(driftAmount, Math.Abs((int)Math.Round((float)distance.x / distance.y))) : driftAmount;

            // Equation for the greater chance is: 25 + (ConvergeAmount * (1 - distance / distanceMax) ^ 2) + driftAmount * (distance / distanceMax) ^ 2 * axisDistance / otherAxisDistance
            // 25 is the base that we can't go lower than. ConvergeAmount makes the two paths converge when they get closer to each other , and drift amount adds drift if the axis is smaller 
            // than the other axis
            if (distance.x > 0)
            {
                westChance = Math.Min(50, 25 + (int)Math.Round(convergeAmount * Math.Pow((1 - distanceRatio), 2) + driftAmount * Math.Pow(distanceRatio, 2) * xOverY));
                eastChance = 50 - westChance;
            }
            else if (distance.x < 0)
            {
                eastChance = Math.Min(50, 25 + (int)Math.Round(convergeAmount * Math.Pow((1 - distanceRatio), 2) + driftAmount * Math.Pow(distanceRatio, 2) * xOverY));
                westChance = 50 - eastChance;
            }
            else
            {
                eastChance = 25;
                westChance = 25;
            }
            if (distance.y > 0)
            {
                southChance = Math.Min(50, 25 + (int)Math.Round(convergeAmount * Math.Pow((1 - distanceRatio), 2) + driftAmount * Math.Pow(distanceRatio, 2) * yOverX));
                northChance = 50 - southChance;
            }
            else if (distance.y < 0)
            {
                northChance = Math.Min(50, 25 + (int)Math.Round(convergeAmount * Math.Pow((1 - distanceRatio), 2) + driftAmount * Math.Pow(distanceRatio, 2) * yOverX));
                southChance = 50 - northChance;
            }
            else
            {
                northChance = 25;
                southChance = 25;
            }

            //int roll = UnityEngine.Random.Range(0, 100);
            int roll = _rng.Next(0, 100);

            if (roll < northChance)
            {
                return Direction.NORTH;
            }
            else if (roll < northChance + eastChance)
            {
                return Direction.EAST;
            }
            else if (roll < northChance + eastChance + southChance)
            {
                return Direction.SOUTH;
            }
            else //if (roll < northChance + eastChance + southChance + westChance)
            {
                return Direction.WEST;
            }
        }
    }
}
