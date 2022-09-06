using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DecisionManager : MonoBehaviour
{
    /// this class manages all of the decisions that the player makes

    public List<DecisionClass> decisionQueue; // the upcoming decisions
    public List<DecisionClass> nightQueue; // the queue which features the results

    [SerializeField] DecisionClass currentDecision; // the selected class
    [SerializeField] int currentDecisionInt; // which one we are on

    // our morale and stability
    float morale, stability; // which of these will you choose?

    // our ui
    [SerializeField] Slider moraleSlider, stabilitySlider; // our slider displays
    [SerializeField] Text moraleDisplay, stabilityDisplay, resultsDisplay; // our text display for our values
    [SerializeField] Text decisionInfo, nightTimeInfo; // the current decision being displayed
    string resultsString;
    // our states
    [SerializeField] GameObject dayParent, nightParent, resultsParent; // to show and hide our UIs
    enum PlayState { day, night, results}
    PlayState state
    {
        get { return _state; }
        set { _state = value; SetState(); } // set the value, set the state
    }
    PlayState _state;

    // setup our instance
    public static DecisionManager instance;
    private void Awake()
    {
        instance = this;
    }


    // our modifier functions
    public void ChangeStats(bool Sink)
    {
        if (Sink) 
        { 
            // add this much to our morale and stability when we make the decision
            morale += currentDecision.decisionSinkMoraleMod;
            stability += currentDecision.decisionSinkStabilityMod;
        }        
        
        if (!Sink) 
        { 
            // add this much to our morale and stability when we make the decision
            morale += currentDecision.decisionSwimMoraleMod;
            stability += currentDecision.decisionSwimStabilityMod;
        }
    }

    private void Start()
    {
        // make sure we do not destroy on load
        // DontDestroyOnLoad(gameObject);

        // setup our first decision
        SetupDecisions();
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

        // nighttime
        nightTimeInfo.text = currentDecision.decisionInfo;

    }

    void SetupDecisions()
    {
        // spawn in all of our decisions and replace them with our instantiations
        for (int i = 0; i < decisionQueue.Count; i++)
        {
            decisionQueue[i] = Instantiate(decisionQueue[i]);
        }

        currentDecision = decisionQueue[0];
    }

    // click to sink
    public void ClickSink()
    {
        AdvanceDecision(DecisionClass.Choices.sink);
    }

    // click to swim
    public void ClickSwim()
    {
        AdvanceDecision(DecisionClass.Choices.swim);
    }

    // clicking at night
    public void ClickNight()
    {
        AdvanceDecision(DecisionClass.Choices.undetermined);
    }

    // advance our decision forward
    void AdvanceDecision(DecisionClass.Choices choice)
    {
        if (currentDecisionInt < decisionQueue.Count && choice != DecisionClass.Choices.undetermined)
        {
            state = PlayState.day;
            // get our current decision, and set it to the decision we made
            currentDecision.choice = choice;
            currentDecision.ChangeInfo();
            // then add that decision to the afternoon queue
            nightQueue.Add(currentDecision);

            // before advancing, apply the modifiers of our current
            if (currentDecision.choice == DecisionClass.Choices.sink)
            ChangeStats(true); // change our stats
            
            if (currentDecision.choice == DecisionClass.Choices.swim)
            ChangeStats(false); // change our stats

            currentDecisionInt++; // move to the next decision
            if (currentDecisionInt < decisionQueue.Count)
            {
                currentDecision = decisionQueue[currentDecisionInt];
            }

            Debug.Log("run choice");
        } 
        
        if (currentDecisionInt >= decisionQueue.Count && choice != DecisionClass.Choices.undetermined)
        {
            // set ourselves to night
            state = PlayState.night;
            currentDecisionInt = 0; // reset our decision counter
            currentDecision = nightQueue[0]; // reset our current decision
            Debug.Log("run final choice");
        }

        // if we are at night then advance through our choices
        if (state == PlayState.night && choice == DecisionClass.Choices.undetermined)
        {

            Debug.Log("run night");
            // check our current decision and see if it was a sink or swim result, and then apply that to our score
            if (currentDecision.choice == DecisionClass.Choices.swim)
            {
                morale += currentDecision.swimMoralMod;
                stability += currentDecision.swimStabilityMod;
            }

            // sink choice
            if (currentDecision.choice == DecisionClass.Choices.sink)
            {
                morale += currentDecision.sinkMoralMod;
                stability += currentDecision.sinkStabilityMod;
            }

            // then advance
            currentDecisionInt++;
            // change if we can
            if (currentDecisionInt < nightQueue.Count)
            {
                currentDecision = nightQueue[currentDecisionInt];
            } else if (currentDecisionInt >= nightQueue.Count)
            {
                state = PlayState.results;
            }

        }
    }

    // when we change our state, change our state
    void SetState()
    {
        if (state == PlayState.day)
        {
            dayParent.SetActive(true);
            nightParent.SetActive(false);
            resultsParent.SetActive(false);
        }

        if (state == PlayState.night)
        {
            nightParent.SetActive(true);
            dayParent.SetActive(false);
            resultsParent.SetActive(false);
        }

        if (state == PlayState.results)
        {
            ShowResults();
            nightParent.SetActive(false);
            dayParent.SetActive(false);
            resultsParent.SetActive(true);
        }

    }

    void ShowResults()
    {


        // morale 5
        if (morale == 5)
        {
            resultsString = "You were such a wonderful ruler that you managed to keep everyone happy. Floterra loves you. All hail the monarch!";
        }

        if (morale == -5 && (stability != 5 || stability != -5))
        {
            resultsString = "You were... terrible. A demon. With an iron fist you crushed your opposition and destroyed Floterra.";
        }

        if (stability == 5)
        {
            resultsString = "You have managed to stabilize Floterra. The kingdom will float on into glorious new waters under your watchful eye.";
        }     
        
        if (stability == -5)
        {
            resultsString = "Under your rule Floterra has collapsed into your control. Through careful destabilization you were able to overthrow the powers standing in your way, and create a wave of chaos.";
        }

        resultsDisplay.text = resultsString;
    }
}
