using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenController : NPC
{
    [SerializeField] Animator animator;
    [SerializeField] GameObject player;

    [SerializeField] DialogObject openingLine;
    [SerializeField] DialogObject winLossLine;
    [SerializeField] TicTacToeScreen ticTacToeScreen; 

    public override void SetOutcome(int result)
    {
        if (result == 0)
        {
            // Start the tarot reading
            StartCoroutine(player.GetComponent<PlayerController>().FocusOnObject(gameObject));

            transform.LookAt(player.transform);

            // Start the tic tac toe
            ticTacToeScreen.StartGame(gameObject);
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
