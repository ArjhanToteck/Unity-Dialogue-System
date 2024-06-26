#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
namespace DialogueSystem.Editor
{
    // TODO: possibly rename to ConversationGraphWindow
    public class ConversationEditorWindow : EditorWindow
    {
        public static List<ConversationEditorWindow> openWindows = new List<ConversationEditorWindow>();
        public ConversationGraphView graphView;
        public string savePath;

        /// <summary>
        /// Creates and opens a new ConversationEditorWindow.
        /// </summary>
        [MenuItem("Window/Dialogue System/Conversation")]
        public static ConversationEditorWindow OpenConversationEditorWindow()
        {
            ConversationEditorWindow window = GetWindow<ConversationEditorWindow>();
            window.titleContent = new GUIContent("Conversation");

            return window;
        }

        /// <summary>
        /// Reset the window to its initial state (no conversation selected).
        /// </summary>
        public void ResetWindow()
        {
            if (openWindows.Contains(this))
            {
                openWindows.Add(this);
            }

            AddGraphView();

            // check if we have a file path, but not loaded yet (this happens on recompile, for example)
            if (!graphView.conversationLoaded && savePath != null)
            {
                ConversationSaveManager.LoadConversation(graphView, savePath);
            }
        }

        private void OnEnable()
        {
            ResetWindow();
        }

        private void OnDisable()
        {
            if (openWindows.Contains(this))
            {
                openWindows.Remove(this);
            }

            // remove graph view
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
    }
}
#endif