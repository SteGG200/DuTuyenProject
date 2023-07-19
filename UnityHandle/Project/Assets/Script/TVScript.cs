using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;
using UnityEngine;
using UnityEngine.Video;
using TMPro;

class DialogueState
{
    string text, flag = "None";
    DialogueState parentState;
    Dictionary<KeyCode, DialogueState> nextState = new Dictionary<KeyCode, DialogueState>();

    public void setText(string newText)
    {
        text = newText;
    }

    public void setFlag(string newFlag)
    {
        flag = newFlag;
    }

    public string getText()
    {
        return text;
    }

    public string getFlag()
    {
        return flag;
    }

    public void setNextState(KeyCode key, string nextText, string nextFlag)
    {
        nextState.Add(key, new DialogueState());
        nextState[key].parentState = this;
        nextState[key].setText(nextText);
        nextState[key].setFlag(nextFlag);
    }
    
    public DialogueState getNextState(KeyCode key)
    {
        if (nextState.ContainsKey(key))
            return nextState[key];
        return this;
    }

    public DialogueState getParentState()
    {
        return parentState;
    }

    public void setChildrenStates(List<Tuple<KeyCode, string, string>> childrenStates)
    {
        DialogueState tempState = this;
        foreach (Tuple<KeyCode, string, string> childrenState in childrenStates)
        {
            KeyCode key = childrenState.Item1;
            string nextText = childrenState.Item2;
            string nextFlag = childrenState.Item3;

            tempState.setNextState(key, nextText, nextFlag);
            tempState = tempState.getNextState(key);
        }
    }
}

public class TVScript : MonoBehaviour
{
	public TextMeshPro textMeshPro;
    public GameObject player;
    public GameObject cinemaPlane;

    private DialogueState rootState;
    private DialogueState state;

    private void Start()
    {
        Renderer renderer = cinemaPlane.GetComponent<Renderer>();

        for (int i = 0; i < renderer.materials.Length; i++)
        {
            renderer.materials[i].color = Color.red;
        }

        // Player
        player = GameObject.FindGameObjectsWithTag("Player")[0];

        // Dialogue
        rootState = new DialogueState();
        rootState.setText("... (No way I'm telling them that they can press B to interact with me...)");

        var childrenStates = new List<Tuple<KeyCode, string, string>>();

        childrenStates.Add(Tuple.Create(KeyCode.B, "Did you just press B? Ah, so you do know of my Initial...", "None"));
        childrenStates.Add(Tuple.Create(KeyCode.B, "Well then, let me properly introduce myself.", "None"));
        childrenStates.Add(Tuple.Create(KeyCode.B, "I am Benq. The one and only Benq. You must have heard of my name on Codeforces, ahahah!", "None"));
        childrenStates.Add(Tuple.Create(KeyCode.B, "That is, until I got sealed here, by none other than...", "None"));
        childrenStates.Add(Tuple.Create(KeyCode.B, "Anyway. You tried to interact with me, so I guess...", "None"));
        childrenStates.Add(Tuple.Create(KeyCode.B, "You expected me to play a video or something, did you?", "None"));
        childrenStates.Add(Tuple.Create(KeyCode.B, "Well, you see... They sealed me here, for one purpose.", "None"));
        childrenStates.Add(Tuple.Create(KeyCode.B, "It is my duty, as a guardian, to hide that Thing away from prying eyes.", "None"));
        childrenStates.Add(Tuple.Create(KeyCode.B, "Have you heard of it?", "None"));
        childrenStates.Add(Tuple.Create(KeyCode.B, "The Thing. The forbidden knowledge. The embodiment of horror itself.", "None"));
        childrenStates.Add(Tuple.Create(KeyCode.B, "The maw from another world, that would devour this reality, should we let it roam free.", "None"));
        childrenStates.Add(Tuple.Create(KeyCode.B, "Many has seen its true form. None has been able to go back to the same self they were before.", "None"));
        childrenStates.Add(Tuple.Create(KeyCode.B, "So please just step back, mortal. Give yourself over to your curiousity, and you will regret it.", "None"));
        childrenStates.Add(Tuple.Create(KeyCode.B, "You will pay for it, just like the other ones. You cannot escape that fate.", "None"));
        childrenStates.Add(Tuple.Create(KeyCode.B, "I could not save those unfortunate souls, but at least I can stop you.", "None"));
        childrenStates.Add(Tuple.Create(KeyCode.B, "Believe me, when I say that you should not go any further.", "None"));
        childrenStates.Add(Tuple.Create(KeyCode.B, "Ignorance is bliss, mortal.", "None"));
        childrenStates.Add(Tuple.Create(KeyCode.B, "This is your last warning. Step back.", "None"));
        childrenStates.Add(Tuple.Create(KeyCode.B, "Do you not hear me?", "None"));
        childrenStates.Add(Tuple.Create(KeyCode.B, "What exactly is it, that you think is worth all of this?", "None"));
        childrenStates.Add(Tuple.Create(KeyCode.B, "You do not understand what you are getting yourself into.", "None"));
        childrenStates.Add(Tuple.Create(KeyCode.B, "You are not changing my mind. Not even if you keep trying to press random keys.", "None"));
        childrenStates.Add(Tuple.Create(KeyCode.E, "Did you just press E? It can't be you know that my seal weakens, the more you spell my name...", "None"));
        childrenStates.Add(Tuple.Create(KeyCode.N, "Did you just press N? No no no, trust me, you will regret this, you WILL regret this.", "None"));
        childrenStates.Add(Tuple.Create(KeyCode.Q, "Did you just press Q? The seal has been broken... Do not hate me for this.", "PlayVideo"));

        rootState.setChildrenStates(childrenStates);

        state = rootState;
        
        textMeshPro.text = state.getText();
    }

    private void Update()
    {
        if (state.getFlag() == "PlayVideo")
        {
            cinemaPlane.GetComponent<VideoPlayer>().Play();
            Debug.Log("Played video");
        }

        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        if (distanceToPlayer > 5)
        {
            textMeshPro.text = "...";
            return;
        }

        textMeshPro.text = state.getText();

        if (!Input.anyKeyDown) return;

        if (!Regex.IsMatch(Input.inputString, @"^[a-zA-Z]+$")) return;

        KeyCode key = (KeyCode)System.Enum.Parse(typeof(KeyCode), Input.inputString, true);
        state = state.getNextState(key);
        textMeshPro.text = state.getText();
    }
}
