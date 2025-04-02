using System;
using UnityEngine;
using TMPro;

public class TimeManager : MonoBehaviour {
    public TMP_Text timeText;

    void Start() {
        timeText = this.GetComponent<TMP_Text>();
    }

    void Update() {
        DateTime currentTime;

        #if UNITY_IOS && !UNITY_EDITOR

        currentTime = TimeZoneInfo.ConvertTimeFromUtc(DataTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Asia/Seoul"));

        #else

        currentTime = DateTime.Now;

        #endif

        timeText.text = currentTime.ToString("dddd, MMMM d, yyyy - hh:mm:ss tt");
    }
}