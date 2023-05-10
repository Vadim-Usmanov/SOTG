using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject _teamsLayout;
    [SerializeField] private Button _playButton;
    [SerializeField] private TeamButton _teamButtonPrefab;
    [SerializeField] private GameObject _selectedTeamPanel;
    [SerializeField] private Image _selectedTeamLogo;
    [SerializeField] private TextMeshProUGUI _selectedTeamName;
    [SerializeField] private GameObject _offPlayUI;
    private Team _selectedTeam;
    public enum States {OffPlay, InPlay, Pause};
    public void Start()
    {
        FillTeams();
    }
    public void SwitchUI(States newState)
    {
        switch (newState) 
        {
            case States.InPlay:
            {
                _offPlayUI.SetActive(false);
                break;
            }
            case States.OffPlay:
            {
                _offPlayUI.SetActive(true);
                break;
            }
            case States.Pause:
            {
                break;
            }
            default: break;
        }
    }
    public void FillTeams()
    {
        GameController gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        foreach (Team team in gameController.Teams)
        {
            TeamButton teamButton = Instantiate(_teamButtonPrefab, _teamsLayout.transform);
            teamButton.AssignTeam(team);
        }
    }
    public void SelectTeam(Team team)
    {
        _selectedTeam = team;
        _selectedTeamLogo.sprite = _selectedTeam.Logo;
        _selectedTeamName.text = _selectedTeam.Name;
        _selectedTeamPanel.SetActive(true);
        _playButton.interactable = true;
    }
    public void StartGame()
    {
        _selectedTeamPanel.SetActive(false);
        _teamsLayout.SetActive(false);
        GameController gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        gameController.PlayGame();
    }
}
