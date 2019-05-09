﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalMirror : ItemObject
{
    public CrystalMirror()
    {
        name = "Crystal Mirror";
        sprite = Resources.Load<Sprite>("Art Assets/Items and Currency/Crystal Mirror");
        tier = 1;
        buyPrice = 3400;
        sellPrice = 1020;
        flavorDescription = "This hand mirror has been enchanted very heavily. It reflects your face very clearly … maybe a little too clearly.";
        mechanicsDescription = "Replace the runes in each town slot for runes from the high tier deck.";
    }

    public override void UseItem(SpellCaster player)
    {
        player.RemoveFromInventory(this);
        player.itemsUsedThisTurn++;

        PanelHolder.instance.displayNotify("Crystal Mirror", "Discard all runes on the board and replace them with runes from the high tier deck.", "InventoryScene");
    }
}