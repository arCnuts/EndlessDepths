using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController playerController;
    public Transform playerTransform;
    public Transform camHolder;
    public Camera playerCamera;
    [SerializeField]
    private float moveSpeed = 5f;
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
        Look();
    }

    private void Move()
    {
        float keyboardX = Input.GetAxisRaw("Horizontal");
        float keyboardY = Input.GetAxisRaw("Vertical");

        Quaternion relativeRotation = Quaternion.Euler(0,playerTransform.transform.eulerAngles.y,0);

        playerController.Move(relativeRotation * new Vector3(keyboardX,0,keyboardY).normalized * moveSpeed * Time.deltaTime);
    }

    private void Look() {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        rotationY = playerTransform.transform.localRotation.eulerAngles.y + mouseX;
        rotationX -= mouseY; rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        camHolder.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        playerTransform.transform.localRotation = Quaternion.Euler(0, rotationY, 0);
    }
}