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
    private Transform playerCamera = null;
    [SerializeField]
    private Transform cameraHolder = null;
    private Vector3 _startPos;
    [SerializeField]
    public CharacterController playerController;

    private void Awake()
    {
        _startPos = playerCamera.localPosition;
    }

    private void PlayMotion(Vector3 motion)
    {
        playerCamera.localPosition += motion; 
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
        if(playerCamera.localPosition == _startPos) return;
        playerCamera.localPosition = Vector3.Lerp(playerCamera.localPosition, _startPos, Time.deltaTime);
    }

    private Vector3 FocusTarget()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + cameraHolder.localPosition.y, transform.position.z);
        pos += cameraHolder.forward * 15.0f;
        return pos;
    }

    void Update()
    {
        if(!_enable) return;
        CheckMotion();
        ResetPosition();
        playerCamera.LookAt(FocusTarget());
    }
}