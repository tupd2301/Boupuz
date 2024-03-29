using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUI : MonoBehaviour
{
    [SerializeField] private Image _mainCharacterGraphic;
    [SerializeField] private Text _mainCharacterInfo;
    [SerializeField] private Text _mainCharacterName;

    [SerializeField] private Image _leftCharacter;
    [SerializeField] private Image _rightCharacter;

    [SerializeField] private Sprite _noneCharacter;

    private int _leftID = 0;

    public int LeftID { get => _leftID; set => _leftID = value; }

    public void ShowUI(int id)
    {
        SoundManager.Instance.PlaySFX("ui");

        int countCharacter = UIManager.Instance.ListCharacter.Count - 1;
        if (LeftID + id < countCharacter - 1 && LeftID + id >= 0)
        {
            LeftID += id;
            _leftCharacter.sprite = UIManager.Instance.ListCharacter[LeftID].Image;
            _rightCharacter.sprite = UIManager.Instance.ListCharacter[LeftID + 2].Image;
            _rightCharacter.color = new Color32(255, 255, 255, 255);
            _leftCharacter.color = new Color32(255, 255, 255, 255);


            _mainCharacterGraphic.sprite = UIManager.Instance.ListCharacter[LeftID + 1].Image;
            _mainCharacterInfo.text = UIManager.Instance.ListCharacter[LeftID + 1].Info;
            _mainCharacterName.text = UIManager.Instance.ListCharacter[LeftID + 1].Name;
        }
        else
        {
            _rightCharacter.color = new Color32(255, 255, 255, 255);
            _leftCharacter.color = new Color32(255, 255, 255, 255);
            if (LeftID + id == countCharacter - 1)
            {
                LeftID += id;
                _leftCharacter.sprite = UIManager.Instance.ListCharacter[LeftID].Image;
                _rightCharacter.sprite = _noneCharacter;
                _rightCharacter.color = new Color32(255, 255, 255, 0);

                _mainCharacterGraphic.sprite = UIManager.Instance.ListCharacter[LeftID + 1].Image;
                _mainCharacterInfo.text = UIManager.Instance.ListCharacter[LeftID + 1].Info;
                _mainCharacterName.text = UIManager.Instance.ListCharacter[LeftID + 1].Name;
            }
            else if (LeftID + id == -1)
            {
                LeftID += id;
                _leftCharacter.sprite = _noneCharacter;
                _rightCharacter.sprite = UIManager.Instance.ListCharacter[LeftID + 2].Image;
                _leftCharacter.color = new Color32(255, 255, 255, 0);

                _mainCharacterGraphic.sprite = UIManager.Instance.ListCharacter[LeftID + 1].Image;
                _mainCharacterInfo.text = UIManager.Instance.ListCharacter[LeftID + 1].Info;
                _mainCharacterName.text = UIManager.Instance.ListCharacter[LeftID + 1].Name;
            }
        }
    }
}
