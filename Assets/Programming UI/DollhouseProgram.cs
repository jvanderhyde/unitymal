//A component that stores a Dollhouse system (program) in a world object
//Created by James Vanderhyde, 17 November 2021

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.UI;

public class DollhouseProgram : MonoBehaviour, IPointerDownHandler
{
    private static DollhouseProgramUI activeUI = null;

    public static DollhouseProgramUI GetActiveProgramUI()
    {
        return activeUI;
    }

    private DollhouseProgramUI programUI;

    void Awake()
    {
        foreach (Transform child in this.transform)
        {
            Canvas c = child.GetComponent<Canvas>();
            if (c != null)
            {
                programUI = c.GetComponentsInChildren<DollhouseProgramUI>(true)[0];

                //Check for VR
                if (XRSettings.enabled)
                {
                    //Change Canvas to world space
                    c.renderMode = RenderMode.WorldSpace;
                    c.transform.position = new Vector3(0, 0, 0);
                    c.transform.forward = new Vector3(0, 0, 1);
                    c.transform.localScale = new Vector3(0.006f,0.006f,0.006f);

                    //Switch to the correct GraphicRaycaster component
                    TrackedDeviceGraphicRaycaster trackedRC = c.GetComponent<TrackedDeviceGraphicRaycaster>();
                    GraphicRaycaster canvasRC = c.GetComponent<GraphicRaycaster>();
                    if ((trackedRC != null) && (canvasRC != null))
                    {
                        trackedRC.enabled = true;
                        canvasRC.enabled = false;
                    }
                }
            }
        }
    }

    public DollhouseProgramUI GetProgramUI()
    {
        return this.programUI;
    }

    void OnMouseDown()
    {
        //Toggle the active state of the corresponding UI.
        GameObject o = this.programUI.gameObject;
        if (o.activeSelf)
        {
            //If this program's UI is active, deactivate it.
            o.SetActive(false);
            DollhouseProgram.activeUI = null;
        }
        else
        {
            //For a world object, create a program reference. For a program, close any open UI.
            //Either way, if there's no open UI, open the UI.
            CreateProgramRefComponent r = this.GetComponent<CreateProgramRefComponent>();
            if ((r != null) && (DollhouseProgram.activeUI != null))
            {
                r.CreateProgramReference(DollhouseProgram.activeUI);
            }
            else
            {          
                if ((r==null) && (DollhouseProgram.activeUI != null))
                    DollhouseProgram.activeUI.gameObject.SetActive(false);

                //Open the UI for this object
                o.SetActive(true);
                DollhouseProgram.activeUI = this.programUI;
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if ((eventData.pointerEnter != null) && (eventData.pointerEnter.GetComponent<DollhouseProgram>() != null))
            this.OnMouseDown();
    }

    public void OnVRSelect(SelectEnterEventArgs eventData)
    {
        this.OnMouseDown();

        //Put the canvas in front of the user
        Canvas c = this.programUI.transform.GetComponentInParent<Canvas>();
        Transform head = Camera.main.transform;
        c.transform.position = head.position;
        c.transform.forward = head.forward;
        c.transform.Translate(new Vector3(0,0,4f));
    }

}
