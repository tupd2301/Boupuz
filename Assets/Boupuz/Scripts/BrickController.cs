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

    public BrickData BrickData {get {return _brickData;}}
    private BrickData _brickData;

    private int _boardWidth, _boardHeight;
    private float _size;

    [SerializeField] private List<Vector3> _listDirection;
    [SerializeField] private List<ContactPoint2D> _listContact;
    [SerializeField] private List<Vector2> _listPosition;

    private void Awake()
    {
        _listContact = new List<ContactPoint2D>();
        _listDirection = new List<Vector3>();
        _listPosition = new List<Vector2>();
    }

    public void Initialize(BrickData brickData, int boardWidth, int boardHeight, float size)
    {
        _boardWidth = boardWidth;
        _boardHeight = boardHeight;
        _size = size;
        _brickData = brickData;
        _view.Setup();
    }

    public void Move(Vector2 direction, int speed)
    {
        
    }

    public void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ball"))
        {
            Vector3 direction = col.gameObject.GetComponent<BallModel>().Direction;
            if (_listDirection.Where(a => a == direction).Count() > 0)
            {
                int index = _listDirection.IndexOf(direction);
                float distance = Vector3.Distance(col.transform.position, _listPosition[index]);
                if (distance < 0.3f)
                {
                    Debug.Log("yub");
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

            //Decrease brick health
            _view.brickData.Hp -= col.gameObject.GetComponent<BallModel>().Damage;

            // When brick health <= 0, disable it
            if ( _view.brickData.Hp <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
