using UnityEngine;

public class LookDirection
{
    private Orientation _orientation = Orientation.Indifferent;
    private Vector3 _objectPosition;
    public LookDirection() {}
    public LookDirection(Orientation orientation)
    {
        _orientation = orientation;
    }
    public LookDirection(Vector3 objectPosition)
    {
        _objectPosition = objectPosition;
    }
    public LookDirection(Transform objectTransform) : this(objectTransform.position) {}
    public LookDirection(MonoBehaviour objectBehaviour) : this(objectBehaviour.transform) {}
    public Orientation Orientation { get => _orientation; }
    public void SetSubject(Vector3 position)
    {
        if (_objectPosition == null) return;
        if (position.x >= _objectPosition.x) _orientation = Orientation.Left;
        else _orientation = Orientation.Right;
    }
    public void SetSubject(Transform transform) { SetSubject(transform.position); }
    public void SetSubject(MonoBehaviour gameObject) { SetSubject(gameObject.transform); }
}
