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
    /// Marks the start of a conversation. Every conversation must have exactly one of these.
    /// </summary>
#if UNITY_EDITOR
    [AssociatedNode(typeof(EntryPointNode))]
#endif
    [Serializable]
    public class EntryPoint : Dialogue
    {
        /// <summary>
        /// The link pointing to the next dialogue object. <c>null</c> indicates the end of the conversation.
        /// </summary>
        public NodeLinkData nextLink = null;
    }
}