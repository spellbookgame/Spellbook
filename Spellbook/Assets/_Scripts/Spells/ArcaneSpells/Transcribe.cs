﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// spell for Arcanist class
public class Transcribe : Spell
{
    public Transcribe()
    {
        iTier = 1;
        iManaCost = 2800;

        combatSpell = false;

        sSpellName = "Transcribe";
        sSpellClass = "Arcanist";
        sSpellInfo = "Discard your rune hand and draw new ones from the top tier deck.";

        requiredRunes.Add("Arcanist A Rune", 1);
        requiredRunes.Add("Illusionist A Rune", 1);
        requiredRunes.Add("Illusionist B Rune", 1);
    }

    public override void SpellCast(SpellCaster player)
    {
        // cast spell for free if Umbra's Eclipse is active
        if (SpellTracker.instance.CheckUmbra())
        {
            PanelHolder.instance.displayNotify(sSpellName, "Discard your rune hand and draw new ones from the top tier deck.", "MainPlayerScene");

            player.numSpellsCastThisTurn++;
            SpellTracker.instance.lastSpellCasted = this;
        }
        else if (player.iMana < iManaCost)
        {
            PanelHolder.instance.displayNotify("Not enough Mana!", "You do not have enough mana to cast this spell.", "OK");
        }
        else
        {
            // subtract mana 
            player.iMana -= iManaCost;

            PanelHolder.instance.displayNotify(sSpellName, "Discard your rune hand and draw new ones from the top tier deck.", "MainPlayerScene");

            player.numSpellsCastThisTurn++;
            SpellTracker.instance.lastSpellCasted = this;
        }
    }
}
