using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Collections;


public class WeatherManager : MonoBehaviour {
    public TMP_Text weatherText;

    public string apiKey = "8cf84a776067a09b71171f342ef00efa";
    public string city = "Seoul";

    void Start() {
        weatherText = this.GetComponent<TMP_Text>();

        StartCoroutine(LocationSend());
    }

    IEnumerator LocationSend() {
        #if UNITY_EDITOR || UNITY_STANDALONE_OSX

        city = "Seoul";
        yield return StartCoroutine(GetByCity(city));

        #elif UNITY_IOS

        if(!Input.location.isEnabledByUser) {
            weatherText.text = "locate system is off";
            yield break;
        }

        Input.location.Start();
        int maxWait = 10;

        while(Input.location.status == LocationServiceStatus.Initializing && maxWait > 0) {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if(Input.location.status == LocationServiceStatus.Failed) {
            weatherText.text = "cant bring locate";
            yield break;
        }
        else {
            float lat = Input.location.lastData.latitude;
            float lon = Input.location.lastData.longitude;
            yield return StartCoroutine(GetByCoords(lat, lon));
        }

        #else

        weatherText.text = "cant support platform";

        #endif
    }

    IEnumerator GetByCity(string city) {
        string url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=metric&lang=en";
        yield return StartCoroutine(GetWeather(url));
    }

    IEnumerator GetByCoords(float lat, float lon) {
        string url = $"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid={apiKey}&units=metric&lang=en";
        yield return StartCoroutine(GetWeather(url));
    }



    IEnumerator GetWeather(string url) {
        using (UnityWebRequest www = UnityWebRequest.Get(url)) {
            yield return www.SendWebRequest();

            if(www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError) weatherText.text = $"Error : {www.error}";
            else {
                WeatherResponse weatherData = JsonUtility.FromJson<WeatherResponse>(www.downloadHandler.text);
                string location  = weatherData.name;
                string description = weatherData.weather[0].description;

                weatherText.text = $"locate : {location}\nweather : {description}";
            }
        }
    }

    [System.Serializable]
    public class WeatherResponse {
        public Weather[] weather;
        public Main main;
        public string name;
    }

    [System.Serializable]
    public class Weather {
        public string description;
    }

    [System.Serializable]
    public class Main {
        public float temp;
    }
}