using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _speed;
    [SerializeField] private LayerMask _groundLayer;

    private ConfigurableJoint _joint;
    private Rigidbody _rb;
    private float _maxAngle = 45;

    private void Awake()
    {
        _joint = GetComponent<ConfigurableJoint>();
        _rb = GetComponent<Rigidbody>();
    }

    //private void FixedUpdate()
    //{
    //    if (!IsGrounded(out float groundAngle)) return;
    //    Vector3 toTarget = _target.position - transform.position;
    //    Vector3 toTargetXZ = new Vector3 (toTarget.x, 0f, toTarget.z);

    //    Quaternion rotation = Quaternion.Inverse(Quaternion.LookRotation(toTargetXZ));
    //    _joint.targetRotation = rotation;

    //    if (_joint.targetRotation == rotation)
    //    {
    //        Debug.Log("OK!");
    //        _rb.AddForce(toTargetXZ * _speed, ForceMode.Impulse);
    //        _rb.velocity = Vector3.ClampMagnitude(_rb.velocity, _speed);
    //    }
    //}

    private void FixedUpdate()
    {
        Debug.DrawLine(transform.position, _target.position, Color.red);
        if (Vector3.Distance(_target.position, transform.position) > 3 && IsGrounded(out float groundAngle) && groundAngle < _maxAngle)
        {
            Vector3 _toTarget = _target.position - transform.position;
            Vector3 _toTargetXZ = new Vector3(_toTarget.x, 0, _toTarget.z);
            Quaternion rotation = Quaternion.LookRotation(_toTargetXZ);

            _joint.targetRotation = Quaternion.Inverse(rotation);
            if (_rb.velocity.magnitude < _speed)
            {
                float bonusSpeed = _speed * (groundAngle / (_speed * 0.7f));
                Debug.Log($"BONUS SPEED: {bonusSpeed}");
                _rb.velocity = Vector3.ClampMagnitude(_rb.velocity, _speed);
                _rb.AddForce((transform.forward + new Vector3(0, groundAngle / 100, 0)) * (_speed + bonusSpeed), ForceMode.Impulse);
            }
        }
    }

    private bool IsGrounded(out float groundAngle)
    {
        groundAngle = 0f;

        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 0.8f, _groundLayer))
        {
            groundAngle = Vector3.Angle(hit.normal, Vector3.up);
            return true;
        }

        return false;
    }
}
