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
    /// A point in the conversation where the user makes a decision, choosing from a list of options.
    /// </summary>
#if UNITY_EDITOR
    [AssociatedNode(typeof(DecisionNode))]
#endif
    [Serializable]
    public partial class Decision : Dialogue
    {
        /// <summary>
        /// The Option objects a player can select.
        /// </summary>
        public List<Option> options = new List<Option>();
    }
}