
using System;

namespace DialogueSystem.Editor
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AssociatedNode : Attribute
    {
        public Type NodeType { get; }

        public AssociatedNode(Type nodeType)
        {
            NodeType = nodeType;
        }
    }
}