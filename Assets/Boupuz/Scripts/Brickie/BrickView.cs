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
    private SpriteRenderer _childGraphic;
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
        //_spriteRenderer.sprite = _mainGraphic;
        // Display health
        DisplayHealth();
    }

    public void DisplayHealth()
    {
        _health.text = _brickData.Hp.ToString();
    }

    public void EnableChildGraphic()
    {
        Debug.Log("Enable child graphic");
        _childGraphic.enabled = true;
    }

    public void DisableChildGraphic()
    {
        _childGraphic.enabled = false;
    }
}
