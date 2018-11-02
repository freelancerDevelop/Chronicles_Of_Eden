﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour {

    enum TutorialComponent : int {
        Move = 0,
        Dodge = 1,
        Attack = 2,
        Pickup = 3,
        Equip = 4,
        UseItem = 5,
        End = 6
    }

    // UI elements for TutorialManager.
    public Text nameText;
    public Text dialogueText;
    public Animator animator;

    // Check if the first item has been picked up.
    public Pickup firstPickup;

    // Dialogues for the tutorial.
    public Dialogue dialogue;
    private List<string> sentences;

    // The current tutorial that we are at.
    // -1 indicates that the tutorial have not begun yet.
    private int popUpIndex = -1;

    // Flag to check if the tutorial has already started.
    private bool startTutorial = false;

    // Check if a dialogue is already typing. 
    private bool typing = false;

    private void Start()
    {
        sentences = new List<string>();
        nameText.text = dialogue.name;
        foreach (string s in dialogue.sentences)
        {
            sentences.Add(s);
        }
    }

    void Update()
    {
        // Wait for a few seconds before beginning the tutorial.
        if (startTutorial == false)
        {
            StartCoroutine(BeginTutorial());
        }

        CheckPlayerInputs();
        Display();
    }

    /// <summary>
    /// Check the player inputs, in order to keep track of the tutorial to 
    /// be displayed. 
    /// </summary>
    void CheckPlayerInputs() {
        if ((popUpIndex == (int)TutorialComponent.Move && 
            (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)))
        {
            // Player learned to move.
            MoveToNextTutorial();
        }
        else if (popUpIndex == (int)TutorialComponent.Dodge &&
            (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) && Input.GetKeyDown(KeyCode.Space))
        {
            // Player learned to dodge.
            MoveToNextTutorial();
        }
        else if (popUpIndex == (int)TutorialComponent.Attack 
            && Input.GetMouseButtonDown(1))
        {
            // Player learned to attack.
            MoveToNextTutorial();
        }
        else if (popUpIndex == (int)TutorialComponent.Pickup 
            && firstPickup.pickedUp)
        {
            // Item has already been successfully picked up.
            MoveToNextTutorial();
        }
        else if (popUpIndex == (int)TutorialComponent.Equip 
            && Input.GetKeyDown(KeyCode.Alpha1))
        {
            // Item is equipped.
            MoveToNextTutorial();
        }
        else if (popUpIndex == (int)TutorialComponent.UseItem 
            && Input.GetMouseButtonDown(0))
        {
            // Item is used.
            MoveToNextTutorial();
        }
        else if (popUpIndex == (int)TutorialComponent.End) {
            // End of tutorial.
            StartCoroutine(EndTutorial());
        }
    }

    /// <summary>
    /// Display the tutorial dialogue if there is still more tutorials
    /// to show. 
    /// </summary>
    void Display() {
        if (popUpIndex >= 0 && popUpIndex < sentences.Count)
        {
            animator.SetBool("IsOpen", true);

            // Display tutorial dialogue if it has not start typing yet. 
            if (typing == false)
            {
                StopAllCoroutines();
                StartCoroutine(TypeSentence(sentences[popUpIndex]));
            }
        }
        

        if(popUpIndex >= sentences.Count) 
        {
            animator.SetBool("IsOpen", false);
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Wait for 2 seconds before we begin the tutorial.
    /// </summary>
    IEnumerator BeginTutorial()
    {
        startTutorial = true;
        yield return new WaitForSeconds(2);
        popUpIndex = 0;
    }

    /// <summary>
    /// Wait for 3 seconds before we end the tutorial.
    /// </summary>
    IEnumerator EndTutorial() {
        yield return new WaitForSeconds(3);
        popUpIndex++;
    }

    /// <summary>
    /// Type the sentence character by character.
    /// </summary>
    /// <param name="sentence">Sentence to type.</param>
    IEnumerator TypeSentence(string sentence)
    {
        typing = true;
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    /// <summary>
    /// Display the next tutorial to the user. 
    /// </summary>
    void MoveToNextTutorial() {
        popUpIndex++;
        typing = false;
    }
}
