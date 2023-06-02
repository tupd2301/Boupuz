using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public BrickData Data {get {return _brickData;}}
    private BrickData _brickData;

    private int _boardWidth, _boardHeight;
    private float _size;

    public void Initialize(BrickData brickData, int boardWidth, int boardHeight, float size)
    {
        _boardWidth = boardWidth;
        _boardHeight = boardHeight;
        _size = size;

        _view.Setup();
    }

}
