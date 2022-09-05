using UnityEngine;

public class Borders : MonoBehaviour
{
    public static Borders Instance;
    
    public float RightBorderX { get; private set; }
    public float LeftBorderX { get; private set; }
    public float TopBorderY { get; private set; }
    public float BottomBorderY { get; private set; }
    
    public Rect Border { get; private set; }

    private void Awake()
    {
        if (Instance != null)
            Destroy(this);
        else
            Instance = this;
    }

    private void Start()
    {
        var topRightCorner = Utils.Instance.MainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
        var bottomLeftCorner = Utils.Instance.MainCamera.ScreenToWorldPoint(Vector3.zero);
        
        RightBorderX = topRightCorner.x;
#if UNITY_EDITOR
        var right = new GameObject("right")
        {
            transform =
            {
                position = new Vector3(RightBorderX, 0, 0)
            }
        };
#endif
        TopBorderY = topRightCorner.y;
#if UNITY_EDITOR
        var top = new GameObject("top")
        {
            transform =
            {
                position = new Vector3(0, TopBorderY, 0)
            }
        };
#endif
        LeftBorderX = bottomLeftCorner.x;
#if UNITY_EDITOR
        var left = new GameObject("left")
        {
            transform =
            {
                position = new Vector3(LeftBorderX, 0, 0)
            }
        };
#endif
        BottomBorderY = bottomLeftCorner.y;
#if UNITY_EDITOR
        var bottom = new GameObject("bottom")
        {
            transform =
            {
                position = new Vector3(0, BottomBorderY, 0)
            }
        };
#endif

        Border = new Rect(LeftBorderX, BottomBorderY, RightBorderX - LeftBorderX, TopBorderY - BottomBorderY);
    }

    public bool CanPlatformMove(float rightPosition, float leftPosition)
    {
        return leftPosition >= LeftBorderX && rightPosition <= RightBorderX;
    }

    public bool TryBounce(out Vector2 normal, Vector2 position, float radius, Vector2 direction)
    {
        if (HitRightWall(position, radius) && FacingBorder(Vector2.left, direction))
        {
            normal = Vector2.left;
            return true;
        }

        if (HitLeftWall(position, radius) && FacingBorder(Vector2.right, direction))
        {
            normal = Vector2.right;
            return true;
        }

        if (HitTopWall(position, radius) && FacingBorder(Vector2.down, direction))
        {
            normal = Vector2.down;
            return true;
        }

        normal = default;
        return false;
    }

    public bool HitBottomWall(Vector2 position, float radius)
    {
        return position.y - radius <= BottomBorderY;
    }

    private bool HitRightWall(Vector2 position, float radius)
    {
        return position.x + radius >= RightBorderX;
    }

    private bool HitLeftWall(Vector2 position, float radius)
    {
        return position.x - radius <= LeftBorderX;
    }

    private bool HitTopWall(Vector2 position, float radius)
    {
        return position.y + radius >= TopBorderY;
    }

    private bool FacingBorder(Vector2 borderNormal, Vector2 direction)
    {
        return Vector2.Dot(borderNormal, direction) < 0;
    }
}
