#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using DialogueSystem.Editor;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogueSystem.Editor
{
    /// <summary>
    /// The search window used to create DialogueNode objects in the graph view context menu.
    /// </summary>
    public class DialogueSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        /// <summary>
        /// The options for creating DialogueNode objects in the editor. To implement custom DialogueNode types, an option to create them should be added here.
        /// </summary>
        public static List<SearchTreeEntry> searchTree = new List<SearchTreeEntry>
        {
            new SearchTreeGroupEntry(new GUIContent("Dialogue")),
            new SearchTreeEntry(new GUIContent("Speech"))
            {
                userData = typeof(SpeechNode),
                level = 1
            },
            new SearchTreeEntry(new GUIContent("Decision"))
            {
                userData = typeof(DecisionNode),
                level = 1
            }
        };

        /// <summary>
        /// The graph view where the search window is open.
        /// </summary>
        public ConversationGraphView graphView;

        /// <summary>
        /// The editor window where the search window is open.
        /// </summary>
        public ConversationEditorWindow window;

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            return searchTree;
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            var worldMousePosition = window.rootVisualElement.ChangeCoordinatesTo(window.rootVisualElement.parent, context.screenMousePosition - window.position.position);
            var localMousePosition = graphView.contentViewContainer.WorldToLocal(worldMousePosition);

            Type nodeType = (Type)SearchTreeEntry.userData;

            // check if we have a type inherited from dialogue node
            if (nodeType.IsSubclassOf(typeof(DialogueNode)))
            {
                // create node from type
                DialogueNode dialogueNode = (DialogueNode)Activator.CreateInstance(nodeType);

                // set position to mouse and update dialogue object to match
                dialogueNode.SetPosition(new Rect(localMousePosition, Vector2.zero));
                dialogueNode.UpdateDialogue();

                // add to graph view
                graphView.AddDialogueNode(dialogueNode);
            }

            return true;
        }
    }
}
#endif