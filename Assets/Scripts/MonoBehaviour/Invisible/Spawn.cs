using UnityEngine;

public class Spawn : Invisible
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    public Vector3 RandomPosition()
    {
        float x = Random.Range(_spriteRenderer.bounds.min.x, _spriteRenderer.bounds.max.x);
        float z = Random.Range(_spriteRenderer.bounds.min.z, _spriteRenderer.bounds.max.z);        
        return new Vector3(x, 0f, z);
    }
}
