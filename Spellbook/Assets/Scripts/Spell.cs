﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spell
{
    public int iTier;
    public int iManaCost;
    public int iCoolDown;

    public string sSpellName;
    public string sSpellClass;
    public string sSpellInfo;

    public bool currentlyActive;

    public Dictionary<string, int> requiredGlyphs;

    // CTOR
    public Spell()
    {
        requiredGlyphs = new Dictionary<string, int>();
    }

    // Virtual functions
    public abstract void SpellCast(SpellCaster player);
}
