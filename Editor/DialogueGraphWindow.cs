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
    public class DialogueGraphWindow : EditorWindow
    {
        private const string dialogueExtension = ".conversation";

        private DialogueGraphView graphView;
        private string savePath;

        public static DialogueGraphWindow OpenDialogueGraphWindow()
        {
            DialogueGraphWindow window = GetWindow<DialogueGraphWindow>();
            window.titleContent = new GUIContent("Dialogue");

            return window;
        }

        [OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            // check what asset was opened
            string assetPath = AssetDatabase.GetAssetPath(instanceID);

            // check if dialogue asset was opened
            if (assetPath.EndsWith(dialogueExtension))
            {
                // open editor window and make note of the loaded asset path
                var window = OpenDialogueGraphWindow();
                window.savePath = assetPath;
                window.graphView.savePath = assetPath;

                // clear old graph view
                window.graphView.ClearGraphView();

                // load conversation
                ConversationFileManager.LoadConversation(window.graphView, window.savePath);
                return true;
            }

            return false;
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