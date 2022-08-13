using UnityEngine;

namespace Platform
{
    public class PowerupsCatcher : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.transform.TryGetComponent(out IPowerup powerup))
            {
                powerup.Activate();
            }
        }
    }

    public interface IPowerup
    {
        void Activate();
    }

    public class BallsMultiplier : MonoBehaviour, IPowerup
    {
        [SerializeField] private int _multiplier;
        private ObjectPool<Ball> _ballsPool;
        
        public void Initialize(ObjectPool<Ball> ballsPool)
        {
            _ballsPool = ballsPool;
        }
        
        public void Activate()
        {
            var activeBalls = _ballsPool.GetActiveObjects();
            foreach(Ball ball in activeBalls)
            {
                //TODO: ask balls factory to spawn x_multiplier balls amount
            }
        }
    }
}