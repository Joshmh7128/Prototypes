using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DecisionClass : MonoBehaviour
{

    // our information for decision making
    public string decisionInfo;
    public float decisionMoraleMod, decisionStabilityMod; // our primary morale and stability mods
    // afternoon
    public string sinkInfo, swimInfo;
    public float sinkMoralMod, sinkStabilityMod; // for our afternoon, our moral and stability modifications
    public float swimMoralMod, swimStabilityMod; // for our afternoon, our moral and stability mods
    // for the afternoon phase did we choose to sink or swim this decision?
    public enum Choices { undetermined, sink, swim }
    public Choices choice;


}
