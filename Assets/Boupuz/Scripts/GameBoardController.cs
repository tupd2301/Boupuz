using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoardController : MonoBehaviour
{
    
    [SerializeField]
    private List<BrickController> _brickControllers = new List<BrickController>();
    public List<BrickController> BrickControllers { get { return _brickControllers; } }

    [SerializeField]
    private int _gridWidth, _gridHeight;

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
            if (newBrick.BrickData.BrickCoordinate.X < _gridWidth && newBrick.BrickData.BrickCoordinate.Y < _gridHeight)
            {
                Grid[newBrick.BrickData.BrickCoordinate.X,newBrick.BrickData.BrickCoordinate.Y] = newBrick;
            }
            
        }
    }

    
}
