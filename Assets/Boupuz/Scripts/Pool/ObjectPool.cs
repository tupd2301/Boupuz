using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ObjectPool
{
    private string _name;
    private List<GameObject> _listObject;
    private GameObject _parent;

    public List<GameObject> ListObject { get => _listObject; set => _listObject = value; }
    public GameObject Parent { get => _parent; set => _parent = value; }
    public string Name { get => _name; set => _name = value; }
}
