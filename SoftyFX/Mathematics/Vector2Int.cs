using System;

namespace SoftyFX.Mathematics
{
    public struct Vector2Int
    {
        public static readonly Vector2Int Zero = new Vector2Int(0, 0);
        public static readonly Vector2Int One = new Vector2Int(1, 1);
        public static readonly Vector2Int Right = new Vector2Int(1, 0);
        public static readonly Vector2Int Up = new Vector2Int(0, 1);
        
        public int X;
        public int Y;
        
        public Vector2Int(int x, int y)
        {
            X = x;
            Y = y;
        }
        
        public int this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return X;
                    case 1:
                        return Y;
                }
                throw new IndexOutOfRangeException();
            }
        }
        
        public static Vector2Int operator +(Vector2Int left, Vector2Int right)
        {
            left.X += right.X;
            left.Y += right.Y;
            return left;
        }
        
        public static Vector2Int operator -(Vector2Int left, Vector2Int right)
        {
            left.X -= right.X;
            left.Y -= right.Y;
            return left;
        }
        
        public static Vector2Int operator *(Vector2Int left, Vector2Int right)
        {
            left.X *= right.X;
            left.Y *= right.Y;
            return left;
        }
        
        public static Vector2Int operator *(Vector2Int vector, int factor)
        {
            vector.X *= factor;
            vector.Y *= factor;
            return vector;
        }
        
        public static Vector2Int operator /(Vector2Int left, Vector2Int right)
        {
            left.X /= right.X;
            left.Y /= right.Y;
            return left;
        }
        
        public static Vector2Int operator /(Vector2Int vector, int factor)
        {
            vector.X /= factor;
            vector.Y /= factor;
            return vector;
        }
    }
}