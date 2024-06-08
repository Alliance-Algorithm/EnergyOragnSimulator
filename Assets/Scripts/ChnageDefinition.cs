using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class ChnageDefinition : MonoBehaviour
{
    public Volume organCamera;
    public DepthOfField depthOfField;
    public Slider definitionSlider;
    public Text definitionText;
    // Start is called before the first frame update
    void Start()
    {
        organCamera = GetComponent<Volume>();
        depthOfField = organCamera.profile.components[0] as DepthOfField;
    }

    // Update is called once per frame
    void Update()
    {
        depthOfField.aperture.value = definitionSlider.value * 32;
        string text;
        float max = depthOfField.aperture.max;
        if (depthOfField.aperture.value < (max / 2f))
        {
            depthOfField.active = true;
            if (depthOfField.aperture.value < (max / 4f))
            {
                text = "模糊";
                definitionText.color = Color.red;
            }
            else
            {
                text = "真实";
                definitionText.color = Color.green;

            }
        }
        else
        {
            if (depthOfField.aperture.value < (max * 3 / 4f))
            {
                text = "清晰";
                definitionText.color = new Color(0.5f, 0.5f, 1);
                depthOfField.active = true;
            }
            else
            {
                text = "无效果";
                definitionText.color = Color.white;
                depthOfField.active = false;
            }

        }
        definitionText.text = text;
    }
}
