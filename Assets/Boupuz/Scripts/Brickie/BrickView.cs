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
    
    //private float _size;
    //private int _boardWidth, _boardHeight;


    public void Setup()
    {
        // TODO
        // Set sprite
        //_spriteRenderer.sprite = _mainGraphic;
        // Display health
        
        DisplayHealth();
        ChangeHealthTextColorBasedOnHealth();
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

    public void FlashingRed()
    {
        _animator.Play("FlashingRed");
    }

    public void StarvyAnim()
    {
        _animator.Play("starvyeating");
    }

    public void ChangeHealthTextColor(Color color)
    {
        _health.color = color;
    }

    public void ChangeHealthTextColorBasedOnHealth()
    {
        //Color lightYellow = new Color(255, 255, 204);
        //Color earthYellow = new Color(225, 169, 95);
        //Color orange = new Color(255, 153, 0);
        //Debug.Log("--------------color");
        switch(_brickData.Hp)
        {
            case int n when (n <= 9):
                ChangeHealthTextColor(Color.white);
                break;
            case int n when (n > 9 && n <= 19):
                ChangeHealthTextColor(new Color32(255, 255, 102, 255));
                break;
            case int n when (n > 19 && n <= 29):
                ChangeHealthTextColor(new Color32(225, 169, 95, 255));
                break;
            case int n when (n > 29 && n <= 39):
                ChangeHealthTextColor(new Color32(255, 153, 0, 255));
                break;
            case int n when (n > 39 && n <= 49):
                ChangeHealthTextColor(new Color32(255, 71, 26, 255));
                break;
            case int n when (n > 49):
                ChangeHealthTextColor(Color.red);
                break;
        }
    }
}
