﻿using System.Collections.Generic;
using UnityEngine;

// spell for Chronomancy class
public class DoubleTime : Spell
{
    public DoubleTime()
    {
        iTier = 1;
        iManaCost = 4000;
        iCoolDown = 3;

        sSpellName = "Double Time";
        sSpellClass = "Chronomancer";
        sSpellInfo = "Take two turns this round. Can cast on an ally.";

        requiredGlyphs.Add("Time A Glyph", 1);
        requiredGlyphs.Add("Time B Glyph", 1);
        requiredGlyphs.Add("Arcane A Glyph", 1);
    }

    public override void SpellCast(SpellCaster player)
    {
        bool canCast = false;
        // checking if player can actually cast the spell
        foreach (KeyValuePair<string, int> kvp in requiredGlyphs)
        {
            if (player.glyphs[kvp.Key] >= 1)
                canCast = true;
        }
        if (canCast && player.iMana > iManaCost)
        {
            // subtract mana and glyph costs
            player.iMana -= iManaCost;
            foreach (KeyValuePair<string, int> kvp in requiredGlyphs)
                player.glyphs[kvp.Key] -= 1;

            PanelHolder.instance.displayNotify("You cast " + sSpellName, "");
        }
        else if (player.iMana < iManaCost)
        {
            PanelHolder.instance.displayNotify("Not enough mana!", "You don't have enough mana to cast this spell.");
        }
        else
        {
            PanelHolder.instance.displayNotify("Not enough glyphs!", "You don't have enough glyphs to cast this spell.");
        }
    }
}
