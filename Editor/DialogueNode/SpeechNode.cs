#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace DialogueSystem.Editor
{
    // TODO: forgot to add speaker name input lol
    public class SpeechNode : DialogueNode
    {
        private TextField speakerNameField;
        private Toggle showSpeakerNameBox;
        private TextField speechField;

        public SpeechNode()
        {
            UpdateDialogue(new Speech());

            title = "Speech";

            // show speaker name checkbox
            showSpeakerNameBox = new Toggle("Show Speaker Name")
            {
                value = true
            };
            showSpeakerNameBox.RegisterValueChangedCallback(evt =>
            {
                speakerNameField.SetEnabled(evt.newValue);
                ((Speech)dialogue).showSpeakerName = evt.newValue;
                graphView.SaveConversation();
            });
            contentContainer.Add(showSpeakerNameBox);

            speakerNameField = new TextField("Speaker Name");
            speakerNameField.RegisterValueChangedCallback(evt =>
            {
                ((Speech)dialogue).speakerName = evt.newValue;
                graphView.SaveConversation();
            });
            contentContainer.Add(speakerNameField);

            // add speech text field
            speechField = new TextField("Speech")
            {
                multiline = true
            };

            speechField.style.minWidth = 250;

            speechField.RegisterValueChangedCallback(evt =>
            {
                ((Speech)dialogue).speech = evt.newValue;
                graphView.SaveConversation();
            });
            contentContainer.Add(speechField);

            // add input and output ports
            AddInputPort();
            Port nextPort = AddOutputPort();

            // create link data
            ((Speech)dialogue).nextLink = NodeLinkData.FromPort(nextPort);
        }

        public override void LoadNodeFromDialogue(Dialogue dialogue)
        {
            base.LoadNodeFromDialogue(dialogue);

            // load speaker name
            showSpeakerNameBox.value = ((Speech)dialogue).showSpeakerName;
            speakerNameField.SetEnabled(showSpeakerNameBox.value);
            speakerNameField.value = ((Speech)dialogue).speakerName;

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
#endif