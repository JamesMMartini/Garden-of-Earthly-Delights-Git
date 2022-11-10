using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FoxController : NPC
{
    [SerializeField] Animator animator;
    [SerializeField] GameObject player;

    [SerializeField] DialogObject openingLine;

    [SerializeField] CinemachineVirtualCamera floorTimeCamera;

    public override void SetOutcome(int result)
    {
        if (result == 0) // Floor time
        {
            player.GetComponent<PlayerController>().EnterFloorTime();
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
