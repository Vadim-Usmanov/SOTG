using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class ControlSwitch : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image _image;
    [SerializeField] private DJ _dj;
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        _dj.Switch();
    }
}
