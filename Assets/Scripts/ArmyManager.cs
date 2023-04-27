using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ArmyManager : MonoBehaviour
{
    [Header("Objects")]
    public Image army;
    public Image[] unassigned;
    public Image[] player;
    public Image[] emperor;
    public TextMeshProUGUI playerPower, emperorPower;
    public TextMeshProUGUI[] unassignedNames;
    public TextMeshProUGUI[] playerNames;
    public TextMeshProUGUI[] emperorNames;
    public TextMeshProUGUI[] unassignedPowerLevels;
    public TextMeshProUGUI[] playerPowerLevels;
    public TextMeshProUGUI[] emperorPowerLevels;

    [Header("Scripts")]
    public JournalManager journalManager; // For access to the army

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) army.gameObject.SetActive(!army.gameObject.activeSelf);

        if (army.gameObject.activeSelf)
        {
            Army army = journalManager.army;
            Personality[] people = journalManager.characters;

            for (int i = 0; i < 7; i++)
            {
                // Unassigned army      
                if (i < army.unassigned.Count)
                {
                    unassigned[i].sprite = army.unassigned[i].sprite;
                    unassignedNames[i].text = army.unassigned[i].name;
                    unassignedPowerLevels[i].text = army.unassigned[i].power.ToString();
                }

                else
                {
                    unassigned[i].sprite = journalManager.transparentSprite;
                    unassignedNames[i].text = "";
                    unassignedPowerLevels[i].text = "";
                }

                // Player army
                if (i < army.player.Count)
                {
                    player[i].sprite = army.player[i].sprite;
                    playerNames[i].text = army.player[i].name;
                    playerPowerLevels[i].text = army.player[i].power.ToString();
                }

                else
                {
                    player[i].sprite = journalManager.transparentSprite;
                    playerNames[i].text = "";
                    playerPowerLevels[i].text = "";
                }

                // Emperor army
                if (i < army.emperor.Count)
                {
                    emperor[i].sprite = army.emperor[i].sprite;
                    emperorNames[i].text = army.emperor[i].name;
                    emperorPowerLevels[i].text = army.emperor[i].power.ToString();
                }

                else
                {
                    emperor[i].sprite = journalManager.transparentSprite;
                    emperorNames[i].text = "";
                    emperorPowerLevels[i].text = "";
                }
            }

            // Player and emperor power levels
            playerPower.text = "Your total power: " + army.calculatePlayerPower().ToString();
            emperorPower.text = "Emperor total power: " + army.calculateEmperorPower().ToString();
        }
    }
}
