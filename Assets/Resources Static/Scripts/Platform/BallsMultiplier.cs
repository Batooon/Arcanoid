using System;
using UnityEngine;

namespace Platform
{
    public class BallsMultiplier : MonoBehaviour, IPowerup
    {
        private const float Tau = 6.28318530718f;
        
        [SerializeField] private int _multiplier;
        [SerializeField] private BallsFactory _ballsFactory;

        private float _angleStep;

        private void Start()
        {
            _angleStep = Tau / _multiplier;
        }

        public void Activate()
        {
            var balls = _ballsFactory.GetAllActiveBalls();
            foreach(Ball ball in balls)
            {
                var direction = ball.GetDirection();
                for (var i = 1; i <= _multiplier; i++)
                {
                    var angRad = _angleStep * i;
                    var newBallDirection = Quaternion.Euler(0, 0, angRad * Mathf.Rad2Deg) * direction;
                    _ballsFactory.SpawnBall(ball.transform.position, newBallDirection);
                }
            }
        }
    }
}