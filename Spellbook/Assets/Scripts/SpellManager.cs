﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/* 
 * namespace / HasChanged() written by Kiwasi Games
 * this script creates a builder that builds strings of item 
 * names as they are dropped into slots 
*/
public class SpellManager : MonoBehaviour, IHasChanged
{
    [SerializeField] Transform slots;
    [SerializeField] Text inventoryText;
    [SerializeField] GameObject arcaneSP;
    [SerializeField] GameObject alchemySP;
    [SerializeField] GameObject chronomancySP;
    [SerializeField] GameObject elementalSP;
    [SerializeField] GameObject tricksterSP;
    [SerializeField] GameObject summonerSP;
    [SerializeField] GameObject panel;

    private bool bSpellCreated;

    Player localPlayer;
    SlotHandler slotHandler;
    ArcaneBlast aBlast1 = new ArcaneBlast();
    
    void Start()
    {
        bSpellCreated = false;

        localPlayer = GameObject.FindGameObjectWithTag("LocalPlayer").GetComponent<Player>();
        slotHandler = GameObject.Find("Slot").GetComponent<SlotHandler>();
        HasChanged();

        int numSpellPieces = localPlayer.Spellcaster.numSpellPieces;
        // populate panel with spell pieces depending on how many player has
        if (panel != null)
        {
            while(numSpellPieces > 0)
            {
                // switch statement that instantiates spell pieces based on classType
                // eventually we want to change this to depending on what spell pieces player currently has
                switch(localPlayer.Spellcaster.classType)
                {
                    case "Arcanist":
                        GameObject g0 = (GameObject)Instantiate(arcaneSP);
                        g0.transform.SetParent(panel.transform, false);
                        numSpellPieces--;
                        break;
                    case "Alchemist":
                        GameObject g1 = (GameObject)Instantiate(alchemySP);
                        g1.transform.SetParent(panel.transform, false);
                        numSpellPieces--;
                        break;
                    case "Chronomancer":
                        GameObject g2 = (GameObject)Instantiate(chronomancySP);
                        g2.transform.SetParent(panel.transform, false);
                        numSpellPieces--;
                        break;
                    case "Elementalist":
                        GameObject g3 = (GameObject)Instantiate(elementalSP);
                        g3.transform.SetParent(panel.transform, false);
                        numSpellPieces--;
                        break;
                    case "Summoner":
                        GameObject g4 = (GameObject)Instantiate(summonerSP);
                        g4.transform.SetParent(panel.transform, false);
                        numSpellPieces--;
                        break;
                    case "Trickster":
                        GameObject g5 = (GameObject)Instantiate(tricksterSP);
                        g5.transform.SetParent(panel.transform, false);
                        numSpellPieces--;
                        break;
                    default:
                        break;
                }
            }
        }
    }

    void Update()
    {
        // ideally this shouldnt be in Update, because we dont want it to keep collecting spells
        // the if statement ensures that this only runs once; once a spell is created, it will stop checking
        if(bSpellCreated == false)
            CheckSpellSlots();
    }

    public void HasChanged()
    {
        System.Text.StringBuilder builder = new System.Text.StringBuilder();
        builder.Append(" - ");
        foreach(Transform slotTransform in slots)
        {
            // will return game object if there is one, or null if there isnt
            GameObject item = slotTransform.GetComponent<SlotHandler>().item;
            // if there is an item returned
            if(item)
            {
                // add item name to builder
                builder.Append(item.name);
                builder.Append(" - ");
            }
        }
        inventoryText.text = builder.ToString();
    }

    // eventually this function should be transferred into Chapter.cs, and called in Update(?)
    // this function only checks for arcane blast as of now
    public void CheckSpellSlots()
    {
        // TODO: make this more efficient?
        // right now, this method makes it so that order matters (left to right, top to bottom)
        int i = 0;
        foreach (Transform slotTransform in slots)
        {
            // if the slot isn't empty
            if(slotTransform.childCount > 0)
            {
                // if the slot's item name matches the required piece of the spell's name
                if (slotTransform.GetChild(0).name == aBlast1.requiredPieces[i])
                {
                    i++;
                    if (i >= 4)
                    {
                        // add spell to player's chapter
                        localPlayer.Spellcaster.CollectSpell(aBlast1, localPlayer.Spellcaster);
                        bSpellCreated = true;
                    }
                }
                else
                    break;
            }
        }
    }
}

namespace UnityEngine.EventSystems
{
    public interface IHasChanged: IEventSystemHandler
    {
        void HasChanged();
    }
}
