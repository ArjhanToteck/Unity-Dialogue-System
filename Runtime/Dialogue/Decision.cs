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
    [AssociatedNode(typeof(DecisionNode))]
#endif
    [Serializable]
    public partial class Decision : Dialogue
    {
        public List<Option> options = new List<Option>();
    }
}