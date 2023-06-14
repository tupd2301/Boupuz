using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlow : MonoBehaviour
{
    public static GameFlow Instance;
    [SerializeField] private PoolManager _poolManager;
    [SerializeField] private BallController _ballController;
    [SerializeField] private FixedJoystick _joystick;
    public Laser _laser;

    public float timeScale = 1;
    public bool canShoot = true;

    public float timeCount = 0;

    public FixedJoystick Joystick { get => _joystick; set => _joystick = value; }

    void Start()
    {
        GameFlow.Instance = this;
        Application.targetFrameRate = 60;
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
            _ballController.SetUp();
            _ballController.GetBall(10);
        }
        else
        {
            Debug.LogError("GameFlow can not see BallController");
        }

        if (Joystick != null)
        {
            if (_laser != null)
            {
                _laser.SetPosition(Joystick.transform.position);
                _laser.SetActive(false);
            }
            Joystick.transform.position = new Vector3(BallController.Instance.GunPosition.x, BallController.Instance.GunPosition.y, Joystick.transform.position.z);
        }
        else
        {
            Debug.LogError("GameFlow can not see Joystick");
        }
        DontDestroyOnLoad(this);
        SkillController.Instance.ShowUISkill();
    }

    public void ChangePositionJoystick(float x)
    {
        Joystick.transform.position = new Vector3(x, BallController.Instance.GunPosition.y, Joystick.transform.position.z);
        _laser.SetPosition(new Vector3(x, Joystick.transform.position.y, Joystick.transform.position.z));
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
        //SpeedUp(2,1.5f);
        //SpeedUp(5,2f);
        if (_ballController.isShooted)
        {
            _ballController.BallRunning();
        }
    }
}
