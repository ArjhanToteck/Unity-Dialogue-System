using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace DialogueSystem.Editor
{
    public class EntryPointNode : DialogueNode
    {
        public EntryPointNode()
        {
            UpdateDialogue(new EntryPoint());

            title = "Entry Point";

            // entry node shouldn't be deletable
            capabilities &= ~Capabilities.Deletable;

            Port nextPort = AddOutputPort();

            // create link data
            ((EntryPoint)dialogue).nextLink = NodeLinkData.FromPort(nextPort);
        }

        public override void OnCreateLink(Edge edge)
        {
            if (edge.output.portName == nextPortName)
            {
                ((EntryPoint)dialogue).nextLink = NodeLinkData.FromEdge(edge);
            }
        }

        public override void OnRemoveLink(Edge edge)
        {
            if (edge.output.portName == nextPortName)
            {
                ((EntryPoint)dialogue).nextLink = null;
            }
        }
    }
}