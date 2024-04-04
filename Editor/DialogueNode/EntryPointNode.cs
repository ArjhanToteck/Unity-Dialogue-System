#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;

namespace DialogueSystem.Editor
{
    public class EntryPointNode : DialogueNode
    {
        public EntryPointNode()
        {
            UpdateDialogue(new EntryPoint());

            title = "Entry Point";

            // entry node shouldn't be deletable
            capabilities &= ~Capabilities.Deletable;

            Port nextPort = AddOutputPort();

            // create link data
            ((EntryPoint)dialogue).nextLink = NodeLinkData.FromPort(nextPort);
        }

        public override void LoadLinksFromDialogue(Dialogue dialogue)
        {
            base.LoadLinksFromDialogue(dialogue);

            // create link (doesn't do anything if not applicable)
            AddLinkFromNodeLinkData(((EntryPoint)dialogue).nextLink);
        }

        public override void OnCreateOutputLink(Edge edge)
        {
            if (edge.output.portName == outputPortName)
            {
                ((EntryPoint)dialogue).nextLink = NodeLinkData.FromEdge(edge);
            }
        }

        public override void OnRemoveOutputLink(Edge edge)
        {
            if (edge.output.portName == outputPortName)
            {
                ((EntryPoint)dialogue).nextLink = null;
            }
        }
    }
}
#endif