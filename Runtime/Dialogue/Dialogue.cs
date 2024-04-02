using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using DialogueSystem.Editor;
#endif

namespace DialogueSystem
{
#if UNITY_EDITOR
    [AssociatedNode(typeof(DialogueNode))]
#endif
    [Serializable]

    public abstract class Dialogue
    {
        [NonSerialized]
        public Conversation parentConversation;

        public NodeData nodeData = new NodeData();

        public Type GetAssociatedNodeType()
        {
            var dialogueType = GetType();
            var attribute = (AssociatedNodeAttribute)Attribute.GetCustomAttribute(dialogueType, typeof(AssociatedNodeAttribute));
            return attribute?.NodeType;
        }
    }
}