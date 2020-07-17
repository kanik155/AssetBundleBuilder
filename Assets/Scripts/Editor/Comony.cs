using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;


public class Comony : EditorWindow
{
    [MenuItem("Comony/Upload Space Data")]
    public static void ShowExample()
    {
        Comony wnd = GetWindow<Comony>();
        wnd.titleContent = new GUIContent("Comony");
    }

    public void OnEnable()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/Editor/Comony.uxml");
        VisualElement labelFromUXML = visualTree.CloneTree();
        root.Add(labelFromUXML);

        // A stylesheet can be added to a VisualElement.
        // The style will be applied to the VisualElement and all of its children.
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/Editor/Comony.uss");
        VisualElement labelWithStyle = new Label("Select Build Scene");
        labelWithStyle.styleSheets.Add(styleSheet);
        root.Add(labelWithStyle);

        var items = new List<string>();
        var items2 = new List<UnityEngine.Object>();

        foreach (var guid in AssetDatabase.FindAssets("t:Scene"))
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            // Debug.Log(AssetDatabase.LoadMainAssetAtPath(path));

            items.Add(AssetDatabase.LoadMainAssetAtPath(path).name);
            items2.Add(AssetDatabase.LoadMainAssetAtPath(path));
        }

        // The "makeItem" function will be called as needed
        // when the ListView needs more items to render
        Func<VisualElement> makeItem = () => new Label();

        // As the user scrolls through the list, the ListView object
        // will recycle elements created by the "makeItem"
        // and invoke the "bindItem" callback to associate
        // the element with the matching data item (specified as an index in the list)
        Action<VisualElement, int> bindItem = (e, i) => (e as Label).text = items[i];

        // Provide the list view with an explict height for every row
        // so it can calculate how many items to actually display
        const int itemHeight = 16;

        var listView = new ListView(items, itemHeight, makeItem, bindItem);
        listView.styleSheets.Add(styleSheet);

        listView.selectionType = SelectionType.Single;

        // listView.onItemChosen += obj => Debug.Log(obj);
        // listView.onSelectionChanged += objects => Debug.Log(objects);

        listView.style.flexGrow = 1.0f;

        rootVisualElement.Add(listView);

        var uploadButton = new Button();
        uploadButton.text = "Upload";
        uploadButton.styleSheets.Add(styleSheet);
        uploadButton.clickable.clicked += () =>
        {
            var selectedScenePath = AssetDatabase.GetAssetPath(items2[listView.selectedIndex]);
            Debug.Log(selectedScenePath);

            ComonyAssetBundleBuild.Build(selectedScenePath);
        };
        rootVisualElement.Add(uploadButton);

    }
}