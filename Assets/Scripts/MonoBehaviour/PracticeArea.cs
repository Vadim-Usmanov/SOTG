using UnityEngine;

public class PracticeArea : MonoBehaviour
{
    [SerializeField] private GameObject _leftSpot;
    [SerializeField] private GameObject _rightSpot;
    private bool _isClaimed = false;
    public GameObject LeftSpot { get => _leftSpot; }
    public GameObject RightSpot { get => _rightSpot; }
    public bool IsClaimed { get => _isClaimed; }
    public void Claim()
    {
        _isClaimed = true;
    }
    public void Release()
    {
        _isClaimed = false;
    }
}
