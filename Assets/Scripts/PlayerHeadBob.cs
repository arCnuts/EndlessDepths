using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeadBob : MonoBehaviour
{
    [SerializeField]
    private bool _enable = true;
    [SerializeField]
    private float _amplitude = 0.003f;
    [SerializeField]
    private float _frequency = 20.0f;

    [SerializeField]
    private Transform _camera = null;
    [SerializeField]
    private Transform _cameraHolder = null;
    private Vector3 _startPos;
    [SerializeField]
    public CharacterController playerController;

    private void Awake()
    {
        _startPos = _camera.localPosition;
    }

    private void PlayMotion(Vector3 motion)
    {
        _camera.localPosition += motion; 
    }

    private Vector3 FootStepMotion()
    {
        Vector3 pos = Vector3.zero;
        pos.x += Mathf.Cos(Time.time * _frequency) * _amplitude;
        pos.y += Mathf.Sin(Time.time * _frequency / 2) * _amplitude * 2;
        return pos;
    }

    private void CheckMotion()
    {
        float currendSpeed = new Vector3(playerController.velocity.x, 0, playerController.velocity.z).magnitude;

        if(currendSpeed < 3f) return;

        PlayMotion(FootStepMotion());
    }

    private void ResetPosition()
    {
        if(_camera.localPosition == _startPos) return;
        _camera.localPosition = Vector3.Lerp(_camera.localPosition, _startPos, 1 * Time.deltaTime);
    }

    private Vector3 FocusTarget()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + _cameraHolder.localPosition.y, transform.position.z);
        pos += _cameraHolder.forward * 15.0f;
        return pos;
    }

    void Update()
    {
        if(!_enable) return;
        // ResetPosition();
        CheckMotion();
        ResetPosition();
        _camera.LookAt(FocusTarget());
    }
}