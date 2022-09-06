using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DecisionClass : MonoBehaviour
{

    // our information for decision making
    [HideInInspector] public string decisionInfo;
    public string dayInfo; // our starter info
    public float decisionSinkMoraleMod, decisionSinkStabilityMod; // our primary morale and stability mods
    public float decisionSwimMoraleMod, decisionSwimStabilityMod; // our primary morale and stability mods
    // afternoon
    public string sinkInfo, swimInfo;
    public float sinkMoralMod, sinkStabilityMod; // for our afternoon, our moral and stability modifications
    public float swimMoralMod, swimStabilityMod; // for our afternoon, our moral and stability mods
    // for the afternoon phase did we choose to sink or swim this decision?
    public enum Choices { undetermined, sink, swim }
    public Choices choice;

    private void Start()
    {
        ChangeInfo();
    }

    public void ChangeInfo()
    {
        if (choice == Choices.undetermined)
        {
            decisionInfo = dayInfo;
        }

        // setup our decision info based on our choice
        if (choice == Choices.sink)
        {
            decisionInfo = sinkInfo;
        }

        if (choice == Choices.swim)
        {
            decisionInfo = swimInfo;
        }
    }

}
