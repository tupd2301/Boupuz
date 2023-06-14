using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallModel : MonoBehaviour
{
    [SerializeField] private int _damage;
    [SerializeField] private Vector3 _direction;
    [SerializeField] private bool _isRunning;
    [SerializeField] private int _impactTime;
    [SerializeField] private Type _typeBall = Type.Boupuzes;
    [SerializeField] private float _canFreeze = 0;


    public Vector3 Direction { get => _direction; set => _direction = value; }
    public bool IsRunning { get => _isRunning; set => _isRunning = value; }
    public int ImpactTime { get => _impactTime; set => _impactTime = value; }
    public Type TypeBall { get => _typeBall; set => _typeBall = value; }
    public int Damage { get => _damage; set => _damage = value; }
    public float CanFreeze { get => _canFreeze; set => _canFreeze = value; }

    public enum Type
    {
        Boupuzes,
        Bounnies,
        Bouchos,
        Bouserks,
        Boujas
    }
}
