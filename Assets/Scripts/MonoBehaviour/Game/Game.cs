using System;
using System.Collections.Generic;
using UnityEngine;
using GameState;

public abstract class Game : MonoBehaviour
{
    [SerializeField] protected List<Player> _players = new List<Player>(RequiredPlayers);
    [SerializeField] protected Disk _disk;
    protected List<PassOption> _passOptions = new List<PassOption>();
    protected BaseState _currentState;
    public IList<Player> Players { get => _players.AsReadOnly(); }
    public IList<PassOption> PassOptions { get => _passOptions.AsReadOnly(); }
    public static int RequiredPlayers { get; }
    public BaseState CurrentState
    {
        get => _currentState;
        set
        {
            if (!value.GetType().IsSubclassOf(typeof(State<Practice>))) throw new ArgumentException("Only Subclass of GameState are allowed to set as State");
            _currentState.AtStateExit();
            _currentState = value;
            _currentState.AtStateEnter();
        }
    }
    public Disk Disk
    {
        get => _disk; 
        set
        {
            if (_disk != null) throw new InvalidOperationException("Cannot overwrite Disk property");
            _disk = value;
        }
    }
    public void Report(Message message)
    {
        _currentState.Report(message);
    }
    public void AddPassOption(PassOption passOption)
    {
        _passOptions.Add(passOption);
    }
    public void RemovePassOptions(int playerId = -1)
    {
        if (playerId == -1) _passOptions.Clear();
        else _passOptions.RemoveAll((item) => item.PlayerId == playerId);
    }
    public void RemovePassOptions() { RemovePassOptions(-1); }
    public Intention GetIntentionFor(int playerId)
    {   if (playerId < 0 || playerId >= _players.Count) throw new ArgumentOutOfRangeException($"No player with Id:{playerId}");
        return _currentState.PlayerIntention(playerId);
    }
    public int Register(Player player)
    {
        if (_players.Contains(player)) throw new InvalidOperationException("Player already registered");
        _players.Add(player);
        return _players.IndexOf(player);
    }
}
