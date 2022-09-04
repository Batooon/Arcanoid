using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class BallsFactory : ScriptableObject
{
    [SerializeField] private Ball _prefab;

    private ObjectPool<Ball> _ballsPool;
    private UnityEvent _hitCallback;

    public void Initialize(UnityEvent hitCallback)
    {
        _ballsPool = new ObjectPool<Ball>(_prefab);
        _hitCallback = hitCallback;
    }

    public Ball SpawnBall(Vector2 position, Vector2 direction, bool activate = true)
    {
        var ball = _ballsPool.GetAvailableObject();
        ball.transform.position = position;
        ball.SetDirection(direction);
        ball.AddOnHitSubscribers(_hitCallback);
        ball.Activate();
        ball.gameObject.SetActive(true);
        
        return ball;
    }

    public Ball[] GetAllActiveBalls()
    {
        return _ballsPool.GetActiveObjects().ToArray();
    }

    public Ball GetAvailableObject()
    {
        return _ballsPool.GetAvailableObject();
    }
}