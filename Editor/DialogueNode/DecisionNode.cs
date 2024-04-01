using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogueSystem.Editor
{
    public class DecisionNode : DialogueNode
    {
        public DecisionNode()
        {
            UpdateDialogue(new Decision());

            title = "Decision";

            AddInputPort();

            var addOptionButton = new Button(() =>
            {
                AddOption();
            });
            addOptionButton.text = "Add Option";
            titleContainer.Add(addOptionButton);
        }

        public override void LoadNodeFromDialogue(Dialogue dialogue = null)
        {
            base.LoadNodeFromDialogue(dialogue);

            // add each option defined in the decision object
            foreach (Option option in ((Decision)dialogue).options)
            {
                AddOption(option);
            }
        }

        public override void LoadLinksFromDialogue(Dialogue dialogue)
        {
            base.LoadLinksFromDialogue(dialogue);

            // add each option defined in the decision object
            foreach (Option option in ((Decision)dialogue).options)
            {
                // create link (doesn't do anything if not applicable)
                AddLinkFromNodeLinkData(option.link);
            }
        }

        private void AddOption(Option option = null)
        {
            // if no option is passed, we create a new one
            if (option == null)
            {
                int optionIndex = ((Decision)dialogue).options.Count;

                // create option object and add to decision object
                option = new Option
                {
                    option = "Option " + optionIndex
                };
                ((Decision)dialogue).options.Add(option);

                // create output port
                Port outputPort = AddOutputPort(option.option);

                // create link
                option.link = NodeLinkData.FromPort(outputPort);

                // update file
                graphView.SaveConversation();
            }
            else
            {
                // create output port
                AddOutputPort(option.option);
            }
        }

        public override void OnCreateLink(Edge edge)
        {
            // loop through options
            foreach (Option option in ((Decision)dialogue).options)
            {
                // check if option matches the new edge
                if (option.link.outputPortName == edge.output.portName)
                {
                    // set guid of connected node
                    option.link.connectedNodeGuid = ((DialogueNode)edge.input.node).guid;
                    option.link.inputPortName = edge.input.portName;

                    break;
                }
            }
        }

        public override void OnRemoveLink(Edge edge)
        {
            // loop through options
            foreach (Option option in ((Decision)dialogue).options)
            {
                // check if option matches the deleted edge
                if (option.link.outputPortName == edge.output.portName)
                {
                    // remove guid of connected node
                    option.link.connectedNodeGuid = null;
                }

                break;
            }
        }
    }
}