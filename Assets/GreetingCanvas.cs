using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreetingCanvas : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Canvas>().enabled = true;
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            GetComponent<Canvas>().enabled = false;
            Time.timeScale = 1;
        }
    }
}
