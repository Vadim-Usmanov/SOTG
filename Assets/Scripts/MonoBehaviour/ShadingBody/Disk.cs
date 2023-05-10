using System;
using System.Collections.Generic;
using UnityEngine;
using Concept;
using Concept.DiskConcept;

public class Disk : ShadingBody
{
    public const float holdingHeight = 0.25f;
    static private Dictionary<ConceptKey, Sprite> _sprites;
    static private Dictionary<ConceptKey, Sprite> _throwSprites;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private bool _isGrounded;
    [SerializeField] private Sprite _defaultSprite;
    private Vector3 _moveDirection;
    private float _speed;
    private Vector3 _aim;
    private float _glideTimer;
    private FlightStage _diskFlightStage = FlightStage.None;
    public enum FlightStage { Kinetic, Glide, Fall, None }
    public enum SpriteType { Stance, Throw }
    public bool IsGrounded { get => _isGrounded; }
    public bool IsAloft { get => _diskFlightStage != FlightStage.None; }
    private FlightStage DiskFlightStage 
    {
        get => _diskFlightStage;
        set
        {
            _diskFlightStage = value;
            if (_diskFlightStage == FlightStage.None) transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            else transform.position = new Vector3(transform.position.x, transform.position.y, 0.25f);
        }
    }
    public void Awake()
    {
        DiskData diskData = GameObject.FindWithTag("GameController").GetComponent<DiskData>();
        _sprites = diskData.GetSpriteDictionary();
        _throwSprites = diskData.GetSpriteDictionary(SpriteType.Throw);
    }
    override protected void LateUpdate()
    {
        if (_diskFlightStage != FlightStage.None) Move();
        base.LateUpdate();
    }
    public void SetSprite(Orientation orientation, Stance hold = Stance.Base, Hand hand = Hand.Righthanded, SpriteType spriteType = SpriteType.Stance)
    {
        ConceptKey key = new ConceptKey(orientation.Val, hold, hand);
        try  
        {
            switch (spriteType)
            {
                case SpriteType.Stance: _spriteRenderer.sprite = _sprites[key]; break;
                case SpriteType.Throw: _spriteRenderer.sprite = _throwSprites[key]; break;
                default: throw new NotImplementedException($"Disk.SpriteType.{spriteType} is not implemented");
            }
        }
        catch (KeyNotFoundException) { Debug.LogError($"Disk sprite not found by key(orientation:{orientation.Val}, hold:{hold}, hand:{hand})"); }
    }
    public void SetSprite(Orientation orientation, Stance hold, bool isLeftHanded, SpriteType spriteType = SpriteType.Stance)
    {
        SetSprite(orientation, hold, isLeftHanded ? Hand.Lefthanded : Hand.Righthanded, spriteType);
    }
    public void SetHeight(float newHaight)
    {
        transform.position = new Vector3(transform.position.x, newHaight, transform.position.z);
    }
    public void Handle()
    {
        _isGrounded = false;
        if (_diskFlightStage != FlightStage.None)
        {
            DiskFlightStage = FlightStage.None;
            _speed = 0.0f;
            _glideTimer = 0.0f;
            _aim = Vector3.zero;
            _moveDirection = Vector3.zero;
        }
    }
    public void Throw(Vector3 aim, float speed)
    {
        _aim = new Vector3(aim.x, 0.25f, aim.z);
        _diskFlightStage = FlightStage.Kinetic;
        _moveDirection = Vector3.Normalize(_aim - transform.position);
        _speed = speed;
        _spriteRenderer.sprite = _defaultSprite;
        _glideTimer = 0.0f;
    }
    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, _aim, _speed * Time.deltaTime);
        if (_diskFlightStage == FlightStage.Glide) _glideTimer += Time.deltaTime;
        if (_glideTimer >= 0.1f)
        {
            _speed *= 0.95f;
            _glideTimer = 0.0f;
        }
        if (Vector3.Distance(transform.position, _aim) < 0.25f)
        {
            _diskFlightStage += 1;
            if (_diskFlightStage != FlightStage.None) _aim = NewAim(_moveDirection, _speed, _diskFlightStage);
            else
            {
                _speed = 0.0f;
                _isGrounded = true;
            }
        }
    }
    private Vector3 NewAim(Vector3 direction, float speed, FlightStage newFlightStage)
    {
        Vector3 vector = direction * speed;
        switch (newFlightStage)
        {
            case FlightStage.Glide:
            {
                if (speed <= 13.5f) vector *= 0.85f;
                speed *= 0.9f;
                return transform.position + vector;
            }
            case FlightStage.Fall:
            {
                vector *= 0.5f;
                return new Vector3(transform.position.x, 0f, transform.position.z) + vector;
            }
            default: throw new InvalidOperationException($"Cannot locate new aim for {newFlightStage} flight stage");
        }
    }
}
