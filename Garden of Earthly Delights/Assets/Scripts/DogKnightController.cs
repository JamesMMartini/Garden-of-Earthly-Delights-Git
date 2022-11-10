using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

public class DogKnightController : NPC
{
    [SerializeField] Transform baseTarget;
    [SerializeField] Transform[] hidingSpots;
    [SerializeField] NavMeshAgent agent;

    [SerializeField] Animator animator;
    [SerializeField] GameObject player;
    [SerializeField] GameObject blackScreen;
    
    [SerializeField] DialogObject openingLine;
    [SerializeField] DialogObject hiddenLine;
    [SerializeField] DialogObject foundLine;

    [SerializeField] VisualEffect poof;
    [SerializeField] AudioSource footstep;

    bool hiding;
    bool isWalking;

    private void Update()
    {
        if (isWalking && agent.remainingDistance == 0f) // Character needs to be stopped
        {
            isWalking = false;
            animator.SetBool("IsWalking", isWalking);

            //Vector3 idleRotation = new Vector3(0, 0, 0);
            //Vector3 lookPos = idleRotation - transform.position;
            //lookPos.y = 0;
            //Quaternion rotation = Quaternion.LookRotation(lookPos);
            //transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 1f);
        }
        else if (!isWalking && agent.remainingDistance > 0f) // Character needs to be started
        {
            isWalking = true;
            animator.SetBool("IsWalking", isWalking);
        }
    }

    public void PuffGround()
    {
        footstep.Play();

        poof.Reinit();
        poof.Play();
    }

    public override void Interacted()
    {
        if (!hiding)
        {
            //StopAllCoroutines();

            StartCoroutine(player.GetComponent<PlayerController>().FocusOnObject(gameObject));

            transform.LookAt(player.transform);

            DialogScreen dialogScreen = GameObject.Find("Dialog Screen").GetComponent<DialogScreen>();
            dialogScreen.StartConversation(openingLine, CharacterName, animator, gameObject);
        }
        else
        {
            StartCoroutine(player.GetComponent<PlayerController>().FocusOnObject(gameObject));

            transform.LookAt(player.transform);

            DialogScreen dialogScreen = GameObject.Find("Dialog Screen").GetComponent<DialogScreen>();
            dialogScreen.StartConversation(foundLine, CharacterName, animator, gameObject);
        }
    }

    public override void SetOutcome(int result)
    {
        if (!hiding)
        {
            if (result == 0) // Start the hide and seek
            {
                StartCoroutine(HideAndSeek());
            }
        }
        else
        {
            hiding = false;
            agent.SetDestination(baseTarget.position);
        }
    }

    IEnumerator HideAndSeek()
    {
        blackScreen.SetActive(true);

        player.GetComponent<PlayerController>().playerInput.SwitchCurrentActionMap("DoNothing");

        int hidingSpotIndex = (int)(Random.value * hidingSpots.Length);
        if (hidingSpotIndex == hidingSpots.Length)
            hidingSpotIndex--;

        agent.Warp(hidingSpots[hidingSpotIndex].position);
        hiding = true;

        yield return new WaitForSeconds(3f);

        blackScreen.SetActive(false);

        player.GetComponent<PlayerController>().FocusOnObject(baseTarget.gameObject);

        DialogScreen dialogScreen = GameObject.Find("Dialog Screen").GetComponent<DialogScreen>();
        dialogScreen.StartConversation(hiddenLine, CharacterName, animator, gameObject);

    }
}
