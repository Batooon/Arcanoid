using Platform;
using UnityEngine;
using UnityEngine.Events;

public class Entry : MonoBehaviour
{
    [SerializeField] private BallsFactory _ballsFactory;
    [SerializeField] private BlocksGenerator _blocksGenerator;
    [SerializeField] private Transform _blocksParent;
    [SerializeField] private Transform _ballSpawnPoint;
    [SerializeField] private PlatformMover _platformMover;
    [SerializeField] private PowerupsSpawner _powerupsSpawner;
    [SerializeField] private UnityEvent _ballHit;

    private Ball _spawnedMainBall;

    private void Start()
    {
        _blocksGenerator.SpawnBlocks(_powerupsSpawner, _blocksParent);
        _ballsFactory.Initialize();
        InitializeStartingBall();
    }

    private void InitializeStartingBall()
    {
        var spawnedBall = _ballsFactory.GetAvailableObject();
        spawnedBall.transform.position = _ballSpawnPoint.position;
        spawnedBall.AddOnHitSubscribers(_ballHit);
        spawnedBall.SetTrailActive(false);
        spawnedBall.gameObject.SetActive(true);
        _spawnedMainBall = spawnedBall;
        _spawnedMainBall.RandomizeInitialDirection();
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