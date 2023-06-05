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
public class BrickData : MonoBehaviour
{
    public int Id;
    public GridCoordinate BrickCoordinate;
    public List<int> BrickSize;
    public int Hp;
    public int Atk;
    public int LvFreeze;
    public float Speed;
    public Vector3 Direction;
    public bool isFreeze;
    public bool isBurn;

    public BrickData()
    {
        Id = -1;
        
    }


}
