//Allows 3D objects in the world to be dragged into the UI
//Created by James Vanderhyde, 12 November 2021
//Modified by James Vanderhyde, 8 Auguest 2024
//  Renamed class (formerly CreateProgramReference) and removed mouse handling.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mal;

public class CreateProgramRefComponent : MonoBehaviour
{
    public void CreateProgramReference(DollhouseProgramUI programUI)
    {
        if (programUI != null)
        {
            //Create the Mal form
            MalEntity result;
            result = (MalEntity)programUI.transform.GetComponentInChildren<MalPrinter>().pr_form(new types.MalObjectReference(this.GetComponent<Entity>()));
        }
        else
            throw new NullReferenceException("programUI must not be null");
    }

}
