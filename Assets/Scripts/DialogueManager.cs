using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    [Header("Objects")]
    public TextMeshProUGUI text;
    public Image character;
    public JournalManager journalManager;
    public TextMeshProUGUI[] options;
    public TextMeshProUGUI mainText;
    public Image background;
    public GameObject chatbox;
    Personality[] people;

    [Header("Background")]
    public Sprite[] backgrounds;
    public Sprite intermission;
    bool isIntermission = true;
    int currentScene = 0;
    bool interMissionDone = false;
    bool gameDone;
    
    string[] intermissionText = new string[]
    {
        "Your name is Zuma Cuahutenco. You're a middle aged man living in the aztec kingdom with a single daughter and the wealth you've built up throughout your life. \n\n On a completely normal day. Your daughter gets chosen by the unpopular emperor to get sacrificed in a ritual in order to prevent a comming drought. \n\n In a fit of rage you start to use your wealth to collect powerful people in order to overthrow the emperor and save your daughter from his brutality. \n\n You go to visit a noble person but accidentally meet a robber on your way.",
        "You keep going until you reach the nobles castle.",
        "You continue your journey towards the noble but get lost in the woods. You meet a hero in the forest.",
        "You continue all the way to the nobles castle but is stopped outside by his commander.",
        "You walk into the nobles castle and into the throne room where the noble sits.",
        "Your journey continues towards a nobles castle.",
        "You hear from someone local that there's a powerful bandit hiding with his gang inside the forest.",
        "With your army gathered you meet the emperor in a final battle for the throne..."
    };
    
    [HideInInspector]
    public bool[] skipScenes;

    // For writing animation
    bool textWriting = false;
    string writeText;
    float animationTime;
    float timePassed;
    bool introduction = true;
    bool awaitingClick = false;

    // For response options
    string[] introductionTexts = new string[]
    {
        "Hello, I'm Zuma Cuahutenco, a highly ranked noble who is currently gathering and army to overthrow our incompetent emperor and restore our nation to it's former glory.",
        "If you join us in our mission, you will recieve rewards beyond anything you ever could imagine."
    };
    string nextText;

    void Start()
    {
        people = journalManager.characters;
        changeText(intermissionText[0], 3);
        changeBackground(intermission);
        skipScenes = new bool[]
        {
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
        };
    }

    void Update()
    {
        Personality person = people[0];
        if (currentScene >= 0 && currentScene <= people.Length - 1) 
        {
            character.sprite = people[currentScene].sprite;
            person = people[currentScene];
        }
        
        if (textWriting)
        {
            if (timePassed >= animationTime)
            {
                if (isIntermission)
                {
                    if (currentScene == intermissionText.Length - 1)
                    {
                        gameDone = true;
                    }

                    mainText.text = writeText;
                    interMissionDone = true;
                }
                else text.text = writeText;

                textWriting = false;
                awaitingClick = true;
            }

            else
            {
                float progress = timePassed / animationTime;
                int writeLength = Mathf.FloorToInt(progress * writeText.Length);
                if (isIntermission) mainText.text = writeText.Substring(0, writeLength);
                else text.text = writeText.Substring(0, writeLength);
                timePassed += Time.deltaTime;
            }
        }

        // Only allow the player to click if there's more text to be shown or ready to switch scene
        else if (awaitingClick && (nextText != "" || writeText == "I would love to fight by your side." || writeText == "I will do everything I can to make sure you don't succeed then!" || writeText.Contains("whether it's") || interMissionDone || gameDone))
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (isIntermission)
                {
                    if (gameDone) 
                    {
                        Debug.Log("Game done");
                        if (journalManager.army.calculateEmperorPower() > journalManager.army.calculatePlayerPower()) SceneManager.LoadScene("LoseScreen");
                        else SceneManager.LoadScene("WinScreen");
                    }

                    for (int i = 0; i < options.Length; i++) options[i].text = "";
                    mainText.text = "";
                    isIntermission = false;
                    changeBackground(backgrounds[currentScene]);
                    chatbox.gameObject.SetActive(true);
                    character.sprite = person.sprite;
                    changeText(introductionTexts[0], 2);
                    nextText = introductionTexts[1];
                    interMissionDone = false;
                    introduction = true;
                }

                else
                {
                    if (writeText == "I would love to fight by your side." || writeText == "I will do everything I can to make sure you don't succeed then!" || writeText.Contains("whether it's"))
                    {
                        if (writeText.Contains("whether it's")) journalManager.army.addToLoyal(person);

                        chatbox.gameObject.SetActive(false);
                        isIntermission = true;
                        awaitingClick = false;
                        currentScene++;

                        Debug.Log("Scene amount: " + skipScenes.Length);
                        Debug.Log("All scenes");
                        foreach(bool thing in skipScenes) Debug.Log(thing);

                        Debug.Log("Current Scene " + currentScene);
                        while (skipScenes[currentScene]) 
                        {
                            currentScene++;
                            Debug.Log("Current Scene " + currentScene);
                        }

                        // Game is finished
                        if (currentScene >= intermissionText.Length)
                        {
                            if (journalManager.army.calculateEmperorPower() > journalManager.army.calculatePlayerPower()) Debug.Log("YOU LOST");
                            else Debug.Log("YOU WON");
                        }

                        changeBackground(intermission);
                        changeText(intermissionText[currentScene], 3);
                    }

                    else
                    {
                        changeText(nextText, 2);
                        nextText = "";
                        awaitingClick = false;
                    }
                }
            }
        }
        
        else
        {
            // Generate player options
            Dictionary<string, int> playerOptions = journalManager.calculateOptions(person);

            string[] keys = new string[playerOptions.Count];
            int[] values = new int[playerOptions.Count];

            int i = 0;
            foreach (var pair in playerOptions)
            {
                keys[i] = pair.Key;
                values[i] = pair.Value;
                i++;
            }

            if (introduction)
            {
                introduction = false;
                nextText = keys[0];
                awaitingClick = true;
            }

            else
            {
                for (i = 1; i < keys.Length; i++) options[i - 1].text = keys[i] + " (" + values[i].ToString() + ")";

                int action = -1;
                if (Input.GetKeyDown(KeyCode.Alpha1) && 1 <= keys.Length - 1) action = 1;
                else if (Input.GetKeyDown(KeyCode.Alpha2)  && 2 <= keys.Length - 1) action = 2;
                else if (Input.GetKeyDown(KeyCode.Alpha3)  && 3 <= keys.Length - 1) action = 3;
                else if (Input.GetKeyDown(KeyCode.Alpha4)  && 4 <= keys.Length - 1) action = 4;
                else if (Input.GetKeyDown(KeyCode.Alpha5)  && 5 <= keys.Length - 1) action = 5;

                if (action != -1 || writeText.Contains("whether it's"))
                {
                    Personality[] peopleToKillList = peopleToKill(keys[action]);
                    string sentence = generateSentence(peopleToKillList, values[action], keys[action].Contains("Refuse"));
                    foreach (Personality personality in peopleToKillList) journalManager.army.addToEmperor(personality);
                    changeText(sentence, 2);
                    if (sentence.Contains("refuse"))
                    {
                        journalManager.army.addToEmperor(person);
                        nextText = "I will do everything I can to make sure you don't succeed then!";
                    }

                    else 
                    {
                        journalManager.army.addToPlayer(person);
                        nextText = "I would love to fight by your side.";
                    }

                }
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

    void changeBackground(Sprite newBackground)
    {
        background.sprite = newBackground;
    }

    Personality[] peopleToKill(string action)
    {
        List<Personality> peopleToKill = new List<Personality>();

        for (int i = 0; i < people.Length; i++)
        {
            if (action.Contains(people[i].name)) peopleToKill.Add(people[i]);
        }

        return peopleToKill.ToArray();
    }

    string generateSentence(Personality[] peopleToKill, int gold, bool refuse)
    {
        if (peopleToKill.Length == 0)
        {
            if (refuse) return "I refuse to pay you.";
            else return "I will give you " + gold.ToString() + " gold if you join me.";
        }

        string[] peopleToKillNames = new string[peopleToKill.Length];
        for (int i = 0; i < peopleToKill.Length; i++) peopleToKillNames[i] = peopleToKill[i].name;

        string sentence = "I will give you the ";
        if (peopleToKillNames.Length > 1) sentence += "heads of ";
        else sentence += "head of ";
        sentence += string.Join(" and ", peopleToKillNames);
        sentence += " and give you " + gold.ToString() + " gold if you join me.";
        return sentence;
    }
}
