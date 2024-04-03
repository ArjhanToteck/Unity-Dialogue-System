
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
        public string outputPortName;
        public string inputPortName = null;
        public string connectedNodeGuid = null;

#if UNITY_EDITOR
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