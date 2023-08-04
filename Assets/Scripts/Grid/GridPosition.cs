using System;

public struct GridPosition : IEquatable<GridPosition> {
    public int x;
    public int z;

    public GridPosition(int x, int z) {
        this.x = x;
        this.z = z;
    }

    public override readonly string ToString()
    {
        return "x: " + x + "; z: " + z;
    }

    public static bool operator ==(GridPosition a, GridPosition b) {
        return a.x == b.x && a.z == b.z;
    }

    public static bool operator !=(GridPosition a, GridPosition b) {
        return !(a == b);
    }

    public override readonly bool Equals(object obj)
    {
        return obj is GridPosition position &&
               x == position.x &&
               z == position.z;
    }

    public override readonly int GetHashCode()
    {
        return HashCode.Combine(x, z);
    }

    public readonly bool Equals(GridPosition other)
    {
        return this == other;
    }

    public static GridPosition operator +(GridPosition a, GridPosition b) {
        return new GridPosition(a.x + b.x, a.z + b.z);
    }
    public static GridPosition operator -(GridPosition a, GridPosition b) {
        return new GridPosition(a.x - b.x, a.z - b.z);
    }

    public readonly GridPosition North() {
        return this + new GridPosition(0, 1);
    }
    
    public readonly GridPosition South() {
        return this + new GridPosition(0, -1);
    }
    
    public readonly GridPosition East() {
        return this + new GridPosition(1, 0);
    }
    
    public readonly GridPosition West() {
        return this + new GridPosition(-1, 0);
    }
}