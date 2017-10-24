using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scr_PassageManager : MonoBehaviour
{

    //This is the script that interprets the C# passage files into a full scenario
    [Header("Passage Stuff")]
    public cls_PassageList passages;
    public cls_Pas currentPassage;

    [Header("UI stuff")]
    public Text passageText;
    public GameObject buttonPanel;
    public GameObject buttonPrefab;

    void Start()
    {
        currentPassage = passages.passages[0];

        SetText(currentPassage.passageName, currentPassage.text);
        SetOptions(currentPassage.options);
    }

    void Update()
    {
        CheckPassageProgress();
    }

    void CheckPassageProgress()
    {
        //Look trough each option of the currently active pasage
        foreach (cls_Option option in currentPassage.options)
        {
            //Count the amount of option
            int boolCount = 0;

            foreach (cls_OptionBool optBool in option.bools)
                if (optBool.value)
                    boolCount++;

            //If all the options are good
            if (boolCount == option.bools.Count)
            {
                //Reset all the bools
                foreach (cls_OptionBool b in option.bools)
                    b.value = false;

                Debug.Log("Setting new passage to " + option.nextPassage);
                if (option.nextPassage != "")
                {
                    //Unload the old passage
                    currentPassage = null;
                    //Set the next passage as per the option
                    currentPassage = GetPassage(option.nextPassage);

                    SetText(currentPassage.passageName, currentPassage.text);
                    SetOptions(currentPassage.options);
                }
                else
                {
                    Debug.Log("No passage found.");
                }
            }
        }
    }

    public void SetText(string _passageName, string passageText)
    {
        passageText = "";
        passageText = _passageName + "\n" + passageText;
    }

    public void SetOptions(List<cls_Option> _options)
    {
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("OptionButton"))
            Destroy(g);

        foreach (cls_Option option in _options) {
            GameObject button = Instantiate(buttonPrefab, buttonPanel.transform);
            button.GetComponentInChildren<Text>().text = option.optionText;
            button.GetComponent<Button>().onClick.AddListener(delegate {
                foreach(cls_OptionBool ob in option.bools)
                    ob.value = true;
            }); 
        }
    }

    cls_Pas GetPassage(string _passageName)
    {
        cls_Pas pas = null;

        foreach (cls_Pas p in passages.passages)
            if (p.passageName == _passageName)
                pas = p;

        return pas;
    }


}

[System.Serializable]
public class cls_PassageList
{
    public List<cls_Pas> passages;
}


[System.Serializable]
public class cls_Pas
{
    public string passageName;
    public string text;
    public List<cls_Option> options;
}

[System.Serializable]
public class cls_Option
{
    public string optionText;
    public string nextPassage;
    public List<cls_OptionBool> bools;
}

[System.Serializable]
public class cls_OptionBool
{
    public string boolName;
    public bool value;
}