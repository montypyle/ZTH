using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusMGR : MonoBehaviour
{
    public static StatusMGR instance;
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

        // Update is called once per frame
        void Update()
        {

        }
    }
}
