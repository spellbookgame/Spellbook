﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// script from Kiwasi Games
public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static GameObject itemToDrag;
    public GameObject itemBeingDragged;
    public Transform originalParent;

    private Vector3 startPos;
    private Transform startParent;

    Player localPlayer;
    SpellManager spellManager;

    void Start()
    {
        originalParent = transform.parent;

        localPlayer = GameObject.FindGameObjectWithTag("LocalPlayer").GetComponent<Player>();
        spellManager = GameObject.Find("Canvas").GetComponent<SpellManager>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // play grab sound
        SoundManager.instance.PlaySingle(spellManager.grabspellpiece);

        // itemToDrag is the game object that this script is on
        itemToDrag = gameObject;
        startPos = transform.position;
        startParent = transform.parent;

        // set the parent to canvas so the spell piece slot will no longer have a child
        transform.SetParent(GameObject.Find("Canvas").transform);

        // if player has enough spell pieces and the slot has less than 1 child in it
        if (localPlayer.Spellcaster.spellPieces[itemToDrag.name] > 0 && originalParent.childCount < 1)
        {
            // instantiate prefab of whatever was dragged, and omit (clone) from its name
            GameObject clone = Instantiate((GameObject)Resources.Load("Spell Pieces/" + itemToDrag.name), originalParent);
            clone.name = itemToDrag.name;

            clone.AddComponent<DragHandler>();

            // subtract 1 from the player's inventory whenever the spell piece is used
            localPlayer.Spellcaster.spellPieces[itemToDrag.name] -= 1;

            // set the instantiated clone's text to the number player has
            clone.transform.GetChild(0).GetComponent<Text>().text = localPlayer.Spellcaster.spellPieces[clone.name].ToString();
        }
        
        // if dragging item has a text component in its first child, then destroy that child
        if(itemToDrag.transform.GetChild(0).GetComponent<Text>())
        {
            Destroy(itemToDrag.transform.GetChild(0).gameObject);
        }

        // allows to pass events through the item being dragged
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // setting transform position to current mouse position
        transform.position = Input.mousePosition;
        itemBeingDragged = itemToDrag;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        itemToDrag = null;
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        // if item's parent is where it started from onBeginDrag() and drag ended without changing parent, snap it back
        if (transform.parent == startParent || transform.parent.tag != "Slot")
        {
            transform.position = startPos;
        }
    }
}
