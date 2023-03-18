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
    [SerializeField]
    private float sensitivity = 120f;
    private float rotationX,rotationY;
    public Vector3 movement;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        InvokeRepeating("MoveSound", 0, 0.3f);
    }

    void Update()
    {
        Move();
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


    [Header("Audio")]
    // public EventInstance Footstep;
    public string EventPath = "event:/Footsteps";

    private void MoveSound() {
        if(movement != Vector3.zero)
        {
            RuntimeManager.PlayOneShot(EventPath, playerTransform.position + Vector3.down);
        }
    }


    private void Look() {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        rotationY = playerTransform.transform.localRotation.eulerAngles.y + mouseX;
        rotationX -= mouseY; rotationX = Mathf.Clamp(rotationX, -89f, 89f);

        camHolder.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        playerTransform.transform.localRotation = Quaternion.Euler(0, rotationY, 0);
    }
}