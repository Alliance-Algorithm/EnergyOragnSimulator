using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Rotate : MonoBehaviour
{
    public GameObject Organ;
    public Slider speedSlider;
    public Text speedText;

    public struct BigPara
    {
        public float a;
        public float w;
        public float t;
        public float b;
        public float speed;
    }
    public BigPara bigPara;
    public bool ifBig;
    public float speedMultiple = 1;
    // Start is called before the first frame update
    void Start()
    {
        initialize();
        bigPara.speed = bigPara.a * math.sin(bigPara.w * bigPara.t) + bigPara.b;
    }

    // Update is called once per frame
    void Update()
    {
        float eularSpeed = 0;
        if (ifBig)
        {
            bigPara.t += Time.deltaTime;
            bigPara.speed = bigPara.a * math.sin(bigPara.w * bigPara.t) + bigPara.b;
            eularSpeed = speedMultiple * 360 * (bigPara.speed / (2f * math.PI));
            Organ.transform.Rotate(new Vector3(1, 0, 0) * eularSpeed * Time.deltaTime);
        }
        else
        {
            eularSpeed = 60 * speedMultiple;
            Organ.transform.Rotate(new Vector3(1, 0, 0) * eularSpeed * Time.deltaTime);
        }

        speedText.text = string.Format("{0:F2}", speedMultiple);
    }
    void initialize()
    {
        bigPara.a = UnityEngine.Random.Range(0.780f, 1.046f);
        bigPara.w = UnityEngine.Random.Range(1.884f, 2.001f);
        bigPara.t = 0;
        bigPara.b = 2.09f - bigPara.a;
    }

    public void ChangeMode()
    {
        initialize();
        ifBig = ifBig == false ? true : false;
    }
    public void ChangeSpeed()
    {
        speedMultiple = (speedSlider.value - 0.5f) * 4;
    }
}
