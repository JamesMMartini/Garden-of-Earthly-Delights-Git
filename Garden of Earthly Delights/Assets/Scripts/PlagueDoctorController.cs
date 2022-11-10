using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
using UnityEngine.VFX;
using UnityEngine.SceneManagement;

public class PlagueDoctorController : NPC
{
    [SerializeField] Animator animator;
    [SerializeField] GameObject player;

    [SerializeField] DialogObject[] dialogOpeners;

    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform[] navMeshTargets;
    [SerializeField] Quaternion idleRotation;

    [SerializeField] VisualEffect poof;
    [SerializeField] AudioSource footstep;

    bool idle;
    bool choosing;
    bool isWalking;

    private void Start()
    {
        idle = true;
        choosing = false;
    }

    public void PuffGround()
    {
        footstep.Play();

        poof.Reinit();
        poof.Play();
    }

    private void Update()
    {
        if (idle)
        {
            if (isWalking && agent.remainingDistance == 0f) // Character needs to be stopped
            {
                isWalking = false;
                animator.SetBool("IsWalking", isWalking);

                transform.rotation = idleRotation;

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
            else if (!isWalking && agent.remainingDistance == 0f && !choosing) // Start a new idle action
            {
                StartCoroutine(NewIdleAction());
            }
        }
    }

    IEnumerator NewIdleAction()
    {
        choosing = true;

        yield return new WaitForSeconds(5f);

        int newTarget = (int)(Random.value * (navMeshTargets.Length + 1));
        if (newTarget == navMeshTargets.Length)
            newTarget--;

        agent.SetDestination(navMeshTargets[newTarget].position);
        isWalking = true;
        animator.SetBool("IsWalking", isWalking);

        choosing = false;
    }

    public override void Interacted()
    {
        StopAllCoroutines();

        StartCoroutine(player.GetComponent<PlayerController>().FocusOnObject(gameObject));

        idle = false;
        transform.LookAt(player.transform);
        agent.SetDestination(transform.position);
        isWalking = false;
        animator.SetBool("IsWalking", isWalking);

        int opener = (int)(Random.value * dialogOpeners.Length);
        if (opener >= dialogOpeners.Length)
            opener--;

        DialogScreen dialogScreen = GameObject.Find("Dialog Screen").GetComponent<DialogScreen>();
        dialogScreen.StartConversation(dialogOpeners[opener], CharacterName, animator, gameObject);
    }

    public override void SetOutcome(int result)
    {
        if (result == 0)
        {
            SceneManager.LoadScene("Earthly Delights Bad");
        }
        else if (result == 1)
        {
            SceneManager.LoadScene("Earthly Delights Middle");
        }
        else if (result == 2)
        {
            SceneManager.LoadScene("Earthly Delights Good");
        }
    }
}
