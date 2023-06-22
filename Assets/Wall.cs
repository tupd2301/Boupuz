using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] private Transform _position;
    private void Start()
    {
        transform.position = _position.position;
    }

    public void Scale(float ratio)
    {
        transform.localScale *= ratio;
    }
}
