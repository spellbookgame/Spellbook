﻿using System.Collections.Generic;
using UnityEngine;

public class ItemList : MonoBehaviour
{
    public List<ItemObject> listOfItems;

    public List<ItemObject> tier1Items;
    public List<ItemObject> tier2Items;
    public List<ItemObject> tier3Items;

    void Awake()
    {
        listOfItems = new List<ItemObject>
        {
            new InfusedSapphire(),
            new AbyssalOre(),
            new GlowingMushroom(),
            new MimeticVellum(),
            new CrystalMirror(),
            new MysticTranslocator(),
            new AromaticTeaLeaves(),
            new OpalAmmonite(),
            new WaxCandle(),
            new HollowCabochon(),
            new RiftTalisman()
        };

        tier1Items = new List<ItemObject>()
        {
            new CrystalMirror(),
            new RiftTalisman(),
            new MimeticVellum()
        };

        tier2Items = new List<ItemObject>()
        {
            new InfusedSapphire(),
            new AbyssalOre(),
            new HollowCabochon(),
            new MysticTranslocator()
        };

        tier3Items = new List<ItemObject>()
        {
            new GlowingMushroom(),
            new WaxCandle(),
            new AromaticTeaLeaves(),
            new OpalAmmonite()
        };
    }

    public ItemObject GetItemFromName(string itemName)
    {
        ItemObject item = new ItemObject();
        switch(itemName)
        {
            case "Infused Sapphire":
                item = new InfusedSapphire();
                break;
            case "Abyssal Ore":
                item = new AbyssalOre();
                break;
            case "Glowing Mushroom":
                item = new GlowingMushroom();
                break;
            case "Mimetic Vellum":
                item = new MimeticVellum();
                break;
            case "Crystal Mirror":
                item = new CrystalMirror();
                break;
            case "Mystic Translocator":
                item = new MysticTranslocator();
                break;
            case "Aromatic Tea Leaves":
                item = new AromaticTeaLeaves();
                break;
            case "Opal Ammonite":
                item = new OpalAmmonite();
                break;
            case "Wax Candle":
                item = new WaxCandle();
                break;
            case "Hollow Cabochon":
                item = new HollowCabochon();
                break;
            case "Rift Talisman":
                item = new RiftTalisman();
                break;
        }
        return item;
    }
}