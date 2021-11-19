//Any action control structure that appears in the UI
//Created by James Vanderhyde, 18 November 2021

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mal;
using TMPro;

public class MalActionStructure : MalForm
{
    public override types.MalVal read_form()
    {
        Transform contents = transform.GetChild(1);

        types.MalList ml = new types.MalList();
        for (int i = contents.childCount-1; i>=0; i--)
        {
            MalForm child = contents.GetChild(i).GetComponent<MalForm>();
            types.MalVal value = child.read_form();
            ml.cons(value);
        }

        string functionName = transform.GetChild(0).GetComponent<TMP_Text>().text;
        ml.cons(new types.MalObjectReference(this.gameObject)); //Inject the MonoBehaviour
        ml.cons(new types.MalSymbol(functionName));

        types.MalList delay = new types.MalList();
        delay.cons(ml);
        delay.cons(new types.MalSymbol("delay"));
        return delay;
    }
}