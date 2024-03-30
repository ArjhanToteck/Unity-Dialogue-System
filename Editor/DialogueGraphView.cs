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
		private readonly Vector2 defaultNodeSize = new Vector2(150, 200);
		private readonly Vector2 defaultNodePosition = new Vector2(100, 100);

		public DialogueGraphView()
		{
			styleSheets.Add(Resources.Load<StyleSheet>("DialogueGraph"));
			SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

			this.AddManipulator(new ContentDragger());
			this.AddManipulator(new SelectionDragger());
			this.AddManipulator(new RectangleSelector());

			var grid = new GridBackground();
			Insert(0, grid);
			grid.StretchToParentSize();

			AddElement(CreateEntryPointNode());
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

		private Port CreatePort(DialogueNode node, Direction portDirection, Port.Capacity capacity = Port.Capacity.Single)
		{
			return node.InstantiatePort(Orientation.Horizontal, portDirection, capacity, typeof(float));
		}

		private Port AddInputPort(DialogueNode dialogueNode, string portName = "Previous")
		{
			// create and add
			var inputPort = CreatePort(dialogueNode, Direction.Input, Port.Capacity.Multi);
			inputPort.portName = portName;

			// refresh
			dialogueNode.inputContainer.Add(inputPort);
			dialogueNode.RefreshExpandedState();
			dialogueNode.RefreshPorts();
			dialogueNode.SetPosition(new Rect(defaultNodePosition, defaultNodeSize));

			return inputPort;
		}

		private Port AddOutputPort(DialogueNode dialogueNode, string portName = "Next")
		{
			// create and add
			Port outputPort = CreatePort(dialogueNode, Direction.Output);
			outputPort.portName = portName;
			dialogueNode.outputContainer.Add(outputPort);

			// refresh
			dialogueNode.RefreshExpandedState();
			dialogueNode.RefreshPorts();

			return outputPort;
		}

		private DialogueNode CreateEntryPointNode()
		{
			// create default entry node
			var dialogueNode = new DialogueNode()
			{
				title = "Entry",
				dialogue = new EntryPoint()
			};

			AddOutputPort(dialogueNode);

			// place node at start position
			dialogueNode.SetPosition(new Rect(defaultNodePosition, defaultNodeSize));

			return dialogueNode;
		}

		public DialogueNode CreateSpeechNode(string nodeName)
		{
			DialogueNode speechNode = new DialogueNode
			{
				title = nodeName,
				dialogue = new Speech()
			};

			AddInputPort(speechNode);
			AddOutputPort(speechNode);

			AddElement(speechNode);

			return speechNode;
		}

		public DialogueNode CreateChoiceNode(string nodeName)
		{
			DialogueNode choiceNode = new DialogueNode
			{
				title = nodeName,
				dialogue = new Choice()
			};

			AddInputPort(choiceNode);

			var addOptionButton = new Button(() =>
			{
				AddChoice(choiceNode);
			});
			addOptionButton.text = "Add Option";
			choiceNode.titleContainer.Add(addOptionButton);

			AddElement(choiceNode);

			return choiceNode;
		}

		private void AddChoice(DialogueNode choiceNode)
		{
			Choice.Option option = new Choice.Option
			{
				option = "New Option"
			};

			AddOutputPort(choiceNode, option.option);

			((Choice)choiceNode.dialogue).options.Add(option);
		}
	}
}