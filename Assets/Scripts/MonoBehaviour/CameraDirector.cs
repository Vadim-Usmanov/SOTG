using UnityEngine;

public class CameraDirector : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Animator _cameraAnimator;
    [SerializeField] private float _cameraMaxSpeed;
    [SerializeField] private float _cameraMinSpeed;
    [SerializeField] private float _cameraFarthestPosition;
    private Quaternion _cameraWantedRotation;
    public void CameraStartGame()
    {
        _cameraAnimator.SetTrigger("StartGame");
    }
}
