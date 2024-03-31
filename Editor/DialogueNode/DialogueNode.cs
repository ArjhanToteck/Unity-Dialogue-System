using System;
using System.Collections;
using System.Collections.Generic;
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
            this.graphView.AddElement(this);
        }

        public void SetToDefaultPosition()
        {
            // place node at start position
            SetPosition(new Rect(defaultNodePosition, defaultNodeSize));
        }

        public void UpdateDialogueObject()
        {
            dialogue.nodeData.guid = guid;
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