using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class TarotScreen : MonoBehaviour
{

    [SerializeField] GameObject uiObjects;
    [SerializeField] GameObject actionScreen;

    [SerializeField] EventSystem eventSystem;
    [SerializeField] GameObject player;

    [SerializeField] TMP_Text dialogLine;
    [SerializeField] GameObject tarotCardPrefab;

    [SerializeField] DialogObject finalLine;

    [SerializeField] AudioSource cardFLip;

    GameObject[] tarotCards;
    DialogObject activeLine;
    GameObject npc;
    int tarotRead;

    public void StartTarot(DialogObject startTarot, GameObject npcObject)
    {
        actionScreen.SetActive(false);
        uiObjects.SetActive(true);

        tarotRead = 0;
        tarotCards = new GameObject[3];

        npc = npcObject;

        activeLine = startTarot;
        dialogLine.text = startTarot.Lines[0];

        StartCoroutine(NextCard());
    }

    IEnumerator NextCard()
    {
        yield return new WaitForSeconds(4f);

        if (tarotRead < 3)
        {
            // Create the card
            GameObject newCard = GameObject.Instantiate(tarotCardPrefab);
            newCard.transform.SetParent(uiObjects.transform);
            newCard.transform.localPosition = new Vector3(380 * (tarotRead - 1), 0f, 0f);
            tarotCards[tarotRead] = newCard;

            // Set the card texture
            Sprite[] allCardSprites = Resources.LoadAll<Sprite>("Tarot Cards");
            int cardSpriteIndex = Random.Range(0, allCardSprites.Length - 1);

            foreach (GameObject card in tarotCards)
            {
                if (card != null)
                {
                    while (card.GetComponent<Image>().sprite == allCardSprites[cardSpriteIndex])
                    {
                        cardSpriteIndex = Random.Range(0, allCardSprites.Length - 1);
                    }
                }
            }

            newCard.GetComponent<Image>().sprite = allCardSprites[cardSpriteIndex];

            // Set the dialog text
            dialogLine.text = allCardSprites[cardSpriteIndex].name;

            // Determine if it is inversed
            int reversed = Random.Range(0, 2);
            if (reversed == 1)
            {
                Vector3 newRotation = new Vector3(0f, 0f, 180f);
                newCard.transform.rotation = Quaternion.Euler(newRotation);

                dialogLine.text += ", reversed";
            }
            
            tarotRead++;

            cardFLip.Play();

            StartCoroutine(NextCard());
        }
        else
        {
            // Go to end of reading
            EndReading();
        }
    }

    void EndReading()
    {
        uiObjects.SetActive(false);

        foreach (GameObject card in tarotCards)
            Destroy(card);

        DialogScreen dialogScreen = GameObject.Find("Dialog Screen").GetComponent<DialogScreen>();
        dialogScreen.StartConversation(finalLine, npc.GetComponent<NPC>().CharacterName, npc.GetComponent<Animator>(), npc);
    }
}
