﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Kiwasi Games, Grace Ko
/// script created to handle dragging of glyphs
/// SpellCreateScene, SpellCastScene
/// </summary>
public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static GameObject itemToDrag;
    public GameObject itemBeingDragged;
    public Transform originalParent;

    private Vector3 startPos;
    private Transform startParent;

    Player localPlayer;

    void Start()
    {
        originalParent = transform.parent;

        localPlayer = GameObject.FindGameObjectWithTag("LocalPlayer").GetComponent<Player>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {

        // itemToDrag is the game object that this script is on
        itemToDrag = gameObject;
        startPos = transform.position;
        startParent = transform.parent;

        // set the anchors of the glyph so it'll follow mouse correctly
        gameObject.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
        gameObject.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);

        // set the parent to canvas so the glyph slot will no longer have a child
        transform.SetParent(GameObject.Find("Canvas").transform);

        // if player has enough glyphs and the slot has less than 1 child in it
        if (localPlayer.Spellcaster.glyphs[itemToDrag.name] > 0 && originalParent.childCount < 1)
        {
            // instantiate prefab of whatever was dragged, and omit (clone) from its name
            GameObject clone = Instantiate((GameObject)Resources.Load("Glyphs/" + itemToDrag.name), originalParent);
            clone.name = itemToDrag.name;

            clone.AddComponent<DragHandler>();

            // subtract 1 from the player's inventory whenever the glyph is used
            localPlayer.Spellcaster.glyphs[itemToDrag.name] -= 1;

            // set the instantiated clone's text to the number player has
            clone.transform.GetChild(0).GetComponent<Text>().text = localPlayer.Spellcaster.glyphs[clone.name].ToString();
        }
        
        // if dragging item has a child, then destroy that child (the text component)
        if(itemToDrag.transform.childCount > 0)
        {
            Destroy(itemToDrag.transform.GetChild(0).gameObject);
        }

        // allows to pass events through the item being dragged
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // setting transform position to current mouse position
        Vector3 screenpoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
        screenpoint.z = 10.0f;
        itemToDrag.transform.position = Camera.main.ScreenToWorldPoint(screenpoint);

        itemBeingDragged = itemToDrag;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        itemToDrag = null;
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        // if item's parent is where it started from onBeginDrag() and drag ended without changing parent, snap it back
        if (transform.parent == startParent || !transform.parent.tag.Equals("Slot"))
        {
            transform.position = startPos;
            Destroy(itemBeingDragged.gameObject);
            localPlayer.Spellcaster.glyphs[itemBeingDragged.name] += 1;
            // update the glyph info text
            ExecuteEvents.ExecuteHierarchy<IHasChanged>(gameObject, null, (x, y) => x.HasChanged());
        }
    }
}
