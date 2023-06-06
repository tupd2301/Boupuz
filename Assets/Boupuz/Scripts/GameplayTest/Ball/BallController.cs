using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public static BallController Instance;
    [SerializeField] private List<GameObject> _balls = new List<GameObject>();
    [SerializeField] private List<BallModel> _listBallModel = new List<BallModel>();
    [SerializeField] private int _totalBall;
    [SerializeField] private float _speedToShoot;
    [SerializeField] private float _speedToRun;
    [SerializeField] private Vector3 _gunPosition;
    [SerializeField] private Vector2 _direction;
    [SerializeField] private int _countBallRunnning;
    [SerializeField] private GameObject _gun;

    public bool isShooted;
    public bool isEndRound;
    private float _xFirstBall;
    private void Awake()
    {
        BallController.Instance = this;
    }
    public void GetBall(int amount)
    {
        isShooted = false;
        _gunPosition = _gun.transform.position;
        _totalBall = amount;
        _xFirstBall = 0;
        _balls.AddRange(PoolManager.Instance.GetObjects("Ball", amount, transform));
        for (int i = 0; i < _totalBall; i++)
        {
            _listBallModel.Add(_balls[i].GetComponentInChildren<BallModel>());
            _listBallModel[i].Direction = Vector3.up;
            _balls[i].transform.position = new Vector3(_gunPosition.x, _gunPosition.y, 0);
            _listBallModel[i].IsRunning = false;
        }
    }


    public IEnumerator BallShooting(Vector2 direction)
    {
        isEndRound = false;
        isShooted = true;
        _countBallRunnning = _totalBall;
        GameFlow.Instance.canShoot = false;
        GameFlow.Instance.timeScale = 1;
        if (_speedToShoot > 0)
        {
            for (int i = 0; i < _totalBall; i++)
            {
                _listBallModel[i].Direction = direction;
                //_balls[i].GetComponentInChildren<BallModel>().Direction = Vector3.up;

                _balls[i].transform.position = new Vector3(_xFirstBall, _gunPosition.y, 0);
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
        if (_countBallRunnning == _totalBall)
        {
            GameFlow.Instance.ChangePositionJoystick(x);
            _xFirstBall = x;
            Debug.Log("huyeah");
        }
    }

    public void BallRunning()
    {
        if (_speedToRun > 0)
        {
            for (int i = 0; i < _balls.Count; i++)
            {
                if (_listBallModel[i].IsRunning)
                {
                    Vector3 direction = _listBallModel[i].Direction;
                    //_balls[i].transform.Translate(direction.normalized * _speedToRun * 0.01f * GameFlow.Instance.timeScale);
                    _balls[i].transform.position = Vector3.MoveTowards(_balls[i].transform.position, (direction.normalized + _balls[i].transform.position), 0.01f * _speedToRun);
                    if (_balls[i].transform.position.y <= _gunPosition.y && _balls[i].transform.position.x != _xFirstBall)
                    {
                        SetUpFirstBallReturned(_balls[i].transform.position.x);
                        _countBallRunnning--;
                        _balls[i].transform.position = new Vector3(_xFirstBall, _gunPosition.y, _gunPosition.z) * GameFlow.Instance.timeScale;
                        _listBallModel[i].IsRunning = false;
                    }
                }
            }
        }
        if (_countBallRunnning <= 0 && !isEndRound) // Scale up speed by time
        {
            isEndRound = true;
            StopAllCoroutines();
            GameFlow.Instance.canShoot = true;
            GameFlow.Instance.timeScale = 1;
        }
    }
}
