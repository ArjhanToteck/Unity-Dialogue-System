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
    public class DialogueSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        public static List<SearchTreeEntry> treeEntry = new List<SearchTreeEntry>
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

        public ConversationGraphView graphView;
        public ConversationEditorWindow window;

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            return treeEntry;
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
                dialogueNode.SetPosition(new Rect(localMousePosition, NodeData.defaultSize));
                dialogueNode.UpdateDialogue();

                // add to graph view
                graphView.AddDialogueNode(dialogueNode);
            }

            return true;
        }
    }
}
#endif