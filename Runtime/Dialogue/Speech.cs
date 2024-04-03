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
    [AssociatedNode(typeof(SpeechNode))]
#endif
    [Serializable]
    public class Speech : Dialogue
    {
        public string speakerName = "";
        public bool showSpeakerName = true;
        public string speech = "";
        public NodeLinkData nextLink = null;
    }
}