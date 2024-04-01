
using System;
using System.Linq;

#if UNITY_EDITOR
using DialogueSystem.Editor;
using UnityEditor.Experimental.GraphView;
#endif

namespace DialogueSystem
{
    [Serializable]
    public class NodeLinkData
    {
        public string portName;
        public string connectedNodeGuid = null;

#if UNITY_EDITOR
        public static NodeLinkData FromEdge(Edge edge)
        {
            NodeLinkData link = new NodeLinkData
            {
                portName = edge.output.portName
            };

            // make sure the link is connected to something
            if (((DialogueNode)edge.input.node) != null)
            {
                // set connection
                link.connectedNodeGuid = ((DialogueNode)edge.input.node).guid;
            }

            return link;
        }

        public static NodeLinkData FromPort(Port port)
        {
            // check if there is a link on the port
            Edge edge = port.connections.FirstOrDefault();
            if (edge != null)
            {
                return NodeLinkData.FromEdge(edge);
            }

            NodeLinkData link = new NodeLinkData
            {
                portName = port.portName
            };

            return link;
        }
#endif
    }
}