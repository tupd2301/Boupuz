using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInfo : MonoBehaviour
{
    // [System.Serializable]
    // public enum LevelType
    // {
    //     Action = 1,
    //     Puzzle = 2
    // }

    //public LevelType levelType;
    //public int LevelTurn;
    
    [SerializeField]
    private int _rewardCoins;
    public int RewardCoins { get => _rewardCoins; set => _rewardCoins = value; }
    

    [SerializeField]
    private int _numAddItem;
    public int NumAddItem { get => _numAddItem; set => _numAddItem = value; }
    

    [SerializeField]
    private int _numDamageItem;
    public int NumDamageItem { get => _numDamageItem; set => _numDamageItem = value; }

    [SerializeField] private bool _haveTutorial = false;
    public bool HaveTutorial { get => _haveTutorial; set => _haveTutorial = value; }
}
