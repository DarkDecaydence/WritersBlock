using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TextBox : MonoBehaviour {

    public string errorMessage = "*winkyface*";

    List<Text> textObjects;
    Transform textArea;
    InputField inputArea;
    Translater translater;

	// Use this for initialization
	void Start () {

        GameData.textBox = this;
        translater = gameObject.GetComponent<Translater>();
        textObjects = new List<Text>();
        textArea = transform.FindChild("TextArea");
        inputArea = transform.FindChild("WriteLine").FindChild("InputField").GetComponent<InputField>();

        for (int i = 0; i < textArea.childCount; i++)
            textObjects.Add(textArea.GetChild(i).GetComponent<Text>());

	}

    public void pushMessageToBox(string s, Color c)
    {
        if (inputArea.text.Length == 0)
            return;

        List<string> temp = new List<string>();
        List<Color> tempColor = new List<Color>();

        for (int i = 0; i < textObjects.Count; i++)
        {
            temp.Add(textObjects[i].text);
            tempColor.Add(textObjects[i].color);
        }

        for (int i = 1; i < textObjects.Count; i++)
        {
            textObjects[i].text = temp[i - 1];
            textObjects[i].color = tempColor[i - 1];
        }

        textObjects[0].text = s;
        textObjects[0].color = c;
    }

    void clearInputBox()
    {
        inputArea.text = "";
    }

    void focusOnInputField()
    {
        inputArea.ActivateInputField();
    }

    // Update is called once per frame
    void Update () {

        if (Input.GetKeyDown(KeyCode.Return))
        {

            Message message = translater.interpretMessage(inputArea.text);

            if(message.valid)
                pushMessageToBox(inputArea.text, Color.black);
            else
                pushMessageToBox(message.errMessage, Color.red);

            clearInputBox();
            focusOnInputField();

        }
            
	}
}
