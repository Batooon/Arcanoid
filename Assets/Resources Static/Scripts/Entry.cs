using UnityEngine;

public class Entry : MonoBehaviour
{
    [SerializeField] private BlocksGenerator _blocksGenerator;
    [SerializeField] private Transform _blocksParent;
    [SerializeField] private Ball _ballPrefab;
    [SerializeField] private Transform _ballSpawnPoint;

    private ObjectPool<Ball> _ballPool;

    private void Start()
    {
        _blocksGenerator.SpawnBlocks(_blocksParent);
        _ballPool = new ObjectPool<Ball>(_ballPrefab);

        var spawnedBall = _ballPool.GetAvailableObject();
        spawnedBall.transform.position = _ballSpawnPoint.position;
        spawnedBall.Activate();
        spawnedBall.gameObject.SetActive(true);
    }
}