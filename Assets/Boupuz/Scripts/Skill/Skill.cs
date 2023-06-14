using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Skill
{
    [SerializeField] private int _id;
    [SerializeField] private string _name;
    [SerializeField] private Sprite _image;
    [SerializeField] private List<string> _level;

    public int Id { get => _id; set => _id = value; }
    public string Name { get => _name; set => _name = value; }
    public Sprite Image { get => _image; set => _image = value; }
    public List<string> Level { get => _level; set => _level = value; }
}
