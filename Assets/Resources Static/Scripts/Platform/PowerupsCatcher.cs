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
}