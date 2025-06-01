using UnityEngine;
using UnityEngine.UIElements;
using System;
using System.Collections.Generic;
using Assets.Scripts;
using Cursor = UnityEngine.UIElements.Cursor;
using Assets;

public class CharacterCreationTableController : MonoBehaviour
{
    private DragManipulator dragManipulator;
    private ResizeManipulator resizeManipulator;
    public VisualTreeAsset panelAsset;

    public static event Action<Adversary> OnActionButtonClicked;

    private GameObject _panelGO;
    private MultiColumnListView _listView;
    private List<Adversary> _items;
    public PanelSettings panelSettings;
    private Vector2 _dragOffset;
    private bool _isResizing;
    private VisualElement _rootElement;
    private VisualElement _resizeHandle;
    private BuildingManager buildingManager;

    public void ShowPanel()
    {
        if (_panelGO != null) return;

        _panelGO = new GameObject("MyPanelRuntimeUI");
        var doc = _panelGO.AddComponent<UIDocument>();
        doc.visualTreeAsset = panelAsset;
        doc.panelSettings = panelSettings;

        InitializePanel(doc.rootVisualElement);
    }

    private void InitializePanel(VisualElement root)
    {
        _rootElement = root;
        dragManipulator = new DragManipulator();

        var header = root.Q<VisualElement>("header");
        if (header != null)
        {
            header.AddManipulator(dragManipulator);
        }

        var main_root = root.Q<VisualElement>("main-root");
        var resize_handle_bottom = root.Q<VisualElement>("resize-handle-bottom");
        var resize_handle_left = root.Q<VisualElement>("resize-handle-left");
        var resize_handle_right = root.Q<VisualElement>("resize-handle-right");
        if (resize_handle_bottom != null && resize_handle_right != null && resize_handle_left != null && main_root != null) {
            main_root.RegisterCallback<FocusInEvent>(evt =>
            {
                EventManager.TriggerEvent(nameof(FocusInEvent));
            });
            main_root.RegisterCallback<FocusOutEvent>(evt =>
            {
                EventManager.TriggerEvent(nameof(FocusOutEvent));
            });
            resize_handle_bottom.AddManipulator(new ResizeManipulator(main_root));
            resize_handle_left.AddManipulator(new ResizeManipulator(main_root));
            resize_handle_right.AddManipulator(new ResizeManipulator(main_root));
        }

        buildingManager = FindFirstObjectByType<BuildingManager>();

        var closeButton = root.Q<Button>("close-button");
        var actionButton = root.Q<Button>("action-button");
        _listView = root.Q<MultiColumnListView>("table-view");

        closeButton.clicked += ClosePanel;
        actionButton.clicked += FireSelected;
        
        PopulateDropdown();
        SetupListView();
    }

    public void PopulateDropdown()
    {
        var adversaryConverter = new AdversaryToJsonClassConverter();
        _items = adversaryConverter.CreateAdversaryList();
    }

    private void ClosePanel()
    {
        if (_panelGO != null)
        {
            EventManager.TriggerEvent(nameof(FocusOutEvent));
            Destroy(_panelGO);
            _panelGO = null;
        }
    }

    private void FireSelected()
    {
        if (_items != null)
        {
            var selected = _items[_listView.selectedIndex];
            buildingManager.InsertObject(selected);
        }
    }

    private void SetupListView()
    {
        _listView.columns.Clear();
        _listView.columns.Add(new Column
        {
            title = "Category",
            makeCell = () => new Label(),
            bindCell = (e, i) => ((Label)e).text = _items[i].Category.CategoryName.ToString(),
            sortable = true,
            stretchable = true,
            comparison = (a, b) => _items[a].Category.CategoryName.CompareTo(_items[b].Category.CategoryName)
        });
        _listView.columns.Add(new Column
        {
            title = "Race",
            makeCell = () => new Label(),
            bindCell = (e, i) => ((Label)e).text = _items[i].Race.RaceName,
            sortable = true,
            stretchable = true,
            comparison = (a, b) => _items[a].Race.RaceName.CompareTo(_items[b].Race.RaceName)
        });
        _listView.columns.Add(new Column
        {
            title = "Name",
            makeCell = () => new Label(),
            bindCell = (e, i) => ((Label)e).text = _items[i].Name.ToString(),
            sortable = true,
            stretchable = true,
            comparison = (a,b) => _items[a].Name.CompareTo(_items[b].Name)
        });
        _listView.columns.Add(new Column
        {
            title = "Attribute Level",
            makeCell = () => new Label(),
            bindCell = (e, i) => ((Label)e).text = _items[i].AttributeLevel.ToString(),
            sortable = true,
            stretchable = true,
            comparison = (a, b) => _items[a].AttributeLevel.CompareTo(_items[b].AttributeLevel)
        });
        _listView.columns.Add(new Column
        {
            title = "Endurance",
            makeCell = () => new Label(),
            bindCell = (e, i) => ((Label)e).text = _items[i].Endurance.ToString(),
            sortable = true,
            stretchable = true,
            comparison = (a, b) => _items[a].Endurance.CompareTo(_items[b].Endurance)
        });
        _listView.columns.Add(new Column
        {
            title = "Might",
            makeCell = () => new Label(),
            bindCell = (e, i) => ((Label)e).text = _items[i].Might.ToString(),
            sortable = true,
            stretchable = true,
            comparison = (a, b) => _items[a].Might.CompareTo(_items[b].Might)
        });
        _listView.columns.Add(new Column
        {
            title = "Hate",
            makeCell = () => new Label(),
            bindCell = (e, i) => ((Label)e).text = _items[i].Hate.ToString(),
            sortable = true,
            stretchable = true,
            comparison = (a, b) => _items[a].Hate.CompareTo(_items[b].Hate)
        });

        _listView.sortingMode = ColumnSortingMode.Default;
        _listView.itemsSource = _items;
        _listView.fixedItemHeight = 30;
        _listView.selectedIndex = -1;
        _listView.selectionType = SelectionType.Single;
    }
}
