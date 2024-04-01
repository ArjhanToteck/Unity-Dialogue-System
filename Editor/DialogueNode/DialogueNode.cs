using System;
using System.Collections;
using System.Collections.Generic;
using Codice.Client.BaseCommands.Import;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace DialogueSystem.Editor
{
    public abstract class DialogueNode : Node
    {
        public const string previousPortName = "Previous";
        public const string nextPortName = "Next";
        public static readonly Vector2 defaultNodeSize = new Vector2(150, 200);
        public static readonly Vector2 defaultNodePosition = new Vector2(100, 100);

        public string guid;
        public Dialogue dialogue;
        public DialogueGraphView graphView;

        public DialogueNode()
        {
            guid = Guid.NewGuid().ToString();
            SetPosition(new Rect(defaultNodePosition, defaultNodeSize));
        }

        public virtual void LoadFromDialogue(Dialogue dialogue = null)
        {
            this.dialogue = dialogue ?? this.dialogue;

            Vector2 position = new Vector2(this.dialogue.nodeData.position[0], this.dialogue.nodeData.position[1]);
            SetPosition(new Rect(position, defaultNodeSize));
        }

        public void UpdateDialogue(Dialogue dialogue)
        {
            this.dialogue = dialogue;
            dialogue.nodeData.guid = guid;
            SavePositionInDialogue();
        }

        public virtual void OnMove()
        {
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
    }
}