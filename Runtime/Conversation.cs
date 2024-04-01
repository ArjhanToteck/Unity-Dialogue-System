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
    }
}
