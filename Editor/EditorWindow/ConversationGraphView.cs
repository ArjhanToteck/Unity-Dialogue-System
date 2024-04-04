#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogueSystem.Editor
{
	public class ConversationGraphView : GraphView
	{
		public ConversationEditorWindow window;
		// we probably shouldn't assume all nodes are dialogue nodes, so let's keep track of them here
		public List<DialogueNode> dialogueNodes;
		public bool doneLoadingFile = false;
		private Label defaultMessage;
		private DialogueSearchWindow searchWindow;

		public ConversationGraphView()
		{
			// grid
			styleSheets.Add(Resources.Load<StyleSheet>("ConversationGraphView"));
			var grid = new GridBackground();
			Insert(0, grid);
			grid.StretchToParentSize();

			// create default message
			defaultMessage = new Label("No conversation object selected.");
			defaultMessage.style.fontSize = 20;
			defaultMessage.style.unityTextAlign = TextAnchor.MiddleCenter;
			defaultMessage.StretchToParentSize();
			Add(defaultMessage);
		}

		public void OnFinishedLoading()
		{
			// remove message label
			if (defaultMessage != null)
			{
				Remove(defaultMessage);
				defaultMessage = null;
			}

			// add zoom and selection
			SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
			this.AddManipulator(new ContentDragger());
			this.AddManipulator(new SelectionDragger());
			this.AddManipulator(new RectangleSelector());

			// listen for changes
			graphViewChanged = OnGraphViewChanged;

			// search window
			AddSearchWindow();
		}

		public GraphViewChange OnGraphViewChanged(GraphViewChange change)
		{
			// don't save anything if we're currently loading
			if (!doneLoadingFile)
			{
				return change;
			}

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

			SaveConversation();

			return change;
		}

		public void ClearGraphView()
		{
			// reset nodes list
			dialogueNodes = new List<DialogueNode>();

			// remove all nodes
			foreach (var node in nodes.ToList())
			{
				RemoveElement(node);
			}

			// remove all edges
			foreach (var edge in edges.ToList())
			{
				RemoveElement(edge);
			}
		}

		public void AddSearchWindow()
		{
			searchWindow = ScriptableObject.CreateInstance<DialogueSearchWindow>();
			searchWindow.graphView = this;
			searchWindow.window = window;

			nodeCreationRequest = (context) =>
			{
				SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), searchWindow);
			};
		}

		public void AddDialogueNode(DialogueNode dialogueNode)
		{
			dialogueNode.graphView = this;
			AddElement(dialogueNode);
			dialogueNodes.Add(dialogueNode);

			SaveConversation();
		}

		public DialogueNode GetDialogueNodeByGuid(string guid)
		{
			return dialogueNodes.Find(node => node.dialogue.nodeData.guid == guid);
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

		public Edge LinkNodes(DialogueNode outputNode, string outputPortName, DialogueNode inputNode, string inputPortName)
		{
			// find output port
			Port outputPort = outputNode.FindPortByName(outputPortName);

			// find input port
			Port inputPort = inputNode.FindPortByName(inputPortName);

			// create edge between ports
			Edge edge = new Edge
			{
				output = outputPort,
				input = inputPort,
			};

			// actually connect
			edge.input.Connect(edge);
			edge.output.Connect(edge);

			// add connection as element
			AddElement(edge);

			return edge;
		}

		public void SaveConversation()
		{
			// don't save anything if we're loading
			if (!doneLoadingFile)
			{
				return;
			}

			ConversationSaveManager.SaveConversation(dialogueNodes, window.savePath);
		}
	}
}
#endif