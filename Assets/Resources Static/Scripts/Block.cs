using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Block : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _renderer;
    
    public void Destroy()
    {
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
