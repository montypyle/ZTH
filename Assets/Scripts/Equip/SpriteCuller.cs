using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteCuller : MonoBehaviour
{
    public SpriteRenderer toHide;
    public bool hiding;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        toHide.enabled = !hiding;
    }
}
