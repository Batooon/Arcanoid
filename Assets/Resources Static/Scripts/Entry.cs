using Platform;
using UnityEngine;
using UnityEngine.Events;

public class Entry : MonoBehaviour
{
    [SerializeField] private BlocksGenerator _blocksGenerator;
    [SerializeField] private Transform _blocksParent;
    [SerializeField] private Ball _ballPrefab;
    [SerializeField] private Transform _ballSpawnPoint;
    [SerializeField] private PlatformMover _platformMover;
    [SerializeField] private UnityEvent _ballHit;

    private Ball _spawnedMainBall;

    private ObjectPool<Ball> _ballPool;

    private void Start()
    {
        _blocksGenerator.SpawnBlocks(_blocksParent);
        _ballPool = new ObjectPool<Ball>(_ballPrefab);
        var spawnedBall = _ballPool.GetAvailableObject();
        spawnedBall.transform.position = _ballSpawnPoint.position;
        spawnedBall.AddOnHitSubscribers(_ballHit);
        spawnedBall.gameObject.SetActive(true);
        _spawnedMainBall = spawnedBall;
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (_spawnedMainBall.Activated == false && Input.GetMouseButton(0))
        {
            _spawnedMainBall.transform.position += _platformMover.MoveDelta;
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            ActivateBall();
        }
#else
        if (Input.touchCount > 0)
        {
            var touch = Input.touches[0];
            if (touch.phase == TouchPhase.Ended)
            {
                ActivateBall();
            }
            else if(touch.phase == TouchPhase.Moved)
            {
                _spawnedMainBall.transform.position += _platformMover.MoveDelta;
            }
        }
#endif
    }

    private void ActivateBall()
    {
        _spawnedMainBall.Activate();
    }
}