using System;
using System.Collections.Generic;

namespace лр1
{
    public struct Vector2
    {
        public float X;
        public float Y;

        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public static Vector2 operator *(double d, Vector2 v)
        {
            return new Vector2((float)(d * v.X), (float)(d * v.Y));
        }

        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X + b.X, a.Y + b.Y);
        }

        public static Vector2 Average(List<Vector2> vectors)
        {
            float x = 0f;
            float y = 0f;

            foreach (Vector2 vector in vectors)
            {
                x += vector.X;
                y += vector.Y;
            }

            return new Vector2(x / vectors.Count, y / vectors.Count);
        }
    }
}
