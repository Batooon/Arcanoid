#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class Ball : MonoBehaviour
{
    [SerializeField] private float _minStartingAngle;
    [SerializeField] private float _arcAngle;

    [SerializeField] private float _radius;
    [SerializeField, Range(0f, 100f)] private float _maxSpeed = 5f;
    [SerializeField, Range(0f, 100f)] private float _maxAcceleration = 10f;
    [SerializeField] private Vector2 _direction;
    [SerializeField] private float _angleDistribution;

    [SerializeField] private UnityEvent _ballHit;

    private bool _canMove;
    private Vector3 _velocity;

    public void Activate()
    {
        RandomizeInitialDirection();
        _canMove = true;
    }

    public void AddOnHitSubscribers(UnityEvent onHitEvent)
    {
        _ballHit.AddListener(onHitEvent.Invoke);
    }

    private void OnValidate()
    {
        _direction = _direction.normalized;
    }

    private void Update()
    {
        if (_canMove == false)
            return;
        var desiredVelocity = new Vector3(_direction.x, _direction.y) * _maxSpeed;
        var maxSpeedChange = _maxAcceleration * Time.deltaTime;

        _velocity.x = Mathf.MoveTowards(_velocity.x, desiredVelocity.x, maxSpeedChange);
        _velocity.y = Mathf.MoveTowards(_velocity.y, desiredVelocity.y, maxSpeedChange);

        // _velocity = Vector3.MoveTowards(_velocity, desiredVelocity, maxSpeedChange);
        transform.localPosition += _velocity * Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (_canMove == false)
            return;
        if (Borders.Instance.TryBounce(out var normal, transform.localPosition, _radius, _direction))
        {
            Bounce(normal);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.TryGetComponent(out Block block))
        {
            block.Destroy();
        }
        var normal = col.contacts[0].normal;
        Bounce(normal);
    }

    private void Bounce(Vector2 normal)
    {
        _ballHit?.Invoke();
        // _direction = Vector2.Reflect(_direction, normal);
        var accurateDirection = Vector2.Reflect(_direction, normal);
        RandomizeDirection(_angleDistribution, _angleDistribution, accurateDirection);
        _velocity = new Vector3(_direction.x, _direction.y) * _maxSpeed;
    }
    
    private void RandomizeDirection(float minAngle, float maxAngle, Vector3 axis)
    {
        //TODO: bounce direction randomization
        var newDirection =
            Quaternion.AngleAxis(Random.Range(minAngle, maxAngle), axis) *
            transform.right;
        _direction = newDirection;
    }

    [ContextMenu("Randomize initial direction")]
    private void RandomizeInitialDirection()
    {
        RandomizeDirection(_minStartingAngle, _minStartingAngle + _arcAngle, transform.forward);
    }
    
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        var tr = transform;
        var position = tr.position;
        var forward = tr.forward;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(position, transform.TransformPoint(_direction));
        Gizmos.DrawWireSphere(position, _radius);
        Gizmos.color = Color.white;
        Handles.color = new Color(0.23f, 0.23f, 0.23f, 0.52f);
        Handles.DrawSolidArc(position, forward, Quaternion.AngleAxis(_minStartingAngle, forward) * transform.right,
            _arcAngle, HandleUtility.GetHandleSize(position) * 2f);
        Handles.color = Color.white;
    }
#endif
}
