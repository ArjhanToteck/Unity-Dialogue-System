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
    /// A simple dialogue from one speaker or entity, including a narrator.
    /// </summary>
#if UNITY_EDITOR
    [AssociatedNode(typeof(SpeechNode))]
#endif
    [Serializable]
    public class Speech : Dialogue
    {
        /// <summary>
        /// The name of the character speaking.
        /// </summary>
        public string speakerName = "";

        /// <summary>
        /// Whether or not a character name should be shown. This could be disabled for things like narration.
        /// </summary>
        public bool showSpeakerName = true;

        /// <summary>
        /// The actual text spoken.
        /// </summary>
        public string speech = "";

        /// <summary>
        /// The link pointing to the next dialogue object. <c>null</c> indicates the end of the conversation.
        /// </summary>
        public NodeLinkData nextLink = null;
    }
}