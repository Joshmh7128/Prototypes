using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DecisionManager : MonoBehaviour
{
    /// this class manages all of the decisions that the player makes

    public List<DecisionClass> decisionQueue; // the upcoming decisions
    public List<DecisionClass> afternoonQueue; // the queue which features the results

    DecisionClass currentDecision; // the selected class
    int currentDecisionInt; // which one we are on


    // our morale and stability
    float morale, stability; // which of these will you choose?

    // our ui
    [SerializeField] Slider moraleSlider, stabilitySlider; // our slider displays
    [SerializeField] Text moraleDisplay, stabilityDisplay; // our text display for our values
    [SerializeField] Text decisionInfo; // the current decision being displayed
    // our modifier functions
    public void ChangeStats()
    {
        // add this much to our morale and stability when we make the decision
        morale += currentDecision.decisionMoraleMod;
        stability += currentDecision.decisionStabilityMod;
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
        // setup our morale and stability sliders to display our amounts properly
        moraleSlider.value = (morale + 5); // when morale is at 0 we want it to be in the center, our sliders have a scale from 0 to 10
        stabilitySlider.value = (stability + 5); // same with this
        // text
        moraleDisplay.text = "Morale: " + (morale); stabilityDisplay.text = "stability: " + (stability); // set the text and adjust the values

        // setup the informational text
        decisionInfo.text = currentDecision.decisionInfo;
    }

    void SetupDecision()
    {
        currentDecision = decisionQueue[0];
    }

    public void ClickSink()
    {
        AdvanceDecision(DecisionClass.Choices.sink);
    }

    public void ClickSwim()
    {
        AdvanceDecision(DecisionClass.Choices.swim);
    }

    void AdvanceDecision(DecisionClass.Choices choice)
    {
        if (currentDecisionInt + 1 <= decisionQueue.Count)
        {
            // get our current decision, and set it to the decision we made
            currentDecision.choice = choice;
            // then add that decision to the afternoon queue
            afternoonQueue.Add(currentDecision);

            // before advancing, apply the modifiers of our current decision
            ChangeStats(); // change our stats
            currentDecisionInt++; // move to the next decision
            currentDecision = decisionQueue[currentDecisionInt];
        } else
        {
            // advance to next scene
            Debug.LogError("Not implemented!");
        }
    }

}
