using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasScript : MonoBehaviour
{
    public KeyCode pauseKey = KeyCode.Escape;


    private Canvas canvas;
    // Start is called before the first frame update
    void Start()
    {
        canvas = GetComponent<Canvas>();
        canvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(pauseKey))
        {
            canvas.enabled = !canvas.enabled;
            Time.timeScale = 1 - Time.timeScale;
        }
    }

    private void OnDestroy()
    {
        Time.timeScale = 1;
    }
}
