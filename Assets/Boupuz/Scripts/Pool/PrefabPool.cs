using System;
using UnityEngine;

[Serializable]
public class PrefabPool
{
    [SerializeField] private string _name;
    [SerializeField] private int _total;
    [SerializeField] private GameObject _prefab;

    public string Name { get => _name; set => _name = value; }
    public int Total { get => _total; set => _total = value; }
    public GameObject Prefab { get => _prefab; set => _prefab = value; }
}
