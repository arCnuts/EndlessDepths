using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;


public class PlayerMovement : MonoBehaviour
{
    public CharacterController playerController;
    public Transform playerTransform;
    public Transform camHolder;
    public Camera playerCamera;
    [SerializeField]
    private float moveSpeed = 5f;
    // [SerializeField]
    // private float footstepInterval = 10f;
    [SerializeField]
    private float sensitivity = 150f;
    private float rotationX,rotationY;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Move();
        MoveSound();
        Look();
    }

    private void Move()
    {
        float keyboardX = Input.GetAxisRaw("Horizontal");
        float keyboardY = Input.GetAxisRaw("Vertical");
        movement = new Vector3(keyboardX,0,keyboardY).normalized;

        Quaternion relativeRotation = Quaternion.Euler(0,playerTransform.transform.eulerAngles.y,0);

        playerController.Move(relativeRotation * movement * moveSpeed * Time.deltaTime);
    }

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [Header("Audio")]
    public Vector3 movement;
    public EventInstance Footstep;
    public string EventPath = "event:/Footsteps/Footsteps";

    void MoveSound() {
        if(movement != Vector3.zero)
        {
            EventInstance Footstep = RuntimeManager.CreateInstance(EventPath);
            Footstep.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject.transform));
            // RuntimeManager.AttachInstanceToGameObject(Walk, transform, RB);
        }
    }

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    private void Look() {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        rotationY = playerTransform.transform.localRotation.eulerAngles.y + mouseX;
        rotationX -= mouseY; rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        camHolder.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        playerTransform.transform.localRotation = Quaternion.Euler(0, rotationY, 0);
    }
}