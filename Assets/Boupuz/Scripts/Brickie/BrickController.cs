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

    public BrickData Data {get {return _data;}}
    [SerializeField]
    private BrickData _data;

    private int _boardWidth, _boardHeight;
    private float _size;
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
    }

    public IEnumerator Move(float duration) 
    {
        for (int s = 0; s < Data.Speed; s++)
        {
        
            Vector3 startPos = transform.localPosition;
            Vector3 endPos = startPos + Data.Direction * _collider.bounds.size[1];
            //Debug.Log("Collider bounds: " + _collider.bounds.size);
            for (float elasped = 0; elasped < duration; elasped += Time.deltaTime) 
            {
                transform.localPosition = Vector3.Lerp(startPos, endPos, elasped / duration);
                yield return null;
            }
            transform.localPosition = endPos;
            
        }
    }

    public void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ball"))
        {
            
            BallReflect(col);
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
            if ( Data.Hp <= 0)
            {
                RemoveBrick();
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
        else if (col.gameObject.CompareTag("Block"))
        {
            BrickController brick = col.gameObject.GetComponent<BrickController>();
            if (brick.Data.Id == 1 && brick.Data.Type == ObjectType.Special) // if trampoline
            {
                RandomChangeBrickPosition(this, 1);
            }
        }
    }

    private GridCoordinate? RandomChangeBrickPosition(BrickController brick, int radius)
    {
        HashSet<GridCoordinate> listCoordinate = new HashSet<GridCoordinate>();
        GridCoordinate newCoordinate = GetRandomAdjacentGridCoordinate(brick.Data.BrickCoordinate, 1);
        while (GameBoardController.Instance.Grid[newCoordinate.X, newCoordinate.Y] != null)
        {
            listCoordinate.Add(newCoordinate);
            if (listCoordinate.Count > 9)
            {
                return null;
            }

            newCoordinate = GetRandomAdjacentGridCoordinate(brick.Data.BrickCoordinate, 1);
        }
        return newCoordinate;
    }

    private GridCoordinate GetRandomAdjacentGridCoordinate(GridCoordinate coordinate, int radius)
    {
        int Xmin = (coordinate.X - radius) < 0 ? 0 : coordinate.X - radius;
        int Xmax = (coordinate.X + radius) > GameBoardController.Instance.GridScreenWidth ? GameBoardController.Instance.GridScreenWidth : coordinate.X + radius;
        int X = UnityEngine.Random.Range(Xmin, Xmax);

        int Ymin = (coordinate.Y - radius) < 0 ? 0 : coordinate.Y - radius;
        int Ymax = (coordinate.Y + radius) > GameBoardController.Instance.GridScreenHeight ? GameBoardController.Instance.GridScreenHeight : coordinate.Y + radius;

        int Y = UnityEngine.Random.Range(Ymin, Ymax);

        return new GridCoordinate(X, Y);
    }

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
            float distance = GridCoordinate.Distance(otherBrick.Data.BrickCoordinate, brick.Data.BrickCoordinate);
            if (distance > 0 && distance < Mathf.Sqrt(5))
            {
                otherBrick.DecreasHpByValue((int)(otherBrick.Data.maxHp / 2));
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


    public void BallReflect(Collision2D col)
    {
        Vector3 direction = col.gameObject.GetComponent<BallModel>().Direction;
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
