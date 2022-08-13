using UnityEngine;

namespace Platform
{
    public class PlatformMover : MonoBehaviour
    {
        private const int LeftMouseButton = 0;

        [SerializeField] private SpriteRenderer _graphic;

        public float RightCheck => _rightCheck.position.x;
        public float LeftCheck => _leftCheck.position.x;
        
        public Vector3 MoveDelta { get; private set; }

        private Transform _transform;

        private float _oldMousePositionX;
        private float _newMousePositionX;

        private Transform _leftCheck;
        private Transform _rightCheck;

        private void Awake()
        {
            _transform = transform;
        }

        private void Start()
        {
            var leftCheckPosition = new Vector3(_graphic.bounds.min.x, transform.position.y, 0);
            _leftCheck = new GameObject("left check").transform;
            _leftCheck.position = leftCheckPosition;
            _leftCheck.SetParent(_transform);

            var rightCheckPosition = new Vector3(_graphic.bounds.max.x, transform.position.y, 0);
            _rightCheck = new GameObject("right check").transform;
            _rightCheck.position = rightCheckPosition;
            _rightCheck.SetParent(_transform);
        }

        private void Update()
        {
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(LeftMouseButton))
            {
                PointerDown(Input.mousePosition);
            }
            
            if (Input.GetMouseButton(LeftMouseButton))
            {
                PointerMove(Input.mousePosition);
            }
#else
            if (Input.touchCount > 0)
            {
                var touch = Input.touches[0];
                if (touch.phase == TouchPhase.Began)
                {
                    PointerDown(touch.position);
                }
                else if (touch.phase == TouchPhase.Moved)
                {
                    PointerMove(touch.position);
                }
            }
#endif
        }

        private void PointerDown(Vector3 position)
        {
            var projectedPointerPosition = Utils.Instance.MainCamera.ScreenToWorldPoint(position);
            _oldMousePositionX = projectedPointerPosition.x;
        }

        private void PointerMove(Vector3 position)
        {
            var projectedPointer = Utils.Instance.MainCamera.ScreenToWorldPoint(position);
            _newMousePositionX = projectedPointer.x;
            var deltaX = _newMousePositionX - _oldMousePositionX;
            MoveDelta = Vector3.right * deltaX;
            // var delta = Vector3.right * deltaX;
            if (Borders.Instance.CanPlatformMove(RightCheck + deltaX, LeftCheck + deltaX))
            {
                _transform.localPosition += MoveDelta;
            }
            else
            {
                if (deltaX > 0)
                {
                    MoveDelta = Vector3.right * (Borders.Instance.RightBorderX - RightCheck);
                }
                else
                {
                    MoveDelta = Vector3.right * (Borders.Instance.LeftBorderX - LeftCheck);
                }

                _transform.localPosition += MoveDelta;
            }

            _oldMousePositionX = _newMousePositionX;
        }
    }
}