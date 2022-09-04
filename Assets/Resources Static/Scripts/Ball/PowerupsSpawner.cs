using Platform;
using UnityEngine;

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
                var spawnedPowerup = Instantiate(powerup, position, powerup.transform.rotation);
                spawnedPowerup.StartMovement();
                return;
            }
        }
    }
}