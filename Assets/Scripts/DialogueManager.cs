using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Image character;
    public Sprite[] characters;

    // For writing animation
    bool textWriting = false;
    string writeText;
    float animationTime;
    float timePassed;

    // For response options
    int currentBranch = 0;
    string first = "Hello, I'm MC, a highly ranked noble who is currently gathering and army to overthrow the emperor.";
    string second = "We already have a lot of other people who have joined us and can guarentee a lot of rewards if you help us.";

    void Update()
    {
        if (textWriting)
        {
            if (timePassed >= animationTime)
            {
                text.text = writeText;
                textWriting = false;
            }

            else
            {
                float progress = timePassed / animationTime;
                int writeLength = Mathf.FloorToInt(progress * writeText.Length);
                text.text = writeText.Substring(0, writeLength);
                timePassed += Time.deltaTime;
            }
        }

        else if (currentBranch == 0 || currentBranch == 1)
        {
            if (currentBranch == 0) {
                changeText(first, 3);
                currentBranch++;
            }

            else if (Input.GetMouseButtonDown(0))
            {
                changeText(second, 3);
                currentBranch++;
            }
        }

        else
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
            }

            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
            }

            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
            }

            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
            }
        }
    }

    void changeText(string newText, float secTime)
    {
        if (!textWriting)
        {
            animationTime = secTime;
            writeText = newText;
            timePassed = 0f;
            textWriting = true;
        }
    }

    void changeCharacter(int index)
    {
        if (index >= 0 && index <= characters.Length - 1) character.sprite = characters[index];
    }
}
