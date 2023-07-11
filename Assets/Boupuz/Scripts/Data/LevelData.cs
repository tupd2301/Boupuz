using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : MonoBehaviour
{
    public int LevelID { get; private set; }
    public int maxBallNumber  { get; private set; }
    public int currentBallNumber { get; private set; }
    [SerializeField]
    private int _totalTurn;
    public int TotalTurn { get => _totalTurn; set => _totalTurn = value; }
    [SerializeField]
    private int _currentTurn;
    public int CurrentTurn { get => _currentTurn; set => _currentTurn = value; }
    [SerializeField]
    private int _ballDamage;
    public int BallDamage { get => _ballDamage; set => _ballDamage = value; }
    [SerializeField] private int _totalBricks;
    public int totalBricks { get {return _totalBricks;}}
    [SerializeField] private int _destroyedBricks;
    //public int destroyedBricks { get {return DestroyedBricks;}}
    [SerializeField] private int _totalCake;
    public int TotalCake { get {return _totalCake;}}
    [SerializeField] private int _collectedCake;
    public int CollectedCake { get {return _collectedCake;}}
    [SerializeField] private int _numCandies;
    public int numCandies { get {return _numCandies;}}
    [SerializeField] private int _numCoins;
    public int numCoins { get {return _numCoins;}}

    public int DestroyedBricks { get => _destroyedBricks; set => _destroyedBricks = value; }

    private int levelSelect;

    public LevelData()
    {
        LevelID = -1;
        _totalBricks = 0;
        DestroyedBricks = 0;
        _numCandies = 0;
        //_numCoins = PlayerPrefs.GetInt("coins");
        _numCoins = 9999;
    }

    public void UpdateDestroyedBricks()
    {
        DestroyedBricks += 1;
    }

    public void GetTotalBricks(int value)
    {
        _totalBricks = value;
    }

    public void GetTotalCakes(int value)
    {
        _totalCake = value;
    }

    public void AddCandies(int value)
    {
        _numCandies += value;
        if (_numCandies % 5 == 0 && levelSelect <_numCandies)
        {
            levelSelect = _numCandies;
            GameFlow.Instance.SelectSkill = true;
        }
    }

    public void AddCoins(int value)
    {
        _numCoins += value;
    }

    public void DecreaseTurn(int value)
    {
        CurrentTurn -= value;
    }

    public void UpdateCollectedCake()
    {
        _collectedCake += 1;
    }
}