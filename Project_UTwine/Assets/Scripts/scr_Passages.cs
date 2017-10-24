using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class scr_Passages : MonoBehaviour
{
    public Button passageButton;
    [Header ("Raw Data")]
    public string passageName;
    public string passageText;
    public Vector3 passagePos;

    [Header("String displays")]
    public InputField name;
    public InputField text;

    public void MovePassage()
    {
        //Vector3 offset = this.GetComponent<RectTransform>().position - Input.mousePosition;

        this.GetComponent<RectTransform>().position = Input.mousePosition;
    }
}
