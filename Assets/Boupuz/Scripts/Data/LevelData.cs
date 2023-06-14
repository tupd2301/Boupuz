using System.Collections.Generic;
using UnityEngine;

public class LevelData : MonoBehaviour
{
    public int LevelID { get; private set; }
    public int maxBallNumber  { get; private set; }
    public int currentBallNumber { get; private set; }
    public int numTurn { get; private set; }
    public int currentTurn { get; private set; }
    public int ballDamage { get; private set; }
    [SerializeField] private int _totalBricks;
    public int totalBricks { get {return _totalBricks;}}
    [SerializeField] private int _destroyedBricks;
    public int destroyedBricks { get {return _destroyedBricks;}}
    [SerializeField] private int _numCandies;
    public int numCandies { get {return _numCandies;}}

    public LevelData()
    {
        LevelID = -1;
        _totalBricks = 0;
        _destroyedBricks = 0;
        _numCandies = 0;
    }

    public void UpdateDestroyedBricks()
    {
        _destroyedBricks += 1;
    }

    public void GetTotalBricks(int value)
    {
        _totalBricks = value;
    }

    public void AddCandies(int value)
    {
        _numCandies += value;
    }

}