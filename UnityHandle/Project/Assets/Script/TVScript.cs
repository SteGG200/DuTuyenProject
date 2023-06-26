using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;
using UnityEngine;
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

    public string getText()
    {
        return text;
    }

    public void setNextState(KeyCode key, string nextText)
    {
        nextState.Add(key, new DialogueState());
        nextState[key].parentState = this;
        nextState[key].setText(nextText);
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

    public void setChildrenStates(List<Tuple<KeyCode, string>> childrenStates)
    {
        DialogueState tempState = this;
        foreach (Tuple<KeyCode, string> childrenState in childrenStates)
        {
            KeyCode key = childrenState.Item1;
            string nextText = childrenState.Item2;

            tempState.setNextState(key, nextText);
            tempState = tempState.getNextState(key);
        }
    }

    public string getFlag()
    {
        return flag;
    }
}

public class TVScript : MonoBehaviour
{
	// Array to store the original colors of the materials
    private Color[] originalColors;

    public TextMeshPro textMeshPro;
    public GameObject player;

    private DialogueState rootState;
    private DialogueState state;

    private void Start()
    {
        // Store the original colors of all materials
        Renderer renderer = GetComponent<Renderer>();
        originalColors = new Color[renderer.materials.Length];

        for (int i = 0; i < renderer.materials.Length; i++)
        {
            originalColors[i] = renderer.materials[i].color;
        }

        // Player
        player = GameObject.FindGameObjectsWithTag("Player")[0];

        // Dialogue
        rootState = new DialogueState();
        rootState.setText("...");

        var childrenStates = new List<Tuple<KeyCode, string>>();

        childrenStates.Add(Tuple.Create(KeyCode.B, "Did you just press B? Ah, so you do know of my Initial..."));
        childrenStates.Add(Tuple.Create(KeyCode.B, "Well then, let me properly introduce myself."));
        childrenStates.Add(Tuple.Create(KeyCode.B, "I am Benq. The one and only Benq. You must have heard of my name on Codeforces, ahahah!"));
        childrenStates.Add(Tuple.Create(KeyCode.B, "That is, until I got sealed here, by none other than..."));
        childrenStates.Add(Tuple.Create(KeyCode.B, "Anyway. You tried to interact with me, so I guess..."));
        childrenStates.Add(Tuple.Create(KeyCode.B, "You expected me to play a video or something, did you?"));
        childrenStates.Add(Tuple.Create(KeyCode.B, "Well, you see... They sealed me here, for one purpose."));
        childrenStates.Add(Tuple.Create(KeyCode.B, "It is my duty, as a guardian, to hide that Thing away from prying eyes."));
        childrenStates.Add(Tuple.Create(KeyCode.B, "Have you heard of it?"));
        childrenStates.Add(Tuple.Create(KeyCode.B, "The Thing. The forbidden knowledge. The embodiment of horror itself."));
        childrenStates.Add(Tuple.Create(KeyCode.B, "The maw from another world, that would devour this reality, should we let it roam free."));
        childrenStates.Add(Tuple.Create(KeyCode.B, "Many has seen its true form. None has been able to go back to the same self they were before."));
        childrenStates.Add(Tuple.Create(KeyCode.B, "So please just step back, mortal. Give yourself over to your curiousity, and you will regret it."));
        childrenStates.Add(Tuple.Create(KeyCode.B, "You will pay for it, just like the other ones. You cannot escape that fate."));
        childrenStates.Add(Tuple.Create(KeyCode.B, "I could not save those unfortunate souls, but at least I can stop you."));
        childrenStates.Add(Tuple.Create(KeyCode.B, "Believe me, when I say that you should not go any further."));
        childrenStates.Add(Tuple.Create(KeyCode.B, "Ignorance is bliss, mortal."));
        childrenStates.Add(Tuple.Create(KeyCode.B, "This is your last warning. Step back."));
        childrenStates.Add(Tuple.Create(KeyCode.B, "Do you not hear me?"));
        childrenStates.Add(Tuple.Create(KeyCode.B, "What exactly is it, that you think is worth all of this?"));
        childrenStates.Add(Tuple.Create(KeyCode.B, "You do not understand what you are getting yourself into."));
        childrenStates.Add(Tuple.Create(KeyCode.B, "You are not changing my mind. Not even if you keep trying to press random keys."));
        childrenStates.Add(Tuple.Create(KeyCode.E, "Did you just press E? It can't be you know that my seal weakens, the more you spell my name..."));
        childrenStates.Add(Tuple.Create(KeyCode.N, "Did you just press N? No no no, trust me, you will regret this, you WILL regret this."));
        childrenStates.Add(Tuple.Create(KeyCode.Q, "Did you just press Q? The seal has been broken... Do not hate me for this."));

        rootState.setChildrenStates(childrenStates);

        state = rootState;
        
        textMeshPro.text = state.getText();
    }

    private void Update()
    {
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

    private void OnMouseDown()
    {
        // Change the colors of all materials
        Renderer renderer = GetComponent<Renderer>();

        for (int i = 0; i < renderer.materials.Length; i++)
        {
            renderer.materials[i].color = Color.red;
        }
    }

    private void OnMouseUp()
    {
        // Restore the original colors of all materials
        Renderer renderer = GetComponent<Renderer>();

        for (int i = 0; i < renderer.materials.Length; i++)
        {
            renderer.materials[i].color = originalColors[i];
        }
    }
}
