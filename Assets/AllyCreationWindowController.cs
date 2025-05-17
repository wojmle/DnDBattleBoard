using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assets.Scripts;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets
{
    public class AllyCreationWindowController : MonoBehaviour
    {
        private DragManipulator dragManipulator;
        private ResizeManipulator resizeManipulator;
        public VisualTreeAsset panelAsset;

        public static event Action<Adversary> OnActionButtonClicked;

        private GameObject _panelGO;
        private DropdownField _modelDropdown; 
        private DropdownField _raceDropdown;
        private TextField _nameTextField;
        private List<AllyObject> _items;
        public PanelSettings panelSettings;
        private Vector2 _dragOffset;
        private bool _isResizing;
        private VisualElement _rootElement;
        private VisualElement _resizeHandle;
        private BuildingManager buildingManager;
        private Dictionary<string, List<string>> raceModelDictionary;
        public static string allyPath = "Assets/Prefabs/Ally";

        public void ShowPanel()
        {
            if (_panelGO != null) return;

            _panelGO = new GameObject("AllyPanel");
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
            var resize_handle_top = root.Q<VisualElement>("resize-handle-top");
            var resize_handle_bottom = root.Q<VisualElement>("resize-handle-bottom");
            var resize_handle_left = root.Q<VisualElement>("resize-handle-left");
            var resize_handle_right = root.Q<VisualElement>("resize-handle-right");
            if (resize_handle_top != null && resize_handle_bottom != null && resize_handle_right != null && resize_handle_left != null && main_root != null)
            {
                resize_handle_top.AddManipulator(new ResizeManipulator(main_root));
                resize_handle_bottom.AddManipulator(new ResizeManipulator(main_root));
                resize_handle_left.AddManipulator(new ResizeManipulator(main_root));
                resize_handle_right.AddManipulator(new ResizeManipulator(main_root));
            }

            buildingManager = FindFirstObjectByType<BuildingManager>();

            var closeButton = root.Q<Button>("close-button");
            var actionButton = root.Q<Button>("action-button");
            _nameTextField = root.Q<TextField>("name");
            _raceDropdown = root.Q<DropdownField>("race-dropdown");
            _modelDropdown = root.Q<DropdownField>("model-dropdown");

            PopulateRaceDropdown();

            // Register event to update model dropdown on race change
            _raceDropdown.RegisterValueChangedCallback(evt =>
            {
                UpdateModelDropdown(evt.newValue);
            });

            closeButton.clicked += ClosePanel;
            actionButton.clicked += FireSelected;

        }

        public void PopulateRaceDropdown()
        {
            string jsonFilePath = @"Assets/Races.json";

            string jsonData = File.ReadAllText(jsonFilePath);

            // Deserialize the JSON into a list of Adversary objects
            var races = JsonConvert.DeserializeObject<List<string>>(jsonData);
            raceModelDictionary = new Dictionary<string, List<string>>();
            foreach (var race in races)
            {
                var raceDir = Path.Combine(allyPath, race);
                var prefabNames = new List<string>();

                if (Directory.Exists(raceDir))
                {
                    var prefabFiles = Directory.GetFiles(raceDir, "*.prefab");
                    foreach (var prefabFile in prefabFiles)
                    {
                        var prefabName = Path.GetFileNameWithoutExtension(prefabFile);
                        prefabNames.Add(prefabName);
                    }
                }

                raceModelDictionary.TryAdd(race, prefabNames);
            }
        
            _raceDropdown.choices.AddRange(races);
            _raceDropdown.value = _raceDropdown.choices.FirstOrDefault();
            _raceDropdown.choices.AddRange(raceModelDictionary[_raceDropdown.value]);
        }

        private void ClosePanel()
        {
            if (_panelGO != null)
            {
                Destroy(_panelGO);
                _panelGO = null;
            }
        }

        private void UpdateModelDropdown(string race)
        {
            if (_modelDropdown == null) return;

            if (raceModelDictionary.TryGetValue(race, out var models))
            {
                _modelDropdown.choices = models;
                _modelDropdown.value = models.Count > 0 ? models[0] : null;
            }
            else
            {
                _modelDropdown.choices = new List<string>();
                _modelDropdown.value = null;
            }
        }

        private void FireSelected()
        {
            if (_raceDropdown.value != null && _nameTextField != null && _raceDropdown.value != null)
            {
                var selected = new Ally(_nameTextField.value, _raceDropdown.value, _modelDropdown.value);
                buildingManager.InsertObject(selected);
            }
        }
    }
}
