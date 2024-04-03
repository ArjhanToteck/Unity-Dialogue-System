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
        private int optionsAdded = 0;

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

        //TODO: add buttons to delete these mfs
        private void AddOption(Option option = null)
        {
            bool optionProvided = option != null;
            Port outputPort;

            // add label for option
            Label optionNameLabel = new Label("Option " + optionsAdded);
            inputContainer.Add(optionNameLabel);

            // add text field
            TextField optionField = new TextField("Option:")
            {
                multiline = true
            };
            optionField.style.minWidth = 250;

            optionField.RegisterValueChangedCallback(evt =>
            {
                // set option in dialogue and update file
                option.option = evt.newValue;
                graphView.SaveConversation();
            });

            inputContainer.Add(optionField);

            // if no option is passed, we create a new one
            if (!optionProvided)
            {
                // create option object and add to decision object
                option = new Option();
                ((Decision)dialogue).options.Add(option);

                // create output port with unique guid as name
                outputPort = AddOutputPort(Guid.NewGuid().ToString());

                // create link
                option.link = NodeLinkData.FromPort(outputPort);

                // update file
                graphView.SaveConversation();
            }
            else
            {
                // recreate output port from option object
                outputPort = AddOutputPort(option.link.outputPortName);
            }

            // set value (either default or loaded value)
            optionField.value = option.option;

            // hide default port name label
            Label oldOutputPortLabel = outputPort.contentContainer.Q<Label>();
            oldOutputPortLabel.style.width = 0;
            oldOutputPortLabel.style.visibility = Visibility.Hidden;

            // add new label
            Label outputPortLabel = new Label(nextPortName);
            outputPort.Add(outputPortLabel);

            optionsAdded++;
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