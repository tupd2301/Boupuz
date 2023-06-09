using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
            _view.DisplayHealth();
            if (Data.Id == 1 && Data.Type == ObjectType.Brickie) // if starvy
            {
                // Disable ball
                Debug.Log("Starvy ate 1 ball");
                col.gameObject.SetActive(false);
                // Total number ball -= 1
                
            }

            // When brick health <= 0, disable it
            if ( Data.Hp <= 0)
            {
                if (Data.Id == 1 && Data.Type == ObjectType.Brickie) // if starvy
                {
                    DecreaseAdjacentBrickHealth();
                }
                else
                {
                    RemoveBrick();
                }
                
            }
        }
    }

    public void DecreaseHP(Collision2D col)
    {
        Data.Hp -= col.gameObject.GetComponent<BallModel>().Damage;
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
    }

    public void RemoveBrick()
    {
        gameObject.SetActive(false);
    }

    public void DecreaseAdjacentBrickHealth()
    {
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (i != 0 && j != 0)
                {
                    
                    GridCoordinate adjacentCoordinate = Data.BrickCoordinate + new Vector3(i,j,0);
                    if (GameBoardController.Instance.Grid[adjacentCoordinate.X, adjacentCoordinate.Y] != null)
                    {   
                        BrickController adjacentBrick = GameBoardController.Instance.Grid[adjacentCoordinate.X, adjacentCoordinate.Y];
                        adjacentBrick.DecreasHpByValue((int)adjacentBrick.Data.Hp / 2);
                    }
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
                if (distance < 0.3f)
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