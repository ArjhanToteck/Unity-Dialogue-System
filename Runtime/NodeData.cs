using System;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    /// <summary>
    /// Stores the data to recreate a DialogueNode in the editor as well as being useful for finding the next Dialogue in a sequence.
    /// </summary>
    [Serializable]
    public class NodeData
    {
        /// <summary>
        /// The global universal id of the node, which can be used to identify connections.
        /// </summary>
        public string guid = Guid.NewGuid().ToString();

        /// <summary>
        /// The position of the DialogueNode. The first element is the x coordinate, the second element is the y coordinate.
        /// </summary>
        // TODO: possibly introduce a Vector2 extension or class or something because this is probably bad practice.
        public float[] position = new float[2];
    }
}