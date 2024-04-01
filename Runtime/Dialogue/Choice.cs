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
    [AssociatedNode(typeof(ChoiceNode))]
#endif
    [Serializable]
    // TODO: maybe rename as decision to avoid confusion with option objects?
    public partial class Choice : Dialogue
    {
        public List<Option> options = new List<Option>();
    }
}