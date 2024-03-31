using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace DialogueSystem.Editor
{
    public static class ConversationFileManager
    {
        public static void SaveConversation(List<DialogueNode> dialogueNodes, string filePath)
        {
            var conversation = ScriptableObject.CreateInstance<Conversation>();

            // loop through each node
            foreach (DialogueNode dialogueNode in dialogueNodes)
            {
                conversation.dialogue.Add(dialogueNode.dialogue);
            }

            string conversationJson = JsonConvert.SerializeObject(conversation);
            File.WriteAllText(filePath, conversationJson);
        }

        public static Conversation LoadConversation(DialogueGraphView graphView, string filePath)
        {
            return null;
        }
    }
}