using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoManager : MonoBehaviour
{
    public Image image;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            image.gameObject.SetActive(!image.gameObject.activeSelf);
        }
    }
}
