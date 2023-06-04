using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private Transform _portalLink;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            StartCoroutine(DeactivePortalLink());
            collision.transform.position = _portalLink.position;
            Vector3 direction = collision.gameObject.GetComponentInChildren<BallModel>().Direction;
            collision.gameObject.transform.Translate(direction.normalized * 10f * 0.03f * GameFlow.Instance.timeScale);
        }
    }
    IEnumerator DeactivePortalLink()
    {
        _portalLink.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForEndOfFrame();
        _portalLink.gameObject.GetComponent<BoxCollider2D>().enabled = true;
    }
}
