using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogObject", menuName = "Dialog/Create New Dialog Object")]
public class DialogObject : ScriptableObject
{
    [SerializeField] DialogType type;
    [SerializeField] string[] lines;
    [SerializeField] DialogObject[] next;
    [SerializeField] string animationTrigger;
    [SerializeField] bool hasOutcome;

    
    public DialogType DialogType { get { return type; } set { type = value; } }

    public string[] Lines { get { return lines; } set { lines = value; } }

    public DialogObject[] NextLines { get { return next; } set { next = value; } }

    public string AnimationTrigger { get { return animationTrigger; } set { animationTrigger = value; } }

    public bool HasOutcome { get { return hasOutcome; } set { hasOutcome = value; } }
}

public enum DialogType
{
    Dialog,
    PlayerChoice
}
