using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace DialogueSystem.Editor
{
    public class DialogueNode : Node
    {
        public string guid = System.Guid.NewGuid().ToString();
        public Dialogue dialogue;

        public void UpdateDialogueObject()
        {
            dialogue.nodeData.guid = guid;
        }
    }
}