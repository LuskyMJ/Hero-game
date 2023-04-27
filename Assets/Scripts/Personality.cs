using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Personality
{
    public Sprite sprite;
    public string name;
    public int power;
    public Attribute[] attributes = new Attribute[] {(Attribute)6, (Attribute)6, (Attribute)6, (Attribute)6};

    // Targets for different attributes (love/hate)
    public int hateIndex;
    public int strongHateIndex;
    public int likeIndex;
    public int strongLikeIndex;

    public Personality(Sprite sprite, string name, int characterIndex, int power) 
    {
        this.sprite = sprite;
        this.name = name;
        this.power = power;

        hateIndex = Random.Range(0, 7);
        while (hateIndex == characterIndex) hateIndex = Random.Range(0, 7);
        strongHateIndex = Random.Range(0, 7);
        while (strongHateIndex == characterIndex || strongHateIndex == hateIndex) strongHateIndex = Random.Range(0, 7);
        likeIndex = Random.Range(0, 7);
        while (likeIndex == characterIndex || likeIndex == hateIndex || likeIndex == strongHateIndex) likeIndex = Random.Range(0, 7);
        strongLikeIndex = Random.Range(0, 7);
        while (strongLikeIndex == characterIndex || strongLikeIndex == likeIndex || strongLikeIndex == hateIndex || strongLikeIndex == strongHateIndex) strongLikeIndex = Random.Range(0, 7);

        int attributeCount = Random.Range(1, 4);
        for (int i = 0; i < attributeCount; i++)
        {
            Attribute attribute = (Attribute)Random.Range(0, 6);
            while (hasAttribute(attribute)) attribute = (Attribute)Random.Range(0, 8);
            attributes[i] = attribute;
        }
    }

    public bool hasAttribute(Attribute attribute)
    {
        for (int i = 0; i < 3; i++) if (attributes[i] == attribute) return true;
        return false;
    }

    public enum Attribute {Hate, StrongHate, Like, StrongLike, Greed, StrongGreed, Nothing};
}