using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlow : MonoBehaviour
{
    public static GameFlow Instance;
    [SerializeField] private PoolManager _poolManager;
    [SerializeField] private BallController _ballController;
    [SerializeField] private FixedJoystick _joystick;

    void Start()
    {
        GameFlow.Instance = this;
        if (_poolManager != null)
        {
            _poolManager.Init();
        }
        else
        {
            Debug.LogError("GameFlow can not see PoolManager");
        }

        if (_ballController != null)
        {
            _ballController.GetBall(1);
        }
        else
        {
            Debug.LogError("GameFlow can not see BallController");
        }

        if (_joystick != null)
        {
        }
        else
        {
            Debug.LogError("GameFlow can not see Joystick");
        }
        DontDestroyOnLoad(this);
    }

    public void Shoot(Vector2 direction)
    {
        StartCoroutine(_ballController.BallShooting(direction));
    }

    void Update()
    {
        if (_ballController.isShooted)
        {
            _ballController.BallRunning();
        }
    }
}
