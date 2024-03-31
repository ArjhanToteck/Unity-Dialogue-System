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
		}

		public void OnGraphViewChanged(GraphViewChange change)
		{
			/*// check if nodes have been moved
			if (change.movedElements != null && change.movedElements.Count > 0)
			{
				foreach (DialogueNode movedNode in change.movedElements)
				{
					movedNode.dialogue.nodeData.position = Vector2.zero + movedNode.GetPosition().position;
					Debug.Log(movedNode.dialogue.nodeData.position);
				}
			}

			// check if connections have been made or broken
			if (change.edgesToCreate != null && change.edgesToCreate.Count > 0)
			{
				foreach (Edge edge in change.edgesToCreate)
				{
					NodeLinkData linkData = new NodeLinkData
					{
						portName = edge.input.portName,
						connectedNodeGuid = ((DialogueNode)edge.output.node).guid
					};

					((DialogueNode)edge.input.node).dialogue.nodeData.links.Add(linkData);
				}
			}*/

			// figure out how to check for broken connection
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