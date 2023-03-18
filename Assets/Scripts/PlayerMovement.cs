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
    private float sensitivity = 2f;
    [SerializeField]
    private float speed = 2f;
    private float rotationX,rotationY;
    private Vector3 movement;
    [SerializeField]
    public GameObject FlashlightLight;
    private bool FlashlightActive = false;
    [Header("Audio")]
    public string EventPath = "event:/Footsteps";
    public string FlashlightOn = "event:/Flashlight/ON";
    public string FlashlightOff = "event:/Flashlight/OFF";

    void Start()
    {
        FlashlightLight.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        InvokeRepeating("MoveSound", 0, 0.3f);
    }

    void Update()
    {
        Move();
        Look();
        Flashlight();
    }

    private void Move()
    {
        float keyboardX = Input.GetAxisRaw("Horizontal");
        float keyboardY = Input.GetAxisRaw("Vertical");
        movement = new Vector3(keyboardX,0,keyboardY).normalized;

        Quaternion relativeRotation = Quaternion.Euler(0,playerTransform.transform.eulerAngles.y,0);
        playerController.Move(relativeRotation * movement * moveSpeed * Time.deltaTime);
    }




    private void MoveSound() {
        if(movement != Vector3.zero)
        {
            RuntimeManager.PlayOneShot(EventPath, playerTransform.position + new Vector3(0, -3, 0));
        }
    }

    private void Flashlight() {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (FlashlightActive == false)
            {
                RuntimeManager.PlayOneShot(FlashlightOn, playerTransform.position + Vector3.down);
                FlashlightLight.gameObject.SetActive(true);
                FlashlightActive = true;
            }
            else
            {
                RuntimeManager.PlayOneShot(FlashlightOff, playerTransform.position + Vector3.down);
                FlashlightLight.gameObject.SetActive(false);
                FlashlightActive = false;
            }
        }
    }

    private void Look() {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * 100; //* Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity; //* Time.deltaTime;

        rotationY = playerTransform.transform.localRotation.eulerAngles.y + mouseX;
        rotationX -= mouseY; rotationX = Mathf.Clamp(rotationX, -89f, 89f);

        Quaternion test1 = Quaternion.Euler(0, rotationY, 0);
        playerTransform.transform.localRotation = Quaternion.Slerp(playerTransform.transform.localRotation, test1, Time.deltaTime * speed);
        // playerTransform.transform.localRotation = Quaternion.Euler(0, rotationY, 0);

        Quaternion test2 = Quaternion.Euler(rotationX, 0, 0);
        camHolder.transform.localRotation = Quaternion.Slerp(camHolder.transform.localRotation, test2, Time.deltaTime * speed);
        // camHolder.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
    }
}