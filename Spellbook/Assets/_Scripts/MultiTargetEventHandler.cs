﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Vuforia;

// for scanning multiple (4) images
public class MultiTargetEventHandler : MonoBehaviour, ITrackableEventHandler
{
    protected TrackableBehaviour mTrackableBehaviour;
    protected TrackableBehaviour.Status m_PreviousStatus;
    protected TrackableBehaviour.Status m_NewStatus;

    private bool isTracked = false;
    private Dictionary<string, int> targets;

    Player localPlayer;

    void Start()
    {
        localPlayer = GameObject.FindGameObjectWithTag("LocalPlayer").GetComponent<Player>();
        targets = new Dictionary<string, int>();

        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
    }

    void OnDestroy()
    {
        if (mTrackableBehaviour)
            mTrackableBehaviour.UnregisterTrackableEventHandler(this);
    }

    public void OnTrackableStateChanged(
        TrackableBehaviour.Status previousStatus,
        TrackableBehaviour.Status newStatus)
    {
        m_PreviousStatus = previousStatus;
        m_NewStatus = newStatus;

        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            OnTrackingFound();
        }
        else if (previousStatus == TrackableBehaviour.Status.TRACKED &&
                 newStatus == TrackableBehaviour.Status.NO_POSE)
        {
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
            OnTrackingLost();
        }
        else
        {
            // For combo of previousStatus=UNKNOWN + newStatus=UNKNOWN|NOT_FOUND
            // Vuforia is starting, but tracking has not been lost or found yet
            // Call OnTrackingLost() to hide the augmentations
            OnTrackingLost();
        }
    }
    protected virtual void OnTrackingFound()
    {
        isTracked = true;
        CheckTargetsTracked();
    }

    protected virtual void OnTrackingLost()
    {
        // when track is lost, reset bool and remove from dictionary
        isTracked = false;
        if (targets.ContainsKey(this.name))
        {
            targets.Remove(this.name);
        }
    }

    // TEST
    private void ScanItem()
    {
        // cast the spell
        switch (localPlayer.Spellcaster.classType)
        {
            // collect spell based on class
            case "Alchemist":
                localPlayer.Spellcaster.CollectSpell(localPlayer.Spellcaster.chapter.spellsAllowed[3]);
                break;
            case "Arcanist":
                localPlayer.Spellcaster.CollectSpell(localPlayer.Spellcaster.chapter.spellsAllowed[3]);
                break;
            case "Chronomancer":
                localPlayer.Spellcaster.CollectSpell(localPlayer.Spellcaster.chapter.spellsAllowed[2]);
                break;
            case "Elementalist":
                localPlayer.Spellcaster.CollectSpell(localPlayer.Spellcaster.chapter.spellsAllowed[2]);
                break;
            case "Summoner":
                localPlayer.Spellcaster.CollectSpell(localPlayer.Spellcaster.chapter.spellsAllowed[2]);
                break;
            case "Trickster":
                localPlayer.Spellcaster.CollectSpell(localPlayer.Spellcaster.chapter.spellsAllowed[2]);
                break;
        }
    }

    // if 4 targets are detected, scan item
    // TO DO: currently not checking duplicates
    // solution: multiple of same target in the scene?
    private void CheckTargetsTracked()
    {
        int targetsTracked = 0;
        foreach(MultiTargetEventHandler m in FindObjectsOfType<MultiTargetEventHandler>())
        {
            if (!m.isTracked)
            {
                // do nothing
            }
            else
            {
                if (targets.ContainsKey(m.name))
                {
                    targets[m.name] += 1;
                }
                else
                    targets.Add(m.name, 1);

                ++targetsTracked;
                Debug.Log("target tracked: " + mTrackableBehaviour.TrackableName);
            }
        }
        // if 4 targets were tracked, start scanning
        if (targetsTracked >= 4)
        {
            Debug.Log("4 items tracked!");
            CompareSpells();
            // ScanItem();
        }
    }

    // compares targets dictionary to spell's required glyphs dictionary
    private void CompareSpells()
    {
        bool isEqual = false;

        Dictionary<string, int> d1 = targets;
        for(int i = 0; i <= localPlayer.Spellcaster.chapter.spellsAllowed.Count; i++)
        {
            Dictionary<string, int> d2 = localPlayer.Spellcaster.chapter.spellsAllowed[i].requiredGlyphs;

            // tier 3 spell: only needs to check if d1 contains the required glyph
            if(localPlayer.Spellcaster.chapter.spellsAllowed[i].iTier == 3)
            {
                if(d2.Keys.All(k => d1.ContainsKey(k)))
                {
                    Debug.Log("tier 3 spell collected");
                    localPlayer.Spellcaster.CollectSpell(localPlayer.Spellcaster.chapter.spellsAllowed[i]);
                    break;
                }
            }
            // tier 2 and 1 spells
            else
            {
                foreach(KeyValuePair<string, int> kvp in d2)
                {
                    if (d1.ContainsKey(kvp.Key))
                    {
                        isEqual = true;
                    }
                    else
                    {
                        isEqual = false;
                        break;
                    }
                }
                if(isEqual)
                {
                    Debug.Log("Tier 1 or 2 spell collected");
                    localPlayer.Spellcaster.CollectSpell(localPlayer.Spellcaster.chapter.spellsAllowed[i]);
                }
            }
        }
    }
}