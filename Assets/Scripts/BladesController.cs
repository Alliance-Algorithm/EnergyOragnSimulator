using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class BladesController : MonoBehaviour
{
    enum EnergyOrganMode
    {
        Big,
        Small
    }
    enum Mode
    {
        Dark,
        ToHit,
        ActivatedBig,
        ActivatedSmall
    };
    
    
    public Material blueMaterial;
    public Material blueIndicate;
    public Material blueNotHit;

    public Material redMaterial;
    public Material redIndicate;
    public Material redNotHit;
    public Material darkMaterial;
    ColoredMaterial _coloredMaterial;


    public Camera hkCamera;
    public List<Vector2> signs;
    Transform _rSign;


    public GameObject energyOrganBlades;
    public List<GameObject> energyOrganBladesList;


    public bool ifBig = true;
    public bool colorSelected = true;
    public int index;
    public string filePath;
    public bool ifGenerate;
    public float generateDuration = 10f;
    
    struct ColoredMaterial
    {
        public Material BaseMaterial;
        public Material Indicate;
        public Material NotHit;
    }

    void Start()
    {
        index = 0;
        _rSign = GameObject.Find("RSign").transform;
        _coloredMaterial = GetColor();
        foreach (Transform blade in energyOrganBlades.transform)
        {
            ChangeBladeShader(blade, Mode.Dark, 0);
        }
        StartCoroutine(ChangeShader());
        StartCoroutine(GenerateDataSet());
    }
    IEnumerator ChangeShader()
    {
        
        List<int> notActivatedBladeIndex = new List<int> { 0, 1, 2, 3, 4 };
        List<int> ringCounts = new List<int>
            {   UnityEngine.Random.Range(0, 5),
                UnityEngine.Random.Range(0, 5),
                UnityEngine.Random.Range(0, 5),
                UnityEngine.Random.Range(0, 5),
                UnityEngine.Random.Range(0, 5) };
        
        while (true)
        {   
            int activateIndex;       
            EnergyOrganMode energyOrganMode = ifBig ? EnergyOrganMode.Big : EnergyOrganMode.Small;
            _coloredMaterial = GetColor();

            if (energyOrganMode == EnergyOrganMode.Big)
            {
                if (notActivatedBladeIndex.Count == 0)
                {
                    notActivatedBladeIndex = new List<int> { 0, 1, 2, 3, 4 };
                    yield return new WaitForSeconds(0.25f);
                    if (!ifGenerate)
                    {
                        int count = 0;
                        while (count < 3)
                        {
                            int i = 0;
                            foreach (Transform blade in energyOrganBlades.transform)
                            {
                                ChangeBladeShader(blade, Mode.Dark, ringCounts[i]);

                            }
                            yield return new WaitForSeconds(0.25f);
                            foreach (Transform blade in energyOrganBlades.transform)
                            {
                                ChangeBladeShader(blade, Mode.ActivatedBig, ringCounts[i]);
                                i++;
                            }
                            yield return new WaitForSeconds(0.25f);
                            count++;
                        }
                    }
                    yield return new WaitForSeconds(5);
                    if (ifGenerate)
                        ChangeColor();
                    foreach (Transform blade in energyOrganBlades.transform)
                    {
                        ChangeBladeShader(blade, Mode.Dark, 0);
                    }
                }
                else
                {
                    int i = UnityEngine.Random.Range(0, notActivatedBladeIndex.Count);
                    activateIndex = notActivatedBladeIndex[i];
                    ChangeBladeShader(energyOrganBladesList[activateIndex].transform, Mode.ToHit, ringCounts[activateIndex]);
                    notActivatedBladeIndex.Remove(activateIndex);
                    yield return new WaitForSeconds(1.5f);
                    ChangeBladeShader(energyOrganBladesList[activateIndex].transform, Mode.ActivatedBig, ringCounts[activateIndex]);
                }
            }else if (energyOrganMode == EnergyOrganMode.Small)
            {
                if (notActivatedBladeIndex.Count == 0)
                {
                    notActivatedBladeIndex = new List<int> { 0, 1, 2, 3, 4 };
                    yield return new WaitForSeconds(0.25f);
                    int count = 0;
                    while (count < 3)
                    {
                        int i = 0;
                        foreach (Transform blade in energyOrganBlades.transform)
                        {
                            ChangeBladeShader(blade, Mode.Dark, ringCounts[i]);

                        }
                        yield return new WaitForSeconds(0.25f);
                        foreach (Transform blade in energyOrganBlades.transform)
                        {
                            ChangeBladeShader(blade, Mode.ActivatedSmall, ringCounts[i]);
                            i++;
                        }
                        yield return new WaitForSeconds(0.25f);
                        count++;
                    }
                    yield return new WaitForSeconds(5);
                    foreach (Transform blade in energyOrganBlades.transform)
                    {
                        ChangeBladeShader(blade, Mode.Dark, 0);
                    }
                }
                else
                {
                    int i = UnityEngine.Random.Range(0, notActivatedBladeIndex.Count);
                    activateIndex = notActivatedBladeIndex[i];
                    ChangeBladeShader(energyOrganBladesList[activateIndex].transform, Mode.ToHit, ringCounts[activateIndex]);
                    notActivatedBladeIndex.Remove(activateIndex);
                    yield return new WaitForSeconds(1.5f);
                    ChangeBladeShader(energyOrganBladesList[activateIndex].transform, Mode.ActivatedSmall, ringCounts[activateIndex]);
                }
            }
        }
    }
    public void ChangeColor()
    {
        colorSelected = colorSelected == false ? true : false;
        StopAllCoroutines();
        foreach (Transform blade in energyOrganBlades.transform)
        {
            ChangeBladeShader(blade, Mode.Dark, 0);
        }
        StartCoroutine(ChangeShader());
        StartCoroutine(GenerateDataSet());
    }
    public void ChangeMode()
    {
        ifBig = !ifBig;
        StopAllCoroutines();
        foreach (Transform blade in energyOrganBlades.transform)
        {
            ChangeBladeShader(blade, Mode.Dark, 0);
        }
        if (ifBig)
            StartCoroutine(ChangeShader());
        else
            StartCoroutine(ChangeShader());
        StartCoroutine(GenerateDataSet());
    }
    ColoredMaterial GetColor()
    {

        ColoredMaterial coloredMaterial;
        if (colorSelected)
        {
            Material[] materials = _rSign.GetComponent<MeshRenderer>().materials;
            for (int i = 0; i < materials.Length; i++)
                materials[i] = blueMaterial;
            _rSign.GetComponent<MeshRenderer>().materials = materials;
            coloredMaterial.BaseMaterial = blueMaterial;
            coloredMaterial.Indicate = blueIndicate;
            coloredMaterial.NotHit = blueNotHit;
        }
        else
        {
            Material[] materials = _rSign.GetComponent<MeshRenderer>().materials;
            for (int i = 0; i < materials.Length; i++)
                materials[i] = redMaterial;
            _rSign.GetComponent<MeshRenderer>().materials = materials;
            coloredMaterial.BaseMaterial = redMaterial;
            coloredMaterial.Indicate = redIndicate;
            coloredMaterial.NotHit = redNotHit;
        }
        return coloredMaterial;
    }
    void ChangeBladeShader(Transform blade, Mode mode, int ringCount)
    {
        switch (mode)
        {
            case Mode.Dark:
                {
                    foreach (Transform child in blade)
                    {
                        Material[] materials = child.GetComponent<MeshRenderer>().materials;
                        for (int i = 0; i < materials.Length; i++)
                            materials[i] = darkMaterial;
                        child.GetComponent<MeshRenderer>().materials = materials;
                        if (child.name == "Circle")
                        {
                            child.transform.localScale = new Vector3(1, 1, 1);
                            Transform cylinder = child.GetChild(0);
                            cylinder.GetComponent<MeshRenderer>().material = darkMaterial;
                            cylinder.transform.localScale = new Vector3(0.26f, 0.00001f, 0.26f);
                        }
                    }
                    break;
                }
            case Mode.ToHit:
                {

                    foreach (Transform child in blade)
                    {
                        string childName = child.name;
                        switch (childName)
                        {
                            case "up1":
                            case "up2":
                                {
                                    Material[] materials = child.GetComponent<MeshRenderer>().materials;
                                    for (int i = 0; i < materials.Length; i++)
                                        materials[i] = _coloredMaterial.BaseMaterial;
                                    child.GetComponent<MeshRenderer>().materials = materials;
                                    continue;
                                }
                            case "Plane":
                                {
                                    child.GetComponent<MeshRenderer>().material = _coloredMaterial.Indicate;
                                    continue;
                                }
                            case "Circle":
                                {
                                    child.GetComponent<MeshRenderer>().material = darkMaterial;
                                    Transform cylinder = child.GetChild(0);
                                    cylinder.GetComponent<MeshRenderer>().material = _coloredMaterial.NotHit;
                                    cylinder.transform.localScale = new Vector3(0.3f, 0.00001f, 0.3f);
                                    continue;
                                }
                            case "Positions":
                                {
                                    signs.Clear();
                                    continue;
                                }
                            default:
                                {
                                    Material[] materials = child.GetComponent<MeshRenderer>().materials;
                                    for (int i = 0; i < materials.Length; i++)
                                        materials[i] = darkMaterial;
                                    child.GetComponent<MeshRenderer>().materials = materials;
                                    continue;
                                }
                        }
                    }
                    break;
                }
            case Mode.ActivatedSmall:
                {
                    foreach (Transform child in blade)
                    {
                        string childName = child.name;
                        switch (childName)
                        {
                            case "Plane":
                                {
                                    child.GetComponent<MeshRenderer>().material = _coloredMaterial.BaseMaterial;
                                    continue;
                                }
                            case "Circle":
                                {
                                    child.GetComponent<MeshRenderer>().material = _coloredMaterial.BaseMaterial;
                                    child.transform.localScale = new Vector3(1, 1, 1);
                                    Transform cylinder = child.GetChild(0);
                                    cylinder.GetComponent<MeshRenderer>().material = darkMaterial;
                                    cylinder.transform.localScale = new Vector3(0.26f, 0.00001f, 0.26f);
                                    continue;
                                }
                            default:
                                {
                                    Material[] materials = child.GetComponent<MeshRenderer>().materials;
                                    for (int i = 0; i < materials.Length; i++)
                                        materials[i] = _coloredMaterial.BaseMaterial;
                                    child.GetComponent<MeshRenderer>().materials = materials;
                                    continue;
                                }
                        }
                    }
                    break;
                }
            case Mode.ActivatedBig:
                {
                    foreach (Transform child in blade)
                    {
                        string childName = child.name;
                        switch (childName)
                        {
                            case "middle":
                                {
                                    child.GetComponent<MeshRenderer>().material = darkMaterial;
                                    Transform plain = child.GetChild(0);
                                    plain.GetComponent<MeshRenderer>().material = _coloredMaterial.BaseMaterial;
                                    continue;
                                }
                            case "Circle":
                                {
                                    float dist = 0.2f;

                                    Transform cylinder = child.GetChild(0);
                                    cylinder.transform.localScale = new Vector3(0.26f, 0.00001f, 0.26f);
                                    cylinder.GetComponent<MeshRenderer>().material = darkMaterial;

                                    child.GetComponent<MeshRenderer>().material = _coloredMaterial.BaseMaterial;
                                    child.transform.localScale = new Vector3(1 - dist * ringCount, 1 - dist * ringCount, 1f);
                                    continue;
                                }
                            default:
                                {
                                    Material[] materials = child.GetComponent<MeshRenderer>().materials;
                                    for (int i = 0; i < materials.Length; i++)
                                        materials[i] = _coloredMaterial.BaseMaterial;
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
        return hkCamera.WorldToScreenPoint(worldPoint);
    }

    IEnumerator GenerateDataSet()
    {
        while (true)
        {
            yield return new WaitForSeconds(1 / generateDuration);
            bool ifHaveLabel = false;
            StreamWriter sr = 
                ifGenerate ?
                new StreamWriter(filePath + "/labels/" + index.ToString() + ".txt", true) 
                : new StreamWriter(filePath + "/labels/temp.txt", true);
            
            foreach (Transform blade in energyOrganBlades.transform)
            {
                int bladeClass = 0;

                List<Vector3> boundPoints = new();
                foreach (Transform bladePart in blade)
                {
                    boundPoints.Add(bladePart.GetComponent<MeshRenderer>().bounds.max);
                    boundPoints.Add(bladePart.GetComponent<MeshRenderer>().bounds.min);
                    boundPoints.Add(bladePart.GetComponent<MeshRenderer>().bounds.center);
                }
                string mainTargetLabel = ReturnBounds(boundPoints, 0.05f);


                foreach (Transform child in blade)
                {
                    if (child.name == "Circle")
                    {
                        if (child.GetComponent<MeshRenderer>().sharedMaterial.name == _coloredMaterial.BaseMaterial.name)
                            bladeClass = 2;
                        else if (child.GetChild(0).GetComponent<MeshRenderer>().sharedMaterial.name == _coloredMaterial.NotHit.name)
                            bladeClass = 1;
                        else
                            bladeClass = 0;

                    }
                    if (child.name == "Positions")
                    {
                        signs.Clear();
                        switch (bladeClass)
                        {
                            case 1:
                                {

                                    foreach (Transform sign in child)
                                    {
                                        Vector3 screenPoint = GetWorldToScreenPoint(sign.transform.position);
                                        signs.Add(new Vector2(screenPoint.x, screenPoint.y));
                                    }
                                    string label = "0 ";
                                    label += mainTargetLabel;
                                    foreach (Vector2 point in signs)
                                    {
                                        label += (point.x / hkCamera.scaledPixelWidth).ToString();
                                        label += " ";
                                        label += (1 - (point.y / hkCamera.scaledPixelHeight)).ToString();
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
                                        Vector3 screenPoint = GetWorldToScreenPoint(sign.transform.position);
                                        signs.Add(new Vector2(screenPoint.x, screenPoint.y));
                                    }
                                    string label = "1 ";
                                    label += mainTargetLabel;
                                    foreach (Vector2 point in signs)
                                    {
                                        label += (point.x / hkCamera.scaledPixelWidth).ToString();
                                        label += " ";
                                        label += (1 - (point.y / hkCamera.scaledPixelHeight)).ToString();
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
            {
                ScreenCapture.CaptureScreenshot(filePath + "/images/" + index.ToString() + ".jpg");
                index++;
            }
        }
    }

    string ReturnBounds(List<Vector3> boundPoints, float duration)
    {
        float x = 0.802f;
        float maxY = -2000, maxZ = -2000, minY = 2000, minZ = 2000;

        foreach (Vector3 point in boundPoints)
        {
            maxY = point.y > maxY ? point.y : maxY;
            maxZ = point.z > maxZ ? point.z : maxZ;
            minY = point.y < minY ? point.y : minY;
            minZ = point.z < minZ ? point.z : minZ;
        }

        Vector3 max = new Vector3(x, maxY, maxZ);
        Vector3 min = new Vector3(x, minY, minZ);
        Vector3 point1 = new Vector3(x, maxY, minZ);
        Vector3 point2 = new Vector3(x, minY, maxZ);

        Debug.DrawLine(max, point1, Color.green, 1 / generateDuration);
        Debug.DrawLine(max, point2, Color.green, 1 / generateDuration);
        Debug.DrawLine(point1, min, Color.green, 1 / generateDuration);
        Debug.DrawLine(point2, min, Color.green, 1 / generateDuration);

        Vector2 screenMax = GetWorldToScreenPoint(max);
        Vector2 screenMin = GetWorldToScreenPoint(min);

        float height = Math.Abs(screenMax.y - screenMin.y);
        float width = Math.Abs(screenMax.x - screenMin.x);
        Vector2 center = new Vector2(screenMin.x + width / 2f, screenMin.y + height / 2f);
        
        return (center.x / hkCamera.scaledPixelWidth) + " " + (1 - (center.y / hkCamera.scaledPixelHeight)) + " " + (width / hkCamera.pixelWidth) + " " + (height / hkCamera.pixelHeight) + " ";
    }


}
