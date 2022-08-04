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
        var topRightAngle = Utils.Instance.MainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
        var bottomLeftAngle = Utils.Instance.MainCamera.ScreenToWorldPoint(Vector3.zero);
        
        RightBorderX = topRightAngle.x;
#if UNITY_EDITOR
        var right = new GameObject("right")
        {
            transform =
            {
                position = new Vector3(RightBorderX, 0, 0)
            }
        };
#endif
        TopBorderY = topRightAngle.y;
#if UNITY_EDITOR
        var top = new GameObject("top")
        {
            transform =
            {
                position = new Vector3(0, TopBorderY, 0)
            }
        };
#endif
        LeftBorderX = bottomLeftAngle.x;
#if UNITY_EDITOR
        var left = new GameObject("left")
        {
            transform =
            {
                position = new Vector3(LeftBorderX, 0, 0)
            }
        };
#endif
        BottomBorderY = bottomLeftAngle.y;
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
        if (position.x + radius >= RightBorderX && FacingBorder(Vector2.left, direction))
        {
            normal = Vector2.left;
            return true;
        }

        if (position.x - radius <= LeftBorderX && FacingBorder(Vector2.right, direction))
        {
            normal = Vector2.right;
            return true;
        }

        if (position.y + radius >= TopBorderY && FacingBorder(Vector2.down, direction))
        {
            normal = Vector2.down;
            return true;
        }

        if (position.y - radius <= BottomBorderY && FacingBorder(Vector2.up, direction))
        {
            normal = Vector2.up;
            return true;
        }

        normal = default;
        return false;
    }

    private bool FacingBorder(Vector2 borderNormal, Vector2 direction)
    {
        return Vector2.Dot(borderNormal, direction) < 0;
    }
}
