using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapGenerator
{
    public struct Vector2
    {
        public int x;
        public int y;

        public Vector2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static Vector2 operator +(Vector2 vector1, Vector2 vector2)
        {
            return new Vector2(vector1.x + vector2.x, vector1.y + vector2.y);
        }

        public static Vector2 operator -(Vector2 vector1, Vector2 vector2)
        {
            return new Vector2(vector1.x - vector2.x, vector1.y - vector2.y);
        }

        public static Vector2 operator *(Vector2 vector, int factor)
        {
            return new Vector2(vector.x * factor, vector.y * factor);
        }

        public static Vector2 operator /(Vector2 vector, int factor)
        {
            return new Vector2(vector.x / factor, vector.y / factor);
        }

        public static Vector2 operator /(Vector2 vector, float factor)
        {
            return new Vector2((int)(vector.x / factor), (int)(vector.y / factor));
        }
    }
}
