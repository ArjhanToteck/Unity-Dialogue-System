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
    public class ConversationEditorWindow : EditorWindow
    {
        public ConversationGraphView graphView;
        public string savePath;

        [MenuItem("Window/Dialogue System/Conversation")]
        public static ConversationEditorWindow OpenConversationEditorWindow()
        {
            ConversationEditorWindow window = GetWindow<ConversationEditorWindow>();
            window.titleContent = new GUIContent("Conversation");

            return window;
        }

        private void OnEnable()
        {
            AddGraphView();
            CreateToolBar();

            // check if we have a file path, but not loaded yet (this happens on recompile, for example)
            if (!graphView.doneLoadingFile && savePath != null)
            {
                ConversationFileManager.LoadConversation(graphView, savePath);
            }
        }

        private void OnDisable()
        {
            // remove window
            rootVisualElement.Remove(graphView);
        }

        private void AddGraphView()
        {
            // create view
            graphView = new ConversationGraphView()
            {
                name = "Conversation",
                window = this
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
                graphView.AddDialogueNode(new DecisionNode());
            })
            {
                text = "Create Choice Node"
            };
            toolbar.Add(createChoiceNodeButton);

            rootVisualElement.Add(toolbar);
        }
    }
}