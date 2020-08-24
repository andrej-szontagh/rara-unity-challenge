using System;
using System.Diagnostics.CodeAnalysis;
using DG.Tweening;
using Entities;
using Entities.Behaviours;
using Game;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace GUI {

    /// <summary>
    ///     Is responsible for GUI state management and
    ///     propagating GUI actions into sub-systems.
    /// </summary>
    public class GUIManager : MonoBehaviour {

        // NOTE: we use singleton only
        // for the sake of this demo!

        public static GUIManager Instance { get; private set; }

        // TODO: decompose !

        [Tooltip ("Wiring")]
        [SerializeField]
        private GameManager gameManager;

        [Tooltip ("Wiring")]
        [SerializeField]
        private GameModeManager modeManager;

        [Tooltip ("Wiring")]
        [SerializeField]
        private EntityGenerator entityGenerator;

        [Tooltip ("Wiring")]
        [SerializeField]
        private UIManager uiManager;

        [Tooltip ("Wiring")]
        [SerializeField]
        private UISelector selector;

        [Space (10)]
        [Tooltip (
            "Behaviour menu root transform used to show/hide behaviours."
        )]
        [SerializeField]
        private RectTransform behavioursTransform;

        [Tooltip ("Behaviour menu slide animation duration.")]
        [SerializeField]
        private float behavioursDuration = 1f;

        private Tweener behavioursTweener;

        [Space (10)]
        [Tooltip ("Toggle UI element that is responsible for state switching.")]
        [SerializeField]
        private Toggle modeToggle;

        [Tooltip ("Text UI element that represents current state.")]
        [SerializeField]
        private Text modeLabel;

        // TODO: make the behaviours extendable !

        [Space (10)]
        [Tooltip ("Toggle button reference for the 'Explode' behaviour.")]
        [SerializeField]
        private Toggle toggleBehavoiurExplode;

        [Tooltip ("Toggle button reference for the 'Points' behaviour.")]
        [SerializeField]
        private Toggle toggleBehavoiurPoints;

        [Space (10)]
        [Tooltip ("Text UI element that represents current score.")]
        [SerializeField]
        private Text pointsLabel;

        /// <summary>
        ///     This helps to prevent entering the feedback loop
        ///     resulting in the stack overflow.
        /// </summary>
        private bool updatingBehaviours;

        public void Awake () {

            Instance = this;

            // Update GUI on selection change
            selector.OnSelected.AddListener (UpdateBehaviours);

            // Update GUI on game mode state change
            modeManager.OnStateChange.AddListener (
                () => {
                    UpdateBehaviours ();
                    UpdateMode (modeManager.Mode);
                }
            );
        }

        // NOTE: called from UI (inspector)
        /// <summary>
        ///     Generates Cube entity.
        /// </summary>
        [SuppressMessage ("ReSharper", "UnusedMember.Global")]
        public void GenerateCube () {

            if (modeManager.Mode != GameMode.Editor) {
                return;
            }

            entityGenerator.Generate (EntityType.Cube);
            uiManager.Save ();
        }

        // NOTE: called from UI (inspector)
        /// <summary>
        ///     Generates Sphere entity.
        /// </summary>
        [SuppressMessage ("ReSharper", "UnusedMember.Global")]
        public void GenerateSphere () {

            if (modeManager.Mode != GameMode.Editor) {
                return;
            }

            entityGenerator.Generate (EntityType.Sphere);
            uiManager.Save ();
        }

        // NOTE: called from UI (inspector)
        /// <summary>
        ///     Restarts the game.
        /// </summary>
        [SuppressMessage ("ReSharper", "UnusedMember.Global")]
        public void Restart () {

            gameManager.Restart ();

            pointsLabel.text = "0";
        }

        // NOTE: called from UI (inspector)
        /// <summary>
        ///     Toggle the game mode.
        /// </summary>
        [SuppressMessage ("ReSharper", "UnusedMember.Global")]
        public void ToggleTest (
            bool selected) {

            modeManager.SetMode (
                selected ?
                    GameMode.Test :
                    GameMode.Editor
            );

            uiManager.Save ();
        }

        // NOTE: called from UI (inspector)
        /// <summary>
        ///     Toggle 'Explode' behaviour for the selected entity.
        /// </summary>
        [SuppressMessage ("ReSharper", "UnusedMember.Global")]
        public void ToggleBehaviourExplode (
            bool on) {

            if (modeManager.Mode != GameMode.Editor) {
                return;
            }

            if (updatingBehaviours) {
                return;
            }

            uiManager.ToggleBehaviour (BehaviourType.Explode, on);
        }

        // NOTE: called from UI (inspector)
        /// <summary>
        ///     Toggle 'Points' behaviour for the selected entity.
        /// </summary>
        [SuppressMessage ("ReSharper", "UnusedMember.Global")]
        public void ToggleBehaviourPoints (
            bool on) {

            if (modeManager.Mode != GameMode.Editor) {
                return;
            }

            if (updatingBehaviours) {
                return;
            }

            uiManager.ToggleBehaviour (BehaviourType.Points, on);
        }

        /// <summary>
        ///     Increase score by specified value.
        /// </summary>
        public void AddPoints (
            int points) {

            if (pointsLabel != null) {
                pointsLabel.text = ""
                  + (int.Parse (pointsLabel.text) + points);
            }
        }

        /// <summary>
        ///     Shows behaviours menu.
        /// </summary>
        private void ShowBehaviours (
            bool show,
            bool animate = true) {

            behavioursTweener?.Kill ();

            if (show) {

                behavioursTweener = behavioursTransform.DOLocalMove (
                    Vector3.right * 100f,
                    animate ?
                        behavioursDuration :
                        0f
                );

            } else {

                behavioursTweener = behavioursTransform.DOLocalMove (
                    Vector3.left * 400f,
                    animate ?
                        behavioursDuration :
                        0f
                );
            }
        }

        /// <summary>
        ///     Update the state of the behaviour menu for
        ///     the selected entity.
        /// </summary>
        private void UpdateBehaviours () {

            ShowBehaviours (
                modeManager.Mode  == GameMode.Editor
             && selector.Selected != null,
                Time.frameCount > 1
            );

            UpdateBehaviours (selector.Selected);
        }

        /// <summary>
        ///     Update the state of the behaviour toggles for
        ///     the specified entity instance.
        /// </summary>
        private void UpdateBehaviours (
            IEntity entity) {

            updatingBehaviours = true;

            toggleBehavoiurExplode.isOn =
                entity?.Behaviours.Contains (BehaviourType.Explode) ?? false;

            toggleBehavoiurPoints.isOn =
                entity?.Behaviours.Contains (BehaviourType.Points) ?? false;

            updatingBehaviours = false;
        }

        /// <summary>
        ///     Update mode button and other GUI states
        ///     according to the game state.
        /// </summary>
        private void UpdateMode (
            GameMode s) {

            switch (s) {

                case GameMode.Editor:

                    modeLabel.text  = GameMode.Editor.ToString ();
                    modeToggle.isOn = false;

                    break;

                case GameMode.Test:

                    modeLabel.text  = GameMode.Test.ToString ();
                    modeToggle.isOn = true;

                    ShowBehaviours (false);

                    break;

                default: throw new ArgumentOutOfRangeException ();
            }
        }
    }

}