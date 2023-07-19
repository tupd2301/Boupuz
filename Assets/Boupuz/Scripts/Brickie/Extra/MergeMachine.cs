using UnityEngine;

public class MergeMachine : MonoBehaviour
{
    [SerializeField] private MergeMachine _otherMergeMachine;
    public MergeMachine otherMergeMachine { get { return _otherMergeMachine;}}

    [SerializeField] private BrickController _heldBrick;
    public BrickController HeldBrick { get => _heldBrick; set => _heldBrick = value; }

    [SerializeField] private bool _mainMergeMachine;
    public bool MainMergeMachine { get => _mainMergeMachine; set => _mainMergeMachine = value; }
    

    void OnCollisionExit2D(Collision2D col)
    {
        // if (col.gameObject.CompareTag("Block"))
        // {
        //     _heldBrick = null;
        // }
    }

    public void Merge()
    {
        
        if (_mainMergeMachine)
        {   
            Debug.Log("-----------Merge");
            HeldBrick.IncreaseHpByValue(_otherMergeMachine.HeldBrick.Data.Hp);
            _otherMergeMachine.HeldBrick.RemoveBrick();
        }
    }

    
    
}
