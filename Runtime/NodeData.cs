using System;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    [Serializable]
    public class NodeData
    {
        public string guid;
        public float[] position;
        public List<NodeLinkData> links = new List<NodeLinkData>();
    }
}