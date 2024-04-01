using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogueSystem.Editor
{
    public class ChoiceNode : DialogueNode
    {
        public ChoiceNode()
        {
            UpdateDialogue(new Choice());

            title = "Choice";

            AddInputPort();

            var addOptionButton = new Button(() =>
            {
                AddOption();
            });
            addOptionButton.text = "Add Option";
            titleContainer.Add(addOptionButton);
        }

        private void AddOption()
        {
            int choiceCount = ((Choice)dialogue).options.Count;

            // create option object and add to choice object
            Option option = new Option
            {
                option = "Option " + choiceCount
            };
            ((Choice)dialogue).options.Add(option);

            // create output port
            Port outputPort = AddOutputPort(option.option);

            // create link
            option.link = NodeLinkData.FromPort(outputPort);
        }

        public override void OnCreateLink(Edge edge)
        {
            // loop through options
            foreach (Option option in ((Choice)dialogue).options)
            {
                // check if option matches the new edge
                if (option.link.portName == edge.output.portName)
                {
                    // set guid of connected node
                    option.link.connectedNodeGuid = ((DialogueNode)edge.input.node).guid;

                    break;
                }
            }
        }

        public override void OnRemoveLink(Edge edge)
        {
            // loop through options
            foreach (Option option in ((Choice)dialogue).options)
            {
                // check if option matches the deleted edge
                if (option.link.portName == edge.output.portName)
                {
                    // remove guid of connected node
                    option.link.connectedNodeGuid = null;
                }

                break;
            }
        }
    }
}