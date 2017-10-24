using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class scr_Project : MonoBehaviour
{
    public string projectName;
    public string projectPath;
    public string projectTime;

    public cls_Passages passages;
    public GameObject passagePrefab;
    public GameObject canvas;

    void Start()
    {
        //UpdatePassages();
        ReadJson();
    }

    void Update()
    {
        CheckForChanges();
    }

    void UpdatePassages()
    {
        foreach (GameObject p in GameObject.FindGameObjectsWithTag("Passage"))
            Destroy(p);

        //Make the new passage GameObjects
        foreach (cls_Passage p in passages.passages)
        {
            GameObject passage = Instantiate(passagePrefab, canvas.transform);
            p.inGamePassage = passage;
            passage.name = p.passageName;

            //Match the values.
            scr_Passages passageScript = p.inGamePassage.GetComponentInChildren<scr_Passages>();

            //Set the name
            passageScript.passageText = p.text;
            passageScript.passageName = p.passageName;

            //Set the text
            passageScript.name.text = p.passageName;
            passageScript.text.text = p.text;

            passageScript.passagePos = p.position;
            passageScript.GetComponent<RectTransform>().position = passageScript.passagePos;

        }
    }

    void CheckForChanges()
    {
        foreach (cls_Passage pa in passages.passages)
        {
            scr_Passages passageScript = pa.inGamePassage.GetComponent<scr_Passages>();

            passageScript.passageName = passageScript.name.text;
            passageScript.passageText = passageScript.text.text;
            passageScript.passagePos = passageScript.GetComponent<RectTransform>().position;

            pa.position = passageScript.passagePos;

            pa.passageName = passageScript.passageName;
            pa.text = passageScript.passageText;
        }
    }

    public void SaveToJSon()
    {
        string JSON = JsonUtility.ToJson(passages, true);

        Debug.Log(JSON);

        //Get the time
        System.DateTime time = System.DateTime.Now;

        //Remove the spaces
        string timeString = time.ToString();
        string[] timeStringArray = timeString.Split();
        projectTime = "";

        foreach (string s in timeStringArray)
        {
            projectTime += s;
            projectTime += "_";
        }

        //Replace all the "/" with "-".
        timeStringArray = projectTime.Split('/');

        projectTime = "";

        foreach (string s in timeStringArray)
        {
            projectTime += s;
            projectTime += "-";
        }

        //Replace all the ":" with "-".
        timeStringArray = projectTime.Split(':');

        projectTime = "";

        foreach (string s in timeStringArray)
        {
            projectTime += s;
            projectTime += "-";
        }

        //Get the path
        string filePath = Application.dataPath + "/" + projectPath + "/" + projectName + "_" + projectTime + ".txt";
        Debug.Log("FilePath: " + filePath);

        //Create a new file and write the text to it.
        File.Create(filePath);
        File.WriteAllText(filePath, JSON);

        //if (File.Exists(projectPath + "/" + projectName + ".txt"))
        //{
        //    File.WriteAllText(projectPath + "/" + projectName + ".txt", JSON);
        //}
        //else {
        //    File.Create(projectPath + "/" + projectName + ".txt");
        //    File.WriteAllText(Application.persistentDataPath + "/" + projectPath + "/" + projectName + ".txt", JSON);
        //}

        Debug.Log("Wrote json to " + projectName);
    }

    public void ReadJson()
    {
        if (projectTime == "")
        {
            string filePath = Application.dataPath + "/" + projectPath + "/" + projectName + ".txt";

            if (File.Exists(filePath))
                JsonUtility.FromJsonOverwrite(File.ReadAllText(filePath), passages);
            else
                Debug.Log("File not found");
        }
        else {
            string filePath = Application.dataPath + "/" + projectPath + "/" + projectName + "_" + projectTime + ".txt";

            if (File.Exists(filePath))
                JsonUtility.FromJsonOverwrite(File.ReadAllText(filePath), passages);
            else
                Debug.Log("File not found");
        }

        UpdatePassages();
    }
}

[System.Serializable]
public class cls_Passages
{
    public List<cls_Passage> passages;
}

[System.Serializable]
public class cls_Passage
{
    //public string fromPassage;
    public string passageName;
    public string text;
    public GameObject inGamePassage;

    public Vector3 position;

    public cls_Passage()
    {
    }

    public cls_Passage(string _passageName, string _text)
    {
        passageName = _passageName;
        text = _text;
    }
}