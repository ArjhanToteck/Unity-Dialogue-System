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
        public ChoiceNode(DialogueGraphView graphView, string nodeName = "New Choice Node") : base(graphView, nodeName)
        {
            dialogue = new Choice();

            AddInputPort();

            var addOptionButton = new Button(() =>
            {
                AddChoice();
            });
            addOptionButton.text = "Add Option";
            titleContainer.Add(addOptionButton);

            FinishCreatingNode();
        }

        private void AddChoice()
        {
            int choiceCount = outputContainer.Query("connector").ToList().Count;

            Choice.Option option = new Choice.Option
            {
                option = "Option " + choiceCount
            };

            AddOutputPort(option.option);

            ((Choice)dialogue).options.Add(option);
        }
    }
}