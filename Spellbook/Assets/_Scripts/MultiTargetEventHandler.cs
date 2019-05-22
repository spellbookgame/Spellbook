﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Vuforia;

/// <summary>
/// Grace Ko
/// Scans multiple target images
/// Put this script on an image target 
/// </summary>
public class MultiTargetEventHandler : MonoBehaviour, ITrackableEventHandler
{
    protected TrackableBehaviour mTrackableBehaviour;
    protected TrackableBehaviour.Status m_PreviousStatus;
    protected TrackableBehaviour.Status m_NewStatus;

    private bool isTracked = false;
    private Dictionary<string, int> targets;
    private int targetsTracked;

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
            Debug.Log(mTrackableBehaviour.TrackableName + " found");
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
        targets.Clear();
    }

    // if 4 targets are detected, scan item
    private void CheckTargetsTracked()
    {
        // reset values at the beginning, so it only reads accurately when there are 4 targets tracked
        targetsTracked = 0;
        targets.Clear();

        foreach (MultiTargetEventHandler m in FindObjectsOfType<MultiTargetEventHandler>())
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
            }
        }
        // if 4 targets were tracked, start scanning
        if (targetsTracked >= 4)
        {
            CompareSpells();
        }
    }

    // compares targets dictionary to spell's required runes dictionary
    private void CompareSpells()
    {
        bool isEqual = false;

        Dictionary<string, int> d1 = targets;
        for (int i = 0; i < localPlayer.Spellcaster.chapter.spellsAllowed.Count; ++i)
        {
            Dictionary<string, int> d2 = localPlayer.Spellcaster.chapter.spellsAllowed[i].requiredRunes;

            // tier 2 and 1 spells
            if (localPlayer.Spellcaster.chapter.spellsAllowed[i].iTier == 2 || localPlayer.Spellcaster.chapter.spellsAllowed[i].iTier == 1)
            {
                foreach (KeyValuePair<string, int> kvp in d2)
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
                if (isEqual)
                {
                    SceneManager.LoadScene("MainPlayerScene");
                    localPlayer.Spellcaster.CollectSpell(localPlayer.Spellcaster.chapter.spellsAllowed[i]);
                    break;
                }
            }
            // tier 3 spell: only needs to check if d1 contains the required rune
            else if (localPlayer.Spellcaster.chapter.spellsAllowed[i].iTier == 3)
            {
                var first = d2.First();     // can use First() here because tier 3 requiredRune will only have 1 entry
                if (d1.ContainsKey(first.Key))
                {
                    isEqual = true;
                    SceneManager.LoadScene("MainPlayerScene");
                    localPlayer.Spellcaster.CollectSpell(localPlayer.Spellcaster.chapter.spellsAllowed[i]);
                    break;
                }
                else
                    isEqual = false;
            }
        }
        if(!isEqual)
        {
            SceneManager.LoadScene("MainPlayerScene");
            PanelHolder.instance.displayNotify("No Spell!", "No spell was found...", "OK");
        }
    }
}