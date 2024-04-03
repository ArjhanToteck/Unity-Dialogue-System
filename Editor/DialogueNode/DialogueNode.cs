using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogueSystem.Editor
{
    public abstract class DialogueNode : Node
    {
        protected const string previousPortName = "Previous";
        protected const string nextPortName = "Next";
        // TODO: refactor, let's only store guid in dialogue.nodeData for simplicity
        public string guid = Guid.NewGuid().ToString();
        public Dialogue dialogue;
        public ConversationGraphView graphView;

        public DialogueNode()
        {
            SetPosition(new Rect(NodeData.defaultPosition, NodeData.defaultSize));
            contentContainer.style.backgroundColor = new Color(0.35f, 0.35f, 0.35f, 0.75f);
        }

        public virtual void LoadNodeFromDialogue(Dialogue dialogue)
        {
            // set dialogue
            this.dialogue = dialogue;

            // set guid
            guid = dialogue.nodeData.guid;

            // set position
            Vector2 position = new Vector2(this.dialogue.nodeData.position[0], this.dialogue.nodeData.position[1]);
            SetPosition(new Rect(position, NodeData.defaultSize));
        }

        public virtual void LoadLinksFromDialogue(Dialogue dialogue)
        {

        }

        protected Edge AddLinkFromNodeLinkData(NodeLinkData link)
        {
            if (link != null && link.connectedNodeGuid != null && link.inputPortName != null)
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

        public VisualElement AddSpacer(VisualElement parent = null)
        {
            parent = parent ?? contentContainer;

            VisualElement spacer = new VisualElement();
            spacer.style.backgroundColor = new StyleColor(Color.black);
            spacer.style.height = 1;
            spacer.style.marginBottom = 5;
            parent.Add(spacer);

            return spacer;
        }

        public Port CreatePort(Direction portDirection, Port.Capacity capacity = Port.Capacity.Single)
        {
            return InstantiatePort(Orientation.Horizontal, portDirection, capacity, typeof(float));
        }

        public Port AddPort(Port port, string portName, VisualElement parent = null)
        {
            parent = parent ?? contentContainer;

            // create and add
            port.portName = portName;
            parent.Add(port);

            // refresh
            RefreshExpandedState();
            RefreshPorts();

            return port;
        }

        public Port AddInputPort(string portName = previousPortName, VisualElement parent = null)
        {
            return AddPort(CreatePort(Direction.Input, Port.Capacity.Multi), portName, parent);
        }

        public Port AddOutputPort(string portName = nextPortName, VisualElement parent = null)
        {
            return AddPort(CreatePort(Direction.Output), portName, parent);
        }

        public Port FindPortByName(string portName, VisualElement parent = null)
        {
            parent = parent ?? contentContainer;

            foreach (var element in parent.Children())
            {
                if (element is Port port && port.portName == portName)
                {
                    return port;
                }

                // if the element is a container, loop back recursively
                if (element is VisualElement container)
                {
                    Port foundPort = FindPortByName(portName, container);
                    if (foundPort != null)
                    {
                        return foundPort;
                    }
                }
            }

            return null;
        }

        public void RemoveConnectionsFromPort(Port port)
        {
            if (port == null)
            {
                return;
            }

            foreach (Edge edge in port.connections) // ToList() creates a copy to avoid modifying the collection while iterating
            {
                edge.input.Disconnect(edge);
                edge.output.Disconnect(edge);
                edge.RemoveFromHierarchy();
            }
        }
    }
}