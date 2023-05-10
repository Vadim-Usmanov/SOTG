using UnityEngine;

[CreateAssetMenu(fileName = "NewSong", menuName = "ScriptableObjects/Songs", order = 51)]
public class Song : ScriptableObject
{
    [SerializeField] AudioClip _audioClip;
    [SerializeField] int _bpm;
    public AudioClip AudioClip { get => _audioClip; }
    public int Bpm { get => _bpm; }
}
