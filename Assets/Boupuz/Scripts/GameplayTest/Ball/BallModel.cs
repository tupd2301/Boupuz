using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallModel : MonoBehaviour
{
    [SerializeField] private int _damage;
    [SerializeField] private Vector3 _direction;
    [SerializeField] private bool _isRunning;
    [SerializeField] private int _impactTime;
    [SerializeField] private Type _typeBall;


    public Vector3 Direction { get => _direction; set => _direction = value; }
    public int Damage { get { return _damage;} }
    public bool IsRunning { get => _isRunning; set => _isRunning = value; }
    public int ImpactTime { get => _impactTime; set => _impactTime = value; }
    public Type TypeBall { get => _typeBall; set => _typeBall = value; }

    public enum Type
    {
        Boupuzes,
        Bounnies,
        Bouchos,
        Bouserks,
        Boujas
    }
}
