using UnityEngine;

namespace Platform
{
    public class PowerupsCatcher : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.transform.TryGetComponent(out Powerup powerup))
            {
                powerup.Activate();
            }
        }
    }
}