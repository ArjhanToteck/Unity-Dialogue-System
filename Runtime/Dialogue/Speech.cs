using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    [Serializable]
    public class Speech : Dialogue
    {
        public string speakerName = "";
        public string speech = "";
        public NodeLinkData nextLink = null;
    }
}