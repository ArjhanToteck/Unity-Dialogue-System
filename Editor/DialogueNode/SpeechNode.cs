using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace DialogueSystem.Editor
{
    public class SpeechNode : DialogueNode
    {
        public SpeechNode()
        {
            UpdateDialogue(new Speech());

            title = "Speech";

            // add input and output ports
            AddInputPort();
            Port nextPort = AddOutputPort();

            // create link data
            ((Speech)dialogue).nextLink = NodeLinkData.FromPort(nextPort);
        }

        public override void OnCreateLink(Edge edge)
        {
            if (edge.output.portName == nextPortName)
            {
                ((Speech)dialogue).nextLink = NodeLinkData.FromEdge(edge);
            }
        }

        public override void OnRemoveLink(Edge edge)
        {
            if (edge.output.portName == nextPortName)
            {
                ((Speech)dialogue).nextLink = null;
            }
        }
    }
}