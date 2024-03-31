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
        public static readonly Vector2 defaultNodeSize = new Vector2(150, 200);
        public static readonly Vector2 defaultNodePosition = new Vector2(100, 100);
        public string guid = Guid.NewGuid().ToString();
        public Dialogue dialogue;
        public DialogueGraphView graphView;

        public DialogueNode(DialogueGraphView graphView, string nodeName = "New Choice Node")
        {
            title = nodeName;
            this.graphView = graphView;
        }

        public void FinishCreatingNode()
        {
            SetPosition(new Rect(defaultNodePosition, defaultNodeSize));
            SavePositionInDialogueObject();
            graphView.AddElement(this);
            graphView.dialogueNodes.Add(this);
            dialogue.nodeData.guid = guid;
        }

        public void SavePositionInDialogueObject()
        {
            Vector2 position = GetPosition().position;
            dialogue.nodeData.position = new float[2] { position.x, position.y };
        }

        public virtual void OnMove()
        {
            SavePositionInDialogueObject();
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

        public Port AddInputPort(string portName = "Previous")
        {
            return AddPort(CreatePort(Direction.Input, Port.Capacity.Multi), portName);
        }

        public Port AddOutputPort(string portName = "Next")
        {
            return AddPort(CreatePort(Direction.Output), portName);
        }
    }
}