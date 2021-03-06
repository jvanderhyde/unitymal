//A world object with a unique ID
//Created by James Vanderhyde, 20 January 2022

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mal;

public class Entity : MonoBehaviour
{
    public string guid = "";

    public types.MalVal read_form()
    {
        types.MalList ml = new types.MalList();
        types.MalMap mm = new types.MalMap();

        mm.assoc(types.MalKeyword.keyword(":guid"), new types.MalString(this.guid));

        ml.cons(mm);
        ml.cons(new types.MalSymbol("entity"));
        return ml;
    }

    public types.MalVal read_create_form()
    {
        types.MalList ml = new types.MalList();
        types.MalMap mm = new types.MalMap();

        mm.assoc(types.MalKeyword.keyword(":name"), new types.MalString(this.gameObject.name));
        mm.assoc(types.MalKeyword.keyword(":guid"), new types.MalString(this.guid));

        var gi = this.GetComponent<GalleryItem>();
        if (gi != null)
            mm.assoc(types.MalKeyword.keyword(":gallery-name"), new types.MalString(gi.galleryItemName));

        types.MalVector transformData = new types.MalVector();
        transformData.conj(new types.MalNumber(this.transform.position.x));
        transformData.conj(new types.MalNumber(this.transform.position.y));
        transformData.conj(new types.MalNumber(this.transform.position.z));
        transformData.conj(new types.MalNumber(this.transform.eulerAngles.x));
        transformData.conj(new types.MalNumber(this.transform.eulerAngles.y));
        transformData.conj(new types.MalNumber(this.transform.eulerAngles.z));
        transformData.conj(new types.MalNumber(this.transform.localScale.x));
        transformData.conj(new types.MalNumber(this.transform.localScale.y));
        transformData.conj(new types.MalNumber(this.transform.localScale.z));
        mm.assoc(types.MalKeyword.keyword(":transform"), transformData);

        types.MalList childData = new types.MalList();
        foreach (Transform child in this.transform)
        {
            Entity item = child.GetComponent<Entity>();
            if (item != null)
                childData.cons(item.read_create_form());
        }
        childData.cons(new types.MalSymbol("list"));
        mm.assoc(types.MalKeyword.keyword(":children"), childData);

        DollhouseProgram p = this.GetComponent<DollhouseProgram>();
        if (p != null)
        {
            types.MalList programData = new types.MalList();

            MalPrinter mp = p.GetProgramUI().GetComponentsInChildren<MalPrinter>(true)[0];
            foreach (Transform codeChild in mp.transform)
            {
                MalForm item = codeChild.GetComponent<MalForm>();
                if (item is MalActionState)
                {
                    //skip it. We can't save the dynamic state.
                }
                else if (item==null)
                {
                    //skip it. This case should not happen, but we don't want a rogue element to stop the save process.
                    Debug.Assert(item != null);
                }
                else
                {
                    types.MalList codeChildData = new types.MalList();
                    codeChildData.cons(item.read_form());
                    codeChildData.cons(new types.MalNumber(codeChild.localPosition.y));
                    codeChildData.cons(new types.MalNumber(codeChild.localPosition.x));
                    types.MalList q = new types.MalList();
                    q.cons(codeChildData);
                    q.cons(new types.MalSymbol("quote"));
                    programData.cons(Highlight.removeHighlights(q));
                }
            }

            programData.cons(new types.MalSymbol("list"));
            mm.assoc(types.MalKeyword.keyword(":program"), programData);
        }

        ml.cons(mm);
        ml.cons(new types.MalSymbol("create-entity"));
        return ml;
    }

}
