using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portals : MonoBehaviour
{
    [SerializeField] private BrickController _otherPortal;
    public BrickController otherPortal { get { return _otherPortal;}}
}
