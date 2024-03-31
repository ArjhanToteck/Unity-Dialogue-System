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

        public override void OnCreateLink(Edge edge)
        {
            Debug.Log(edge.output.portName);
            // TODO: should probably introduce a constant for these port names?
            if (edge.output.portName == "Next")
            {
                ((Speech)dialogue).nextLink = new NodeLinkData(edge);
            }
        }

        public override void OnRemoveLink(Edge edge)
        {
            if (edge.output.portName == "Next")
            {
                ((Speech)dialogue).nextLink = null;
            }
        }
    }
}