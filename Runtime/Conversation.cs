using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace DialogueSystem
{
    [System.Serializable]

    public class Conversation
    {
        public List<Dialogue> dialogues = new List<Dialogue>();

        public static Conversation DefaultConversation()
        {
            // create dialogue with entry point
            Conversation conversation = new Conversation();
            conversation.AddDialogue(new EntryPoint());

            return conversation;
        }

        public void AddDialogue(Dialogue dialogue)
        {
            dialogue.parentConversation = this;
            dialogues.Add(dialogue);
        }
    }
}
