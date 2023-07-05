using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceUpward : MonoBehaviour
{
    [SerializeField] private int _turnExist;
    [SerializeField] private bool _touched;
    public int TurnExist { get => _turnExist; set => _turnExist = value; }
    public bool Touched { get => _touched; set => _touched = value; }
}
