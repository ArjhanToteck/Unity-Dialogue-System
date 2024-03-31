
using System;

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

        public string connectedNodeGuid;

#if UNITY_EDITOR
        public NodeLinkData(Edge edge)
        {
            portName = edge.input.portName;
            connectedNodeGuid = ((DialogueNode)edge.output.node).guid;
        }
#endif
    }
}