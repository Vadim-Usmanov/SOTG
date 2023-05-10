using System;
using UnityEngine;
using PlayerState;

 public class Player : ShadingBody
{
    private const float _minSpeed = 0.5f;
    [SerializeField] private string _name;
    [SerializeField] private int _id = -1;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Animator _animator;
    [SerializeField] private PlayerSpeech _speech;
    [SerializeField] private DiskHandling _diskHandling;
    [SerializeField] private Color _color;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _jogSpeed;
    [SerializeField] private float _maxForce;
    [SerializeField] private float _stoppingFactor;
    [SerializeField] private Quaternion _rightSide;
    [SerializeField] private Quaternion _leftSide;
    [SerializeField] private Orientation _lookingAt;
    private Team _team;
    private Practice _game;
    private Intention _intention;
    private BaseState _currentState;
    public int Id { get => _id; }
    public string Name { get => _name; set => _name = value; }
    public float MaxSpeed { get => _maxSpeed; }
    public float JogSpeed { get => _jogSpeed; }
    public Intention Intention { get => _intention; }
    public DiskHandling DiskHandling { get => _diskHandling; }
    public Orientation LookingAt { get => _lookingAt; }
    public Practice Game 
    {
        get => _game;
        set
        {
            if (_game != null) throw new InvalidOperationException("Cannot overwrite Game property");
            _game = value;
            _id = _game.Register(this);
        }
    }
    public Team Team 
    {
        get => _team;
        set 
        {
            _team = value;
            this.Color = _team.UsedColor;
        }
    }
    public Color Color 
    {
        get => _color;
        set
        {
            _color = value;
            _spriteRenderer.material.SetColor("_ShirtColor", _color);
        }
    }
    public BaseState CurrentState
    { 
        get => _currentState;
        set
        {
            if (!value.GetType().IsSubclassOf(typeof(State<Player>))) throw new ArgumentException("Only Subclass of PlayerState are allowed to set as State");
            _currentState.AtStateExit();
            _currentState = value;
            _currentState.AtStateEnter();
            _intention = null;
        }
    }
    
    public void Awake()
    {
        _currentState = new IdleState(this);
        _currentState.AtStateEnter();
    }
    public void Update()
    {
        CurrentState.Update();
        BaseState newState = FindNewState(CurrentState);
        if (newState != null) CurrentState = newState;
    }
    public void FixedUpdate()
    {
        if (_rigidbody.velocity.magnitude > _maxSpeed) _rigidbody.velocity = Vector3.ClampMagnitude(_rigidbody.velocity, _maxSpeed);
        _animator.SetFloat("Speed", _rigidbody.velocity.magnitude);
    }
    public void LookAt(LookDirection direction)
    {
        direction.SetSubject(this);
        _lookingAt = direction.Orientation;
        switch (_lookingAt.Val)
        {
            case Orientation.Value.Left:
            {
                transform.rotation = _leftSide;
                _speech.SetQuaternion(_rightSide);
                break;
            } 
            case Orientation.Value.Right:
            {
                transform.rotation = _rightSide;
                _speech.SetQuaternion(_rightSide);
                break;
            }
            case Orientation.Value.Indifferent: break;
            default: throw new ArgumentOutOfRangeException($"Player cannot look at {direction.Orientation}");
        }
        _diskHandling.UpdateOrientation();
    }
    public void MoveToPosition(Vector3 position, float distance)
        {
            Vector3 direction = position - transform.position;
            if (Vector3.Distance(transform.position, position) > distance) Move(direction);
            else if (transform.hasChanged) Stop();
        }
    public void Move(Vector3 direction, bool rush = false)
    {
        float speed;
        float force;
        if (rush) { speed = _maxSpeed; force = _maxForce;     }
        else      { speed = _jogSpeed; force = _maxForce * 0.75f; }
        _rigidbody.AddForce(direction.normalized * force, ForceMode.VelocityChange);
    }
    public void Stop()
    {
        float slower = 1 + _stoppingFactor * Time.deltaTime;
        _rigidbody.velocity = _rigidbody.velocity / slower;
        if (_rigidbody.velocity.magnitude < _minSpeed) _rigidbody.velocity = Vector3.zero;
        _animator.SetFloat("Speed", _rigidbody.velocity.magnitude);
    }
    public void CheckIntention()
    {
        _intention = _game.GetIntentionFor(_id);
    }
    public void ShakeIdleState()
    {
        if (_currentState.GetType() == typeof(IdleState)) CurrentState = new WarriedState(this);
    }
    public void Verbalize(string words)
    {
        _speech.Display(words);
        Debug.Log($"{_name}: {words}");
    }
    private BaseState FindNewState(BaseState fromState)
    {
        switch (fromState)
        {
            case IdleState: break;
            case WarriedState:
            {
                if (_intention != null) return IntentionToState(_intention);
                break;
            }
            case GoToPositionState:
            {
                if (CurrentState.IsDone)
                {
                    if (_intention != null) return IntentionToState(_intention);
                    else return new IdleState(this);
                }
                break;
            }
            case GoForDiskState:
            {
                if (!Game.Disk.IsGrounded) return new WarriedState(this);
                break;
            }
            case WaitForPassState:
            {
                if (Game.Disk.IsAloft) return new CatchDiskState(this);
                if (Game.Disk.IsGrounded) return new WarriedState(this);
                break;
            }
            case HandleDiskState:
            {
                if (!_diskHandling.IsHoldingDisk) return new WarriedState(this);
                break;
            }
            case CatchDiskState:
            {
                if (_diskHandling.IsHoldingDisk) return new HandleDiskState(this);
                if (Game.Disk.IsGrounded) return new WarriedState(this);
                break;
            }
            default:
            {
                return new WarriedState(this);
            }
        }
        return null;
    }
    private BaseState IntentionToState(Intention intention)
    {
        _intention = null;
        switch (intention.Type)
        {
            case Intention.PlayerStateType.Idle: return new IdleState(this, intention);
            case Intention.PlayerStateType.Warried: return new WarriedState(this, intention);
            case Intention.PlayerStateType.GoToPosition: return new GoToPositionState(this, intention);
            case Intention.PlayerStateType.GoForDisk: return new GoForDiskState(this, intention);
            case Intention.PlayerStateType.HandleDisk: return new HandleDiskState(this, intention);
            case Intention.PlayerStateType.WaitForPass: return new WaitForPassState(this, intention);
            default: throw new NotImplementedException($"Not implemented IntentionToState for {intention}");
        }
    }
}
