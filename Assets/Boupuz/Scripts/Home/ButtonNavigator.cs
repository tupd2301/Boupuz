using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonNavigator : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private GameObject _parent;

    public GameObject Parent { get => _parent; set => _parent = value; }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Parent.transform.localScale != new Vector3(1.4f, 1.4f, 1))
        {
            Parent.transform.localScale = new Vector3(1.4f, 1.4f, 1);
            UIManager.Instance.LoadUI(Parent.name);
        }
    }
}
