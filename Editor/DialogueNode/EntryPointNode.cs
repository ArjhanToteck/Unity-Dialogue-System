using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace DialogueSystem.Editor
{
    public class EntryPointNode : DialogueNode
    {
        public EntryPointNode(DialogueGraphView graphView, string nodeName = "Entry") : base(graphView, nodeName)
        {
            dialogue = new EntryPoint();

            // entry node shouldn't be deletable
            capabilities &= ~Capabilities.Deletable;

            Port nextPort = AddOutputPort();

            // create link data
            ((EntryPoint)dialogue).nextLink = NodeLinkData.FromPort(nextPort);

            FinishCreatingNode();
        }

        public override void OnCreateLink(Edge edge)
        {
            if (edge.output.portName == "Next")
            {
                ((EntryPoint)dialogue).nextLink = NodeLinkData.FromEdge(edge);
            }
        }

        public override void OnRemoveLink(Edge edge)
        {
            if (edge.output.portName == "Next")
            {
                ((EntryPoint)dialogue).nextLink = null;
            }
        }
    }
}