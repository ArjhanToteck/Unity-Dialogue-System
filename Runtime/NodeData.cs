using System;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    [Serializable]
    public class NodeData
    {
        public static readonly Vector2 defaultSize = new Vector2(150, 200);
        public static readonly Vector2 defaultPosition = new Vector2(100, 100);

        public string guid = Guid.NewGuid().ToString();
        public float[] position = new float[] { defaultPosition.x, defaultPosition.y };
    }
}