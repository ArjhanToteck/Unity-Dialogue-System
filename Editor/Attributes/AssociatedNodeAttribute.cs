#if UNITY_EDITOR
using System;

namespace DialogueSystem.Editor
{
    /// <summary>
    /// Editor-only attribute that marks a Dialogue-inherited class with the DialogueNode-inherited class to use when creating nodes for it in the editor.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class AssociatedNodeAttribute : Attribute
    {
        /// <summary>
        /// The type to use when creating nodes for it in the editor. Must be inherited from DialogueNode.
        /// </summary>
        public Type NodeType { get; }

        public AssociatedNodeAttribute(Type nodeType)
        {
            NodeType = nodeType;
        }
    }
}
#endif