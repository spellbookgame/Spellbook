﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// spell for Chronomancy class
public class Echo : Spell
{
    public Echo()
    {
        iTier = 3;
        iManaCost = 600;

        combatSpell = false;

        sSpellName = "Echo";
        sSpellClass = "Chronomancer";
        sSpellInfo = "If you are not satisfied with your first roll, you may roll again. Can cast on an ally.";

        requiredRunes.Add("Chronomancer C Rune", 1);
    }

    public override void SpellCast(SpellCaster player)
    {
        // cast spell for free if Umbra's Eclipse is active
        if (SpellTracker.instance.CheckUmbra())
        {
            PanelHolder.instance.displayNotify(sSpellName, "Next time you roll, you may roll again.", "MainPlayerScene");
            player.activeSpells.Add(this);

            player.numSpellsCastThisTurn++;
            SpellTracker.instance.lastSpellCasted = this;
        }
        else if (player.iMana < iManaCost)
        {
            PanelHolder.instance.displayNotify("Not enough Mana!", "You do not have enough mana to cast this spell.", "OK");
        }
        else
        {
            // subtract mana and glyph costs
            player.iMana -= iManaCost;

            PanelHolder.instance.displayNotify(sSpellName, "Next time you roll, you may roll again.", "MainPlayerScene");
            player.activeSpells.Add(this);

            player.numSpellsCastThisTurn++;
            SpellTracker.instance.lastSpellCasted = this;
        }
    }
}
