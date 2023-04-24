using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Personality
{
    Image image;
    public Attribute[] attributes = new Attribute[3];

    // WARNING MAKE SURE
    // Targets for different attributes (love/hate)
    public int hateIndex = Random.Range(0, 7);
    public int strongHateIndex = Random.Range(0, 7);
    public int likeIndex = Random.Range(0, 7);
    public int strongLikeIndex = Random.Range(0, 7);

    public Personality() 
    {
        int attributeCount = Random.Range(1, 4);
        for (int i = 0; i < attributeCount; i++)
        {
            Attribute attribute = (Attribute)Random.Range(0, 6);
            attributes[i] = attribute;

            
            /* 
            Idk there's something wrong with this code here
            while (hasAttribute(attribute))
            {
                attribute = (Attribute)Random.Range(0, 8);
                attributes[i] = attribute;
            }*/
        }

        // Pad the reming attribute with nothing
        for (int i = 0; i < 3 - attributeCount; i++)
        {
            attributes[attributes.Length-1-i] = Attribute.Nothing;
        }
    }

    bool hasAttribute(Attribute attribute)
    {
        for (int i = 0; i < 3; i++) if (attributes[i] == attribute) return true;
        return false;
    }

    public enum Attribute {Hate, StrongHate, Like, StrongLike, Greed, StrongGreed, Nothing};
}