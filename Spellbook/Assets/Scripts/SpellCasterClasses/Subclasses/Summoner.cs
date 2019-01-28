﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
  This class inherits from SpellCaster,
  "because a Summoner is a type of spellcaster
  it should have spellcaster characteristics".

  Summoner also has it's own unique features.
     */
public class Summoner : SpellCaster
{
    public Summoner()
    {
        
        //You can override variables in here.
        //Example:
        // numSpellPieces = 5;
        classType = "Summoner";

        // creating the class-specific chapter
        chapter = new Chapter(classType);
    }

    public override void SpellCast()
    {
        Debug.Log("SpellCast not implemented");
    }
}