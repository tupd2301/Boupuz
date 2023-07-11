using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserItem : MonoBehaviour
{
    private BrickController _brick;
    [SerializeField] private SpriteRenderer _laser;
    [SerializeField] private bool _isTouched;
    public bool isTouched { get { return _isTouched; }}
    private int colIndex;
    private int rowIndex;

    void Start()
    {
        _brick = GetComponent<BrickController>();
        colIndex = _brick.Data.BrickCoordinate.X;
        rowIndex = _brick.Data.BrickCoordinate.Y;
    }

    public void UpdateIndex()
    {
        colIndex = _brick.Data.BrickCoordinate.X;
        rowIndex = _brick.Data.BrickCoordinate.Y;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            //StopCoroutine(SummonLaser());
            _isTouched = true;
            UpdateIndex();
            StartCoroutine(SummonLaser());
            if (_brick.Data.Id == 2)
            {
                DecreaseBrickieHPinSameCol();
            }
            else if (_brick.Data.Id == 3)
            {
                DecreaseBrickieHPinSameRow();
            }
        }
        
    }

    private IEnumerator SummonLaser()
    {
        _laser.enabled = false;
        _laser.enabled = true;
        yield return new WaitForSeconds(0.2f);
        _laser.enabled = false;
    }

    private void DecreaseBrickieHPinSameCol()
    {
        for (int i = 0; i < GameBoardController.Instance.GridScreenHeight; i++)
        {
            try 
            {
                BrickController otherBrick = GameBoardController.Instance.Grid[colIndex, i];
                if (otherBrick.gameObject.activeInHierarchy)
                {
                    if (otherBrick.Data.Id != 1 && otherBrick.Data.Type == ObjectType.Brickie) // if
                    {
                        otherBrick.DecreasHpByValue(1);
                    }
                }
                
            }
            catch
            {

            }
            
        }
    }

    private void DecreaseBrickieHPinSameRow()
    {
        for (int i = 0; i < GameBoardController.Instance.GridScreenWidth; i++)
        {
            try 
            {
                //Debug.Log("row2");
                BrickController otherBrick = GameBoardController.Instance.Grid[i, rowIndex];
                if (otherBrick.gameObject.activeInHierarchy)
                {
                    //Debug.Log("row1");
                    if (otherBrick.Data.Id != 1 && otherBrick.Data.Type == ObjectType.Brickie)
                    {
                        //Debug.Log("row");
                        otherBrick.DecreasHpByValue(1);
                    }
                }
            }
            catch
            {

            }
        }
    }
}
