using System;
using System.Collections;
using UnityEngine;

public class DiskHandling : MonoBehaviour
{
    private const float _throwAnimationShift = 0.5f;
    [SerializeField] private Player _player;
    [SerializeField] private float _diskPickRange;
    [SerializeField] private float _diskHoldDistance;
    [SerializeField] private bool _isLeftHanded = false;
    private Disk _heldDisk;
    private Orientation _diskOrientation;
    private Coroutine _stanceTransit;
    private Coroutine _throwExecution;
    private Concept.Stance _stance = Concept.Stance.Base;
    public bool IsHoldingDisk { get => _heldDisk != null; }
    public float DiskPickRange { get => _diskPickRange; }
    public bool IsLeftHanded { get => _isLeftHanded; set => _isLeftHanded = value; }
    public bool IsThrowing { get => _throwExecution != null; }
    public Disk HeldDisk 
    {
        get => _heldDisk;
        set
        {
            _heldDisk = value;
            _diskOrientation = SpotDiskOrientation();
        }
    }
    public bool IsTransitStance 
    {
        get
        {
            if (_stanceTransit == null) return false;
            return true;
        }
    }
    public Concept.Stance Stance { get => _stance; }
    public void Update()
    {
        if (IsHoldingDisk && transform.hasChanged && !IsThrowing) MoveDisk();
    }
    public void LateUpdate()
    {
        transform.hasChanged = false;
    }
    public void ThrowDiskAt(Vector3 target)
    {
        if (_throwExecution != null) throw new InvalidOperationException("Throw already in progress");
        if (_stance == Concept.Stance.Base) throw new InvalidOperationException("Cannot throw disk from Base Stance");
        _throwExecution = StartCoroutine(ExecuteThrow(target));
    }
    public void TryCatchDisk(Vector3 fromPosition, Disk disk, Orientation lookingAt)
    {
        if (Vector3.Distance(transform.position, disk.transform.position) > _diskPickRange) return;
        disk.Handle();
        disk.SetSprite(lookingAt);
        HeldDisk = disk;
        HeldDisk.SetHeight(Disk.holdingHeight);
        MoveDisk();
    }
    public void MoveDisk()
    {
        _heldDisk.transform.position = new Vector3(transform.position.x, Disk.holdingHeight, transform.position.z) + (_diskOrientation.ToVector3() * _diskHoldDistance);
    }
    public void ChangeStance(Concept.Stance newStance)
    {
        if (_stanceTransit != null) return;
        if (!IsHoldingDisk)
        {
            if (newStance != Concept.Stance.Base) throw new ArgumentException($"Cannot change stance to {newStance} when no disk is held");
            _stance = newStance;
        }
        else _stanceTransit = StartCoroutine(StanceTransit(newStance));
    }
    public void UpdateOrientation()
    {
        _diskOrientation = SpotDiskOrientation();
    }
    private IEnumerator StanceTransit(Concept.Stance newStance)
    {
        yield return new WaitForSeconds(0.3f);
        _stance = newStance;
        _diskOrientation = SpotDiskOrientation();
        _heldDisk.SetSprite(_player.LookingAt, _stance, _isLeftHanded);
        MoveDisk();
        _stanceTransit = null;
    }
    private IEnumerator ExecuteThrow(Vector3 target)
    {
        if (_stanceTransit != null) throw new InvalidOperationException("Attempt to throw during Stance transit");
        _heldDisk.transform.position = _heldDisk.transform.position + _player.LookingAt.ToVector3() * (-_throwAnimationShift);
        yield return new WaitForSeconds(2.0f/12.0f);
        _heldDisk.transform.position = new Vector3(transform.position.x, Disk.holdingHeight, transform.position.z) 
                                     + (_diskOrientation.ToVector3() * _diskHoldDistance) 
                                     + (_player.LookingAt.ToVector3() * _throwAnimationShift);
        _heldDisk.SetSprite(_player.LookingAt, _stance, _isLeftHanded, Disk.SpriteType.Throw);
        yield return new WaitForSeconds(1.0f/12.0f);
        _heldDisk.Throw(RandomTarget(target), RandomSpeed(15.0f));
        _heldDisk = null;
        ChangeStance(Concept.Stance.Base);
        _throwExecution = null;
    }
    private Orientation SpotDiskOrientation()
    {
        if (_stance == Concept.Stance.Base) return _player.LookingAt;
        if (_stance == Concept.Stance.Backhand ^ _isLeftHanded) return _player.LookingAt.CounterClockwise;
        else return _player.LookingAt.Clockwise;
    }
    private float RandomSpeed(float desiredSpeed)
    {
        return UnityEngine.Random.Range(desiredSpeed * 0.9f, desiredSpeed * 1.1f);
    }
    private Vector3 RandomTarget(Vector3 desiredTarget)
    {
        Vector2 randomCircle = UnityEngine.Random.insideUnitCircle;
        randomCircle *= desiredTarget.magnitude * 0.1f;
        return desiredTarget + new Vector3(randomCircle.x, 0, randomCircle.y);
    }
}
