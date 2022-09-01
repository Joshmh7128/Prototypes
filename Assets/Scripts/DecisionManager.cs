using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DecisionManager : MonoBehaviour
{
    /// this class manages all of the decisions that the player makes

    public List<DecisionClass> decisionQueue; // the upcoming decisions
    public List<DecisionClass> afternoonQueue; // the queue which features the results

    DecisionClass currentDecision; // the selected class
    int currentDecisionInt; // which one we are on

    // our morale and virtue
    float morale, virtue; // which of these will you choose?

    // our ui
    [SerializeField] Slider moraleSlider, virtueSlider; // our slider displays
    [SerializeField] Text moraleDisplay, virtueDisplay; // our text display for our values
    [SerializeField] Text decisionInfo; // the current decision being displayed
    // our modifier functions
    public void ChangeMorale(float amount)
    {
        // add this much to our morale
        morale += amount;
    }

    private void Start()
    {
        // setup our first decision
        SetupDecision();
    }

    private void FixedUpdate()
    {
        // process our UI
        ProcessUI();
    }

    // update our UI
    void ProcessUI()
    {
        // setup our morale and virtue sliders to display our amounts properly
        moraleSlider.value = morale + 5; // when morale is at 0 we want it to be in the center
        virtueSlider.value = virtue + 5; // same with this
        // text
        moraleDisplay.text = "Morale: " + morale.ToString(); virtueDisplay.text = "Virtue: " + virtue.ToString(); // set the text

        // setup the informational text
        decisionInfo.text = currentDecision.decisionInfo;
    }

    void SetupDecision()
    {
        currentDecision = decisionQueue[0];
    }

}
