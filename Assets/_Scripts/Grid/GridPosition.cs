using System;
using System.Diagnostics.CodeAnalysis;

namespace Grid
{
    public readonly struct GridPosition : IEquatable<GridPosition>
    {
        [SuppressMessage("ReSharper", "InconsistentNaming")] 
        public readonly int x;
        [SuppressMessage("ReSharper", "InconsistentNaming")] 
        public readonly int z;

        public GridPosition(int x, int z)
        {
            this.x = x;
            this.z = z;
        }

        public override string ToString() => $"x: {x}; z: {z}";

        #region Operators
        public static bool operator ==(GridPosition a, GridPosition b) => a.x == b.x && a.z == b.z;

        public static bool operator !=(GridPosition a, GridPosition b) => !(a == b);
        
        public bool Equals(GridPosition other) => this == other;
        
        public override bool Equals(object obj) => obj is GridPosition other && Equals(other);
        
        public override int GetHashCode() => HashCode.Combine(x, z);

        public static GridPosition operator +(GridPosition a, GridPosition b) => new(a.x + b.x, a.z + b.z);
        
        public static GridPosition operator -(GridPosition a, GridPosition b) => new(a.x - b.x, a.z - b.z);
        #endregion
    }
}