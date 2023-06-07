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
    public List<int> BrickSize;
    public int Hp;
    public int Atk;
    public int LvFreeze;
    public float Speed;
    public Vector3 Direction;
    public bool isFreeze;
    public bool isBurn;
    public bool movable;
    //public int numBallEat;

    public BrickData()
    {
        Id = -1;
        
    }


}
