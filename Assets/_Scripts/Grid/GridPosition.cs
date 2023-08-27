using System;

namespace Grid
{
    public readonly struct GridPosition : IEquatable<GridPosition>
    {
        public readonly int X;
        public readonly int Z;

        public GridPosition(int x, int z)
        {
            X = x;
            Z = z;
        }

        public override string ToString() => $"x: {X}; z: {Z}";

        #region Operators
        public static bool operator ==(GridPosition a, GridPosition b) => a.X == b.X && a.Z == b.Z;

        public static bool operator !=(GridPosition a, GridPosition b) => !(a == b);
        
        public bool Equals(GridPosition other) => this == other;
        
        public override bool Equals(object obj) => obj is GridPosition other && Equals(other);
        
        public override int GetHashCode() => HashCode.Combine(X, Z);
        #endregion
    }
}