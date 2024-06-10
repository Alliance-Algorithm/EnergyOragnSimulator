using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GeneraterController : MonoBehaviour
{
    public Transform blades;
    public Canvas uiCanvas;
    public Canvas tempUICanvas;
    public Canvas waitPage;
    BladesController _materialController;
    // Start is called before the first frame update
    void Start()
    {
        waitPage.enabled = false;
        tempUICanvas.enabled = false;
        _materialController = blades.GetComponent<BladesController>();
    }

    // Update is called once per frame
    void Update()
    {
        tempUICanvas.transform.GetChild(0).GetComponent<Text>().text = "已生成 " + _materialController.index.ToString() + " 张数据集";
        if (Input.GetKeyDown(KeyCode.P))
        {
            tempUICanvas.enabled = true;
            _materialController.ifGenerate = false;
        }
        if (Input.GetKeyUp(KeyCode.P))
        {
            tempUICanvas.enabled = false;
            _materialController.ifGenerate = true;
        }
        if (Input.GetKey(KeyCode.Escape))
        {
            uiCanvas.enabled = true;
            _materialController.ifGenerate = false;
        }
    }


    public void WaitForGenerate()
    {
        Time.timeScale = 0;
        waitPage.enabled = true;
    }
    public void CancelWaitPage()
    {
        Time.timeScale = 1;
        waitPage.enabled = false;
    }

    public void StartGenerateDataset()
    {
        Time.timeScale = 1;
        waitPage.enabled = false;
        uiCanvas.enabled = false;
        _materialController.ifGenerate = true;
    }
}

