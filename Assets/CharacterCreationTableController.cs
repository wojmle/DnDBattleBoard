using UnityEngine;
using UnityEngine.UIElements;
using System;
using System.Collections.Generic;
using Assets.Scripts;

public class CharacterCreationTableController : MonoBehaviour
{
    public VisualTreeAsset panelAsset;

    public static event Action<Adversary> OnActionButtonClicked;

    private GameObject _panelGO;
    private MultiColumnListView _listView;
    private List<Adversary> _items;
    public PanelSettings panelSettings;
    private Vector2 _dragOffset;
    private bool _isDragging;
    private bool _isResizing;
    private VisualElement _rootElement;
    private VisualElement _resizeHandle;

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

        var header = root.Q<VisualElement>("header");
        var closeButton = root.Q<Button>("close-button");
        var actionButton = root.Q<Button>("action-button");
        _listView = root.Q<MultiColumnListView>("table-view");
        PopulateDropdown(); 

        closeButton.clicked += ClosePanel;
        actionButton.clicked += FireSelected;

        SetupListView();
    }

    private void MakeDraggable(VisualElement header)
    {
        header.RegisterCallback<PointerDownEvent>(evt =>
        {
            _dragOffset = evt.position - _rootElement.worldBound.position;
            _isDragging = true;
            evt.StopPropagation();
        });

        header.RegisterCallback<PointerMoveEvent>(evt =>
        {
            if (_isDragging)
            {
                Vector2 newPos = evt.position - _dragOffset;
                _rootElement.style.left = newPos.x;
                _rootElement.style.top = newPos.y;
            }
        });

        header.RegisterCallback<PointerUpEvent>(evt =>
        {
            _isDragging = false;
        });
    }

    private void MakeResizable(VisualElement handle)
    {
        handle.RegisterCallback<PointerDownEvent>(evt =>
        {
            _dragOffset = evt.position;
            _isResizing = true;
            evt.StopPropagation();
        });

        handle.RegisterCallback<PointerMoveEvent>(evt =>
        {
            if (_isResizing)
            {
                var delta = evt.position - _dragOffset;
                _dragOffset = evt.position;

                var width = _rootElement.resolvedStyle.width + delta.x;
                var height = _rootElement.resolvedStyle.height + delta.y;

                _rootElement.style.width = Mathf.Max(300, width);
                _rootElement.style.height = Mathf.Max(200, height);
            }
        });

        handle.RegisterCallback<PointerUpEvent>(evt =>
        {
            _isResizing = false;
        });
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
            Destroy(_panelGO);
            _panelGO = null;
        }
    }

    private void FireSelected()
    {
        if (_listView.selectedIndex >= 0 && _items != null)
        {
            var selected = _items[_listView.selectedIndex];
            OnActionButtonClicked?.Invoke(selected);
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
        _listView.selectionType = SelectionType.Single;
    }
}
