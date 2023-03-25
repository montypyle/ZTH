using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMGR : MonoBehaviour
{
    public static SettingsMGR instance;
    public Slider masterVol;
    public float defaultVol;
    public string reviewURL;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        if ( PlayerPrefs.HasKey("volume"))
        { 
            masterVol.value = PlayerPrefs.GetFloat("volume"); 
        }
        else
        {
            masterVol.value = defaultVol;
            PlayerPrefs.SetFloat("volume", defaultVol);

        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void Review()
    {
        Application.OpenURL(reviewURL);
    }
    public void SaveVol()
    {
        float vol = masterVol.value;
        PlayerPrefs.SetFloat("volume", vol);
    }
}
