using UnityEngine;

[CreateAssetMenu(fileName = "NewTeam", menuName = "ScriptableObjects/Teams", order = 50)]
public class Team : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private Sprite _logo;
    [SerializeField] private Color _darkColor;
    [SerializeField] private Color _lightColor;
    [SerializeField] private Color _usedColor;
    public string Name { get => _name; }
    public Sprite Logo { get => _logo; }
    public Color DarkColor { get => _darkColor; }
    public Color LightColor { get => _lightColor; }
    public Color UsedColor { get => _usedColor; }
    public enum Shirts {Dark, Light}
    public void SetColor(Shirts shirts)
    {
        if (shirts == Shirts.Dark) _usedColor = _darkColor;
        else _usedColor = _lightColor;
    }
}
