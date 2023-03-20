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
    private bool walking = true;

    private float rotationX,rotationY;
    private float footstepFrequency = 0.6f;
    private float time;
    private Vector3 combinedMovement;

    private bool timerIsRunning = true;
    private float timeRemaining = 1f;
    private int notificationCount = 0;

    [Header("Player Settings")]
    [SerializeField]
    private float runSpeed = 10f;
    [SerializeField]
    private float shiftSpeed = 5f;
    [SerializeField]
    private float sensitivity = 2f;
    [SerializeField]
    private bool smoothCamera = true;
    [SerializeField]
    private float cameraSmoothness = 15f;
    [Header("Audio")]
    public string FootstepRunningPath = "event:/FootstepsRunning";
    public string FootstepWalkingPath = "event:/FootstepsWalking";
    public string FlashlightOnPath = "event:/Flashlight/ON";
    public string FlashlightOffPath = "event:/Flashlight/OFF";
    public string AmbiencePath = "event:/Ambience";
    public string NotificationPath = "event:/Notification";


    FMOD.Studio.EventInstance ambienceInstance;
    FMOD.Studio.EventInstance notificationInstance;

    void Start() {
        flashlightLight.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        // RuntimeManager.PlayOneShot(AmbiencePath);
        ambienceInstance = FMODUnity.RuntimeManager.CreateInstance(AmbiencePath);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(ambienceInstance, playerTransform);
        ambienceInstance.start();
        ambienceInstance.release();
    }

    void Update() {
        InputManager();
        FootstepManager();
        TutorialNotificationSounds();
        Move();
        Look();
        LookingAt();
    }

    private void InputManager() {
        if (Input.GetKeyDown(KeyCode.F))
        {
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

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            walking = false;
            footstepFrequency = 0.3f;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            walking = true;
            footstepFrequency = 0.6f;
        }
    }

    private void FootstepManager() {
        if (Time.time > time) {
            time = Time.time + footstepFrequency;
            float currendSpeed = new Vector3(playerController.velocity.x, 0, playerController.velocity.z).magnitude;
            if(currendSpeed < 3f) return;
                if (walking) {
                    RuntimeManager.PlayOneShot(FootstepWalkingPath, playerTransform.position + new Vector3(0, -3, 0));
                }
                else {
                    RuntimeManager.PlayOneShot(FootstepRunningPath, playerTransform.position + new Vector3(0, -3, 0));
                }
        }
    }

    void TutorialNotificationSounds()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                if (notificationCount == 0)
                {
                    notificationInstance = FMODUnity.RuntimeManager.CreateInstance(NotificationPath);
                    FMODUnity.RuntimeManager.AttachInstanceToGameObject(notificationInstance, playerTransform);
                    notificationInstance.start();
                    notificationInstance.release();
                    notificationCount++;
                    timeRemaining = 5f;
                }
                else if (notificationCount == 1)
                {
                    notificationInstance = FMODUnity.RuntimeManager.CreateInstance(NotificationPath);
                    FMODUnity.RuntimeManager.AttachInstanceToGameObject(notificationInstance, playerTransform);
                    notificationInstance.start();
                    notificationInstance.release();
                    notificationCount++;
                    timeRemaining = 6f;
                }
                else if (notificationCount == 2)
                {
                    notificationInstance = FMODUnity.RuntimeManager.CreateInstance(NotificationPath);
                    FMODUnity.RuntimeManager.AttachInstanceToGameObject(notificationInstance, playerTransform);
                    notificationInstance.start();
                    notificationInstance.release();
                    timerIsRunning = false;
                }
            }
        }
    }

    private void Move() {
        float keyboardX = Input.GetAxisRaw("Horizontal");
        float keyboardY = Input.GetAxisRaw("Vertical");
        combinedMovement = new Vector3(keyboardX,0,keyboardY).normalized;

        Quaternion relativeRotation = Quaternion.Euler(0,playerTransform.transform.eulerAngles.y,0);
        if (walking) {
            playerController.Move(relativeRotation * combinedMovement * shiftSpeed * Time.deltaTime);
        }
        else {
            playerController.Move(relativeRotation * combinedMovement * runSpeed * Time.deltaTime);
        }
    }

    private void LookingAt() {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward * 2f);

        RaycastHit hit;

        if (!Input.GetKeyDown(KeyCode.E)) return;
        if (Physics.Raycast(ray, out hit) && hit.collider.CompareTag("Artifact"))
        {
            Debug.Log("hewwoooooo");
        }
    }

    private void Look() {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;
        
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