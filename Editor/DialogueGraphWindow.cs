using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
namespace DialogueSystem.Editor
{
    // TODO: possibly rename to ConversationGraphWindow
    public class DialogueGraphWindow : EditorWindow
    {
        public DialogueGraphView graphView;

        public static DialogueGraphWindow OpenDialogueGraphWindow()
        {
            DialogueGraphWindow window = GetWindow<DialogueGraphWindow>();
            window.titleContent = new GUIContent("Dialogue");

            // clear old graph view
            window.graphView.ClearGraphView();

            return window;
        }

        private void OnEnable()
        {
            CreateGraphView();
            CreateToolBar();
        }

        private void CreateGraphView()
        {
            // create view
            graphView = new DialogueGraphView()
            {
                name = "Dialogue"
            };

            graphView.StretchToParentSize();

            // actually display view
            rootVisualElement.Add(graphView);
        }

        private void CreateToolBar()
        {
            Toolbar toolbar = new Toolbar();

            // create speech node
            var createSpeechNodeButton = new Button(() =>
            {
                graphView.AddDialogueNode(new SpeechNode());
            });
            createSpeechNodeButton.text = "Create Speech Node";
            toolbar.Add(createSpeechNodeButton);

            // create choice node
            var createChoiceNodeButton = new Button(() =>
            {
                graphView.AddDialogueNode(new ChoiceNode());
            })
            {
                text = "Create Choice Node"
            };
            toolbar.Add(createChoiceNodeButton);

            rootVisualElement.Add(toolbar);
        }

        private void OnDisable()
        {
            // remove window
            rootVisualElement.Remove(graphView);
        }
    }
}