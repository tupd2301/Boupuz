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
    public int AddDamageBySkill { get => _addDamageBySkill; set => _addDamageBySkill = value; }
    public int AddBallBySkill { get => _addBallBySkill; set => _addBallBySkill = value; }
    public float AddFreezeBySkill { get => _addFreezeBySkill; set => _addFreezeBySkill = value; }
    public int StartBall { get => _startBall; set => _startBall = value; }

    [SerializeField] private float _speedToShoot;
    [SerializeField] private float _speedToRun;
    [SerializeField] private GameObject _gun;
    [SerializeField] private Vector3 _gunPosition;
    [SerializeField] private Vector2 _direction;
    [SerializeField] private int _countBallRunnning;
    [SerializeField] private int _startBall;

    private int _addBall;
    public bool isShooted;
    public bool isEndRound;
    private float _xFirstBall = 0;
    private bool _firstBall = false;

    [SerializeField] private float _timeCheckLoop = 5;
    [SerializeField] private float _timeShoot = 0;
    private List<Vector3> _listDirectionRegister;
    [SerializeField] private float _timeRunning = 0;

    [SerializeField] private int _addDamageBySkill;
    [SerializeField] private int _addBallBySkill;
    [SerializeField] private float _addFreezeBySkill;

    [SerializeField] private float _speedUp;
    [SerializeField] private float _timeSpeedUp = 30;
    private void Awake()
    {
        BallController.Instance = this;
    }

    public void SetUp()
    {
        AddDamageBySkill = 0;
        AddFreezeBySkill = 0;
        AddBallBySkill = 0;
        isShooted = false;
        GunPosition = Gun.transform.position;
    }
    public void GetBall(int amount)
    {

        TotalBall = amount;
        _balls.AddRange(PoolManager.Instance.GetObjects("Ball", amount, transform));
        for (int i = 0; i < amount; i++)
        {
            _listBallModel.Add(_balls[i].GetComponent<BallModel>());
            _listBallModel[i].Direction = Vector3.up;
            _balls[i].transform.position = new Vector3(_xFirstBall, GunPosition.y, 0);
            _listBallModel[i].IsRunning = false;
        }
        UIManager.Instance.UpdateTotalBall(_totalBall);
    }
    public void AddNewBall(int amount)
    {
        _balls.AddRange(PoolManager.Instance.GetObjects("Ball", amount, transform));
        TotalBall = _balls.Count;
        _listBallModel = new List<BallModel>();
        for (int i = 0; i < _balls.Count; i++)
        {
            _listBallModel.Add(_balls[i].GetComponent<BallModel>());
            _listBallModel[i].Direction = Vector3.up;
            _balls[i].transform.position = new Vector3(_xFirstBall, GunPosition.y, 0);
            _listBallModel[i].IsRunning = false;
        }
        UIManager.Instance.UpdateTotalBall(_totalBall);

    }

    public IEnumerator BallShooting(Vector2 direction)
    {
        _timeShoot= 0;
        _addBall = 0;
        _listRemove = new List<GameObject>();
        _timeRunning = 0;
        isEndRound = false;
        isShooted = true;
        _firstBall = false;
        _countBallRunnning = 0;
        GameFlow.Instance.timeScale = 1;
        float x = _xFirstBall;
        if (_speedToShoot > 0)
        {
            for (int i = 0; i < TotalBall; i++)
            {
                _listBallModel[i].Direction = direction;
                //_balls[i].GetComponentInChildren<BallModel>().Direction = Vector3.up;

                _balls[i].transform.position = new Vector3(x, GunPosition.y, 0);
                _listBallModel[i].IsRunning = true;
                _listBallModel[i].ImpactTime = 0;
                _listBallModel[i].Damage = _addDamageBySkill + 1;
                _listBallModel[i].CanFreeze = _addFreezeBySkill;
                _countBallRunnning += 1;
                UIManager.Instance.UpdateTotalBall(_totalBall - _countBallRunnning);
                yield return new WaitForSeconds(_speedToShoot * GameFlow.Instance.timeScale);
            }
        }
    }

    public void CheckContact(ContactPoint2D contact, GameObject ball, bool isWall)
    {
        if (!isWall)
        {
            _timeRunning = 0;
        }
        Vector3 direction = Vector3.Reflect(ball.GetComponent<BallModel>().Direction, contact.normal);
        ball.GetComponent<BallModel>().Direction = direction;
        ball.GetComponent<BallModel>().ImpactTime += 1;
    }

    public void BallThroughBrickies(GameObject ball)
    {
        Vector3 direction = Vector3.Reflect(ball.GetComponent<BallModel>().Direction, Vector3.zero);
        ball.GetComponent<BallModel>().Direction = direction;
    }

    public void SetUpFirstBallReturned(float x)
    {
        if (!_firstBall)
        {
            _firstBall = true;
            GameFlow.Instance.ChangePositionJoystick(x);
            _xFirstBall = x;
            Debug.Log("huyeah");
        }
    }

    public void CheckLoop()
    {
        for (int i = 0; i < _balls.Count; i++)
        {
            if (_listBallModel[i].IsRunning)
            {
                Vector3 direction = Vector3.down;
                _listBallModel[i].Direction = direction;
            }
        }
    }

    public void BallRunning()
    {
        _timeShoot += 1f / 60f;
        if (!GameFlow.Instance.canShoot)
        {
            _timeRunning += 1f / 60f;
        }
        if (_timeRunning >= _timeCheckLoop)
        {
            _timeRunning = 0;
            CheckLoop();
        }
        if (SpeedToRun > 0)
        {
            for (int i = 0; i < _balls.Count; i++)
            {
                if (_listBallModel[i].IsRunning)
                {
                    Vector3 direction = _listBallModel[i].Direction;
                    _balls[i].transform.position = Vector3.MoveTowards(_balls[i].transform.position, (direction.normalized + _balls[i].transform.position), 0.01f * (_timeShoot>=_timeSpeedUp? _speedUp: SpeedToRun));
                    if (_balls[i].transform.position.y < GunPosition.y && _balls[i].transform.position.x != _xFirstBall)
                    {
                        SetUpFirstBallReturned(_balls[i].transform.position.x);
                        CountBallRunnning--;
                        _balls[i].transform.position = new Vector3(_xFirstBall, GunPosition.y, 0) * GameFlow.Instance.timeScale;
                        _listBallModel[i].IsRunning = false;
                        UIManager.Instance.UpdateTotalBall(_totalBall - CountBallRunnning);
                    }
                }
            }
        }
        if (CountBallRunnning - _listRemove.Count <= 0 && !isEndRound) // Scale up speed by time
        {
            _timeShoot = 0;
            _timeRunning = 0;
            isEndRound = true;
            StopAllCoroutines();
            GameFlow.Instance.timeScale = 1;
            if (GameBoardController.Instance.LevelInfo.levelType == LevelInfo.LevelType.Action)
            {
                isShooted = false;
                GameBoardController.Instance.MoveAll();
            }
            else if (GameBoardController.Instance.LevelInfo.levelType == LevelInfo.LevelType.Puzzle)
            {
                GameBoardController.Instance.LevelData.DecreaseTurn(1);
                GameFlow.Instance.canShoot = true;
                if (GameBoardController.Instance.LevelData.CurrentTurn <= 0 &&
                    GameBoardController.Instance.LevelData.CollectedCake < GameBoardController.Instance.LevelData.TotalCake)
                {
                    GameFlow.Instance.canShoot = false;
                    UIManager.Instance.LoadLoseUI();
                }   
            }

            
            
            RemoveBalls();

            if(_totalBall < 1)
                AddBall(1);

            CalculateBalls();
            UIManager.Instance.UpdateTotalBall(_totalBall);
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

    public void CalculateBalls()
    {
        AddNewBall(_addBall);
    }

    public void AddBall(int i)
    {
        _addBall += i;
    }
}
