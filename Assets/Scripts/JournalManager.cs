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
    public Sprite[] characterSprites;
    public Sprite transparentSprite;

    int currentPage = 0;
    int pageCount;
    Personality[] characters;
    string[] characterNames = new string[]
    {
        "Adelig 1",
        "Adelig 2",
        "Adelig 3",
        "Strong 1",
        "Strong 2",
        "Strong 3",
        "Strong 4"
    };

    void Start()
    {
        characters = new Personality[characterSprites.Length];
        for (int i = 0; i < 7; i++) characters[i] = new Personality();
        pageCount = Mathf.FloorToInt(characterSprites.Length / 2) + 1;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J)) journal.gameObject.SetActive(!journal.gameObject.activeSelf);
        
        // Page turning
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentPage < pageCount - 1) currentPage++;
        }

        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentPage > 0) currentPage--;
        }

        // Info stuff WARNING ADD BOOLEAN TO SHOW WHETHER THERE IS SOMETHING ON SECOND PAGE
        int firstCharacterIndex = currentPage * 2;
        int secondCharacterIndex = currentPage * 2 + 1;
        int firstPageNumber = (firstCharacterIndex + 1);
        int secondPageNumber = (secondCharacterIndex + 1);

        // Changing page numbers
        firstPage.text = firstPageNumber.ToString();
        secondPage.text = secondPageNumber.ToString();

        // Changing character name
        firstName.text = characterNames[firstCharacterIndex];
        if (secondCharacterIndex < characterSprites.Length) secondName.text = characterNames[secondCharacterIndex];
        else secondName.text = "???";

        // Changing character sprites
        firstCharacter.sprite = characterSprites[firstCharacterIndex];
        if (secondCharacterIndex < characterSprites.Length) secondCharacter.sprite = characterSprites[secondCharacterIndex];
        else secondCharacter.sprite = transparentSprite;

        // Changing character attributes WARNING THIS CODE IS FUCKING GARBAGE AND NEEDS OPTIMIZATION
        for (int i = 0; i < 3; i++)
        {
            Personality.Attribute attribute = characters[firstCharacterIndex].attributes[i];
            if (attribute == Personality.Attribute.Nothing) firstPageAttributes[i].text = "";
            else if (attribute == Personality.Attribute.Greed) firstPageAttributes[i].text = "Greed";
            else if (attribute == Personality.Attribute.StrongGreed) firstPageAttributes[i].text = "Strong Greed";
            else if (attribute == Personality.Attribute.Like) firstPageAttributes[i].text = "Likes " + characterNames[characters[firstCharacterIndex].likeIndex];
            else if (attribute == Personality.Attribute.StrongLike) firstPageAttributes[i].text = "Strongly likes " + characterNames[characters[firstCharacterIndex].strongLikeIndex];
            else if (attribute == Personality.Attribute.Hate) firstPageAttributes[i].text = "Hates " + characterNames[characters[firstCharacterIndex].hateIndex];
            else if (attribute == Personality.Attribute.StrongHate) firstPageAttributes[i].text = "Strongly hates " + characterNames[characters[firstCharacterIndex].strongHateIndex];
        }

        if (secondCharacterIndex < characterSprites.Length)
        {
            for (int i = 0; i < 3; i++)
            {
                Personality.Attribute attribute = characters[secondCharacterIndex].attributes[i];
                if (attribute == Personality.Attribute.Nothing) secondPageAttributes[i].text = "";
                else if (attribute == Personality.Attribute.Greed) secondPageAttributes[i].text = "Greed";
                else if (attribute == Personality.Attribute.StrongGreed) secondPageAttributes[i].text = "Strong Greed";
                else if (attribute == Personality.Attribute.Like) secondPageAttributes[i].text = "Likes " + characterNames[characters[firstCharacterIndex].likeIndex];
                else if (attribute == Personality.Attribute.StrongLike) secondPageAttributes[i].text = "Strongly likes " + characterNames[characters[firstCharacterIndex].strongLikeIndex];
                else if (attribute == Personality.Attribute.Hate) secondPageAttributes[i].text = "Hates " + characterNames[characters[firstCharacterIndex].hateIndex];
                else if (attribute == Personality.Attribute.StrongHate) secondPageAttributes[i].text = "Strongly hates " + characterNames[characters[firstCharacterIndex].strongHateIndex];
            }
        }

        else
        {
            for (int i = 0; i < 3; i++)
            {   
                secondPageAttributes[i].text = "";
            }
        }
    }
}
