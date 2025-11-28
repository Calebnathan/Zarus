using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UIElements;
using Zarus.Map;
using Zarus.Systems;

namespace Zarus.UI
{
    /// <summary>
    /// Controls the world-space province panel that appears above selected provinces.
    /// This creates a diegetic UI experience where province info floats in game world.
    /// </summary>
    public class ProvincePanelController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private UIDocument uiDocument;

        [SerializeField]
        private RegionMapController mapController;

        [SerializeField]
        private OutbreakSimulationController outbreakSimulation;

        [SerializeField]
        private Camera mainCamera;

        [Header("Settings")]
        [SerializeField]
        private Vector3 worldOffset = new Vector3(0, 2f, 0);

        [SerializeField]
        private Vector2 screenOffset = new Vector2(0, -100f);

        // UI Elements
        private VisualElement root;
        private Label provinceNameLabel;
        private Label infectionValueLabel;
        private Label outpostStatusLabel;
        private Button deployOutpostButton;
        private Label outpostCostLabel;

        // State
        private RegionEntry currentProvince;
        private bool isVisible;

        private void Awake()
        {
            if (mainCamera == null)
            {
                mainCamera = Camera.main;
            }

            if (uiDocument == null)
            {
                uiDocument = GetComponent<UIDocument>();
            }

            if (mapController == null)
            {
                mapController = FindFirstObjectByType<RegionMapController>();
            }

            if (outbreakSimulation == null)
            {
                outbreakSimulation = FindFirstObjectByType<OutbreakSimulationController>();
            }
        }

        private void Start()
        {
            InitializeUI();
            SubscribeToEvents();
            Hide();
        }

        private void OnDestroy()
        {
            UnsubscribeFromEvents();

            if (deployOutpostButton != null)
            {
                deployOutpostButton.clicked -= OnDeployClicked;
            }
        }

        private void LateUpdate()
        {
            if (isVisible && currentProvince != null)
            {
                UpdatePanelPosition();
            }
        }

        private void InitializeUI()
        {
            if (uiDocument == null || uiDocument.rootVisualElement == null)
            {
                Debug.LogError("[ProvincePanelController] UIDocument or root element is null!");
                return;
            }

            root = uiDocument.rootVisualElement.Q<VisualElement>("ProvincePanelRoot");
            provinceNameLabel = root?.Q<Label>("ProvinceNameLabel");
            infectionValueLabel = root?.Q<Label>("InfectionValueLabel");
            outpostStatusLabel = root?.Q<Label>("OutpostStatusLabel");
            deployOutpostButton = root?.Q<Button>("DeployOutpostButton");
            outpostCostLabel = root?.Q<Label>("OutpostCostLabel");

            if (deployOutpostButton != null)
            {
                deployOutpostButton.clicked += OnDeployClicked;
            }
        }

        private void SubscribeToEvents()
        {
            if (mapController != null)
            {
                mapController.OnRegionSelected.AddListener(OnProvinceSelected);
            }

            if (outbreakSimulation != null)
            {
                outbreakSimulation.ProvinceStateChanged += OnProvinceStateChanged;
                outbreakSimulation.GlobalStateChanged += OnGlobalStateChanged;
            }
        }

        private void UnsubscribeFromEvents()
        {
            if (mapController != null)
            {
                mapController.OnRegionSelected.RemoveListener(OnProvinceSelected);
            }

            if (outbreakSimulation != null)
            {
                outbreakSimulation.ProvinceStateChanged -= OnProvinceStateChanged;
                outbreakSimulation.GlobalStateChanged -= OnGlobalStateChanged;
            }
        }

        private void OnProvinceSelected(RegionEntry province)
        {
            if (province == null)
            {
                Hide();
                return;
            }

            currentProvince = province;
            Show();
            RefreshProvinceData();
        }

        private void OnProvinceStateChanged(ProvinceInfectionState state)
        {
            if (currentProvince == null || state == null)
            {
                return;
            }

            if (string.Equals(state.RegionId, currentProvince.RegionId, StringComparison.OrdinalIgnoreCase))
            {
                RefreshProvinceData();
            }
        }

        private void OnGlobalStateChanged(GlobalCureState state)
        {
            if (isVisible)
            {
                UpdateDeployButton();
            }
        }

        private void RefreshProvinceData()
        {
            if (currentProvince == null)
            {
                return;
            }

            // Update province name
            if (provinceNameLabel != null)
            {
                provinceNameLabel.text = $"★ {currentProvince.DisplayName.ToUpper()} ★";
            }

            // Get province state from simulation
            if (outbreakSimulation != null && outbreakSimulation.TryGetProvinceState(currentProvince.RegionId, out var state))
            {
                UpdateInfectionDisplay(state);
                UpdateOutpostDisplay(state);
                UpdateDeployButton();
            }
        }

        private void UpdateInfectionDisplay(ProvinceInfectionState state)
        {
            if (infectionValueLabel == null)
            {
                return;
            }

            var percent = Mathf.RoundToInt(Mathf.Clamp01(state.Infection01) * 100f);
            infectionValueLabel.text = string.Format(CultureInfo.InvariantCulture, "{0}%", percent);

            // Update color based on severity
            infectionValueLabel.RemoveFromClassList("province-stat__value--muted");
            infectionValueLabel.RemoveFromClassList("province-stat__value--warning");
            infectionValueLabel.RemoveFromClassList("province-stat__value--danger");
            infectionValueLabel.RemoveFromClassList("province-stat__value--success");

            if (state.IsFullyInfected)
            {
                infectionValueLabel.AddToClassList("province-stat__value--danger");
            }
            else if (state.Infection01 >= 0.5f)
            {
                infectionValueLabel.AddToClassList("province-stat__value--warning");
            }
            else
            {
                infectionValueLabel.AddToClassList("province-stat__value--success");
            }
        }

        private void UpdateOutpostDisplay(ProvinceInfectionState state)
        {
            if (outpostStatusLabel == null)
            {
                return;
            }

            outpostStatusLabel.RemoveFromClassList("province-stat__value--muted");
            outpostStatusLabel.RemoveFromClassList("province-stat__value--warning");
            outpostStatusLabel.RemoveFromClassList("province-stat__value--danger");
            outpostStatusLabel.RemoveFromClassList("province-stat__value--success");

            if (!state.HasOutpost)
            {
                outpostStatusLabel.text = "None";
                outpostStatusLabel.AddToClassList("province-stat__value--muted");
            }
            else if (state.OutpostDisabled)
            {
                outpostStatusLabel.text = string.Format(CultureInfo.InvariantCulture, "{0} Disabled", state.OutpostCount);
                outpostStatusLabel.AddToClassList("province-stat__value--warning");
            }
            else
            {
                outpostStatusLabel.text = string.Format(CultureInfo.InvariantCulture, "{0} Active", state.OutpostCount);
                outpostStatusLabel.AddToClassList("province-stat__value--success");
            }
        }

        private void UpdateDeployButton()
        {
            if (deployOutpostButton == null || outpostCostLabel == null || currentProvince == null)
            {
                return;
            }

            var canBuild = outbreakSimulation.CanBuildOutpost(currentProvince.RegionId, out var costR, out var error);
            deployOutpostButton.SetEnabled(canBuild);

            outpostCostLabel.text = string.Format(CultureInfo.InvariantCulture, "Cost: R {0}", costR);

            // Update button text based on error
            if (!canBuild)
            {
                switch (error)
                {
                    case OutbreakSimulationController.OutpostBuildError.ProvinceFullyInfected:
                        deployOutpostButton.text = "Province Lost";
                        break;
                    case OutbreakSimulationController.OutpostBuildError.NotEnoughZar:
                        deployOutpostButton.text = "Insufficient Funds";
                        break;
                    default:
                        deployOutpostButton.text = "Cannot Deploy";
                        break;
                }
            }
            else
            {
                deployOutpostButton.text = "Deploy Cure Outpost";
            }
        }

        private void OnDeployClicked()
        {
            if (currentProvince == null || outbreakSimulation == null)
            {
                return;
            }

            if (outbreakSimulation.TryBuildOutpost(currentProvince.RegionId, out _, out _))
            {
                RefreshProvinceData();
            }
        }

        private void UpdatePanelPosition()
        {
            if (root == null || currentProvince == null || mainCamera == null)
            {
                return;
            }

            // Get province center position in world space
            var provinceBounds = currentProvince.Bounds;
            var provinceCenter = provinceBounds.center + worldOffset;

            // Convert to screen position
            var screenPos = mainCamera.WorldToScreenPoint(provinceCenter);

            // Check if province is behind camera
            if (screenPos.z < 0)
            {
                Hide();
                return;
            }

            // Ensure panel is visible
            if (!isVisible)
            {
                Show();
            }

            // Apply screen offset and set position
            screenPos.x += screenOffset.x;
            screenPos.y = Screen.height - screenPos.y + screenOffset.y; // Flip Y for UI coordinates

            root.style.left = screenPos.x - (root.resolvedStyle.width / 2f);
            root.style.top = screenPos.y;
            root.style.position = Position.Absolute;
        }

        public void Show()
        {
            if (root == null)
            {
                return;
            }

            isVisible = true;
            root.RemoveFromClassList("hidden");
            root.AddToClassList("province-panel--visible");
        }

        public void Hide()
        {
            if (root == null)
            {
                return;
            }

            isVisible = false;
            currentProvince = null;
            root.RemoveFromClassList("province-panel--visible");
            
            // Use schedule to avoid immediate removal during animation
            root.schedule.Execute(() =>
            {
                if (!isVisible)
                {
                    root.AddToClassList("hidden");
                }
            }).StartingIn(200); // Wait for transition
        }
    }
}
