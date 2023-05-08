using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoManager : MonoBehaviour
{
    public Image image;
    public Image armyScreen, journalScreen;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (!image.gameObject.activeSelf)
            {
                armyScreen.gameObject.SetActive(false);
                journalScreen.gameObject.SetActive(false);
            }
            image.gameObject.SetActive(!image.gameObject.activeSelf);
        }
    }
}