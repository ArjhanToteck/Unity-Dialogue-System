using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace DialogueSystem
{
    [System.Serializable]

    public class Conversation
    {
        public List<Dialogue> dialogue = new List<Dialogue>();

        public static Conversation DefaultConversation()
        {
            // create dialogue with entry point
            Conversation conversation = new Conversation();
            conversation.dialogue.Add(new EntryPoint());

            return conversation;
        }
    }
}
