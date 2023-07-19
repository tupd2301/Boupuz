using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private GameObject _laserGraphic;
    [SerializeField] private GameObject _subLaser;

    public GameObject LaserGraphic { get => _laserGraphic; set => _laserGraphic = value; }
    public GameObject SubLaser { get => _subLaser; set => _subLaser = value; }

    public void SetPosition(Vector2 position)
    {
        transform.position = position;
    }

    public void SetActive(bool isActive)
    {
        LaserGraphic.SetActive(isActive);
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
        RaycastHit2D hit = Physics2D.CircleCast(this.transform.position, 0.15f, direction, 10f, _layerMask);
        if (hit.collider == null)
        {
            LaserGraphic.GetComponent<SpriteRenderer>().size = new Vector2(50,0.25f);
        }
        else
        {
            LaserGraphic.GetComponent<SpriteRenderer>().size = new Vector2(hit.distance*2, 0.25f);
            //AddLaserBooster(hit, direction);
        }
    }
    public void AddLaserBooster(RaycastHit2D ray, Vector2 direction)
    {
        _subLaser.transform.parent.transform.position = ray.point;
        direction = Vector2.Reflect(direction, ray.normal.normalized);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        _subLaser.transform.parent.transform.localRotation = Quaternion.Euler(0, 0, angle);
        RaycastHit2D hit = Physics2D.Raycast(ray.point, direction, 10f, _layerMask);
        if (hit.collider == null)
        {
            SubLaser.GetComponent<SpriteRenderer>().size = new Vector2(50, 0.25f);
        }
        else
        {
            SubLaser.GetComponent<SpriteRenderer>().size = new Vector2(hit.distance * 2, 0.25f);
        }
    }
}
