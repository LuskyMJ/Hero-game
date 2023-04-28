using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class JournalManager : MonoBehaviour
{
    [Header("Journal Info")]
    public GameObject journal;
    public Image firstCharacter, secondCharacter;
    public TextMeshProUGUI firstPage, secondPage;
    public TextMeshProUGUI firstName, secondName;
    public TextMeshProUGUI[] firstPageAttributes, secondPageAttributes;

    [Header("Images")]
    [SerializeField]
    Sprite[] characterSprites;
    public Sprite transparentSprite;

    int currentPage = 0;
    public DialogueManager dialogueManager; // This just needs to be passed to the army so it can change the skipScenes field
    int pageCount;
    public Personality[] characters;
    public Army army;
    string[] characterNames = new string[]
    {
        "Robber",
        "Noble 1",
        "Hero",
        "Commander",
        "Noble 2",
        "Noble 3",
        "Bandit"
    };

    void Start()
    {
        characters = new Personality[characterSprites.Length];
        for (int i = 0; i < 7; i++) characters[i] = new Personality(characterSprites[i], characterNames[i], i, 10);
        pageCount = Mathf.FloorToInt(characterSprites.Length / 2) + 1;
        
        // Making sure two people aren't loyal to each other
        for (int i = 0; i < 7; i++) 
        {
            if (characters[i].hasAttribute(Personality.Attribute.StrongLike))
            {
                Personality likes = characters[characters[i].strongLikeIndex];
                if (likes.hasAttribute(Personality.Attribute.StrongLike) && likes.strongLikeIndex == i)
                {
                    for (int j = 0; j < 3; j++) if (characters[i].attributes[j] == Personality.Attribute.StrongLike) characters[i].attributes[j] = Personality.Attribute.Nothing;
                }
            }
        }

        army = new Army(characters, dialogueManager);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J)) journal.gameObject.SetActive(!journal.gameObject.activeSelf);
        
        // Page turning
        if (Input.GetKeyDown(KeyCode.RightArrow) && journal.gameObject.activeSelf && currentPage < pageCount - 1) currentPage++;
        if (Input.GetKeyDown(KeyCode.LeftArrow) && journal.gameObject.activeSelf && currentPage > 0) currentPage--;


        // Info stuff WARNING ADD BOOLEAN TO SHOW WHETHER THERE IS SOMETHING ON SECOND PAGE
        int firstCharacterIndex = currentPage * 2;
        int secondCharacterIndex = currentPage * 2 + 1;
        int firstPageNumber = (firstCharacterIndex + 1);
        int secondPageNumber = (secondCharacterIndex + 1);
        bool showSecondPage = secondCharacterIndex < characterSprites.Length; // Whether there's a character on the second page

        // Changing page numbers
        firstPage.text = firstPageNumber.ToString();
        secondPage.text = secondPageNumber.ToString();

        // Changing character name
        firstName.text = characterNames[firstCharacterIndex];
        if (showSecondPage) secondName.text = characterNames[secondCharacterIndex];
        else secondName.text = "";

        // Changing character sprites
        firstCharacter.sprite = characterSprites[firstCharacterIndex];
        if (showSecondPage) secondCharacter.sprite = characterSprites[secondCharacterIndex];
        else secondCharacter.sprite = transparentSprite;

        // Changing character attributes WARNING THIS CODE IS FUCKING GARBAGE AND NEEDS OPTIMIZATION
        for (int i = 0; i < 3; i++)
        {
            Personality.Attribute attribute = characters[firstCharacterIndex].attributes[i];
            Personality firstCharacter = characters[firstCharacterIndex];

            if (attribute == (Personality.Attribute)6) firstPageAttributes[i].text = "";
            else if (attribute == (Personality.Attribute)5) firstPageAttributes[i].text = "Strong Greed";
            else if (attribute == (Personality.Attribute)4) firstPageAttributes[i].text = "Greed";
            else if (attribute == (Personality.Attribute)3) firstPageAttributes[i].text = "Strongly likes " + characterNames[firstCharacter.strongLikeIndex];
            else if (attribute == (Personality.Attribute)2) firstPageAttributes[i].text = "Likes " + characterNames[firstCharacter.likeIndex];
            else if (attribute == (Personality.Attribute)1) firstPageAttributes[i].text = "Strongly hates " + characterNames[firstCharacter.strongHateIndex];
            else if (attribute == (Personality.Attribute)0) firstPageAttributes[i].text = "Hates " + characterNames[firstCharacter.hateIndex];

            if (showSecondPage)
            {
                attribute = characters[secondCharacterIndex].attributes[i];
                Personality secondCharacter = characters[secondCharacterIndex];
                if (attribute == (Personality.Attribute)6) secondPageAttributes[i].text = "";
                else if (attribute == (Personality.Attribute)5) secondPageAttributes[i].text = "Strong Greed";
                else if (attribute == (Personality.Attribute)4) secondPageAttributes[i].text = "Greed";
                else if (attribute == (Personality.Attribute)3) secondPageAttributes[i].text = "Strongly likes " + characterNames[secondCharacter.strongLikeIndex];
                else if (attribute == (Personality.Attribute)2) secondPageAttributes[i].text = "Likes " + characterNames[secondCharacter.likeIndex];
                else if (attribute == (Personality.Attribute)1) secondPageAttributes[i].text = "Strongly hates " + characterNames[secondCharacter.strongHateIndex];
                else if (attribute == (Personality.Attribute)0) secondPageAttributes[i].text = "Hates " + characterNames[secondCharacter.hateIndex];
            }
            else secondPageAttributes[i].text = "";
        }
    }

    public Dictionary<string, int> calculateOptions(Personality person)
    {
        Dictionary<string, int> options = new Dictionary<string, int>();
        if (person.hasAttribute(Personality.Attribute.StrongLike)) 
        {
            options.Add("I am only loyal to " + characterNames[person.strongLikeIndex] + ". May we meet again soon, whether it's with our swords draw at each other or defending each others back.", 0);
            return options;
        }

        else
        {

            // Determening the base price
            int baseprice = 1000;
            if (person.hasAttribute(Personality.Attribute.Like))
            {
                if (army.emperorHas(characters[person.likeIndex])) {
                    options.Add("I'm tempted to join the emperors side because I like " + characterNames[person.likeIndex] + ", but I will join you for a high enough price.", 0);
                    baseprice += 250;
                }
                else if (army.playerHas(characters[person.likeIndex])) {
                    options.Add("I'm tempted to join your side because I like " + characterNames[person.likeIndex] + ", but I wont do it for free.", 0);
                    baseprice -= 250;
                }

                else options.Add("I don't care who I fight for as long as I get paid for it.", 0);
            }

            else options.Add("I don't care who I fight for as long as I get paid for it.", 0);
            
            // WARNING add logic for greed

    
            bool hatePlayer = person.hasAttribute(Personality.Attribute.Hate) && army.playerHas(characters[person.hateIndex]);
            bool strongHatePlayer = person.hasAttribute(Personality.Attribute.StrongHate) && army.playerHas(characters[person.strongHateIndex]);
            if (hatePlayer) baseprice += 250;
            if (strongHatePlayer) baseprice += 500;
            if (person.hasAttribute(Personality.Attribute.Greed)) baseprice += 250;
            if (person.hasAttribute(Personality.Attribute.StrongGreed)) baseprice += 500;

            options.Add("Pay the price.", baseprice);


            // The tempPrice variable is used to negate the increase in base price that happens when a character hates a person on your team.
            // Example: If the person strongly hates someone on your team the base price will incrase by 500. If you promise that persons head
            // the price should both decrease by 500 for promising the head and 500 again for not having to work with the person on your team.
            if (person.hasAttribute(Personality.Attribute.Hate) && person.hasAttribute(Personality.Attribute.StrongHate))
            {
                int tempPrice = 0;
                if (hatePlayer) tempPrice = 250;
                options.Add("Head of " + characterNames[person.hateIndex] + ".", baseprice - 250 - tempPrice);

                tempPrice = 0;
                if (strongHatePlayer) tempPrice = 500;
                options.Add("Head of " + characterNames[person.strongHateIndex] + ".", baseprice - 500 - tempPrice);

                tempPrice = 0;
                if (hatePlayer) tempPrice += 250;
                if (strongHatePlayer) tempPrice += 500;
                options.Add("Heads of " + characterNames[person.strongHateIndex] + " and " + characterNames[person.hateIndex] + ".", baseprice - 750 - tempPrice);
            }

            else if (person.hasAttribute(Personality.Attribute.Hate))
            {
                int tempPrice = 0;
                if (hatePlayer) tempPrice = 250;
                options.Add("Head of " + characterNames[person.hateIndex] + ".", baseprice - 250 - tempPrice);
            }

            else if (person.hasAttribute(Personality.Attribute.StrongHate))
            {
                int tempPrice = 0;
                if (hatePlayer) tempPrice = 500;
                options.Add("Head of " + characterNames[person.strongHateIndex] + ".", baseprice - 500 - tempPrice);
            }

            options.Add("Refuse to pay.", 0);
            return options;
        }
    }
}