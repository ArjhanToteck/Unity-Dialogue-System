#if UNITY_EDITOR
using System;

namespace DialogueSystem.Editor
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AssociatedNodeAttribute : Attribute
    {
        public Type NodeType { get; }

        public AssociatedNodeAttribute(Type nodeType)
        {
            NodeType = nodeType;
        }
    }
}
#endif