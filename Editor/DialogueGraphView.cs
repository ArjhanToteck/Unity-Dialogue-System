using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogueSystem.Editor
{
	public class DialogueGraphView : GraphView
	{
		// we probably shouldn't assume all nodes are dialogue nodes, so let's keep track of them here
		public List<DialogueNode> dialogueNodes = new List<DialogueNode>();

		public DialogueGraphView()
		{
			// grid
			styleSheets.Add(Resources.Load<StyleSheet>("DialogueGraph"));
			var grid = new GridBackground();
			Insert(0, grid);
			grid.StretchToParentSize();

			// zoom and selection
			SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
			this.AddManipulator(new ContentDragger());
			this.AddManipulator(new SelectionDragger());
			this.AddManipulator(new RectangleSelector());

			// add entry point
			new EntryPointNode(this);

			// listen for changes
			graphViewChanged = OnGraphViewChanged;
		}

		public GraphViewChange OnGraphViewChanged(GraphViewChange change)
		{
			foreach (DialogueNode dialogueNode in dialogueNodes)
			{
				dialogueNode.OnGraphViewChanged(change);
			}

			return change;
		}

		public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
		{
			var compatiblePorts = new List<Port>();

			foreach (Port port in ports)
			{
				// make sure port doesn't connect to itself
				if (startPort == port) continue;

				// make sure input doesn't connect to input and output doesn't connect to output
				if (startPort.direction == port.direction) continue;

				compatiblePorts.Add(port);
			}

			return compatiblePorts;
		}
	}
}