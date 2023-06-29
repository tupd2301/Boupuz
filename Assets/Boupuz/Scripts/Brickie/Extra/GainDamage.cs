using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GainDamage : MonoBehaviour
{
    [SerializeField]
    private int _gainDamageValue;

    public int GainDamageValue { get => _gainDamageValue; set => _gainDamageValue = value; }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ball"))
        {
            Debug.Log("Gain Damage");
            GameBoardController.Instance.LevelData.BallDamage += _gainDamageValue;
        }
    }
}
