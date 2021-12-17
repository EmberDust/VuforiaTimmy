using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimmyAnimations : MonoBehaviour
{
    [SerializeField] Timmy _mainScript;

    Animator _animator;
    Rigidbody _rb;

    int _velocityHash;
    int _runningHash;
    int _danceHash;
    int _jumpHash;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();

        _animator = GetComponent<Animator>();

        _velocityHash = Animator.StringToHash("Velocity");
        _runningHash = Animator.StringToHash("Running");
        _danceHash = Animator.StringToHash("Dance");
        _jumpHash = Animator.StringToHash("Jump");
    }

    private void Update()
    {
        _animator.SetBool(_runningHash, _mainScript.Running);
        _animator.SetFloat(_velocityHash, _rb.velocity.magnitude);
    }

    public void TriggerDance()
    {
        _animator.SetTrigger(_danceHash);
    }

    public void TriggerJump()
    {
        _animator.SetTrigger(_jumpHash);
    }

}
