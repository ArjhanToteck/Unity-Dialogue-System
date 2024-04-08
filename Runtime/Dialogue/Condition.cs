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
    /// A point in the conversation that can split into two depending on a boolean function at runtime.
    /// </summary>
#if UNITY_EDITOR
    [AssociatedNode(typeof(ConditionNode))]
#endif
    [Serializable]
    public partial class Condition : Dialogue
    {
        /// <summary>
        /// A serializable callback object with a bool return type used to decide which path to follow at runtime.
        /// </summary>
        //public SerializableCallback<bool> condition;

        /// <summary>
        /// The link pointing to the next dialogue object if the condition is true.
        /// </summary>
        public NodeLinkData trueLink = null;

        /// <summary>
        /// The link pointing to the next dialogue object if the condition is false.
        /// </summary>
        public NodeLinkData falseLink = null;
    }
}