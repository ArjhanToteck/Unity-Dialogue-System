using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System;

namespace DialogueSystem.Editor
{

    public static class ConversationFileManager
    {

        static readonly JsonSerializerSettings settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto
        };

        public static void SaveConversation(List<DialogueNode> dialogueNodes, string filePath)
        {
            var conversation = new Conversation();

            // loop through each node
            foreach (DialogueNode dialogueNode in dialogueNodes)
            {
                conversation.dialogue.Add(dialogueNode.dialogue);
            }

            string conversationJson = JsonConvert.SerializeObject(conversation, settings);
            File.WriteAllText(filePath, conversationJson);
        }

        public static Conversation LoadConversation(DialogueGraphView graphView, string filePath)
        {
            // deserialize conversation
            string json = File.ReadAllText(filePath);
            Conversation conversation = JsonConvert.DeserializeObject<Conversation>(json, settings);

            // loop through dialogue objects
            foreach (Dialogue dialogue in conversation.dialogue)
            {
                // get associated node type of object
                Type nodeType = dialogue.GetAssociatedNodeType();

                // create node and load data from dialogue
                DialogueNode dialogueNode = (DialogueNode)Activator.CreateInstance(nodeType);
                dialogueNode.LoadFromDialogue(dialogue);

                // add to graph view
                graphView.AddDialogueNode(dialogueNode);
            }

            return conversation;
        }
    }
}