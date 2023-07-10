using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlockTest : MonoBehaviour
{
    [SerializeField] private List<Vector3> _listDirection;
    [SerializeField] private List<ContactPoint2D> _listContact;
    [SerializeField] private List<Vector2> _listPosition;

    private void Awake()
    {
        _listContact = new List<ContactPoint2D>();
        _listDirection = new List<Vector3>();
        _listPosition = new List<Vector2>();
    }

    public void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Ball"))
        {
            Vector3 direction = coll.gameObject.GetComponent<BallModel>().Direction;
            if (_listDirection.Where(a => a == direction).Count() > 0 && _listPosition.Where(a=>a == new Vector2(coll.transform.position.x, coll.transform.position.y)).Count()>0)
            {
                int index = _listDirection.IndexOf(direction);
                float distance = Vector3.Distance(coll.transform.position, _listPosition[index]);
                if (distance < 0.05f)
                {
                    Debug.Log("yub");
                    coll.transform.position = _listPosition[index];
                    BallController.Instance.CheckContact(_listContact[index], coll.gameObject, true);
                }
                else
                {
                    _listDirection.Add(direction);
                    _listPosition.Add(coll.transform.position);
                    _listContact.Add(coll.contacts[0]);
                    BallController.Instance.CheckContact(coll.contacts[0], coll.gameObject, true);
                }
            }
            else
            {
                _listDirection.Add(direction);
                _listPosition.Add(coll.transform.position);
                _listContact.Add(coll.contacts[0]);
                BallController.Instance.CheckContact(coll.contacts[0], coll.gameObject, true);
            }
        }
    }
}
