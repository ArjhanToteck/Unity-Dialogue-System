using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueGraphView : GraphView
{
	private readonly Vector2 defaultNodeSize = new Vector2(150, 200);
	private readonly Vector2 defaultNodePosition = new Vector2(100, 100);

	public DialogueGraphView()
	{
		this.AddManipulator(new ContentDragger());
		this.AddManipulator(new SelectionDragger());
		this.AddManipulator(new RectangleSelector());

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

	private Port AddInputPort(DialogueNode dialogueNode){
		// create and add
		var inputPort = CreatePort(dialogueNode, Direction.Input, Port.Capacity.Multi);
		inputPort.portName = "Previous";

		// refresh
		dialogueNode.inputContainer.Add(inputPort);
		dialogueNode.RefreshExpandedState();
		dialogueNode.RefreshPorts();
		dialogueNode.SetPosition(new Rect(defaultNodePosition, defaultNodeSize));

		return inputPort;
	}

	private Port AddOutputPort(DialogueNode dialogueNode){
		// create and add
		Port outputPort = CreatePort(dialogueNode, Direction.Output);
		outputPort.portName = "Next";
		dialogueNode.outputContainer.Add(outputPort);

		// refresh
		dialogueNode.RefreshExpandedState();
		dialogueNode.RefreshPorts();

		return outputPort;
	}

    private DialogueNode CreateEntryPointNode()
    {
        // create default entry node
        var dialogueNode = new EntryPointNode();

		AddOutputPort(dialogueNode);

        // place node at start position
        dialogueNode.SetPosition(new Rect(defaultNodePosition, defaultNodeSize));

        return dialogueNode;
    }

	public SpeechNode CreateSpeechNode(string nodeName)
	{
		SpeechNode speechNode = new SpeechNode{
			title = nodeName,
		};
		
		AddInputPort(speechNode);
		AddOutputPort(speechNode);

		AddElement(speechNode);

		return speechNode;
	}
}