using System;
using System.Collections;
using System.Collections.Generic;
using Codice.CM.Client.Gui;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueGraphWindow : EditorWindow
{
    private DialogueGraphView graphView;

	[MenuItem("Graph/Dialogue")]
	public static void OpenDialogueGraphWindow()
	{
		DialogueGraphWindow window = GetWindow<DialogueGraphWindow>();
        window.titleContent = new GUIContent("Dialogue");
	}

    private void OnEnable()
    {
        CreateGraphView();
        CreateToolBar();
    }

    private void CreateGraphView(){
        // create window
        graphView = new DialogueGraphView(){
            name = "Dialogue"
        };

        graphView.StretchToParentSize();

        // actually open window
        rootVisualElement.Add(graphView);
    }

    private void CreateToolBar(){
        Toolbar toolbar = new Toolbar();

        var createNodeButton = new Button(() => {
            graphView.CreateSpeechNode("new dialogue node");
        });
        createNodeButton.text = "CreateNodeButton";
        toolbar.Add(createNodeButton);

        rootVisualElement.Add(toolbar);
    }

    private void OnDisable()
    {
        // remove window
        rootVisualElement.Remove(graphView);
    }
}
