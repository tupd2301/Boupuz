using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct GridCoordinate
{
    public int X;
    public int Y;

    public GridCoordinate(int x, int y)
    {
        X = x;
        Y = y;
    }

    public static float Distance(GridCoordinate c1, GridCoordinate c2)
    {
        float distance = Mathf.Abs(Mathf.Pow(c1.X - c2.X, 2) + Mathf.Pow(c1.Y - c2.Y, 2));
                                                
        return distance;
    }

    public static bool IsNegative(GridCoordinate c)
    {
        return (c.X < 0) && c.Y < 0;
    }

    public static bool operator == (GridCoordinate c1, GridCoordinate c2)
    {
        return (c1.X == c2.X) & (c1.Y == c2.Y);
    }

    public static bool operator != (GridCoordinate c1, GridCoordinate c2)
    {
        return (c1.X == c2.X) | (c1.Y == c2.Y);
    }

    public static GridCoordinate operator +(GridCoordinate c, Vector3 addition)
    {
        return new GridCoordinate(c.X + (int)addition.x, c.Y + (int)addition.y);
    }

    public static GridCoordinate operator +(GridCoordinate c1, GridCoordinate c2)
    {
        return new GridCoordinate(c1.X + c2.X, c1.Y + c2.Y);
    }

    public static GridCoordinate operator -(GridCoordinate c, int subtraction)
    {
        return new GridCoordinate(c.X + subtraction, c.Y + subtraction);
    }

    public static GridCoordinate operator *(GridCoordinate c, int multiplier)
    {
        return new GridCoordinate(c.X * multiplier, c.Y * multiplier);
    }

    public static GridCoordinate operator /(GridCoordinate c, int multiplier)
    {
        return new GridCoordinate(c.X / multiplier, c.Y / multiplier);
    }

    public override bool Equals(object o)
    {
        if (o == null)
            return false;

        GridCoordinate c = (GridCoordinate)o;
        return (X == c.X & Y == c.Y);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}

[System.Serializable]
public enum ObjectType
{
    //TODO
    Brickie = 1,
    Item = 2,
    Special = 3,
}

[System.Serializable]
public class BrickData : MonoBehaviour
{
    public int Id;
    public ObjectType Type;
    public GridCoordinate BrickCoordinate;
    //public List<int> BrickSize;
    public int maxHp;
    public int Hp;
    public int Atk;
    public int Speed;
    public Vector3 Direction;
    public bool isFreeze;
    public int LvFreeze;
    public bool isBurn;
    public bool movable;
    public bool hasCandy;
    //public int numBallEat;

    public BrickData()
    {
        Id = -1;
        isFreeze = !movable;
    }


}
