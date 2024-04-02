using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogueSystem.Editor
{
    public class SpeechNode : DialogueNode
    {
        private TextField speechField;

        public SpeechNode()
        {
            UpdateDialogue(new Speech());

            title = "Speech";

            // add speech text field
            speechField = new TextField("Dialogue:")
            {
                multiline = true
            };

            speechField.style.minWidth = 250;

            speechField.RegisterValueChangedCallback(evt =>
            {
                ((Speech)dialogue).speech = evt.newValue;
                graphView.SaveConversation();
            });

            inputContainer.Add(speechField);

            // add input and output ports
            AddInputPort();
            Port nextPort = AddOutputPort();

            // create link data
            ((Speech)dialogue).nextLink = NodeLinkData.FromPort(nextPort);
        }

        public override void LoadNodeFromDialogue(Dialogue dialogue)
        {
            base.LoadNodeFromDialogue(dialogue);

            // load speech text
            speechField.value = ((Speech)dialogue).speech;
        }

        public override void LoadLinksFromDialogue(Dialogue dialogue)
        {
            base.LoadLinksFromDialogue(dialogue);

            // create link (doesn't do anything if not applicable)
            AddLinkFromNodeLinkData(((Speech)dialogue).nextLink);
        }

        public override void OnCreateLink(Edge edge)
        {
            if (edge.output.portName == nextPortName)
            {
                ((Speech)dialogue).nextLink = NodeLinkData.FromEdge(edge);
            }
        }

        public override void OnRemoveLink(Edge edge)
        {
            if (edge.output.portName == nextPortName)
            {
                ((Speech)dialogue).nextLink = null;
            }
        }
    }
}