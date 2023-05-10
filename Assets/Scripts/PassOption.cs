using UnityEngine;

public class PassOption
{
    private int _playerId;
    private Vector3 _position;
    private float _time;
    public PassOption(int playerId, Vector3 position, float time = 0f)
    {
        _playerId = playerId;
        _position = position;
        _time = time;
    }
    public int PlayerId { get => _playerId; }
    public Vector3 Position { get => _position; }
    public float Time { get => _time; }
}
