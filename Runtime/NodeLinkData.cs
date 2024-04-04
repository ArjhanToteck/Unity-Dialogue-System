
using System;
using System.Linq;

#if UNITY_EDITOR
using DialogueSystem.Editor;
using UnityEditor.Experimental.GraphView;
#endif

namespace DialogueSystem
{
    /// <summary>
    /// Stores the data to recreate a link between DialogueNode objects in the editor as well as being useful for finding the next Dialogue in a sequence.
    /// </summary>
    [Serializable]
    public class NodeLinkData
    {
        /// <summary>
        /// The portName of the output port.
        /// </summary>
        public string outputPortName;

        /// <summary>
        /// The portName of the input port.
        /// </summary>
        public string inputPortName = null;

        /// <summary>
        /// The guid property of the connected DialogueNode.
        /// </summary>
        public string connectedNodeGuid = null;

#if UNITY_EDITOR
        /// <summary>
        /// Editor-only method that creates an instance of NodeLinkData from an Edge.
        /// </summary>
        public static NodeLinkData FromEdge(Edge edge)
        {
            NodeLinkData link = new NodeLinkData
            {
                outputPortName = edge.output.portName,
                inputPortName = edge.input.portName,
                connectedNodeGuid = ((DialogueNode)edge.input.node).dialogue.nodeData.guid
            };

            return link;
        }

        /// <summary>
        /// Editor-only method that creates an instance of NodeLinkData from a Port.
        /// </summary>
        public static NodeLinkData FromPort(Port port)
        {
            // check if there is a link on the port
            Edge edge = port.connections.FirstOrDefault();
            if (edge != null)
            {
                return FromEdge(edge);
            }

            NodeLinkData link = new NodeLinkData
            {
                outputPortName = port.portName
            };

            return link;
        }
#endif
    }
}