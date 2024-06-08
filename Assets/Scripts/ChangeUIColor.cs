using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeUIColor : MonoBehaviour
{
    public Button button;
    public Text text;
    public Color UIBlue = new Color(120, 120, 255);
    public Color UIRed = new Color(255, 90, 90);
    public ColorBlock cb;
    // Start is called before the first frame update
    void Start()
    {
        cb.normalColor = UIBlue;
        cb.selectedColor = UIBlue;
        if (button)
            button.colors = cb;
    }

    public void changeUIcolor()
    {
        cb.normalColor = (cb.normalColor == UIRed) ? UIBlue : UIRed;
        cb.selectedColor = (cb.selectedColor == UIRed) ? UIBlue : UIRed;
        button.colors = cb;
    }

    public void changeUItexture()
    {
        text.text = string.Compare(text.text, "大") == 0 ? "小" : "大";
    }
}
