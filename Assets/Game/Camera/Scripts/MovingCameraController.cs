using Cinemachine;
using UnityEngine;

namespace Game.Camera.Scripts
{
    public class MovingCameraController : MonoBehaviour
    {
        [SerializeField] private UnityEngine.Camera mainCamera;
        private CinemachineVirtualCamera _virtualCamera;
        private CinemachineConfiner _cinemachineConfiner;
        
        [SerializeField] private Collider cameraBoundsCollider;
        
        private Vector3 _targetPosition;
        
        private RaycastHit[] _raycastHits =  new RaycastHit[5];
        private bool _isMiddleMousePressed;
        
        private void Awake()
        {
            _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        }

        private void FixedUpdate()
        {
            MoveCameraByMouse();
        }

        private void Update()
        {
            CheckMiddleMouseButtonPress();
        }

        private void MoveCameraByMouse()
        {
            if (_isMiddleMousePressed)
            {
                Ray mouseRay = mainCamera.ScreenPointToRay(Input.mousePosition);
                var hitsSize = Physics.RaycastNonAlloc(mouseRay, _raycastHits);

                for (int i = 0; i < hitsSize; i++)
                {
                    var objectHit = _raycastHits[i].collider.gameObject;
                    if (objectHit.CompareTag("CameraMovingPlane"))
                    {
                        Vector3 hitPosition = _raycastHits[i].point;
                        hitPosition.y = 0f;
                        
                        Vector3 movingOffset = _targetPosition - hitPosition;
                        Vector3 newPosition = _virtualCamera.transform.position + movingOffset;
                        
                        _targetPosition -= newPosition - cameraBoundsCollider.bounds.ClosestPoint(newPosition);

                        _virtualCamera.transform.position =
                            cameraBoundsCollider.bounds.ClosestPoint(newPosition);
                    }
                }
            }
        }

        private void CheckMiddleMouseButtonPress()
        {
            if (Input.GetMouseButtonDown(2))
            {
                Ray mouseRay = mainCamera.ScreenPointToRay(Input.mousePosition);
                var hitsSize = Physics.RaycastNonAlloc(mouseRay, _raycastHits);

                for (int i = 0; i < hitsSize; i++)
                {
                    var objectHit = _raycastHits[i].collider.gameObject;
                    if (objectHit.CompareTag("CameraMovingPlane"))
                    {
                        _targetPosition = _raycastHits[i].point;
                        _targetPosition.y = 0f;
                        _isMiddleMousePressed = true;
                        break;
                    }
                }
            }

            if (Input.GetMouseButtonUp(2))
            {
                _isMiddleMousePressed = false;
            }
        }
    }
}
