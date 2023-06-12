using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public static BallController Instance;
    [SerializeField] private List<GameObject> _balls = new List<GameObject>();
    [SerializeField] private List<GameObject> _listRemove = new List<GameObject>();
    [SerializeField] private List<BallModel> _listBallModel = new List<BallModel>();
    [SerializeField] private int _totalBall;

    public float SpeedToRun { get => _speedToRun; set => _speedToRun = value; }
    public Vector3 GunPosition { get => _gunPosition; set => _gunPosition = value; }
    public Vector2 Direction { get => _direction; set => _direction = value; }
    public int CountBallRunnning { get => _countBallRunnning; set => _countBallRunnning = value; }
    public GameObject Gun { get => _gun; set => _gun = value; }
    public int TotalBall { get => _totalBall; set => _totalBall = value; }

    [SerializeField] private float _speedToShoot;
    [SerializeField] private float _speedToRun;
    [SerializeField] private Vector3 _gunPosition;
    [SerializeField] private Vector2 _direction;
    [SerializeField] private int _countBallRunnning;
    [SerializeField] private GameObject _gun;

    public bool isShooted;
    public bool isEndRound;
    private float _xFirstBall;

    [SerializeField] private float _timeCheckLoop = 5;
    private List<Vector3> _listDirectionRegister;
    private float _timeRunning = 0;

    private void Awake()
    {
        BallController.Instance = this;
    }
    public void GetBall(int amount)
    {
        isShooted = false;
        GunPosition = Gun.transform.position;
        TotalBall = amount;
        _xFirstBall = 0;
        _balls.AddRange(PoolManager.Instance.GetObjects("Ball", amount, transform));
        for (int i = 0; i < TotalBall; i++)
        {
            _listBallModel.Add(_balls[i].GetComponentInChildren<BallModel>());
            _listBallModel[i].Direction = Vector3.up;
            _balls[i].transform.position = new Vector3(GunPosition.x, GunPosition.y, 0);
            _listBallModel[i].IsRunning = false;
        }
    }


    public IEnumerator BallShooting(Vector2 direction)
    {
        _listRemove = new List<GameObject>();
        _timeRunning = 0;
        isEndRound = false;
        isShooted = true;
        CountBallRunnning = TotalBall;
        GameFlow.Instance.canShoot = false;
        GameFlow.Instance.timeScale = 1;
        if (_speedToShoot > 0)
        {
            for (int i = 0; i < TotalBall; i++)
            {
                _listBallModel[i].Direction = direction;
                //_balls[i].GetComponentInChildren<BallModel>().Direction = Vector3.up;

                _balls[i].transform.position = new Vector3(_xFirstBall, GunPosition.y, 0);
                _listBallModel[i].IsRunning = true;
                yield return new WaitForSeconds(_speedToShoot * GameFlow.Instance.timeScale);
            }
        }
    }

    public void CheckContact(ContactPoint2D contact, GameObject ball)
    {
        //Debug.Log("pos:" + ball.transform.position);
        Vector3 direction = Vector3.Reflect(ball.GetComponentInChildren<BallModel>().Direction, contact.normal);
        ball.GetComponentInChildren<BallModel>().Direction = direction;
    }

    public void SetUpFirstBallReturned(float x)
    {
        if (CountBallRunnning == TotalBall)
        {
            GameFlow.Instance.ChangePositionJoystick(x);
            _xFirstBall = x;
            Debug.Log("huyeah");
        }
    }

    public void CheckLoop()
    {
    }

    public void BallRunning()
    {
        _timeRunning += 1 / 60 * GameFlow.Instance.timeScale;
        if (_timeRunning == _timeCheckLoop)
        {
            CheckLoop();
        }
        if (SpeedToRun > 0)
        {
            for (int i = 0; i < _balls.Count; i++)
            {
                if (_listBallModel[i].IsRunning)
                {
                    Vector3 direction = _listBallModel[i].Direction;
                    //_balls[i].transform.Translate(direction.normalized * _speedToRun * 0.01f * GameFlow.Instance.timeScale);
                    _balls[i].transform.position = Vector3.MoveTowards(_balls[i].transform.position, (direction.normalized + _balls[i].transform.position), 0.01f * SpeedToRun);
                    if (_balls[i].transform.position.y <= GunPosition.y && _balls[i].transform.position.x != _xFirstBall)
                    {
                        SetUpFirstBallReturned(_balls[i].transform.position.x);
                        CountBallRunnning--;
                        _balls[i].transform.position = new Vector3(_xFirstBall, GunPosition.y, GunPosition.z) * GameFlow.Instance.timeScale;
                        _listBallModel[i].IsRunning = false;
                    }
                }
            }
        }
        if (CountBallRunnning - _listRemove.Count <= 0 && !isEndRound) // Scale up speed by time
        {
            RemoveBalls();
            _timeRunning = 0;
            isEndRound = true;
            StopAllCoroutines();
            GameFlow.Instance.canShoot = true;
            GameFlow.Instance.timeScale = 1;
            GameBoardController.Instance.MoveAll();
        }
    }

    public void AddListRemoveBall(GameObject ball)
    {
        ball.GetComponentInChildren<BallModel>().IsRunning = false;
        _listRemove.Add(ball);
    }

    public void RemoveBalls()
    {
        for (int i = 0; i < _listRemove.Count; i++)
        {
            _totalBall -= 1;
            _balls.Remove(_listRemove[i]);
            _listBallModel.Remove(_listRemove[i].GetComponentInChildren<BallModel>());
        }
    }
}
