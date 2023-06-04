using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlockTest : MonoBehaviour
{

    public void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Ball"))
        {
            BallController.Instance.CheckContact(coll.contacts[0], coll.gameObject);
        }
    }
}
