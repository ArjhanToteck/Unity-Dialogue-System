using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    // TODO: maybe rename as decision to avoid confusion with option objects?
    [Serializable]
    public partial class Choice : Dialogue
    {
        public List<Option> options = new List<Option>();
    }
}