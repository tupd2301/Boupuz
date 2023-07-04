using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseGame : MonoBehaviour
{
    [SerializeField] private Image _background;
    [SerializeField] private List<GameObject> _listOption;
    private bool isPause;

    public Image Background { get => _background; set => _background = value; }
    public List<GameObject> ListOption { get => _listOption; set => _listOption = value; }
    public bool IsPause { get => isPause; set => isPause = value; }
}
