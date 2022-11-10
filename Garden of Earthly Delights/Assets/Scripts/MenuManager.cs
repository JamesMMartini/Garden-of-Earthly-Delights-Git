using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void OnInteract(InputValue value)
    {
        SceneManager.LoadScene("Apricot and Fern");
    }
}
