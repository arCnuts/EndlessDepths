using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class textTimer : MonoBehaviour
{
    public Transform playerTransform;
    public TextMeshProUGUI timeText;

    public bool levelTimerIsRunning = true;
    public float levelTimerSeconds = 300f;

    private bool notificationTimerIsRunning = true;
    private float notificationTimerSeconds = 1f;
    private float timeToDisplay;
    private int notificationCount = 0;

    public Animator[] animator;
    public string NotificationPath = "event:/Notification";

    FMOD.Studio.EventInstance notificationInstance;
    FMOD.Studio.Bus MasterBus;


    void Start() {
        for (int i = 0; i < animator.Length; i++) {
            animator[i].enabled = false;
        }
        MasterBus = FMODUnity.RuntimeManager.GetBus("Bus:/");
    }

    void Update() {
        LevelTimer();
        NotificationTimer();
    }

    private void LevelTimer() {
        if (levelTimerIsRunning) {
            if (levelTimerSeconds > 0) {
                levelTimerSeconds -= Time.deltaTime;
                DisplayTime(levelTimerSeconds);
            }
            else {
                levelTimerSeconds = 0f;
                levelTimerIsRunning = false;
                MasterBus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                animator[3].enabled = true;
                StartCoroutine(PlaySoundAfterAnimation(1f));
            }
        }
    }

    void DisplayTime(float timeToDisplay) {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60); 
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void NotificationTimer()
    {
        if (notificationTimerIsRunning)
        {
            if (notificationTimerSeconds > 0)
            {
                notificationTimerSeconds -= Time.deltaTime;
            }
            else
            {
                if (notificationCount == 0)
                {
                    animator[0].enabled = true;
                    StartCoroutine(PlaySoundAfterAnimation(1f));
                    notificationCount++;
                    notificationTimerSeconds = 5f;
                }
                else if (notificationCount == 1)
                {
                    animator[1].enabled = true;
                    StartCoroutine(PlaySoundAfterAnimation(1f));
                    notificationCount++;
                    notificationTimerSeconds = 5f;
                }
                else if (notificationCount == 2)
                {
                    animator[2].enabled = true;
                    StartCoroutine(PlaySoundAfterAnimation(1f));
                    notificationTimerIsRunning = false;
                }
            }
        }
    }

    IEnumerator PlaySoundAfterAnimation(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        
        notificationInstance = FMODUnity.RuntimeManager.CreateInstance(NotificationPath);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(notificationInstance, playerTransform);
        notificationInstance.start();
        notificationInstance.release();
    }
}
