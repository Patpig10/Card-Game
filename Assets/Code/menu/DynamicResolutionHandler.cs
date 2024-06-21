using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicResolutionHandler : MonoBehaviour
{
    private CanvasScaler canvasScaler;

    void Start()
    {
        canvasScaler = GetComponent<CanvasScaler>();
        if (canvasScaler == null)
        {
            Debug.LogError("CanvasScaler component not found!");
            return;
        }

        UpdateCanvasScaler();
    }

    void Update()
    {
        // This can be optimized to check only when resolution changes
        if (Screen.width != canvasScaler.referenceResolution.x || Screen.height != canvasScaler.referenceResolution.y)
        {
            UpdateCanvasScaler();
        }
    }

    void UpdateCanvasScaler()
    {
        canvasScaler.referenceResolution = new Vector2(Screen.width, Screen.height);
    }
}