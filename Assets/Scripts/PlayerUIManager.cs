using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    public Camera playerCamera;
    public GameObject[] pages;
    private bool pageOpened = false;

    void Start() {

    }

    void Update() {
        EscChecker();
        LookingAt();
    }

    private void LookingAt() {

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward * 2f);
        RaycastHit hit;

        if (!Input.GetKeyDown(KeyCode.E)) return;
        if (Physics.Raycast(ray, out hit) && hit.collider.CompareTag("Page"))
        {
            if (pageOpened) return;
            pageOpened = true;
            pages[0].gameObject.SetActive(true);
        }
    }

    private void EscChecker() {
        if (!Input.GetKeyDown(KeyCode.E)) return;
            if(pageOpened) {
                pageOpened = false;
                pages[0].gameObject.SetActive(false);
            }
    }

}