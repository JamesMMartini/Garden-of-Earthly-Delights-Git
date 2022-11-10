using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TicTacToeScreen : MonoBehaviour
{
    [SerializeField] GameObject uiObjects;
    [SerializeField] GameObject pieces;
    [SerializeField] GameObject actionScreen;

    [SerializeField] GameObject xMarker;
    [SerializeField] GameObject oMarker;

    [SerializeField] GameObject[] gameSpaces;

    [SerializeField] EventSystem eventSystem;
    [SerializeField] GameObject player;

    [SerializeField] DialogObject winLine;
    [SerializeField] DialogObject loseLine;

    [SerializeField] AudioSource tic;
    
    GameObject npc;
    GameObject[][] spaces;
    GameObject[][] markers;
    bool choosing;
    public bool GamePlaying;

    public void StartGame(GameObject npcObject)
    {
        GamePlaying = true;
        choosing = false;

        npc = npcObject;

        uiObjects.SetActive(true);
        actionScreen.SetActive(false);

        markers = new GameObject[3][];
        for (int i = 0; i < markers.Length; i++)
            markers[i] = new GameObject[3];

        spaces = new GameObject[3][];
        for (int i = 0; i < spaces.Length; i++)
            spaces[i] = new GameObject[3];

        for (int row = 0; row < spaces.Length; row++)
        {
            for (int col = 0; col < spaces[row].Length; col++)
            {
                spaces[row][col] = gameSpaces[(row * 3) + col];
            }
        }
    }

    public void Click()
    {
        if (!choosing)
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
                if (result.gameObject.tag == "TicTacSpace")
                {
                    Debug.Log("CLICKED");
                    for (int row = 0; row < spaces.Length; row++)
                    {
                        for (int col = 0; col < spaces[row].Length; col++)
                        {
                            Debug.Log(spaces[row][col].GetInstanceID() + " - " + result.gameObject.GetInstanceID());
                            if (spaces[row][col].GetInstanceID() == result.gameObject.GetInstanceID())
                            {
                                GameObject newMarker = GameObject.Instantiate(oMarker);
                                newMarker.transform.position = spaces[row][col].transform.position;
                                newMarker.transform.SetParent(pieces.transform);

                                markers[row][col] = newMarker;

                                tic.Play();

                                if (FindWinner() == null)
                                    StartCoroutine(aiPlay());
                                else
                                    EndGame();
                            }
                        }
                    }
                }
            }
        }
    }

    string FindWinner()
    {
        for (int row = 0; row < markers.Length; row++)
            for (int col = 0; col < markers[row].Length; col++)
                Debug.Log(MarkerSymbol(row, col));

        // Check dialgonals
        if (MarkerSymbol(0, 0) != "" && (MarkerSymbol(0, 0) == MarkerSymbol(1, 1) && MarkerSymbol(0, 0) == MarkerSymbol(2, 2)))
        {
            if (MarkerSymbol(0, 0) == "O")
                return "Player";
            else
                return "Ava";
        }
        else if (MarkerSymbol(0, 2) != "" && (MarkerSymbol(0, 2) == MarkerSymbol(1, 1) && MarkerSymbol(0, 2) == MarkerSymbol(2, 0)))
        {
            if (MarkerSymbol(0, 2) == "O")
                return "Player";
            else
                return "Ava";
        }

        // Check Verticals
        if (MarkerSymbol(0, 0) != "" && (MarkerSymbol(0, 0) == MarkerSymbol(1, 0) && MarkerSymbol(0, 0) == MarkerSymbol(2, 0)))
        {
            if (MarkerSymbol(0, 0) == "O")
                return "Player";
            else
                return "Ava";
        }
        else if (MarkerSymbol(0, 1) != "" && (MarkerSymbol(0, 1) == MarkerSymbol(1, 1) && MarkerSymbol(0, 0) == MarkerSymbol(2, 1)))
        {
            if (MarkerSymbol(0, 1) == "O")
                return "Player";
            else
                return "Ava";
        }
        else if (MarkerSymbol(0, 2) != "" && (MarkerSymbol(0, 2) == MarkerSymbol(1, 2) && MarkerSymbol(0, 2) == MarkerSymbol(2, 2)))
        {
            if (MarkerSymbol(0, 2) == "O")
                return "Player";
            else
                return "Ava";
        }

        // Check Horizontals
        if (MarkerSymbol(0, 0) != "" && (MarkerSymbol(0, 0) == MarkerSymbol(0, 1) && MarkerSymbol(0, 0) == MarkerSymbol(0, 2)))
        {
            if (MarkerSymbol(0, 0) == "O")
                return "Player";
            else
                return "Ava";
        }
        else if (MarkerSymbol(1, 0) != "" && (MarkerSymbol(1, 0) == MarkerSymbol(1, 1) && MarkerSymbol(1, 0) == MarkerSymbol(1, 2)))
        {
            if (MarkerSymbol(1, 0) == "O")
                return "Player";
            else
                return "Ava";
        }
        else if (MarkerSymbol(2, 0) != "" && (MarkerSymbol(2, 0) == MarkerSymbol(2, 1) && MarkerSymbol(2, 0) == MarkerSymbol(2, 2)))
        {
            if (MarkerSymbol(2, 0) == "O")
                return "Player";
            else
                return "Ava";
        }

        return null;
    }

    string MarkerSymbol(int row, int col)
    {
        if (markers[row][col] != null)
            return markers[row][col].GetComponent<TMP_Text>().text;
        else
            return "";
    }

    IEnumerator aiPlay()
    {
        choosing = true;

        yield return new WaitForSeconds(3f);

        int row = Random.Range(0, 3);
        int col = Random.Range(0, 3);

        while (markers[row][col] != null)
        {
            row = Random.Range(0, 3);
            col = Random.Range(0, 3);
        }

        GameObject newMarker = GameObject.Instantiate(xMarker);
        newMarker.transform.position = spaces[row][col].transform.position;
        newMarker.transform.SetParent(pieces.transform);

        markers[row][col] = newMarker;

        tic.Play();

        if (FindWinner() != null)
            EndGame();
        else
            choosing = false;
    }

    void EndGame()
    {
        GamePlaying = false;

        uiObjects.SetActive(false);

        foreach (GameObject[] markerArray in markers)
            foreach (GameObject marker in markerArray)
                Destroy(marker);

        DialogScreen dialogScreen = GameObject.Find("Dialog Screen").GetComponent<DialogScreen>();

        if (FindWinner() == "Player")
            dialogScreen.StartConversation(winLine, npc.GetComponent<NPC>().CharacterName, npc.GetComponent<Animator>(), npc);
        else
            dialogScreen.StartConversation(loseLine, npc.GetComponent<NPC>().CharacterName, npc.GetComponent<Animator>(), npc);
    }
}
