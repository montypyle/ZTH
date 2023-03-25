using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using TMPro;


public class CharMGR : MonoBehaviour
{
    public Button no;
    public GameObject nameField, nameBox;
    public TextMeshProUGUI heroName;
    public Transform spawn;

    // Start is called before the first frame update

    void Start()
    {
        GM.instance.SetSpawn(spawn);
    
        if (File.Exists(Application.persistentDataPath + "/appearanceInfo.dat"))
        {
            no.enabled = true;
            heroName.text = GM.instance.appCtrl.charName;
            if (!GM.instance.appCtrl.charName.Equals(null))
            {
                heroName.text = GM.instance.appCtrl.charName;
                nameBox.SetActive(true);
                nameField.SetActive(false);
            }
        }
        else
        {
            no.enabled = false;
            nameBox.SetActive(false);
            nameField.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Exit()
    {
        Debug.Log("Exiting character customisation");
        GM.instance.appCtrl.SaveAppearance();
        SceneManager.LoadScene("Home");
    }
}
