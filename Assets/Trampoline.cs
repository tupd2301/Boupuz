using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            Debug.Log("Trampoline");
            Vector3 direction = new Vector3(Random.Range(-180f,180f), Random.Range(-180f, 180f), 0).normalized;
            collision.gameObject.GetComponent<BallModel>().Direction = direction;
            collision.gameObject.transform.position = Vector3.MoveTowards(collision.transform.position, (direction.normalized + collision.transform.position), 0.01f * BallController.Instance.SpeedToRun);
        }
    }
}
