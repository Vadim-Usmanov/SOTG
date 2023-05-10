using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TeamButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private Image _image;
    [SerializeField] private Team _team;
    public void AssignTeam(Team team)
    {
        _team = team;
        _name.text = team.Name;
        _image.sprite = team.Logo;
    }
    public void Selected()
    {
        UIController uiController = GameObject.FindWithTag("UIController").GetComponent<UIController>();
        uiController.SelectTeam(_team);
    }
}
