using System.Collections.Generic;
using UnityEngine;

public class DJ : ShadingBody
{
    [SerializeField] private GameObject _volumeController;
    [SerializeField] private GameObject _playStopButton;
    [SerializeField] private Animator _animator;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private List<Song> _songs;
    [SerializeField] private int _playingNow;
    [SerializeField] private int _defaultBpm;
    [SerializeField] private float _animationSpeed;
    [SerializeField] private float _animationSpeedMultiplier;
    [SerializeField] private int _startingSong;
    [SerializeField] private bool _isPlaying;
    private bool _isInterfaceActive = false;
    public float AnimationSpeed { get => _animationSpeed; set => SetAnimationSpeed(value); }
    public void Awake()
    {
        ShuffleSongs();
    }
    public void Switch()
    {
        _isInterfaceActive = !_isInterfaceActive;
        _volumeController.SetActive(_isInterfaceActive);
        _playStopButton.SetActive(_isInterfaceActive);
    }
    public void PlayStop()
    {
        if (_isPlaying) Stop();
        else Play();
        _isPlaying = !_isPlaying;
    }
    public void Play()
    {
        _audioSource.Play();
        _animator.StartPlayback();
    }
    public void Stop()
    {
        _audioSource.Pause();
        _animator.StopPlayback();
    }
    public void Update()
    {
        if (_isPlaying && !_audioSource.isPlaying)
        {
            int songToPlay = _playingNow + 1;
            PlaySong(songToPlay >= _songs.Count ? 0 : songToPlay);
        }
    }
    protected override void Start()
    {
        base.Start();
        if (_isPlaying) PlaySong(_startingSong);
    }
    private void SetAnimationSpeed(float newSpeed)
    {
        _animationSpeed = newSpeed;
        _animator.speed = _animationSpeed;
    }
    private void PlaySong(int index)
    {
        _audioSource.clip = _songs[index].AudioClip;
        _audioSource.Play();
        AnimationSpeed = (float)_songs[index].Bpm / (float)_defaultBpm;
        AnimationSpeed *= _animationSpeedMultiplier;
        _playingNow = index;
    }
    private void ShuffleSongs()
    {
        for (int i = 0; i < _songs.Count; i++) 
        {
            Song temp = _songs[i];
            int randomIndex = Random.Range(i, _songs.Count);
            _songs[i] = _songs[randomIndex];
            _songs[randomIndex] = temp;
        }
    }
}
