using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using DialogueSystem.Editor;
#endif

namespace DialogueSystem
{
    /// <summary>
    /// The Dialogue objects that compose a conversation.
    /// </summary>
#if UNITY_EDITOR
    [AssociatedNode(typeof(DialogueNode))]
#endif
    [Serializable]
    public abstract class Dialogue
    {
        /// <summary>
        /// The data of the node used to create this dialogue object.
        /// </summary>
        public NodeData nodeData = new NodeData();

#if UNITY_EDITOR
        /// <summary>
        /// Returns the associated DialogueNode-inherited type, as set by the AssociatedNode attribute.
        /// </summary>
        public Type GetAssociatedNodeType()
        {
            var dialogueType = GetType();
            var attribute = (AssociatedNodeAttribute)Attribute.GetCustomAttribute(dialogueType, typeof(AssociatedNodeAttribute));
            return attribute?.NodeType;
        }
#endif
    }
}