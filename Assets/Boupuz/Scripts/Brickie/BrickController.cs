using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class BrickController : MonoBehaviour
{
    [SerializeField]
    private BrickView _view;
    public BrickView View
    {
        get
        {
            return _view;
        }
    }

    public BrickData Data { get { return _data; } }
    [SerializeField]
    private BrickData _data;

    private int _boardWidth, _boardHeight;
    [SerializeField] private float _moveDistance;
    [SerializeField]
    private Collider2D _collider;

    [SerializeField] private List<Vector3> _listDirection;
    [SerializeField] private List<ContactPoint2D> _listContact;
    [SerializeField] private List<Vector2> _listPosition;

    private void Awake()
    {
        _listContact = new List<ContactPoint2D>();
        _listDirection = new List<Vector3>();
        _listPosition = new List<Vector2>();
    }

    public void Initialize()
    {
        //_boardWidth = boardWidth;
        //_boardHeight = boardHeight;
        //_size = size;
        //_data = brickData;
        _view.Setup();
        SetUpPositionAndSize();
    }

    public IEnumerator Move(float duration)
    {
        for (int s = 0; s < Data.Speed; s++)
        {
            Vector3 startPos = transform.localPosition;
            Vector3 endPos = startPos + Data.Direction * _moveDistance;
            //Debug.Log("Collider bounds: " + _collider.bounds.size);
            for (float elasped = 0; elasped < duration; elasped += Time.deltaTime)
            {
                transform.localPosition = Vector3.Lerp(startPos, endPos, elasped / duration);
                yield return null;
            }
            transform.localPosition = endPos;
            SetScenePositionBasedOnGridCoordinate();
        }
    }

    public void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ball") && Data.BrickCoordinate.Y < 11)
        {
            BallController.Instance.BallThroughBrickies(col.gameObject);
        }
    }

    public void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ball") && Data.BrickCoordinate.Y < 11)
        {
            if (gameObject.CompareTag("Block")||gameObject.CompareTag("Item"))
            {
                if (!gameObject.CompareTag("Item"))
                {
                    BallReflect(col);
                }
                DecreaseHP(col);

                if (Data.Id == 1 && Data.Type == ObjectType.Brickie) // if starvy
                {
                    // Disable ball
                    Debug.Log("Starvy ate 1 ball");
                    col.gameObject.SetActive(false);
                    //BallController.Instance.TotalBall -= 1;
                    BallController.Instance.AddListRemoveBall(col.gameObject);
                    // Total number ball -= 1

                }

                // When brick health <= 0, disable it and activate its unique ability
                if (Data.Hp <= 0)
                {
                    RemoveBrick();
                    if (!gameObject.CompareTag("Item"))
                    {
                        GameBoardController.Instance.UpdateDestroyedBricks();
                    }
                    if (Data.hasCandy)
                    {
                        GameBoardController.Instance.UpdateCandy(1);
                    }


                    if (Data.Id == 1 && Data.Type == ObjectType.Brickie) // if starvy
                    {
                        DecreaseAdjacentBrickHealth(this);
                        //RemoveBrick();
                    }
                    else if (Data.Id == 2 && Data.Type == ObjectType.Brickie) // if icy
                    {
                        FreezeAdjacentBrick(this);
                    }


                }
            }
            else if (gameObject.CompareTag("Portal"))
            {
                col.transform.position = gameObject.GetComponent<Portals>().otherPortal.transform.position;
                Vector3 direction = col.gameObject.GetComponentInChildren<BallModel>().Direction;
                col.gameObject.transform.position = Vector3.MoveTowards(col.transform.position, (direction.normalized + col.transform.position), 5*0.01f * BallController.Instance.SpeedToRun);
                col.gameObject.GetComponentInChildren<TrailRenderer>().Clear();

            }
        }
        else if (col.gameObject.CompareTag("Trampoline"))
        {
            gameObject.SetActive(false);
            GridCoordinate newCoordinate = RandomChangeBrickCoordinate(this, 1);
            if (!GridCoordinate.IsNegative(newCoordinate))
            {
                Data.BrickCoordinate = newCoordinate;
            }
        }
        else if (col.gameObject.CompareTag("Portal"))
        {
            gameObject.SetActive(false);
            BrickController otherPortal = col.gameObject.GetComponent<Portals>().otherPortal;
            GridCoordinate newCoordinate = otherPortal.Data.BrickCoordinate + Data.Direction;
            if (newCoordinate.X >= 0 && newCoordinate.X < GameBoardController.Instance.GridScreenWidth &&
                newCoordinate.Y >= 0 && newCoordinate.Y < GameBoardController.Instance.GridScreenHeight)
            {
                if (GameBoardController.Instance.Grid[newCoordinate.X, newCoordinate.Y] == null)
                {
                    Data.BrickCoordinate = newCoordinate;
                }
                else
                {
                    // add health 
                    GameBoardController.Instance.Grid[newCoordinate.X, newCoordinate.Y].Data.Hp += Data.Hp;
                }
            }
            else
            {
                Debug.Log("Portal: new coordinate is invalid");
            }

        }
    }
    #region Trampoline
    private GridCoordinate RandomChangeBrickCoordinate(BrickController brick, int radius)
    {
        HashSet<GridCoordinate> listCoordinate = new HashSet<GridCoordinate>();
        GridCoordinate newCoordinate = GetRandomAdjacentGridCoordinate(brick.Data.BrickCoordinate, 1);
        while (GameBoardController.Instance.Grid[newCoordinate.X, newCoordinate.Y] != null)
        {
            listCoordinate.Add(newCoordinate);
            if (listCoordinate.Count > 9)
            {
                AddHealthToLowestAdjacentBrick(brick, listCoordinate);
                return new GridCoordinate(-1, -1);
            }

            newCoordinate = GetRandomAdjacentGridCoordinate(brick.Data.BrickCoordinate, 1);
        }
        return newCoordinate;
    }

    private GridCoordinate GetRandomAdjacentGridCoordinate(GridCoordinate coordinate, int radius)
    {
        int Xmin = (coordinate.X - radius) < 0 ? 0 : coordinate.X - radius;
        int Xmax = (coordinate.X + radius) >= GameBoardController.Instance.GridScreenWidth ? GameBoardController.Instance.GridScreenWidth - 1 : coordinate.X + radius;
        int X = UnityEngine.Random.Range(Xmin, Xmax);

        int Ymin = (coordinate.Y - radius) < 0 ? 0 : coordinate.Y - radius;
        int Ymax = (coordinate.Y + radius) >= GameBoardController.Instance.GridScreenHeight ? GameBoardController.Instance.GridScreenHeight - 1 : coordinate.Y + radius;

        int Y = UnityEngine.Random.Range(Ymin, Ymax);

        return new GridCoordinate(X, Y);
    }

    private void AddHealthToLowestAdjacentBrick(BrickController brick, HashSet<GridCoordinate> adjacentCoordinate)
    {
        int lowestHP = 0;
        GridCoordinate lowestCoord = new GridCoordinate(0, 0);
        foreach (GridCoordinate coord in adjacentCoordinate)
        {
            if (GameBoardController.Instance.Grid[coord.X, coord.Y]?.Data.Hp < lowestHP && coord != brick.Data.BrickCoordinate)
            {
                lowestHP = GameBoardController.Instance.Grid[coord.X, coord.Y].Data.Hp;
                lowestCoord = coord;
            }
        }
        GameBoardController.Instance.Grid[lowestCoord.X, lowestCoord.Y].Data.Hp += brick.Data.Hp;
    }
    #endregion
    public void DecreaseHP(Collision2D col)
    {
        // if (Data.Id == 3 && Data.Type == ObjectType.Brickie)
        // {
        //     DecreasHpByValue(col.gameObject.GetComponent<BallModel>().Damage - 1);
        // }
        // else
        // {
        //     DecreasHpByValue(1);
        // }

        Data.Hp -= col.gameObject.GetComponent<BallModel>().Damage;

        _view.DisplayHealth();
    }

    public void DecreasHpByValue(int value)
    {
        if (Data.Hp <= value)
        {
            RemoveBrick();
        }
        else
        {
            Data.Hp -= value;
        }
        _view.DisplayHealth();
    }

    public void RemoveBrick()
    {
        gameObject.SetActive(false);
        GameBoardController.Instance.BrickControllers.Remove(this);

    }

    public void DecreaseAdjacentBrickHealth(BrickController brick)
    {
        //Debug.Log("hello");
        for (int i = 0; i < GameBoardController.Instance.BrickControllers.Count; i++)
        {
            BrickController otherBrick = GameBoardController.Instance.BrickControllers[i];
            if (otherBrick.Data.BrickCoordinate.Y < GameBoardController.Instance.GridScreenHeight)
            {
                if (otherBrick.Data.Type == ObjectType.Brickie)
                {
                    float distance = GridCoordinate.Distance(otherBrick.Data.BrickCoordinate, brick.Data.BrickCoordinate);
                    if (distance > 0 && distance < Mathf.Sqrt(5))
                    {
                        otherBrick.DecreasHpByValue((int)(otherBrick.Data.maxHp / 2));
                    }
                }
            }
        }
    }

    public void FreezeAdjacentBrick(BrickController brick)
    {
        for (int i = 0; i < GameBoardController.Instance.BrickControllers.Count; i++)
        {
            BrickController otherBrick = GameBoardController.Instance.BrickControllers[i];
            if (otherBrick.Data.BrickCoordinate.Y < GameBoardController.Instance.GridScreenHeight)
            {
                if (otherBrick.Data.Type == ObjectType.Brickie)
                {
                    float distance = GridCoordinate.Distance(otherBrick.Data.BrickCoordinate, brick.Data.BrickCoordinate);
                    if (distance > 0 && distance < Mathf.Sqrt(5))
                    {
                        otherBrick.Data.isFreeze = true;
                        otherBrick.Data.LvFreeze = 2;
                        otherBrick.View.EnableChildGraphic();
                    }
                }
            }
        }
    }

    public void FreezeBySkill(BallModel data)
    {
        int isFreeze = UnityEngine.Random.Range(1, 100);
        if (isFreeze <= data.CanFreeze)
        {
            _data.isFreeze = true;
            _data.LvFreeze = 2;
            _view.EnableChildGraphic();
        }
    }


    public void BallReflect(Collision2D col)
    {
        Vector3 direction = col.gameObject.GetComponent<BallModel>().Direction;

        FreezeBySkill(col.gameObject.GetComponent<BallModel>());
        if (Data.BrickCoordinate.Y < 11)
        {
            if (_listDirection.Where(a => a == direction).Count() > 0)
            {
                int index = _listDirection.IndexOf(direction);
                float distance = Vector3.Distance(col.transform.position, _listPosition[index]);
                if (distance < 0.05f)
                {
                    //Debug.Log("yub");
                    col.transform.position = _listPosition[index];
                    BallController.Instance.CheckContact(_listContact[index], col.gameObject);
                }
                else
                {
                    _listDirection.Add(direction);
                    _listPosition.Add(col.transform.position);
                    _listContact.Add(col.contacts[0]);
                    BallController.Instance.CheckContact(col.contacts[0], col.gameObject);
                }
            }
            else
            {
                _listDirection.Add(direction);
                _listPosition.Add(col.transform.position);
                _listContact.Add(col.contacts[0]);
                BallController.Instance.CheckContact(col.contacts[0], col.gameObject);
            }
        }
    }

    public void SetUpPositionAndSize()
    {
        GameObject ori1 = GameBoardController.Instance.BrickOri1;
        GameObject ori2 = GameBoardController.Instance.BrickOri2;
        _moveDistance = Vector3.Distance(ori1.transform.position, ori2.transform.position);
        Vector3 oriPosition = ori1.transform.position;
        transform.position = new Vector3(oriPosition.x + _moveDistance * Data.BrickCoordinate.X, oriPosition.y + _moveDistance * Data.BrickCoordinate.Y);
        _view.transform.localScale = new Vector3(0.68f, 0.68f, 0.68f);
    }

    public void SetScenePositionBasedOnGridCoordinate()
    {
        GameObject ori1 = GameBoardController.Instance.BrickOri1;
        Vector3 oriPosition = ori1.transform.position;
        transform.position = new Vector3(oriPosition.x + _moveDistance * Data.BrickCoordinate.X,
                                         oriPosition.y + _moveDistance * Data.BrickCoordinate.Y);
        gameObject.SetActive(true);
    }
}
