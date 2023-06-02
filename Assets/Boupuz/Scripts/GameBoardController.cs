using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoardController : MonoBehaviour
{
    public List<BrickController> TileControllers { get { return _brickControllers; } }
    [SerializeField]
    private List<BrickController> _brickControllers = new List<BrickController>();

    //public List<Sprite> Sprites = new List<Sprite>();
    //public GameObject TilePrefab;
    private const int _gridWidth = 7;
    private const int _gridHeight = 11;
    public float Distance = 1.0f;
    private BrickController[,] Grid;

    void Start()
    {

    }

    void InitGrid()
    {
        //Vector3 positionOffset = transform.position - new Vector3(_gridWidth * Distance / 2.0f, _gridHeight * Distance / 2.0f, 0); // 1
        for (int brickIndex = 0; brickIndex < _brickControllers.Count; brickIndex++)
        {
            BrickController newBrick = _brickControllers[brickIndex];
            if (newBrick.Data.BrickCoordinate.X < _gridWidth && newBrick.Data.BrickCoordinate.Y < _gridHeight)
            {
                Grid[newBrick.Data.BrickCoordinate.X,newBrick.Data.BrickCoordinate.Y] = newBrick;
            }
            
        }
    }
}
