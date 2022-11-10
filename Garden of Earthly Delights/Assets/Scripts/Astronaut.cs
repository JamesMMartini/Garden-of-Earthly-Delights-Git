using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astronaut : NPC
{
    [SerializeField] Animator animator;
    [SerializeField] GameObject player;

    [SerializeField] DialogObject openingLine;
    [SerializeField] DialogObject readingLine;
    [SerializeField] TarotScreen tarotScreen;

    public override void SetOutcome(int result)
    {
        if (result == 0) // Floor time
        {
            // Start the tarot reading
            StartCoroutine(player.GetComponent<PlayerController>().FocusOnObject(gameObject));

            transform.LookAt(player.transform);

            tarotScreen.StartTarot(readingLine, gameObject);
        }
    }

    public override void Interacted()
    {
        StartCoroutine(player.GetComponent<PlayerController>().FocusOnObject(gameObject));

        transform.LookAt(player.transform);

        DialogScreen dialogScreen = GameObject.Find("Dialog Screen").GetComponent<DialogScreen>();
        dialogScreen.StartConversation(openingLine, CharacterName, animator, gameObject);
    }
}
