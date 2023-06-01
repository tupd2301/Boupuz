using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _mainSprite;

    public SpriteRenderer MainSprite { get => _mainSprite; set => _mainSprite = value; }
}
