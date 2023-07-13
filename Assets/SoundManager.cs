using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance; 
    [SerializeField] private AudioSource _shooting;
    [SerializeField] private AudioSource _BrickContact;
    [SerializeField] private AudioSource _bGM;
    [SerializeField] private bool _isMuteBGM;
    [SerializeField] private bool _isMuteSFX;

    public AudioSource Shooting { get => _shooting; set => _shooting = value; }
    public AudioSource BrickContact { get => _BrickContact; set => _BrickContact = value; }
    public AudioSource BGM { get => _bGM; set => _bGM = value; }

    private void Awake()
    {
        SoundManager.Instance = this;
        GetMuteInfo();
    }

    void GetMuteInfo()
    {
        if (PlayerPrefs.GetInt("MuteSFX") != -1 && PlayerPrefs.GetInt("MuteBGM") != -1)
        {
            PlayerPrefs.SetInt("MuteSFX", 0);
            PlayerPrefs.SetInt("MuteBGM", 0);
        }
        _isMuteSFX = PlayerPrefs.GetInt("MuteSFX") == 0 ? false : true;
        _isMuteBGM = PlayerPrefs.GetInt("MuteBGM") == 0 ? false : true;
        PlayBGM();
    }

    public void PlaySFX(string name)
    {
        if(!_isMuteSFX)
        switch (name)
        {
            case "shooting": _shooting.Play(); break;
            case "contact": _BrickContact.Play(); break;
            default: break;
        }
    }
    public void PauseSFX(string name)
    {
        switch (name)
        {
            case "shooting": _shooting.Pause(); break;
            case "contact": _BrickContact.Pause(); break;
            default: break;
        }
    }
    public void MuteBGM()
    {
        _isMuteBGM = !_isMuteBGM;
        PlayerPrefs.SetInt("MuteBGM", _isMuteBGM ? 1: 0);
        PlayBGM();
    }
    public void MuteSFX()
    {
        _isMuteSFX = !_isMuteSFX;
        PlayerPrefs.SetInt("MuteSFX", _isMuteSFX ? 1 : 0);
    }

    public void PlayBGM()
    {
        if (_isMuteBGM)
        {
            BGM.Pause();
        }
        else
        {
            BGM.Play();
        }
    }
}
