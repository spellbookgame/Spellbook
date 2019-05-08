﻿using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class SummoningErrandQuest : Quest
{
    public SummoningErrandQuest(int turnStarted)
    {
        questName = "Summoning Errand Quest";
        questType = "Errand";
        questFlavor = "I need to summon somebody... I need a Mystic Translocator.";
        questTask = "Bring a Mystic Translocator to the Summoner Town.";
        questHint = "If the Capital and Forest don't have it, maybe a friend does.";

        startTurn = turnStarted;
        expiration = 20;

        spaceName = "town_summoner";
        itemName = "Mystic Translocator";

        rewards.Add("Rune", "Summoner A Rune");
        rewards.Add("Class Rune", "A Rune");

        questCompleted = false;
    }

    // return a string that contains the rewards of the quest
    public override string DisplayReward()
    {
        StringBuilder sb = new StringBuilder();

        foreach (KeyValuePair<string, string> kvp in rewards)
        {
            sb.Append(kvp.Value);
            sb.Append("\n");
        }

        return sb.ToString();
    }
}
