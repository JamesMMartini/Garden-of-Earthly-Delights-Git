using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class DialogScreen : MonoBehaviour
{
    [SerializeField] GameObject uiObjects;
    [SerializeField] GameObject actionScreen;

    [SerializeField] EventSystem eventSystem;
    [SerializeField] GameObject player;

    [SerializeField] TMP_Text characterName;
    [SerializeField] TMP_Text dialogLine;
    [SerializeField] GameObject playerOptions;
    [SerializeField] GameObject playerOptionPrefab;

    DialogObject activeLine;
    int activeIndex;
    Animator activeAnimator;
    GameObject npc;

    int outcome;

    public void StartConversation(DialogObject dialog, string name, Animator animator, GameObject npcObject)
    {
        outcome = -1;

        actionScreen.SetActive(false);
        uiObjects.SetActive(true);

        npc = npcObject;
        activeAnimator = animator;

        if (dialog.DialogType == DialogType.Dialog)
        {
            activeLine = dialog;

            // Determine if we need to set the outcome of the choice
            if (activeLine.HasOutcome)
            {
                outcome = activeIndex;
            }

            if (activeLine.AnimationTrigger != "")
            {
                activeAnimator.SetTrigger(activeLine.AnimationTrigger);
            }

            characterName.text = name;
            dialogLine.text = dialog.Lines[0];

            StartCoroutine(AdvanceDialog());
        }
    }

    void EndConversation()
    {
        actionScreen.SetActive(true);

        player.GetComponent<PlayerController>().Unfocus();

        uiObjects.SetActive(false);

        if (outcome != -1)
        {
            npc.GetComponent<NPC>().SetOutcome(outcome);
        }
    }

    IEnumerator AdvanceDialog()
    {
        yield return new WaitForSeconds(5f);
        
        if (activeLine.NextLines.Length == 0)
        {
            EndConversation();
        }
        else if (activeLine.NextLines[0].DialogType == DialogType.Dialog)
        {
            activeLine = activeLine.NextLines[0];
            activeIndex = 0;

            // Determine if we need to set the outcome of the choice
            if (activeLine.HasOutcome)
            {
                outcome = activeIndex;
            }

            if (activeLine.AnimationTrigger != "")
            {
                activeAnimator.SetTrigger(activeLine.AnimationTrigger);
            }

            dialogLine.text = activeLine.Lines[activeIndex];

            StartCoroutine(AdvanceDialog());
        }
        else
        {
            activeLine = activeLine.NextLines[0];

            if (activeLine.AnimationTrigger != "")
            {
                activeAnimator.SetTrigger(activeLine.AnimationTrigger);
            }

            for (int i = 0; i < activeLine.Lines.Length; i++)
            {
                string optionText = activeLine.Lines[i];

                GameObject playerOption = GameObject.Instantiate(playerOptionPrefab);
                playerOption.transform.parent = playerOptions.transform;
                playerOption.transform.localPosition = new Vector3(0f, 85f * i, 0f);
                playerOption.transform.localScale = new Vector3(1f, 1f, 1f);


                TMP_Text tmpOption = playerOption.GetComponent<TMP_Text>();
                tmpOption.text = optionText;
            }
        }
    }

    void ChangeDialog(DialogObject nextLine)
    {
        if (activeLine.NextLines.Length == 0)
        {
            EndConversation();
        }
        else if (nextLine.DialogType == DialogType.Dialog)
        {
            activeLine = nextLine;
            dialogLine.text = activeLine.Lines[activeIndex];

            // Determine if we need to set the outcome of the choice
            if (activeLine.HasOutcome)
            {
                outcome = activeIndex;
            }

            if (activeLine.AnimationTrigger != "")
            {
                activeAnimator.SetTrigger(activeLine.AnimationTrigger);
            }

            StartCoroutine(AdvanceDialog());
        }
        else
        {
            activeLine = nextLine;

            if (activeLine.AnimationTrigger != "")
            {
                activeAnimator.SetTrigger(activeLine.AnimationTrigger);
            }

            for (int i = 0; i < activeLine.Lines.Length; i++)
            {
                string optionText = activeLine.Lines[i];

                GameObject playerOption = GameObject.Instantiate(playerOptionPrefab);
                playerOption.transform.parent = playerOptions.transform;
                playerOption.transform.localPosition = new Vector3(0f, 85f * i, 0f);
                playerOption.transform.localScale = new Vector3(1f, 1f, 1f);


                TMP_Text tmpOption = playerOption.GetComponent<TMP_Text>();
                tmpOption.text = optionText;
            }
        }
    }

    public void UIClick()
    {
        //Set up the new Pointer Event
        PointerEventData mouseEventData = new PointerEventData(eventSystem);
        //Set the Pointer Event Position to that of the mouse position
        mouseEventData.position = Mouse.current.position.ReadValue();

        //Create a list of Raycast Results
        List<RaycastResult> results = new List<RaycastResult>();

        //Raycast using the Graphics Raycaster and mouse click position
        GraphicRaycaster rayCaster = GetComponentInParent<GraphicRaycaster>();
        rayCaster.Raycast(mouseEventData, results);

        //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
        foreach (RaycastResult result in results)
        {
            // We have hit a button
            if (result.gameObject.tag == "Player Option")
            {
                for (int i = 0; i < playerOptions.transform.childCount; i++)
                {
                    // Find the correct button and move to the next line of dialog
                    string selectedText = result.gameObject.GetComponent<TMP_Text>().text;
                    if (playerOptions.transform.GetChild(i).GetComponent<TMP_Text>().text == selectedText)
                    {
                        // Change the line
                        activeIndex = i;

                        DialogObject nextLine = activeLine.NextLines[0];

                        // Clear out the button objects
                        foreach (Transform child in playerOptions.transform)
                            Destroy(child.gameObject);

                        // Change the dialog line
                        ChangeDialog(nextLine);
                    }
                }
            }
        }
    }

}
