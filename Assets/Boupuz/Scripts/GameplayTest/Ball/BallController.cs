using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] private List<GameObject> _balls = new List<GameObject>();
    [SerializeField] private int _totalBall;
    [SerializeField] private float _speedToShoot;
    [SerializeField] private float _speedToRun;
    [SerializeField] private Vector3 _gunPosition;
    [SerializeField] private Vector2 _direction;

    public bool isShooted;

    public void GetBall(int amount)
    {
        isShooted = false;
        _gunPosition = GameObject.Find("GunPosition").transform.position;
        _totalBall = amount;
        _balls.AddRange(PoolManager.Instance.GetObjects("Ball", amount, transform));
        for (int i = 0; i < _totalBall; i++)
        {
            _balls[i].GetComponentInChildren<BallModel>().Direction = Vector3.up;
            _balls[i].transform.position = _gunPosition;
            _balls[i].GetComponentInChildren<BallModel>().IsRunning = false;

        }
    }

    public IEnumerator BallShooting(Vector2 direction)
    {
        isShooted = true;
        if (_speedToShoot > 0)
        {
            for (int i = 0; i < _totalBall; i++)
            {
                _balls[i].GetComponentInChildren<BallModel>().Direction = direction;
                //_balls[i].GetComponentInChildren<BallModel>().Direction = Vector3.up;

                _balls[i].transform.position = _gunPosition;
                _balls[i].GetComponentInChildren<BallModel>().IsRunning = true;
                yield return new WaitForSeconds(_speedToShoot);
            }
        }
    }

    public GameObject FindBlockNearest(Vector3 posTarget)
    {
        GameObject[] listBlock = GameObject.FindGameObjectsWithTag("Block");
        float minDistance = 99999;
        int minIndex = -1;
        for (int i = 0; i < listBlock.Length; i++)
        {
            float distance = Vector3.Distance(posTarget, listBlock[i].transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                minIndex = i;
            }
        }
        return listBlock[minIndex];
    }

    public float CheckContact(GameObject ball)
    {
        GameObject blockNearest = FindBlockNearest(ball.transform.position);
        float heightBlock = blockNearest.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        //float heightBlock = 0.5f;
        Vector3 direction = ball.transform.position - blockNearest.transform.position;
        float alpha = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float radiusBall = ball.GetComponentInChildren<BallView>().MainSprite.bounds.size.x / 2;
        float distanceContact = Mathf.Abs((heightBlock / (2 * Mathf.Cos(alpha))) + radiusBall); // wrong
        float distanceBlockToBall = Vector2.Distance(ball.transform.position, blockNearest.transform.position);
        //Debug.Log("Alpha: " + alpha);
        //Debug.Log("A: " + heightBlock);
        //Debug.Log("r: " + radiusBall);
        //Debug.Log("disB-B: "+distanceBlockToBall);
        if (Mathf.Abs(alpha) <= 45 || Mathf.Abs(alpha) >= 135)
        {
            float y = ball.transform.position.y - blockNearest.transform.position.y;
            distanceContact = Mathf.Sqrt(heightBlock * heightBlock + y * y) + radiusBall;
        }
        else
        {
            float x = ball.transform.position.x - blockNearest.transform.position.x;
            distanceContact = Mathf.Sqrt(heightBlock * heightBlock + x * x) + radiusBall;
        }
        //Debug.Log("dis: " + blockNearest.name);

        float beta = Mathf.Atan2(ball.GetComponentInChildren<BallModel>().Direction.y, ball.GetComponentInChildren<BallModel>().Direction.x) * Mathf.Rad2Deg;
        beta = Mathf.FloorToInt(beta);
        if (distanceBlockToBall < distanceContact)
        {
            Debug.Log("Beta1:" + beta);
            beta = Mathf.Abs(beta) == 0 || Mathf.Abs(beta) == 90 || Mathf.Abs(beta) == 180 || Mathf.Abs(beta) == 45 || Mathf.Abs(beta) == 135 ? 180 :
                    ((Mathf.Abs(alpha) < 45 && Mathf.Abs(alpha) > 0) || Mathf.Abs(alpha) > 135) ?
                    (direction.x < 0 ? 2 * (90 - beta) : -2 * (beta-90)) : -beta * 2;
            //ball.transform.position += ball.GetComponentInChildren<BallModel>().Direction * _speedToRun;
            Debug.Log("Beta2:" + beta);
            return Mathf.FloorToInt(beta);
        }
        return 0f;
    }

    public Vector2 CalcDirectionByDegree(float degree, Vector2 originalDirection)
    {
        degree *= Mathf.Deg2Rad;
        float x = originalDirection.x * Mathf.Cos(degree) - originalDirection.y * Mathf.Sin(degree);
        float y = originalDirection.x * Mathf.Sin(degree) + originalDirection.y * Mathf.Cos(degree);
        return new Vector2(x, y).normalized;
    }

    public void BallRunning()
    {
        if (_speedToRun > 0)
        {
            for (int i = 0; i < _balls.Count; i++)
            {
                if (_balls[i].activeInHierarchy && _balls[i].GetComponentInChildren<BallModel>().IsRunning)
                {
                    Vector3 direction = _balls[i].GetComponentInChildren<BallModel>().Direction;
                    direction = CalcDirectionByDegree(CheckContact(_balls[i]), direction);
                    _balls[i].GetComponentInChildren<BallModel>().Direction = direction;
                    _balls[i].transform.position += direction * _speedToRun * 0.01f;
                }
            }
        }
    }
}
