using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInfo : MonoBehaviour
{
    [System.Serializable]
    public enum LevelType
    {
        Action = 1,
        Puzzle = 2
    }

    public LevelType levelType;
    public int LevelTurn;
}
