using System;
using System.Collections.Generic;
using UnityEngine;
using GameState;

public class Practice : Game
{
    public const int _requiredPlayers = 2;
    private PracticeArea _claimedPracticeArea;
    public PracticeArea ClaimedPracticeArea 
    {
        get => _claimedPracticeArea;
        set
        {
            if (_claimedPracticeArea != null) throw new InvalidOperationException("Cannot overwrite _takenPracticeArea");
            _claimedPracticeArea = value;
        }
    }
    public new int RequiredPlayers { get => _requiredPlayers; }
    public void Awake()
    {
        _currentState = new SpawnState(this);
        _currentState.AtStateEnter();
    }
    public void Update()
    {
        _currentState.Update();
        switch (_currentState)
        {
            case SpawnState:
            {
                if (_disk != null && _players.Count >= _requiredPlayers) CurrentState = new PrepareState(this);
                break;
            }
            case PrepareState:
            {
                if (IsPrepareReady()) CurrentState = new PracticeState(this);
                break;
            }
            case PracticeState: 
            {
                if (Disk.IsGrounded) CurrentState = new PrepareState(this);
                break;
            }
            default: break;
        }
    }
    public Player GetPartnerFor(int playerId)
    {
        int toReturn = playerId + 1;
        if (_players.Count == toReturn) toReturn = 0;
        return _players[toReturn];
    }
    private bool IsPrepareReady()
    {
        if (_disk.IsGrounded) return false;
        Player diskHolder = _players.Find((player) => player.DiskHandling.IsHoldingDisk);
        foreach (Player player in _players)
        {
            if (player.CurrentState.GetType() != typeof(PlayerState.IdleState)) return false;
        }
        return true;
    }
}
