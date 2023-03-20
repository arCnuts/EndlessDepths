using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;


public class PlayerMovement : MonoBehaviour
{
    public CharacterController playerController;
    public Transform playerTransform;
    public Transform cameraHolder;
    public Camera playerCamera;
    public GameObject flashlightLight;

    private bool flashlightActive = false;
    private bool isRunning = false;

    private float rotationX,rotationY;
    private float footstepFrequency = 0.6f;
    private float time;
    private Vector3 combinedMovement;

    [Header("Player Settings")]
    [SerializeField]
    private float walkSpeed = 5f;
    [SerializeField]
    private bool smoothCamera = true;
    [SerializeField]
    private float cameraSmoothness = 15f;
    [SerializeField]
    private float cameraSensitivity = 2f;
    [Header("Audio")]
    public string FootstepRunningPath = "event:/FootstepsRunning";
    public string FootstepWalkingPath = "event:/FootstepsWalking";
    public string FlashlightOnPath = "event:/Flashlight/ON";
    public string FlashlightOffPath = "event:/Flashlight/OFF";
    public string AmbiencePath = "event:/Ambience";
    
    FMOD.Studio.EventInstance ambienceInstance;

    void Start() {
        flashlightLight.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        ambienceInstance = FMODUnity.RuntimeManager.CreateInstance(AmbiencePath);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(ambienceInstance, playerTransform);
        ambienceInstance.start();
        ambienceInstance.release();

    }

    void Update() {
        Flashlight();
        Running();
        FootstepManager();
        Move();
        Look();
    }

    private void Flashlight() {
        if (!Input.GetKeyDown(KeyCode.F)) return;
            if (flashlightActive == false)
            {
                RuntimeManager.PlayOneShot(FlashlightOnPath, playerTransform.position + Vector3.down);
                flashlightLight.gameObject.SetActive(true);
                flashlightActive = true;
                
            }
            else
            {
                RuntimeManager.PlayOneShot(FlashlightOffPath, playerTransform.position + Vector3.down);
                flashlightLight.gameObject.SetActive(false);
                flashlightActive = false;
            }
    }

    private void Running() {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isRunning = true;
            footstepFrequency = 0.3f;
            walkSpeed = 10f;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRunning = false;
            footstepFrequency = 0.6f;
            walkSpeed = 5f;
        }
    }

    private void FootstepManager() {
        if (Time.time > time) {
            time = Time.time + footstepFrequency;
            float currendSpeed = new Vector3(playerController.velocity.x, 0, playerController.velocity.z).magnitude;
            if(currendSpeed < 3f) return;
                if (isRunning) {
                    RuntimeManager.PlayOneShot(FootstepRunningPath, playerTransform.position + new Vector3(0, -3, 0));
                }
                else {
                    RuntimeManager.PlayOneShot(FootstepWalkingPath, playerTransform.position + new Vector3(0, -3, 0));
                }
        }
    }

    private void Move() {
        float keyboardX = Input.GetAxisRaw("Horizontal");
        float keyboardY = Input.GetAxisRaw("Vertical");
        combinedMovement = new Vector3(keyboardX,0,keyboardY).normalized;

        Quaternion relativeRotation = Quaternion.Euler(0,playerTransform.transform.eulerAngles.y,0);
        playerController.Move(relativeRotation * combinedMovement * walkSpeed * Time.deltaTime);

    }

    private void Look() {
        float mouseX = Input.GetAxis("Mouse X") * cameraSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * cameraSensitivity;
        
        if(smoothCamera)
        {
            rotationY += mouseX;
            rotationX -= mouseY + playerTransform.transform.localRotation.eulerAngles.x;
            rotationX = Mathf.Clamp(rotationX, -89f, 89f);
            
            cameraHolder.transform.localRotation = Quaternion.Slerp(cameraHolder.transform.localRotation, Quaternion.Euler(rotationX, 0, 0), cameraSmoothness * Time.deltaTime);
            playerTransform.transform.localRotation = Quaternion.Slerp(playerTransform.transform.localRotation, Quaternion.Euler(0, rotationY, 0), cameraSmoothness * Time.deltaTime);
        }
        else
        {
            rotationY = playerTransform.transform.localRotation.eulerAngles.y + mouseX;
            rotationX -= mouseY; rotationX = Mathf.Clamp(rotationX, -90f, 90f);

            cameraHolder.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            playerTransform.transform.localRotation = Quaternion.Euler(0, rotationY, 0);
        }
    }
}