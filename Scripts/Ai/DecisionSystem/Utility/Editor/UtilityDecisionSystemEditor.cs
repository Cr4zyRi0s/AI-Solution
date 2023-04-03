using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;
public class UtilityDecisionSystemEditor : EditorWindow
{
    //public 


    [MenuItem("Tools/DecisionSystems/Utility")]
    public static void CreateWindow() {
        var window = GetWindow<UtilityDecisionSystemEditor>();
        window.titleContent = new GUIContent("Utility System Editor");
    }

    public void CreateGUI()
    {
        var mainSplitPanel = new TwoPaneSplitView(1, 250, TwoPaneSplitViewOrientation.Horizontal);
      
        var listView = new ListView();

        var actionTypes = typeof(ActionType).GetEnumNames().ToList();
        var actionSelectionField = new DropdownField(actionTypes,0);
        var actionTypeLabel = new Label("Action Type");
       
        var choiceCreationView = new ScrollView();
        var createChoiceButton = new Button();
        createChoiceButton.text = "Create New Choice";
        choiceCreationView.Add(actionTypeLabel);
        choiceCreationView.Add(actionSelectionField);
        choiceCreationView.Add(createChoiceButton);

        var choiceListView = new ScrollView();

        var leftSideSplitPanel = new TwoPaneSplitView(1,50, TwoPaneSplitViewOrientation.Vertical);
        leftSideSplitPanel.Add(choiceCreationView);
        leftSideSplitPanel.Add(choiceListView);

        mainSplitPanel.Add(leftSideSplitPanel);
        mainSplitPanel.Add(new VisualElement());

        rootVisualElement.Add(mainSplitPanel);        
    }
}
