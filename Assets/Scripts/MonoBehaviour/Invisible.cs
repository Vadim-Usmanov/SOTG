using UnityEngine;

public class Invisible : MonoBehaviour
{
    [SerializeField] private Renderer _invisibleRenderer;
    void Awake()
    {
        _invisibleRenderer.enabled = false;
    }
}
