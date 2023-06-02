using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickView : MonoBehaviour
{
    [SerializeField]
    private Sprite _mainGraphic;
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private BrickData _brickData;
    private float _size;
    private int _boardWidth, _boardHeight;

    public void Setup()
    {
        // TODO
    }
}
