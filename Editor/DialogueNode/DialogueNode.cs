using System;
using System.Collections;
using System.Collections.Generic;
using Codice.Client.BaseCommands.Import;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace DialogueSystem.Editor
{
    // TODO: refactor some of the methods here to go into the view class
    public abstract class DialogueNode : Node
    {
        public const string previousPortName = "Previous";
        public const string nextPortName = "Next";
        public static readonly Vector2 defaultNodeSize = new Vector2(150, 200);
        public static readonly Vector2 defaultNodePosition = new Vector2(100, 100);

        public string guid = Guid.NewGuid().ToString();
        public Dialogue dialogue;
        public DialogueGraphView graphView;

        public DialogueNode()
        {
            SetPosition(new Rect(defaultNodePosition, defaultNodeSize));
        }

        public virtual void LoadNodeFromDialogue(Dialogue dialogue)
        {
            // set dialogue
            this.dialogue = dialogue;

            // set guid
            guid = dialogue.nodeData.guid;

            // set position
            Vector2 position = new Vector2(this.dialogue.nodeData.position[0], this.dialogue.nodeData.position[1]);
            SetPosition(new Rect(position, defaultNodeSize));
        }

        public virtual void LoadLinksFromDialogue(Dialogue dialogue)
        {

        }

        public Edge AddLinkFromNodeLinkData(NodeLinkData link)
        {
            if (link.connectedNodeGuid != null && link.inputPortName != null)
            {
                return graphView.LinkNodes(this, link.outputPortName, graphView.GetDialogueNodeByGuid(link.connectedNodeGuid), link.inputPortName);
            }

            return null;
        }

        public void UpdateDialogue(Dialogue dialogue)
        {
            this.dialogue = dialogue;
            dialogue.nodeData.guid = guid;
            SavePositionInDialogue();
        }

        public void SavePositionInDialogue()
        {
            if (dialogue == null)
            {
                return;
            }

            Vector2 position = GetPosition().position;
            dialogue.nodeData.position = new float[2] { position.x, position.y };
        }

        public virtual void OnMove()
        {
            SavePositionInDialogue();
        }

        public virtual void OnRemoveNode()
        {
            graphView.dialogueNodes.Remove(this);
        }

        public virtual void OnCreateLink(Edge edge)
        {

        }

        public virtual void OnRemoveLink(Edge edge)
        {

        }

        public Port CreatePort(Direction portDirection, Port.Capacity capacity = Port.Capacity.Single)
        {
            return InstantiatePort(Orientation.Horizontal, portDirection, capacity, typeof(float));
        }

        public Port AddPort(Port port, string portName)
        {
            // create and add
            port.portName = portName;
            inputContainer.Add(port);

            // refresh
            RefreshExpandedState();
            RefreshPorts();

            return port;
        }

        public Port AddInputPort(string portName = previousPortName)
        {
            return AddPort(CreatePort(Direction.Input, Port.Capacity.Multi), portName);
        }

        public Port AddOutputPort(string portName = nextPortName)
        {
            return AddPort(CreatePort(Direction.Output), portName);
        }

        public Port FindPortByName(string portName)
        {
            // find the port with the specified name in input or output container

            foreach (var port in inputContainer.Children())
            {
                if (port is Port && ((Port)port).portName == portName)
                {
                    return (Port)port;
                }
            }

            foreach (var port in outputContainer.Children())
            {
                if (port is Port && ((Port)port).portName == portName)
                {
                    return (Port)port;
                }
            }

            return null;
        }

    }
}