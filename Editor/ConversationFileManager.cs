using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System;
using UnityEditor.Callbacks;

namespace DialogueSystem.Editor
{

    public static class ConversationFileManager
    {
        public const string conversationExtension = ".conversation";
        public const string defaultConversationFileName = "Conversation" + conversationExtension;

        static readonly JsonSerializerSettings settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto
        };

        // handle creating conversation files in editor
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

        // handle opening conversation files in editor
        [OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            // check what asset was opened
            string assetPath = AssetDatabase.GetAssetPath(instanceID);

            // check if dialogue asset was opened
            if (assetPath.EndsWith(ConversationFileManager.conversationExtension))
            {
                // open editor window and make note of the loaded asset path
                var window = ConversationEditorWindow.OpenConversationEditorWindow();
                window.graphView.savePath = assetPath;

                // load conversation
                LoadConversation(window.graphView, assetPath);
                return true;
            }

            return false;
        }

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

        public static void SaveConversation(Conversation conversation, string filePath)
        {
            string conversationJson = JsonConvert.SerializeObject(conversation, settings);
            File.WriteAllText(filePath, conversationJson);
        }

        public static Conversation LoadConversation(ConversationGraphView graphView, string filePath)
        {
            // deserialize conversation
            string json = File.ReadAllText(filePath);
            Conversation conversation = JsonConvert.DeserializeObject<Conversation>(json, settings);

            // mark as loading and clear
            graphView.loadingFile = true;
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
            graphView.loadingFile = false;

            return conversation;
        }
    }
}