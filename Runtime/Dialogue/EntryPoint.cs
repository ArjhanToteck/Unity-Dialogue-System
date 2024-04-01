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
    [AssociatedNode(typeof(EntryPointNode))]
#endif
    [Serializable]
    public class EntryPoint : Dialogue
    {
        public bool entryPoint = true;
        public NodeLinkData nextLink = null;
    }
}