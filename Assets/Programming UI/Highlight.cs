//MAL command to highlight a line of code while it executes
//Created by James Vanderhyde, 18 January 2021

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mal;

public class Highlight : types.MalFunc
{
    private static IEnumerator highlight(Dollhouse.DollhouseActionState actionState, MalForm component)
    {
        UnityEngine.UI.Image im = component.GetComponent<UnityEngine.UI.Image>();
        if (im != null)
        {
            im.color = new Color32(253, 229, 154, 255);
            while (!actionState.IsDone())
                yield return null;
            im.color = new Color32(185, 185, 185, 255);
        }
    }

    public override types.MalVal apply(types.MalList arguments)
    {
        //Parse the arguments
        if (arguments.isEmpty() || arguments.rest().isEmpty())
            throw new ArgumentException("highlight is missing a value.");
        if (!(arguments.first() is types.MalObjectReference))
            throw new ArgumentException("First argument to highlight must be an instruction in the programming UI.");
        types.MalObjectReference mor = arguments.first() as types.MalObjectReference;
        GameObject obj = (GameObject)mor.value;
        MalForm component = obj.GetComponent<MalForm>();
        if (component == null)
            throw new ArgumentException("First argument to highlight must be an instruction in the programming UI.");
        if (!(arguments.rest().first() is Dollhouse.DollhouseActionState))
            throw new ArgumentException("First argument to highlight must be the result of a Dollhouse Action.");
        Dollhouse.DollhouseActionState state = arguments.rest().first() as Dollhouse.DollhouseActionState;

        //Start the coroutine to highlight, wait for the instruction to finish, and dehighlight
        component.StartCoroutine(highlight(state, component));
        return state;
    }
}