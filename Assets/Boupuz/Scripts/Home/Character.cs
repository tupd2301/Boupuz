using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Character
{
    [SerializeField] private string _Name;
    [SerializeField] private string _ID;
    [SerializeField] private string _Info;
    [SerializeField] private Sprite _Image;

    public string ID { get => _ID; set => _ID = value; }
    public string Name { get => _Name; set => _Name = value; }
    public string Info { get => _Info; set => _Info = value; }
    public Sprite Image { get => _Image; set => _Image = value; }
}
