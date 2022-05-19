using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICameraSetter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Canvas canvas = GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = Camera.main;
        canvas.planeDistance = 1;
    }

    // Update is called once per frame
    void Update()
    {
    }
}