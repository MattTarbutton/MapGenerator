using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapGenerator
{
    public struct Vector2i
    {
        public int x;
        public int y;

        public Vector2i(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static Vector2i operator +(Vector2i vector1, Vector2i vector2)
        {
            return new Vector2i(vector1.x + vector2.x, vector1.y + vector2.y);
        }

        public static Vector2i operator -(Vector2i vector1, Vector2i vector2)
        {
            return new Vector2i(vector1.x - vector2.x, vector1.y - vector2.y);
        }

        public static Vector2i operator *(Vector2i vector, int factor)
        {
            return new Vector2i(vector.x * factor, vector.y * factor);
        }

        public static Vector2i operator /(Vector2i vector, int factor)
        {
            return new Vector2i(vector.x / factor, vector.y / factor);
        }

        public static Vector2i operator /(Vector2i vector, float factor)
        {
            return new Vector2i((int)(vector.x / factor), (int)(vector.y / factor));
        }
    }
}
