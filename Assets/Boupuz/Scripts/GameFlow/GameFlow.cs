using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlow : MonoBehaviour
{
    public static GameFlow Instance;
    [SerializeField] private PoolManager _poolManager;
    [SerializeField] private BallController _ballController;
    [SerializeField] private FixedJoystick _joystick;

    public float timeScale = 1;
    public bool canShoot = true;

    public float timeCount = 0;

    void Start()
    {
        GameFlow.Instance = this;
        timeScale = 1;
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
            _ballController.GetBall(10);
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

    public void ChangePositionJoystick(float x)
    {
        _joystick.transform.position = new Vector3(x, _joystick.transform.position.y, _joystick.transform.position.z);
    }

    public void Shoot(Vector2 direction)
    {
        StartCoroutine(_ballController.BallShooting(direction));
    }

    public void SpeedUp(float time, float scaleUp)
    {
        timeScale = scaleUp;
        Debug.Log("SpeedUp:" + timeScale);
    }

    void FixedUpdate()
    {
        SpeedUp(2,1.5f);
        SpeedUp(5,2f);
        if (_ballController.isShooted)
        {
            _ballController.BallRunning();
        }
    }
}
