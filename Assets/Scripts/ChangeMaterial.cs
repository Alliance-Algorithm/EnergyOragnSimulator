using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ChangeMaterial : MonoBehaviour
{
    enum EnergyOrganMode
    {
        Big,
        Small
    }
    enum Mode
    {
        Dark,
        Tohit,
        ActivedBig,
        ActivedSmall
    };
    public Material BlueMaterial;
    public Material BlueIndicate;
    public Material BlueNohit;

    public Material RedMaterial;
    public Material RedIndicate;
    public Material RedNohit;

    public Camera HkCamera;
    public Material DarkMaterial;
    public GameObject EnergyOrganBlades;
    public List<GameObject> EnergyOrganblades;
    public bool IfBig = true;
    public bool ColorSelected = true;
    public ColoredMaterial coloredMaterial;
    public List<Vector2> Signs;
    Transform RSign;
    public int index;
    public string filePath;
    public bool ifGenerate;


    public struct ColoredMaterial
    {
        public Material BaseMaterial;
        public Material Indicate;
        public Material Nohit;
    }

    void Start()
    {
        index = 0;
        RSign = GameObject.Find("RSign").transform;
        coloredMaterial = GetColor();
        foreach (Transform blade in EnergyOrganBlades.transform)
        {
            ChangeBladeShader(blade, Mode.Dark, 0);
        }
        StartCoroutine(ChangeShader(EnergyOrganMode.Big));
        StartCoroutine(GenerateDataSet());
    }
    IEnumerator ChangeShader(EnergyOrganMode energyOrganMode)
    {
        List<int> NotactivedBladeIndex = new List<int> { 0, 1, 2, 3, 4 };
        List<int> RingCounts = new List<int>
            {   UnityEngine.Random.Range(0, 5),
                UnityEngine.Random.Range(0, 5),
                UnityEngine.Random.Range(0, 5),
                UnityEngine.Random.Range(0, 5),
                UnityEngine.Random.Range(0, 5) };
        int activeIndex = -1;
        while (true)
        {
            energyOrganMode = IfBig ? EnergyOrganMode.Big : EnergyOrganMode.Small;
            coloredMaterial = GetColor();

            if (energyOrganMode == EnergyOrganMode.Big)
            {
                if (NotactivedBladeIndex.Count == 0)
                {
                    NotactivedBladeIndex = new List<int> { 0, 1, 2, 3, 4 };
                    yield return new WaitForSeconds(0.25f);
                    int count = 0;
                    while (count < 3)
                    {
                        int index = 0;
                        foreach (Transform blade in EnergyOrganBlades.transform)
                        {
                            ChangeBladeShader(blade, Mode.Dark, RingCounts[index]);

                        }
                        yield return new WaitForSeconds(0.25f);
                        foreach (Transform blade in EnergyOrganBlades.transform)
                        {
                            ChangeBladeShader(blade, Mode.ActivedBig, RingCounts[index]);
                            index++;
                        }
                        yield return new WaitForSeconds(0.25f);
                        count++;
                    }
                    yield return new WaitForSeconds(5);
                    foreach (Transform blade in EnergyOrganBlades.transform)
                    {
                        ChangeBladeShader(blade, Mode.Dark, 0);
                    }
                }
                else
                {
                    int index = UnityEngine.Random.Range(0, NotactivedBladeIndex.Count);
                    activeIndex = NotactivedBladeIndex[index];
                    ChangeBladeShader(EnergyOrganblades[activeIndex].transform, Mode.Tohit, RingCounts[activeIndex]);
                    NotactivedBladeIndex.Remove(activeIndex);
                    yield return new WaitForSeconds(1.5f);
                    ChangeBladeShader(EnergyOrganblades[activeIndex].transform, Mode.ActivedBig, RingCounts[activeIndex]);
                }
                Debug.Log("change " + activeIndex);
            }
            else if (energyOrganMode == EnergyOrganMode.Small)
            {
                if (NotactivedBladeIndex.Count == 0)
                {
                    NotactivedBladeIndex = new List<int> { 0, 1, 2, 3, 4 };
                    yield return new WaitForSeconds(0.25f);
                    int count = 0;
                    while (count < 3)
                    {
                        int index = 0;
                        foreach (Transform blade in EnergyOrganBlades.transform)
                        {
                            ChangeBladeShader(blade, Mode.Dark, RingCounts[index]);

                        }
                        yield return new WaitForSeconds(0.25f);
                        foreach (Transform blade in EnergyOrganBlades.transform)
                        {
                            ChangeBladeShader(blade, Mode.ActivedSmall, RingCounts[index]);
                            index++;
                        }
                        yield return new WaitForSeconds(0.25f);
                        count++;
                    }
                    yield return new WaitForSeconds(5);
                    foreach (Transform blade in EnergyOrganBlades.transform)
                    {
                        ChangeBladeShader(blade, Mode.Dark, 0);
                    }
                }
                else
                {
                    int index = UnityEngine.Random.Range(0, NotactivedBladeIndex.Count);
                    activeIndex = NotactivedBladeIndex[index];
                    ChangeBladeShader(EnergyOrganblades[activeIndex].transform, Mode.Tohit, RingCounts[activeIndex]);
                    NotactivedBladeIndex.Remove(activeIndex);
                    yield return new WaitForSeconds(1.5f);
                    ChangeBladeShader(EnergyOrganblades[activeIndex].transform, Mode.ActivedSmall, RingCounts[activeIndex]);
                }
                Debug.Log("change " + activeIndex);
            }
        }
    }
    public void ChangeColor()
    {
        ColorSelected = ColorSelected == false ? true : false;
        StopAllCoroutines();
        foreach (Transform blade in EnergyOrganBlades.transform)
        {
            ChangeBladeShader(blade, Mode.Dark, 0);
        }
        StartCoroutine(ChangeShader(EnergyOrganMode.Big));
        StartCoroutine(GenerateDataSet());
    }
    public void ChangeMode()
    {
        IfBig = IfBig == false ? true : false;
        StopAllCoroutines();
        foreach (Transform blade in EnergyOrganBlades.transform)
        {
            ChangeBladeShader(blade, Mode.Dark, 0);
        }
        if (IfBig)
            StartCoroutine(ChangeShader(EnergyOrganMode.Big));
        else
            StartCoroutine(ChangeShader(EnergyOrganMode.Small));
        StartCoroutine(GenerateDataSet());
    }
    ColoredMaterial GetColor()
    {

        ColoredMaterial coloredMaterial;
        if (ColorSelected)
        {
            Material[] materials = RSign.GetComponent<MeshRenderer>().materials;
            for (int i = 0; i < materials.Length; i++)
                materials[i] = BlueMaterial;
            RSign.GetComponent<MeshRenderer>().materials = materials;
            coloredMaterial.BaseMaterial = BlueMaterial;
            coloredMaterial.Indicate = BlueIndicate;
            coloredMaterial.Nohit = BlueNohit;
        }
        else
        {
            Material[] materials = RSign.GetComponent<MeshRenderer>().materials;
            for (int i = 0; i < materials.Length; i++)
                materials[i] = RedMaterial;
            RSign.GetComponent<MeshRenderer>().materials = materials;
            coloredMaterial.BaseMaterial = RedMaterial;
            coloredMaterial.Indicate = RedIndicate;
            coloredMaterial.Nohit = RedNohit;
        }
        return coloredMaterial;
    }
    void ChangeBladeShader(Transform blade, Mode mode, int RingCount)
    {
        switch (mode)
        {
            case Mode.Dark:
                {
                    foreach (Transform child in blade)
                    {
                        Material[] materials = child.GetComponent<MeshRenderer>().materials;
                        for (int i = 0; i < materials.Length; i++)
                            materials[i] = DarkMaterial;
                        child.GetComponent<MeshRenderer>().materials = materials;
                        if (child.name == "Circle")
                        {
                            child.transform.localScale = new Vector3(1, 1, 1);
                            Transform Cylinder = child.GetChild(0);
                            Cylinder.GetComponent<MeshRenderer>().material = DarkMaterial;
                            Cylinder.transform.localScale = new Vector3(0.26f, 0.00001f, 0.26f);
                        }
                    }
                    break;
                }
            case Mode.Tohit:
                {

                    foreach (Transform child in blade)
                    {
                        string ChildName = child.name;
                        switch (ChildName)
                        {
                            case "up1":
                            case "up2":
                                {
                                    Material[] materials = child.GetComponent<MeshRenderer>().materials;
                                    for (int i = 0; i < materials.Length; i++)
                                        materials[i] = coloredMaterial.BaseMaterial;
                                    child.GetComponent<MeshRenderer>().materials = materials;
                                    continue;
                                }
                            case "Plane":
                                {
                                    child.GetComponent<MeshRenderer>().material = coloredMaterial.Indicate;
                                    continue;
                                }
                            case "Circle":
                                {
                                    child.GetComponent<MeshRenderer>().material = DarkMaterial;
                                    Transform Cylinder = child.GetChild(0);
                                    Cylinder.GetComponent<MeshRenderer>().material = coloredMaterial.Nohit;
                                    Cylinder.transform.localScale = new Vector3(0.3f, 0.00001f, 0.3f);
                                    continue;
                                }
                            case "Positions":
                                {
                                    Signs.Clear();
                                    continue;
                                }
                            default:
                                {
                                    Material[] materials = child.GetComponent<MeshRenderer>().materials;
                                    for (int i = 0; i < materials.Length; i++)
                                        materials[i] = DarkMaterial;
                                    child.GetComponent<MeshRenderer>().materials = materials;
                                    continue;
                                }
                        }
                    }
                    break;
                }
            case Mode.ActivedSmall:
                {
                    foreach (Transform child in blade)
                    {
                        string ChildName = child.name;
                        switch (ChildName)
                        {
                            case "Plane":
                                {
                                    child.GetComponent<MeshRenderer>().material = coloredMaterial.BaseMaterial;
                                    continue;
                                }
                            case "Circle":
                                {
                                    child.GetComponent<MeshRenderer>().material = coloredMaterial.BaseMaterial;
                                    child.transform.localScale = new Vector3(1, 1, 1);
                                    Transform Cylinder = child.GetChild(0);
                                    Cylinder.GetComponent<MeshRenderer>().material = DarkMaterial;
                                    Cylinder.transform.localScale = new Vector3(0.26f, 0.00001f, 0.26f);
                                    continue;
                                }
                            default:
                                {
                                    Material[] materials = child.GetComponent<MeshRenderer>().materials;
                                    for (int i = 0; i < materials.Length; i++)
                                        materials[i] = coloredMaterial.BaseMaterial;
                                    child.GetComponent<MeshRenderer>().materials = materials;
                                    continue;
                                }
                        }
                    }
                    break;
                }
            case Mode.ActivedBig:
                {
                    foreach (Transform child in blade)
                    {
                        string ChildName = child.name;
                        switch (ChildName)
                        {
                            case "middle":
                                {
                                    child.GetComponent<MeshRenderer>().material = DarkMaterial;
                                    Transform plain = child.GetChild(0);
                                    plain.GetComponent<MeshRenderer>().material = coloredMaterial.BaseMaterial;
                                    continue;
                                }
                            case "Circle":
                                {
                                    float Dist = 0.2f;

                                    Transform Cylinder = child.GetChild(0);
                                    Cylinder.transform.localScale = new Vector3(0.26f, 0.00001f, 0.26f);
                                    Cylinder.GetComponent<MeshRenderer>().material = DarkMaterial;

                                    child.GetComponent<MeshRenderer>().material = coloredMaterial.BaseMaterial;
                                    child.transform.localScale = new Vector3(1 - Dist * RingCount, 1 - Dist * RingCount, 1f);
                                    continue;
                                }
                            default:
                                {
                                    Material[] materials = child.GetComponent<MeshRenderer>().materials;
                                    for (int i = 0; i < materials.Length; i++)
                                        materials[i] = coloredMaterial.BaseMaterial;
                                    child.GetComponent<MeshRenderer>().materials = materials;
                                    continue;
                                }
                        }
                    }
                    break;
                }
        }
    }
    Vector3 GetWorldToScreenPoint(Vector3 worldPoint)
    {
        return HkCamera.WorldToScreenPoint(worldPoint);
    }

    IEnumerator GenerateDataSet()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            bool ifHaveLabel = false;
            StreamWriter sr;
            if (ifGenerate)
            {
                sr = new StreamWriter(filePath + "/labels/" + index.ToString() + ".txt", true);
            }
            else
            {
                sr = new StreamWriter(filePath + "/labels/temp.txt", true);
            }


            foreach (Transform blade in EnergyOrganBlades.transform)
            {
                int BladeClass = 0;

                List<Vector3> boundPoints = new();
                foreach (Transform bladePart in blade)
                {
                    boundPoints.Add(bladePart.GetComponent<MeshRenderer>().bounds.max);
                    boundPoints.Add(bladePart.GetComponent<MeshRenderer>().bounds.min);
                    boundPoints.Add(bladePart.GetComponent<MeshRenderer>().bounds.center);
                }
                string mainTargetLabel = drawBounds(boundPoints, 0.05f);


                foreach (Transform child in blade)
                {
                    if (child.name == "Circle")
                    {
                        if (child.GetComponent<MeshRenderer>().sharedMaterial.name == coloredMaterial.BaseMaterial.name)
                            BladeClass = 2;
                        else if (child.GetChild(0).GetComponent<MeshRenderer>().sharedMaterial.name == coloredMaterial.Nohit.name)
                            BladeClass = 1;
                        else
                            BladeClass = 0;

                    }
                    if (child.name == "Positions")
                    {
                        Signs.Clear();
                        switch (BladeClass)
                        {
                            case 1:
                                {

                                    foreach (Transform sign in child)
                                    {
                                        Vector3 ScreenPoint = GetWorldToScreenPoint(sign.transform.position);
                                        Signs.Add(new Vector2(ScreenPoint.x, ScreenPoint.y));
                                    }
                                    string label = "0 ";
                                    label += mainTargetLabel;
                                    foreach (Vector2 point in Signs)
                                    {
                                        label += (point.x / HkCamera.scaledPixelWidth).ToString();
                                        label += " ";
                                        label += (1 - (point.y / HkCamera.scaledPixelHeight)).ToString();
                                        label += " 1 ";
                                    }
                                    Debug.Log(label);

                                    sr.WriteLine(label);
                                    ifHaveLabel = true;
                                    break;
                                }
                            case 2:
                                {
                                    foreach (Transform sign in child)
                                    {
                                        Vector3 ScreenPoint = GetWorldToScreenPoint(sign.transform.position);
                                        Signs.Add(new Vector2(ScreenPoint.x, ScreenPoint.y));
                                    }
                                    string label = "1 ";
                                    label += mainTargetLabel;
                                    foreach (Vector2 point in Signs)
                                    {
                                        label += (point.x / HkCamera.scaledPixelWidth).ToString();
                                        label += " ";
                                        label += (1 - (point.y / HkCamera.scaledPixelHeight)).ToString();
                                        label += " 1 ";
                                    }
                                    Debug.Log(label);

                                    sr.WriteLine(label);
                                    ifHaveLabel = true;
                                    break;
                                }
                            case 0:
                                {
                                    break;
                                }
                        }
                    }
                }
            }
            sr.Close();
            sr.Dispose();
            if (ifHaveLabel && ifGenerate)
                ScreenCapture.CaptureScreenshot(filePath + "/images/" + index.ToString() + ".jpg");
            index++;
        }

    }

    String drawBounds(List<Vector3> boundPoints, float duration)
    {
        Color color = Color.green;
        float x = 1f;
        float max_y = -2000, max_z = -2000, min_y = 2000, min_z = 2000;

        foreach (Vector3 point in boundPoints)
        {
            max_y = point.y > max_y ? point.y : max_y;
            max_z = point.z > max_z ? point.z : max_z;
            min_y = point.y < min_y ? point.y : min_y;
            min_z = point.z < min_z ? point.z : min_z;
        }

        Vector3 max = new Vector3(x, max_y, max_z);
        Vector3 min = new Vector3(x, min_y, min_z);

        Debug.DrawLine(max, min, Color.green, 0.25f);
        Debug.DrawLine(max, new Vector3(max.x, max.y + 0.05f, max.z), Color.red, 0.25f);
        Vector2 screenMax = GetWorldToScreenPoint(max);
        Vector2 screenMin = GetWorldToScreenPoint(min);

        float height = Math.Abs(screenMax.y - screenMin.y);
        float width = Math.Abs(screenMax.x - screenMin.x);
        Vector2 center = new Vector2(screenMin.x + width / 2f, screenMin.y + height / 2f);


        return (center.x / HkCamera.scaledPixelWidth).ToString() + " " + (1 - (center.y / HkCamera.scaledPixelHeight)).ToString() + " " + (width / HkCamera.pixelWidth).ToString() + " " + (height / HkCamera.pixelHeight).ToString() + " ";
    }
}
