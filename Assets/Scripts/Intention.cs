using System;
using UnityEngine;

public class Intention
{
    private Vector3 _position;
    private PlayerStateType _type;
    private Type _createdBy;
    private LookDirection _lookAt;
    public Intention(Type createdBy, PlayerStateType type)
    {
        _type = type;
    }
    public Intention(Type createdBy, PlayerStateType kind, Vector3 position, LookDirection lookAt = null) : this(createdBy, kind)
    {
        _position = position;
        if (lookAt == null) _lookAt = new LookDirection();
        else _lookAt = lookAt;
    }
    public enum PlayerStateType {Idle, Warried, GoToPosition, GoForDisk, HandleDisk, WaitForPass}
    public Vector3 Position { get => _position; }
    public PlayerStateType Type { get => _type; }
    public Type CreatedBy { get => _createdBy; }
    public LookDirection LookAt { get => _lookAt; }
    static public Intention Idle(Type createdBy)
    {
        return new Intention(createdBy, PlayerStateType.Idle);
    }
    static public Intention GoToPosition(Type createdBy, Vector3 position, LookDirection lookAt = null)
    {
        return new Intention(createdBy, PlayerStateType.GoToPosition, position);
    }
    static public Intention PickUpDisk(Type createdBy)
    {
        return new Intention(createdBy, PlayerStateType.GoForDisk);
    }
    static public Intention HandleDisk(Type createdBy)
    {
        return new Intention(createdBy, PlayerStateType.HandleDisk);
    }
    static public Intention WaitForPass(Type createdBy)
    {
        return new Intention(createdBy, PlayerStateType.WaitForPass);
    }
}
