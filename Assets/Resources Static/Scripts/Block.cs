using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Block : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _renderer;
    private PowerupsSpawner _spawner;

    public void Init(PowerupsSpawner spawner)
    {
        _spawner = spawner;
    }
    
    public void Destroy()
    {
        _spawner.TrySpawn(transform.position);
        Destroy(gameObject);
    }

    public void SetSprite(Sprite sprite)
    {
        _renderer.sprite = sprite;
    }

    public Vector2 GetSize()
    {
        return transform.localScale;
    }
}
