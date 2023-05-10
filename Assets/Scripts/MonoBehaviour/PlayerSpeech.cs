using UnityEngine;
using System.Collections;
using TMPro;

public class PlayerSpeech : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] GameObject _canvas;
    private IEnumerator _coroutine;
    public IEnumerator Coroutine 
    {
        get => _coroutine;
        set
        {
            if (_coroutine != null) StopCoroutine(_coroutine);
            _coroutine = value;
            StartCoroutine(_coroutine);
        }
    }
    public void Display(string text, float seconds = 10f)
    {
        _text.SetText(text);
        _canvas.SetActive(true);
        Coroutine = WaitAndHide(seconds);
    }
    public void SetQuaternion(Quaternion quaternion)
    {
        _text.transform.rotation = quaternion;
    }
    private IEnumerator WaitAndHide(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        _canvas.SetActive(false);
        _coroutine = null;
    }
}
