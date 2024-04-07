#if UNITY_EDITOR
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogueSystem.Editor
{
    /// <summary>
    /// Nodes used to view and edit Dialogue objects from the editor.
    /// </summary>
    public abstract class DialogueNode : Node
    {
        /// <summary>
        /// The default name for input ports.
        /// </summary>
        protected const string inputPortName = "Previous";

        /// <summary>
        /// The default name for output ports.
        /// </summary>
        protected const string outputPortName = "Next";

        /// <summary>
        /// The associated Dialogue object.
        /// </summary>
        public Dialogue dialogue;

        /// <summary>
        /// The graph view displaying the DialogueNode object.
        /// </summary>
        public ConversationGraphView graphView;

        public DialogueNode()
        {
            contentContainer.style.backgroundColor = new Color(0.35f, 0.35f, 0.35f, 0.75f);
        }

        /// <summary>
        /// Called to load node data from a Dialogue object that does not require any other nodes to already be created.
        /// </summary>
        public virtual void LoadNodeFromDialogue(Dialogue dialogue)
        {
            // set dialogue
            this.dialogue = dialogue;

            // set position
            Vector2 position = new Vector2(this.dialogue.nodeData.position[0], this.dialogue.nodeData.position[1]);
            SetPosition(new Rect(position, Vector2.zero));
        }

        /// <summary>
        /// Called to load node data from a Dialogue object that does require other nodes to already be created, including links.
        /// </summary>
        public virtual void LoadLinksFromDialogue(Dialogue dialogue)
        {

        }

        /// <summary>
        /// Creates a link to another node based on a NodeLinkData object.
        /// </summary>
        protected Edge AddLinkFromNodeLinkData(NodeLinkData link)
        {
            if (link != null && link.connectedNodeGuid != null && link.inputPortName != null)
            {
                return graphView.LinkDialogueNodes(this, link.outputPortName, graphView.GetDialogueNodeByGuid(link.connectedNodeGuid), link.inputPortName);
            }

            return null;
        }

        /// <summary>
        /// Overwrite the dialogue field with data from the DialogueNode.
        /// </summary>
        public void UpdateDialogue(Dialogue dialogue = null)
        {
            // set dialogue if needed
            if (dialogue != null)
            {
                this.dialogue = dialogue;
            }

            SavePositionInDialogue();
        }

        /// <summary>
        /// Overwrite the position in the dialogue field with the position of the DialogueNode.
        /// </summary>
        public void SavePositionInDialogue()
        {
            if (dialogue == null)
            {
                return;
            }

            Vector2 position = GetPosition().position;
            dialogue.nodeData.position = new float[2] { position.x, position.y };
        }

        /// <summary>
        /// Called to handle the node being moved in the editor.
        /// </summary>
        public virtual void OnMove()
        {
            SavePositionInDialogue();
        }

        /// <summary>
        /// Called to handle the node being removed in the editor.
        /// </summary>
        public virtual void OnRemoveNode()
        {
            graphView.dialogueNodes.Remove(this);
        }

        /// <summary>
        /// Called to handle the node having an output link created in the editor.
        /// </summary>
        public virtual void OnCreateOutputLink(Edge edge)
        {

        }

        /// <summary>
        /// Called to handle the node having an output link removed in the editor.
        /// </summary>
        public virtual void OnRemoveOutputLink(Edge edge)
        {

        }

        /// <summary>
        /// Adds a spacer to the end of the node to separate entries.
        /// </summary>
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

        /// <summary>
        /// Creates a port.
        /// </summary>
        public Port CreatePort(Direction portDirection, Port.Capacity capacity = Port.Capacity.Single)
        {
            return InstantiatePort(Orientation.Horizontal, portDirection, capacity, typeof(float));
        }

        /// <summary>
        /// Adds a port to the node.
        /// </summary>
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

        /// <summary>
        /// Adds an input port to the node.
        /// </summary>
        public Port AddInputPort(string portName = inputPortName, VisualElement parent = null)
        {
            return AddPort(CreatePort(Direction.Input, Port.Capacity.Multi), portName, parent);
        }

        /// <summary>
        /// Adds an output port to the node.
        /// </summary>
        public Port AddOutputPort(string portName = outputPortName, VisualElement parent = null)
        {
            return AddPort(CreatePort(Direction.Output), portName, parent);
        }

        /// <summary>
        /// Find a port on the node by its name.
        /// </summary>
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

        /// <summary>
        /// Remove all input and output connections from a port.
        /// </summary>
        public void RemoveAllConnectionsFromPort(Port port)
        {
            if (port == null)
            {
                return;
            }

            foreach (Edge edge in port.connections.ToList())
            {
                edge.input.Disconnect(edge);
                edge.output.Disconnect(edge);
                edge.RemoveFromHierarchy();
            }
        }
    }
}
#endif