using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Army
{
    public List<Personality> emperor, player, loyalty, unassigned;
    DialogueManager dialogueManager; // For changing which scenes to skip
    Personality[] people;

    public Army(Personality[] people, DialogueManager dialogueManager)
    {
        emperor = new List<Personality>();
        player = new List<Personality>();
        loyalty = new List<Personality>();
        unassigned = new List<Personality>();
        this.people = people;
        this.dialogueManager = dialogueManager;
        foreach (Personality person in people) unassigned.Add(person);
    }

    public bool emperorHas(Personality person)
    {
        for (int i = 0; i < emperor.Count; i++) if (emperor[i] == person) return true;
        return false;
    }

    public bool playerHas(Personality person)
    {
        for (int i = 0; i < player.Count; i++) if (player[i] == person) return true;
        return false;
    }

    public bool loyaltyHas(Personality person)
    {
        for (int i = 0; i < loyalty.Count; i++) if (loyalty[i] == person) return true;
        return false;
    }

    public int calculatePlayerPower()
    {
        int powerLevel = 0;
        foreach (Personality person in player) powerLevel += person.power;
        return powerLevel;
    }

    public int calculateEmperorPower()
    {
        int powerLevel = 0;
        foreach (Personality person in emperor) powerLevel += person.power;
        return powerLevel;
    }

    // Adding to lists
    public void addToEmperor(Personality person)
    {
        Debug.Log("Adding to emperor: " + person.name);
        for (int i = 0; i < people.Length; i++) if (people[i] == person) dialogueManager.skipScenes[i] = true;
        if (!emperorHas(person)) emperor.Add(person);

        // Remove person from all other lists
        if (unassigned.IndexOf(person) != -1) unassigned.RemoveAt(unassigned.IndexOf(person));
        if (loyalty.IndexOf(person) != -1) loyalty.RemoveAt(loyalty.IndexOf(person));
        if (player.IndexOf(person) != -1) player.RemoveAt(player.IndexOf(person));
        
        List<Personality> peopleToAdd = new List<Personality>();
        for (int i = 0; i < loyalty.Count; i++) if (loyalty[i].hasAttribute(Personality.Attribute.StrongLike) && people[loyalty[i].strongLikeIndex] == person) 
        {
            Debug.Log(loyalty[i].name + " likes " + person.name);
            peopleToAdd.Add(loyalty[i]);
        }
        for (int i = 0; i < unassigned.Count; i++) if (unassigned[i].hasAttribute(Personality.Attribute.StrongLike) && people[unassigned[i].strongLikeIndex] == person)
        {
            Debug.Log(unassigned[i].name + " likes " + person.name);
            peopleToAdd.Add(unassigned[i]);
        }
        
        foreach (Personality personality in peopleToAdd) addToEmperor(personality);
    }

    public void addToPlayer(Personality person)
    {
        for (int i = 0; i < 7; i++) if (people[i] == person) dialogueManager.skipScenes[i] = true;
        if (!playerHas(person)) player.Add(person);
        if (unassigned.IndexOf(person) != -1) unassigned.RemoveAt(unassigned.IndexOf(person));
        if (loyalty.IndexOf(person) != -1) loyalty.RemoveAt(loyalty.IndexOf(person));
        if (emperor.IndexOf(person) != -1) emperor.RemoveAt(emperor.IndexOf(person));

        List<Personality> peopleToAdd = new List<Personality>();
        for (int i = 0; i < loyalty.Count; i++) if (loyalty[i].hasAttribute(Personality.Attribute.StrongLike) && people[loyalty[i].strongLikeIndex] == person) peopleToAdd.Add(loyalty[i]);
        for (int i = 0; i < unassigned.Count; i++) if (unassigned[i].hasAttribute(Personality.Attribute.StrongLike) && people[unassigned[i].strongLikeIndex] == person) peopleToAdd.Add(unassigned[i]);
        foreach (Personality personality in peopleToAdd) addToEmperor(personality);
    }

    public void addToLoyal(Personality person)
    {
        if (!loyaltyHas(person)) loyalty.Add(person);
        if (unassigned.IndexOf(person) != -1) unassigned.RemoveAt(unassigned.IndexOf(person));
        if (player.IndexOf(person) != -1) player.RemoveAt(player.IndexOf(person));
        if (emperor.IndexOf(person) != -1) emperor.RemoveAt(emperor.IndexOf(person));
    }
}