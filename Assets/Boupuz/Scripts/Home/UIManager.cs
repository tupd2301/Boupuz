using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [SerializeField] private List<GameObject> _listUI;
    [SerializeField] private List<ButtonNavigator> _listNavigator;
    [SerializeField] private List<Character> _listCharacter;
    [SerializeField] private CharacterUI _characterUI;

    public List<Character> ListCharacter { get => _listCharacter; set => _listCharacter = value; }

    private void Awake()
    {
        UIManager.Instance = this;
    }

    public void LoadUI(string name)
    {
        for (int i = 0; i < _listUI.Count; i++)
        {
            if(_listNavigator[i].Parent.name != name)
            {
                Debug.Log("ok");
                _listNavigator[i].Parent.transform.localScale = Vector3.one;
            }
            if(_listUI[i].name != name + "Layout")
            {
                _listUI[i].transform.localPosition = new Vector3(1200,0,0);
            }
            else
            {
                _listUI[i].transform.localPosition = Vector3.zero;
                if(name == "Character")
                {
                    _characterUI.ShowUI(0);
                }
            }
        }
    }

    private void Start()
    {
        LoadUI("Home");
    }
}
