using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance; 
    [SerializeField] private AudioSource _shooting;
    [SerializeField] private AudioSource _brickContact;
    [SerializeField] private AudioSource _wallContact;
    [SerializeField] private AudioSource _uI;
    [SerializeField] private AudioSource _bGM;
    [SerializeField] private AudioSource _die;
    [SerializeField] private AudioSource _starvyEating;
    [SerializeField] private AudioSource _portal;
    [SerializeField] private AudioSource _fart;
    [SerializeField] private bool _isMuteBGM;
    [SerializeField] private bool _isMuteSFX;

    public AudioSource Shooting { get => _shooting; set => _shooting = value; }
    public AudioSource BGM { get => _bGM; set => _bGM = value; }
    public AudioSource BrickContact { get => _brickContact; set => _brickContact = value; }

    private void Awake()
    {
        SoundManager.Instance = this;
    }
    private void Start()
    {
        GetMuteInfo();
    }

    void GetMuteInfo()
    {
        if (PlayerPrefs.GetInt("MuteSFX") == -1 && PlayerPrefs.GetInt("MuteBGM") == -1)
        {
            PlayerPrefs.SetInt("MuteSFX", 0);
            PlayerPrefs.SetInt("MuteBGM", 0);
        }
        _isMuteSFX = PlayerPrefs.GetInt("MuteSFX") == 0 ? false : true;
        _isMuteBGM = PlayerPrefs.GetInt("MuteBGM") == 0 ? false : true;
        PlayBGM();
        UIManager.Instance.UpdateUIBGM(_isMuteBGM);
        UIManager.Instance.UpdateUISFX(_isMuteSFX);
    }

    public void PlaySFX(string name)
    {
        if(!_isMuteSFX)
        switch (name)
        {
            case "shooting": _shooting.Play(); break;
            case "contact": _brickContact.Play(); break;
            case "wall": _wallContact.Play(); break;
            case "ui": _uI.Play(); break;
            case "die": _die.Play(); break;
            case "starvyEating": _starvyEating.Play(); break;
            case "portal": _portal.Play(); break;
            case "fart": _fart.Play(); break;
            default: break;
        }
    }
    public void PauseSFX(string name)
    {
        switch (name)
        {
            case "shooting": _shooting.Pause(); break;
            case "contact": _brickContact.Pause(); break;
            default: break;
        }
    }
    public void MuteBGM()
    {
        _isMuteBGM = !_isMuteBGM;
        PlayerPrefs.SetInt("MuteBGM", _isMuteBGM ? 1: 0);
        PlayBGM();
        UIManager.Instance.UpdateUIBGM(_isMuteBGM);
    }
    public void MuteSFX()
    {
        _isMuteSFX = !_isMuteSFX;
        PlayerPrefs.SetInt("MuteSFX", _isMuteSFX ? 1 : 0);
        UIManager.Instance.UpdateUISFX(_isMuteSFX);
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
