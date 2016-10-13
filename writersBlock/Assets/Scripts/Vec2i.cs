using UnityEngine;
using System.Collections;

public struct Vec2i
{
    public int x;
    public int y;

    public Vec2i(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public static Vec2i operator +(Vec2i left, Vec2i right)
    {
        return new Vec2i(left.x + right.x, left.y + right.y);
    }

    public static Vec2i operator -(Vec2i left, Vec2i right)
    {
        return new Vec2i(left.x - right.x, left.y - right.y);
    }

    public override string ToString()
    {
        return "{" + x + "," + y + "}";
    }

    public override bool Equals(object obj)
    {
        if (obj == null)
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != typeof(Vec2i))
            return false;
        Vec2i other = (Vec2i)obj;
        return x == other.x && y == other.y;
    }


    public override int GetHashCode()
    {
        unchecked
        {
            return x.GetHashCode() ^ y.GetHashCode();
        }
    }

    public Vector3 ToVec3()
    {
        return new Vector3(x, 0, y);
    }

    public static Vector3 Lerp(Vec2i a, Vec2i b, float t, float y)
    {
        return Vector3.Lerp(new Vector3(a.x, y, a.y), new Vector3(b.x, y, b.y), t);   
    }
}
