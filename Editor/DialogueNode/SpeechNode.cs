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

            // add input and output ports
            AddInputPort();
            Port nextPort = AddOutputPort();

            // create link data
            ((Speech)dialogue).nextLink = NodeLinkData.FromPort(nextPort);

            FinishCreatingNode();
        }

        public override void OnCreateLink(Edge edge)
        {
            Debug.Log(edge.output.portName);
            // TODO: should probably introduce a constant for these port names?
            if (edge.output.portName == "Next")
            {
                ((Speech)dialogue).nextLink = NodeLinkData.FromEdge(edge);
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