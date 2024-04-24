using ReturnableUnityEvents;
using UnityEditor;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEngine.Events;
using System;
using UnityEditor.Experimental.GraphView;

namespace DialogueSystem.Editor
{
    public class BranchNode : DialogueNode
    {
        public BranchNode()
        {
            UpdateDialogue(new Branch());

            title = "Branch";

            // add returnable event field
            
            // get serialized property iterator
            SerializedObject serializedCondition = new SerializedObject(ScriptableObject.CreateInstance<ReturnableUnityEventSerializableWrapper>());
            SerializedProperty conditionProperty = serializedCondition.GetIterator();

            // start searching
            conditionProperty.Next(true);
            
            // skip 1st item (script reference)
            conditionProperty.NextVisible(false);

            // loop through remaining properties
            while (conditionProperty.NextVisible(false))
            {
                PropertyField propertyField = new PropertyField(conditionProperty);
                propertyField.Bind(conditionProperty);
                contentContainer.Add(propertyField);
                
                Debug.Log(conditionProperty.displayName);
                Debug.Log(propertyField);
            }
            
            EditorGUILayout.PropertyField(serializedCondition.GetIterator());

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