using UnityEngine;

public class ShadingBody : MonoBehaviour
{
    [SerializeField] private Sprite _shadowSprite;
    private GameObject _shadow;
    private Vector3 _position;
    protected virtual void Start()
    {
        if (_shadow == null)
        {
            Field field = GameObject.FindWithTag("Field").GetComponent<Field>();
            _shadow = field.CreateShadow(_shadowSprite);
        }
        UpdateShadowPosition();
    }
    protected virtual void LateUpdate()
    {
        if (transform.hasChanged) UpdateShadowPosition();
        transform.hasChanged = false;
    }
    private void UpdateShadowPosition()
    {
        _position = transform.position;
        _position.y = 0f;
        _shadow.transform.SetPositionAndRotation(_position, _shadow.transform.rotation);
    }
}
