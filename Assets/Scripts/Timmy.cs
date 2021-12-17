using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Timmy : MonoBehaviour
{
    [SerializeField] float _walkSpeed = 0.2f;
    [SerializeField] float _runningSpeed = 0.4f;
    [SerializeField] float _rotationSpeed = 20.0f;

    public bool Running { get => _running; set => _running = value; }

    bool _running = false;
    bool _isJumping = false;

    bool _reachedDestination = true;
    Vector3 _currentDestination;
    Vector3 _directionToDestination;
    float _angleToDestination;

    Camera _camera;
    Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();

        _running = false;
        _isJumping = false;
        _currentDestination = transform.position;
    }

    private void Start()
    {
        _camera = FindObjectOfType<Camera>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SetDestination();
        }
    }

    private void FixedUpdate()
    {
        _rb.velocity = Vector3.zero;

        if (!_reachedDestination && !_isJumping)
        {
            RotateToDestination();

            // Wait for rotation if the angle is too wide
            if (_angleToDestination < 45f)
            {
                MoveToDestination();
            }
        }
    }

    public void StartJump()
    {
        _isJumping = true;
    }

    public void EndJump()
    {
        _isJumping = false;
    }

    private void SetDestination()
    {
        // Don't trigger raycast on UI clicks
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.CompareTag("MovementPlane"))
            {
                _currentDestination = hit.point;
                _reachedDestination = false;

                _directionToDestination = (_currentDestination - transform.position).normalized;
            }
        }
    }

    private void MoveToDestination()
    {
        float distance = Mathf.Abs(Vector3.Distance(_currentDestination, transform.position));
        float speed = Running ? _runningSpeed : _walkSpeed;

        if (distance > 0.02f)
        {
            _rb.velocity = _directionToDestination * speed;
        }
        else
        {
            _reachedDestination = true;
        }
    }

    private void RotateToDestination()
    {
        _angleToDestination = Vector3.SignedAngle(_directionToDestination, transform.forward, Vector3.up);
        float yRotation = Mathf.Min(Mathf.Abs(_rotationSpeed), Mathf.Abs(_angleToDestination)) * Mathf.Sign(_angleToDestination);
        transform.Rotate(0.0f, -yRotation, 0.0f);
    }
}
