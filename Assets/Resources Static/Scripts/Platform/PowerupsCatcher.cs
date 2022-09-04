using UnityEngine;

namespace Platform
{
    public class PowerupsCatcher : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.transform.TryGetComponent(out Powerup powerup))
            {
                powerup.Activate();
            }
        }
    }

    public abstract class Powerup : MonoBehaviour
    {
        [SerializeField] private float _movementSpeed;
        [SerializeField] private float _acceleration;
        [SerializeField, Range(0.01f, 1f)] private float _spawnChance;

        private Vector3 _velocity;
        
        private Transform _transform;
        private Vector2 _moveDirection;
        private bool _canMove;

        private void Start()
        {
            _transform = transform;
            _moveDirection = -_transform.up;
        }

        public virtual void StartMovement()
        {
            _canMove = true;
        }

        public float GetSpawnChance()
        {
            return _spawnChance;
        }

        private void Update()
        {
            if (_canMove == false)
                return;
            var desiredVelocity = _moveDirection * _movementSpeed;
            var maxSpeedChange = _acceleration * Time.deltaTime;

            _velocity.x = Mathf.MoveTowards(_velocity.x, desiredVelocity.x, maxSpeedChange);
            _velocity.y = Mathf.MoveTowards(_velocity.y, desiredVelocity.y, maxSpeedChange);
            _transform.localPosition += _velocity * Time.deltaTime;
        }

        public abstract void Activate();
    }
    
    // public interface IPowerup
    // {
    //     void Activate();
    // }
}