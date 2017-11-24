using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapGenerator
{
    class MazeNode
    {
        public Dictionary<Direction, MazeNode> connections;
        public Vector2i position;
        public Vector2i gridIndex;
        public int distanceFromStart;

        public MazeNode(int x, int y)
        {
            connections = new Dictionary<Direction, MazeNode>();

            position = new Vector2i(x, y);
            gridIndex = new Vector2i(x, y);

            distanceFromStart = -1;
        }

        public int GetDistance(MazeNode cameFrom, MazeNode targetNode, int currentCount)
        {
            if (currentCount > 20)
                return 20;
            if (this == targetNode)
                return currentCount + 1;
            else
            {
                int smallestDistance = 9999;
                foreach (MazeNode connection in connections.Values)
                {
                    if (connection == cameFrom)
                        continue;

                    int distance = connection.GetDistance(this, targetNode, currentCount + 1);
                    if (distance < smallestDistance)
                        smallestDistance = distance;
                }

                if (smallestDistance > 0)
                    return smallestDistance;
            }

            return -1;
        }

        public Direction LastDirection(MazeNode cameFrom, Direction currentDirection)
        {
            foreach (Direction connection in connections.Keys)
            {
                // Skip connections that are in a straight line from the last direction
                if (connections[connection] == cameFrom || connection == currentDirection)
                    continue;

                // If we made it here, we are going in a different direction
                return connection;
            }

            if (connections.ContainsKey(currentDirection))
            {
                // If we didn't find a different direction, move to the next node
                return connections[currentDirection].LastDirection(this, currentDirection);
            }
            else
            {
                return Direction.error;
            }
        }
    }
}
