using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private Transform _portalLink;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            StartCoroutine(DeactivePortalLink());
            Debug.Log("portal");
            collision.transform.position = _portalLink.position;
            Vector3 direction = collision.gameObject.GetComponentInChildren<BallModel>().Direction;
            collision.gameObject.transform.position = Vector3.MoveTowards(collision.transform.position, (direction.normalized + collision.transform.position), 0.01f * BallController.Instance.SpeedToRun);
        }
    }
    IEnumerator DeactivePortalLink()
    {
        _portalLink.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(0.05f);
        _portalLink.gameObject.GetComponent<BoxCollider2D>().enabled = true;
    }
}
