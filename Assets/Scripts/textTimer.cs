using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class textTimer : MonoBehaviour
{
    public Transform playerTransform;

    public float timeRemainingTEST = 300f;
    public bool timerIsRunningTEST = true;
    public TextMeshProUGUI timeText;

    public AnimationClip animationClip;
    private float timeToDisplay;

    public Animator[] animator;

    private bool timerIsRunning = true;
    private float timeRemaining = 1f;
    private int notificationCount = 0;

    public string NotificationPath = "event:/Notification";

    FMOD.Studio.EventInstance notificationInstance;


    void Start() {
        for (int i = 0; i < animator.Length; i++) {
            animator[i].enabled = false;
        }
    }

    void Update() {
        TimerNotification();
        TutorialNotificationSounds();
    }

    private void TimerNotification() {
        if (timerIsRunningTEST) {
            if (timeRemainingTEST > 0) {
                timeRemainingTEST -= Time.deltaTime;
                DisplayTime(timeRemainingTEST);
            }
            else {
                timeRemainingTEST = 0f;
                timerIsRunningTEST = false;
                animator[3].enabled = true;
            }
        }
    }

    void DisplayTime(float timeToDisplay) {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60); 
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
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
                    animator[0].enabled = true;
                    notificationCount++;
                    StartCoroutine(PlaySoundAfterAnimation(1));
                    timeRemaining = 5f;
                }
                else if (notificationCount == 1)
                {
                    animator[1].enabled = true;
                    notificationCount++;
                    StartCoroutine(PlaySoundAfterAnimation(1));
                    timeRemaining = 5f;
                }
                else if (notificationCount == 2)
                {
                    animator[2].enabled = true;
                    StartCoroutine(PlaySoundAfterAnimation(1));
                    timerIsRunning = false;
                }
            }
        }
    }

    IEnumerator PlaySoundAfterAnimation(int index)
    {
        yield return new WaitForSeconds(1f);
        
        notificationInstance = FMODUnity.RuntimeManager.CreateInstance(NotificationPath);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(notificationInstance, playerTransform);
        notificationInstance.start();
        notificationInstance.release();
    }
}
