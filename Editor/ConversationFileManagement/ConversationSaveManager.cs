#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System;
using UnityEditor.Callbacks;

namespace DialogueSystem.Editor
{
    /// <summary>
    /// Manages saving and loading from a conversation file.
    /// </summary>
    // TODO: probably refactor this and put some of this functionality in runtime for when the conversation file is actually used
    public static class ConversationSaveManager
    {
        /// <summary>
        /// The file extension for conversation files.
        /// </summary>
        public const string conversationExtension = ".conversation";

        /// <summary>
        /// The default name for conversation files.
        /// </summary>
        public const string defaultConversationFileName = "Conversation" + conversationExtension;

        private static readonly JsonSerializerSettings settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto
        };

        /// <summary>
        /// Handles creating new conversation files from the editor.
        /// </summary>
        [MenuItem("Assets/Create/Dialogue System/Conversation")]
        public static void CreateConversationAsset()
        {
            // get currently selected object
            UnityEngine.Object selectedObject = Selection.activeObject;

            // get path
            string folderPath = AssetDatabase.GetAssetPath(selectedObject);

            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                Debug.LogError("Attempted to create conversation in invalid folder");
                return;
            }

            // create file path
            string filePath = Path.Combine(folderPath, defaultConversationFileName);
            filePath = AssetDatabase.GenerateUniqueAssetPath(filePath);

            // create default conversation and save
            Conversation defaultConversation = Conversation.DefaultConversation();
            SaveConversation(defaultConversation, filePath);

            // select created file
            AssetDatabase.Refresh();
            UnityEngine.Object conversationFile = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(filePath);
            Selection.activeObject = conversationFile;
        }

        /// <summary>
        /// Handles opening conversation files from the editor.
        /// </summary>
        [OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            // check what asset was opened
            string assetPath = AssetDatabase.GetAssetPath(instanceID);

            // check if dialogue asset was opened
            if (assetPath.EndsWith(ConversationSaveManager.conversationExtension))
            {
                // open editor window and make note of the loaded asset path
                var window = ConversationEditorWindow.OpenConversationEditorWindow();
                window.savePath = assetPath;

                // load conversation
                LoadConversation(window.graphView, assetPath);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Saves a conversation given a list of dialogue nodes and path.
        /// </summary>
        public static void SaveConversation(List<DialogueNode> dialogueNodes, string filePath)
        {
            var conversation = new Conversation();

            // loop through each node
            foreach (DialogueNode dialogueNode in dialogueNodes)
            {
                conversation.AddDialogue(dialogueNode.dialogue);
            }

            // save as conversation now
            SaveConversation(conversation, filePath);
        }

        /// <summary>
        /// Saves a conversation given a graph view and path.
        /// </summary>
        public static void SaveConversation(Conversation conversation, string filePath)
        {
            string conversationJson = JsonConvert.SerializeObject(conversation, settings);
            File.WriteAllText(filePath, conversationJson);
        }

        /// <summary>
        /// Loads a conversation into a graph view given a path.
        /// </summary>
        public static Conversation LoadConversation(ConversationGraphView graphView, string filePath)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }

            // deserialize conversation
            string json = File.ReadAllText(filePath);
            Conversation conversation = JsonConvert.DeserializeObject<Conversation>(json, settings);

            // mark as loading and clear
            graphView.conversationLoaded = false;
            graphView.ClearGraphView();

            // loop through dialogue objects
            foreach (Dialogue dialogue in conversation.dialogues)
            {
                // get associated node type of object
                Type nodeType = dialogue.GetAssociatedNodeType();

                // create node and load data from dialogue
                DialogueNode dialogueNode = (DialogueNode)Activator.CreateInstance(nodeType);
                dialogueNode.graphView = graphView;
                dialogueNode.LoadNodeFromDialogue(dialogue);

                // add to graph view
                graphView.AddDialogueNode(dialogueNode);
            }

            // loop through dialogue objects again (we need to load links now)
            foreach (DialogueNode dialogueNode in graphView.dialogueNodes)
            {
                dialogueNode.LoadLinksFromDialogue(dialogueNode.dialogue);
            }

            // mark as no longer loading
            graphView.conversationLoaded = true;
            graphView.OnFinishedLoading();

            return conversation;
        }
    }
}
#endif