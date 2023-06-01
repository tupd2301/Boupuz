using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallModel : MonoBehaviour
{
    [SerializeField] private int _damage;
    [SerializeField] private Vector3 _direction;
    [SerializeField] private bool _isRunning;

    public Vector3 Direction { get => _direction; set => _direction = value; }
    public int Damage { get => _damage; set => _damage = value; }
    public bool IsRunning { get => _isRunning; set => _isRunning = value; }
}
