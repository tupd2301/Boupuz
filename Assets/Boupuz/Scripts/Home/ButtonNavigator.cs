using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonNavigator : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private GameObject _parent;

    public GameObject Parent { get => _parent; set => _parent = value; }

    public void OnPointerDown(PointerEventData eventData)
    {
        SoundManager.Instance.PlaySFX("ui");
        if (Parent.GetComponent<RectTransform>().anchoredPosition.y != 55f)
        {
            Parent.GetComponent<RectTransform>().anchoredPosition += new Vector2(0,55);
            GetComponent<Image>().color = Color.white;
            UIManager.Instance.LoadUI(Parent.name);
        }
        else
        {
            if (Parent.name == "Play")
            {
                UIManager.Instance.Ready();
            }
        }
    }
}
