using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Zarus.Systems;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Zarus.UI
{
    /// <summary>
    /// Controls the Game Over / End scene menu.
    /// </summary>
    [RequireComponent(typeof(UIDocument))]
    public class EndMenuController : MonoBehaviour
    {
        [Header("Scenes")]
        [SerializeField]
        private string gameplaySceneName = "Main";

        [SerializeField]
        private string startSceneName = "Start";

        private UIDocument uiDocument;
        private VisualElement root;
        private VisualElement menuPanel;
        private Label outcomeTitleLabel;
        private Label outcomeSubtitleLabel;
        private Label statsDaysLabel;
        private Label statsCureLabel;
        private Label statsProvincesLabel;
        private Label statsOutpostsLabel;
        private Label statsZarLabel;
        private Button restartButton;
        private Button menuButton;
        private Button exitButton;
        private bool isInitialized;

        private void Awake()
        {
            uiDocument = GetComponent<UIDocument>();
        }

        private void OnEnable()
        {
            TryInitialize();
        }

        private void Start()
        {
            TryInitialize();
        }

        private void OnDisable()
        {
            UnregisterCallbacks();
        }

        private void TryInitialize()
        {
            if (isInitialized)
            {
                return;
            }

            if (uiDocument == null || uiDocument.rootVisualElement == null)
            {
                return;
            }

            root = uiDocument.rootVisualElement;
            menuPanel = root.Q<VisualElement>("MenuPanel");
            outcomeTitleLabel = root.Q<Label>("OutcomeTitleLabel");
            outcomeSubtitleLabel = root.Q<Label>("OutcomeSubtitleLabel");
            statsDaysLabel = root.Q<Label>("StatsDaysLabel");
            statsCureLabel = root.Q<Label>("StatsCureLabel");
            statsProvincesLabel = root.Q<Label>("StatsProvincesLabel");
            statsOutpostsLabel = root.Q<Label>("StatsOutpostsLabel");
            statsZarLabel = root.Q<Label>("StatsZarLabel");
            restartButton = root.Q<Button>("RestartButton");
            menuButton = root.Q<Button>("MenuButton");
            exitButton = root.Q<Button>("ExitButton");

            ActivateMenuPanelAnimation();
            RegisterCallbacks();
            PopulateOutcomeDetails();
            isInitialized = true;
        }

        private void RegisterCallbacks()
        {
            if (restartButton != null)
            {
                restartButton.clicked += RestartGame;
            }

            if (menuButton != null)
            {
                menuButton.clicked += ReturnToMenu;
            }

            if (exitButton != null)
            {
                exitButton.clicked += ExitGame;
            }
        }

        private void UnregisterCallbacks()
        {
            if (restartButton != null)
            {
                restartButton.clicked -= RestartGame;
            }

            if (menuButton != null)
            {
                menuButton.clicked -= ReturnToMenu;
            }

            if (exitButton != null)
            {
                exitButton.clicked -= ExitGame;
            }
        }

        private void RestartGame()
        {
            if (string.IsNullOrEmpty(gameplaySceneName))
            {
                Debug.LogWarning("[EndMenu] Gameplay scene name not set.");
                return;
            }

            SceneManager.LoadScene(gameplaySceneName);
        }

        private void ReturnToMenu()
        {
            if (string.IsNullOrEmpty(startSceneName))
            {
                Debug.LogWarning("[EndMenu] Start scene name not set.");
                return;
            }

            SceneManager.LoadScene(startSceneName);
        }

        private void ExitGame()
        {
            Debug.Log("[EndMenu] Exit clicked.");
    #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
    #else
            Application.Quit();
#endif
        }

        private void ActivateMenuPanelAnimation()
        {
            if (menuPanel == null)
            {
                return;
            }

            if (!menuPanel.ClassListContains("slide-up--active"))
            {
                menuPanel.AddToClassList("slide-up--active");
            }
        }

        private void PopulateOutcomeDetails()
        {
            var outcome = GameOutcomeState.LastOutcome;
            var hasOutcome = outcome != GameOutcomeKind.None;
            var isVictory = outcome == GameOutcomeKind.Victory;

            if (outcomeTitleLabel != null)
            {
                outcomeTitleLabel.text = hasOutcome
                    ? (isVictory ? "CURE DEPLOYED – VICTORY" : "OUTBREAK LOST – DEFEAT")
                    : "MISSION COMPLETE";
            }

            if (outcomeSubtitleLabel != null)
            {
                outcomeSubtitleLabel.text = hasOutcome
                    ? (isVictory ? "Hope restored across the republic." : "Containment failed – regroup and try again.")
                    : "Review your path and jump back in.";
            }

            if (menuPanel != null)
            {
                menuPanel.RemoveFromClassList("end-outcome--victory");
                menuPanel.RemoveFromClassList("end-outcome--defeat");
                if (hasOutcome)
                {
                    menuPanel.AddToClassList(isVictory ? "end-outcome--victory" : "end-outcome--defeat");
                }
            }

            var dayIndex = Mathf.Max(1, GameOutcomeState.LastDayIndex);
            var curePercent = Mathf.RoundToInt(GameOutcomeState.LastCureProgress01 * 100f);
            var saved = GameOutcomeState.LastSavedProvinces;
            var fullyInfected = GameOutcomeState.LastFullyInfectedProvinces;
            var totalProvinces = Mathf.Max(saved + fullyInfected, 0);

            if (statsDaysLabel != null)
            {
                statsDaysLabel.text = hasOutcome
                    ? string.Format("Days elapsed: {0}", dayIndex)
                    : "Days elapsed: --";
            }

            if (statsCureLabel != null)
            {
                statsCureLabel.text = hasOutcome
                    ? string.Format("Cure progress: {0}%", curePercent)
                    : "Cure progress: --";
            }

            if (statsProvincesLabel != null)
            {
                statsProvincesLabel.text = hasOutcome
                    ? string.Format("Provinces saved: {0} / {1}", saved, Mathf.Max(totalProvinces, 1))
                    : "Provinces saved: --";
            }

            if (statsOutpostsLabel != null)
            {
                statsOutpostsLabel.text = hasOutcome
                    ? string.Format("Outposts: {0} active / {1} total", GameOutcomeState.LastActiveOutposts, GameOutcomeState.LastTotalOutposts)
                    : "Outposts: --";
            }

            if (statsZarLabel != null)
            {
                statsZarLabel.text = hasOutcome
                    ? string.Format("Budget remaining: R {0}", GameOutcomeState.LastZarBalance)
                    : "Budget remaining: --";
            }
        }
    }
}
