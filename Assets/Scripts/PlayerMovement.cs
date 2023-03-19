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

<<<<<<< HEAD
    private void MoveSound()
    {
        float currentSpeed = new Vector3(playerController.velocity.x, 0, playerController.velocity.z).magnitude;
        if(currentSpeed < toggleSpeed) return;
        RuntimeManager.PlayOneShot(FootstepPath, playerTransform.position + new Vector3(0, -3, 0));
=======



    private void MoveSound() {
        if(movement != Vector3.zero)
        {
            RuntimeManager.PlayOneShot(EventPath, playerTransform.position + new Vector3(0, -3, 0));
        }
>>>>>>> parent of cf0e007 (1)
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

<<<<<<< HEAD
    private Ray interactionRay;

    private void Hide()
    {
        Ray interactionRay = new Ray(playerTransform.position, playerTransform.forward);
        RaycastHit interactionRayHit;
        Vector3 interactionRayEndpoint = playerTransform.forward;

        Debug.DrawLine(playerTransform.position, interactionRayEndpoint);

        bool hitFound = Physics.Raycast(interactionRay, out interactionRayHit, 10f);
        if(hitFound)
        {
            Debug.Log("Hi");
            if(interactionRayHit.collider.gameObject.name == "table")
            {
                Debug.Log("Hello");
            }
        }
    }

    private void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;
=======
    private void Look() {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * 100; //* Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity; //* Time.deltaTime;
>>>>>>> parent of cf0e007 (1)

        rotationY += mouseX + camHolder.transform.localRotation.eulerAngles.y;
        rotationX -= mouseY + playerTransform.transform.localRotation.eulerAngles.x;
        
        rotationX = Mathf.Clamp(rotationX, -89f, 89f);

<<<<<<< HEAD
=======
        Quaternion test1 = Quaternion.Euler(0, rotationY, 0);
        playerTransform.transform.localRotation = Quaternion.Slerp(playerTransform.transform.localRotation, test1, Time.deltaTime * speed);
>>>>>>> parent of cf0e007 (1)
        // playerTransform.transform.localRotation = Quaternion.Euler(0, rotationY, 0);
        playerTransform.transform.localRotation = Quaternion.Slerp(playerTransform.transform.localRotation, Quaternion.Euler(0, rotationY, 0), speed * Time.deltaTime);

<<<<<<< HEAD
=======
        Quaternion test2 = Quaternion.Euler(rotationX, 0, 0);
        camHolder.transform.localRotation = Quaternion.Slerp(camHolder.transform.localRotation, test2, Time.deltaTime * speed);
>>>>>>> parent of cf0e007 (1)
        // camHolder.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        camHolder.transform.localRotation = Quaternion.Slerp(camHolder.transform.localRotation, Quaternion.Euler(rotationX, 0, 0), speed * Time.deltaTime);
    }
}