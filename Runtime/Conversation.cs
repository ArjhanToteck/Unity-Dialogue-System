using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace DialogueSystem
{
    /// <summary>
    /// A container for dialogue.
    /// </summary>
    [System.Serializable]
    public class Conversation
    {
        /// <summary>
        /// A list of the Dialogue objects that make up the conversation.
        /// </summary>
        public List<Dialogue> dialogues = new List<Dialogue>();

        /// <summary>
        /// Returns the default conversation with an entry point.
        /// </summary>
        public static Conversation DefaultConversation()
        {
            // create dialogue with entry point
            Conversation conversation = new Conversation();
            conversation.AddDialogue(new EntryPoint());

            return conversation;
        }

        // Add dialogue to the Conversation.
        public void AddDialogue(Dialogue dialogue)
        {
            dialogues.Add(dialogue);
        }
    }
}
