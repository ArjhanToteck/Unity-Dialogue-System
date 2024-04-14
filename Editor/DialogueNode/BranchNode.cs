#if UNITY_EDITOR
using System;
using ReturnableUnityEvents;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogueSystem.Editor
{
    public class BranchNode : DialogueNode
    {
        public BranchNode()
        {
            UpdateDialogue(new Branch());

            title = "Branch";

            // add returnable event field
            SerializedObject serializedCondition = new SerializedObject((Branch)dialogue);
            PropertyField conditionField = new PropertyField(serializedCondition.FindProperty("condition"));
            Debug.Log(serializedCondition.FindProperty("condition").managedReferenceValue);
            contentContainer.Add(conditionField);
            Debug.Log("done");

            // add input and output ports
            AddInputPort();
            Port truePort = AddOutputPort("True");
            Port falsePort = AddOutputPort("False");

            // create link data
            ((Branch)dialogue).trueLink = NodeLinkData.FromPort(truePort);
            ((Branch)dialogue).falseLink = NodeLinkData.FromPort(falsePort);
        }

        public override void LoadNodeFromDialogue(Dialogue dialogue)
        {
            base.LoadNodeFromDialogue(dialogue);

        }

        public override void LoadLinksFromDialogue(Dialogue dialogue)
        {
            base.LoadLinksFromDialogue(dialogue);

            // create link (doesn't do anything if not applicable)
            AddLinkFromNodeLinkData(((Branch)dialogue).trueLink);
            AddLinkFromNodeLinkData(((Branch)dialogue).falseLink);
        }

        public override void OnCreateOutputLink(Edge edge)
        {
            if (edge.output.portName == "True")
            {
                ((Branch)dialogue).trueLink = NodeLinkData.FromEdge(edge);
            }
            else if (edge.output.portName == "False")
            {
                ((Branch)dialogue).falseLink = NodeLinkData.FromEdge(edge);
            }
        }

        public override void OnRemoveOutputLink(Edge edge)
        {
            if (edge.output.portName == outputPortName)
            {
                ((Speech)dialogue).nextLink = null;
            }
        }
    }
}
#endif