using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BrickView : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _spriteRenderer;
    [SerializeField]
    private Sprite _mainGraphic;
    [SerializeField]
    private TextMeshPro _health;
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private BrickData _brickData;
    
    private float _size;
    private int _boardWidth, _boardHeight;

    public void Setup()
    {
        // TODO
        // Set sprite
        _spriteRenderer.sprite = _mainGraphic;
        // Display health
        _health.text = _brickData.Hp.ToString();
    }
}
