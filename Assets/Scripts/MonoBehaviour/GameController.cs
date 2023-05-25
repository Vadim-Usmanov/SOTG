using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private Player _playerPrefab;
    [SerializeField] private Disk _diskPrefab;
    [SerializeField] private List<Team> _teams;
    [SerializeField] private Team _darkTeam;
    [SerializeField] private Team _lightTeam;
    [SerializeField] private CameraDirector _cameraDirector;
    [SerializeField] private UIController _uiController;
    [SerializeField] private List<string> _names;
    [SerializeField] private List<Color> _neutralColors;    
    public enum Sides {Left, Right};
    public Player PlayerPrefab { get => _playerPrefab; }
    public Disk DiskPrefab { get => _diskPrefab; }
    public IList<Team> Teams { get => _teams.AsReadOnly(); }
    public void Start()
    {
        AddPracticeGames(1);
    }
    public void AddPracticeGames(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject gameObject = new GameObject("Game");
            gameObject.AddComponent<Practice>();
        }
    }
    public void PlayGame()
    {
        _cameraDirector.CameraStartGame();
        _uiController.SwitchUI(UIController.States.InPlay);
    }
    public Color RandomNeutralColor()
    {
        int index = UnityEngine.Random.Range(0, _neutralColors.Count - 1);
        return _neutralColors[index];
    }
    public string RandomName()
    {
        int index = UnityEngine.Random.Range(0, _names.Count - 1);
        string name = _names[index];
        _names.RemoveAt(index);
        return name;
    }
}
