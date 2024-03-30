using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace DialogueSystem
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "Conversation.conversation.asset", menuName = "Dialogue System/Conversation", order = 1)]

    public class Conversation : ScriptableObject
    {
        public List<Dialogue> dialogue = new List<Dialogue>();
    }
}
