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

		public string savePath;

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
			// check if any elements were deleted
			if (change.elementsToRemove != null)
			{
				foreach (var element in change.elementsToRemove)
				{
					// check if we have deleted a dialogue node
					if (element is DialogueNode dialogueNode)
					{
						dialogueNode.OnRemoveNode();
					}
					// check if link broken
					else if (element is Edge edge)
					{
						// check if link was from a dialogue node
						if (edge.output.node is DialogueNode dialogueNode1)
						{
							dialogueNode1.OnRemoveLink(edge);
						}
					}
				}
			}

			// check if any elements were moved
			if (change.movedElements != null)
			{
				foreach (var element in change.movedElements)
				{
					// check if we moved a dialogue node
					if (element is DialogueNode dialogueNode)
					{
						dialogueNode.OnMove();
					}
				}
			}

			// check if any edges created
			if (change.edgesToCreate != null)
			{
				foreach (Edge edge in change.edgesToCreate)
				{
					// check if link is from a dialogue node
					if (edge.output.node is DialogueNode dialogueNode)
					{
						dialogueNode.OnCreateLink(edge);
					}
				}
			}

			ConversationFileManager.SaveConversation(dialogueNodes, savePath);

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