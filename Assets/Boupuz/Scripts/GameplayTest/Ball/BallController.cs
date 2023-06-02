using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public static BallController Instance;
    [SerializeField] private List<GameObject> _balls = new List<GameObject>();
    [SerializeField] private int _totalBall;
    [SerializeField] private float _speedToShoot;
    [SerializeField] private float _speedToRun;
    [SerializeField] private Vector3 _gunPosition;
    [SerializeField] private Vector2 _direction;

    public bool isShooted;
    private float _xFirstBall;
    private void Awake()
    {
        BallController.Instance = this;
    }
    public void GetBall(int amount)
    {
        isShooted = false;
        _gunPosition = GameObject.Find("GunPosition").transform.position;
        _totalBall = amount;
        _balls.AddRange(PoolManager.Instance.GetObjects("Ball", amount, transform));
        for (int i = 0; i < _totalBall; i++)
        {
            _balls[i].GetComponentInChildren<BallModel>().Direction = Vector3.up;
            _balls[i].transform.position = new Vector3(_gunPosition.x, _gunPosition.y, 0);
            _balls[i].GetComponentInChildren<BallModel>().IsRunning = false;
        }
    }


    public IEnumerator BallShooting(Vector2 direction)
    {
        isShooted = true;
        GameFlow.Instance.canShoot = false;
        GameFlow.Instance.timeScale = 1;
        if (_speedToShoot > 0)
        {
            for (int i = 0; i < _totalBall; i++)
            {
                _balls[i].GetComponentInChildren<BallModel>().Direction = direction;
                //_balls[i].GetComponentInChildren<BallModel>().Direction = Vector3.up;

                _balls[i].transform.position = new Vector3(_xFirstBall, _gunPosition.y, 0);
                _balls[i].GetComponentInChildren<BallModel>().IsRunning = true;
                yield return new WaitForSeconds(_speedToShoot * GameFlow.Instance.timeScale);
            }
        }
        _xFirstBall = -10000;
    }

    public void CheckContact(ContactPoint2D contact, GameObject ball)
    {
        //Debug.Log("pos:" + ball.transform.position);
        Vector3 direction = Vector3.Reflect(ball.GetComponentInChildren<BallModel>().Direction.normalized, contact.normal.normalized);
        ball.GetComponentInChildren<BallModel>().Direction = direction;
    }

    public void SetUpFirstBallReturned(float x)
    {
        if (_xFirstBall == -10000)
        {
            GameFlow.Instance.ChangePositionJoystick(x);
            _xFirstBall = x;
        }
    }

    public void BallRunning()
    {
        if (_speedToRun > 0)
        {
            for (int i = 0; i < _balls.Count; i++)
            {
                if (_balls[i].activeInHierarchy && _balls[i].GetComponentInChildren<BallModel>().IsRunning)
                {
                    Vector3 direction = _balls[i].GetComponentInChildren<BallModel>().Direction;
                    _balls[i].transform.Translate(direction.normalized * _speedToRun * 0.01f * GameFlow.Instance.timeScale);
                    if (_balls[i].transform.position.y <= _gunPosition.y)
                    {
                        SetUpFirstBallReturned(_balls[i].transform.position.x);
                        _balls[i].transform.position = new Vector3(_xFirstBall, _gunPosition.y, _gunPosition.z * GameFlow.Instance.timeScale);
                        _balls[i].GetComponentInChildren<BallModel>().IsRunning = false;
                    }
                }
            }
        }
        if (_balls.Where(ball => ball.GetComponentInChildren<BallModel>().IsRunning).Count() <= 0) // Scale up speed by time
        {
            StopAllCoroutines();
            GameFlow.Instance.canShoot = true;
            GameFlow.Instance.timeScale = 1;
        }
    }
}
