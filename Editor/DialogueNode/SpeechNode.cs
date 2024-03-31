using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace DialogueSystem.Editor
{
    public class SpeechNode : DialogueNode
    {
        public SpeechNode(DialogueGraphView graphView, string nodeName = "New Speech Node") : base(graphView, nodeName)
        {
            dialogue = new Speech();

            AddInputPort();
            AddOutputPort();

            FinishCreatingNode();
        }
    }
}