using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    [Serializable]
    public class Choice : Dialogue
    {
        public List<Option> options = new List<Option>();

        public class Option
        {
            public string option;
            public NodeLinkData link;
        }
    }
}