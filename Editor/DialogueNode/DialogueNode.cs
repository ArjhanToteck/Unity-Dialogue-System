using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DialogueNode : Node
{
    public string GUID;

    public DialogueNode(){
        GUID = Guid.NewGuid().ToString();
    }
}