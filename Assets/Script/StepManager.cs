using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

public class StepManager : MonoBehaviour {
    public TMP_Text totalStepsText;
    public TMP_Text sessionStepsText;

    #if UNITY_IOS && !UNITY_EDITOR
    
    [DllImport("__Internal")]
    private static extern void StartStepTracking();

    [DllImport("__Internal")]
    private static extern int GetTodayStepCount();

    [DllImport("__Internal")]
    private static extern int GetSessionStepCount();

    #endif

    void Start() {
        totalStepsText = GameObject.Find("TotalStepData").GetComponent<TMP_Text>();
        sessionStepsText = GameObject.Find("SessionStepData").GetComponent<TMP_Text>();
        
        #if UNITY_IOS && !UNITY_EDITOR

        StartStepTracking();

        #endif
    }

    void Update() {
        #if UNITY_IOS && !UNITY_EDITOR

        int today = GetTodayStepCount();
        int session = GetSessionStepCount();

        totalStepsText.text = $"today step Count : {today:N0}";
        sessionStepsText.text = $"session step Count : {session:N0}";

        #else

        totalStepsText.text = "is Only in iPhone";
        sessionStepsText.text = "";

        #endif
    }
}