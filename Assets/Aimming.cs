using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Aimming : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (GameFlow.Instance.canShoot)
        {
            GameFlow.Instance.Joystick.OnPointerDown(eventData);
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (GameFlow.Instance.canShoot)
        {
            GameFlow.Instance.Joystick.OnDrag(eventData);
        }
    }
    public virtual void OnPointerUp(PointerEventData eventData)
    {
        if (GameFlow.Instance.canShoot)
        {
            GameFlow.Instance.Joystick.OnPointerUp(eventData);
        }
    }
}
