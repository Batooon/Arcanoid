#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using Platform;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class PowerupsSpawner : MonoBehaviour
{
    [SerializeField] private Powerup[] _powerups;

    public void TrySpawn(Vector2 position)
    {
        for (var i = 0; i < _powerups.Length; i++)
        {
            var powerup = _powerups[i];
            if (Random.value <= powerup.GetSpawnChance())
            {
                //TODO: spawn powerup
                return;
            }
        }
    }
}

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
    [SerializeField] private TrailRenderer _trail;

    public bool Activated { get; private set; }
    private bool _canMove;
    private Vector3 _velocity;

    private Transform _transform;

    private void Start()
    {
        _transform = transform;
    }

    public void SetTrailActive(bool isActive)
    {
        _trail.enabled = isActive;
    }

    public void SetDirection(Vector2 direction)
    {
        _direction = direction.normalized;
    }

    public Vector2 GetDirection()
    {
        return _direction;
    }

    public void AddOnHitSubscribers(UnityEvent onHitEvent)
    {
        if (onHitEvent != null)
            _ballHit.AddListener(onHitEvent.Invoke);
    }

    private void OnValidate()
    {
        SetDirection(_direction);
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
        _transform.localPosition += _velocity * Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (_canMove == false)
            return;
        if (Borders.Instance.TryBounce(out var normal, _transform.localPosition, _radius, _direction))
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
        var accurateDirection = Vector2.Reflect(_direction, normal);
        var randomizedDirection = RandomizeDirection(-_angleDistribution, _angleDistribution, accurateDirection);
        SetDirection(randomizedDirection);
        _velocity = new Vector3(_direction.x, _direction.y) * _maxSpeed;
        
        _ballHit?.Invoke();
    }

    private Vector2 RandomizeDirection(float minAngle, float maxAngle, Vector3 axis)
    {
        var randomizedRotation = Quaternion.AngleAxis(Random.Range(minAngle, maxAngle), transform.forward);
        var newDirection = randomizedRotation * axis;
        return newDirection;
    }
    
    public void Activate()
    {
        _canMove = true;
        Activated = true;
        SetTrailActive(true);
    }

    [ContextMenu("Randomize initial direction")]
    public void RandomizeInitialDirection()
    {
        var randomizedDirection = RandomizeDirection(_minStartingAngle, _minStartingAngle + _arcAngle, transform.right);
        SetDirection(randomizedDirection);
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
