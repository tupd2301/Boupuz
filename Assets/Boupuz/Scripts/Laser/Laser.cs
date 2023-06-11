using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private GameObject _laserGraphic;

    public void SetPosition(Vector2 position)
    {
        transform.position = position;
    }

    public void SetActive(bool isActive)
    {
        _laserGraphic.SetActive(isActive);
    }

    //public void ChnageDirection(Vector2 direction)
    //{
    //    float angle = Mathf.Atan2(direction.y, direction.x)*Mathf.Rad2Deg;
    //    transform.localRotation = Quaternion.Euler(0,0,angle);
    //    RaycastHit2D hit = Physics2D.Raycast(this.transform.position,direction, 50f,_layerMask);
    //    if(hit.collider == null)
    //    {
    //        _laserGraphic.transform.localScale = new Vector3(50, _laserGraphic.transform.localScale.y, 1);
    //    }
    //    else
    //    {
    //        _laserGraphic.transform.localScale = new Vector3(hit.distance, _laserGraphic.transform.localScale.y, 1);
    //    }
    //}

    public void ChnageDirection(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.localRotation = Quaternion.Euler(0, 0, angle);
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, direction, 20f, _layerMask);
        if (hit.collider == null)
        {
            _laserGraphic.GetComponent<SpriteRenderer>().size = new Vector2(50,0.25f);
        }
        else
        {
            _laserGraphic.GetComponent<SpriteRenderer>().size = new Vector2(hit.distance*2, 0.25f);
        }
    }
}
