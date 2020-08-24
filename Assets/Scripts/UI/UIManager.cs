using System;
using Entities.Behaviours;
using Game;
using UnityEngine;

namespace UI {

    /// <summary>
    ///     Is responsible for high-level UI layer
    ///     connecting GUI and deeper sub-systems.
    /// </summary>
    public class UIManager : MonoBehaviour {

        [Tooltip ("Wiring")]
        [SerializeField]
        private GameModeManager mode;

        [Tooltip ("Wiring")]
        [SerializeField]
        private GameStateManager state;

        [Tooltip ("Wiring")]
        [SerializeField]
        private BehaviourFactory factory;

        [Tooltip ("Wiring")]
        [SerializeField]
        private UISelector selector;

        private void Start () {

            selector.OnSelected.AddListener (
                () => {

                    switch (mode.Mode) {

                        case GameMode.Editor:

                            // Switching the entity highlight as the
                            // entity gets selected in the edit mode.

                            selector.Previous?.Highlight (false);
                            selector.Selected?.Highlight (true);

                            break;

                        case GameMode.Test:

                            // Run the attached behaviours when the user
                            // taps on the entity game object.

                            if (selector.Selected != null) {
                                selector.Selected.Behaviours.RunAll ();

                                // NOTE: we unselect right away to restore the
                                // state since we don't need selection but we
                                // need the events to fire every time!

                                selector.Select (null);
                            }

                            break;

                        default: throw new ArgumentOutOfRangeException ();
                    }
                }
            );

            // Unselect entity when the game state changes.
            mode.OnStateChange.AddListener (
                () => {

                    selector.Selected?.Highlight (false);

                    selector.Select (null, true);
                }
            );
        }

        /// <summary>
        ///     Saves the game state to the persistent data store
        ///     excluding initialization (first frame).
        /// </summary>
        public void Save () {

            // Avoid saving on initialization.
            if (Time.frameCount > 1) {

                state.Save ();
            }
        }

        /// <summary>
        ///     Toggle specific behaviour for the selected entity.
        /// </summary>
        public void ToggleBehaviour (
            BehaviourType type,
            bool          on) {

            if (on) {
                AddBehaviour (type);
            } else {
                RemoveBehaviour (type);
            }

            Save ();
        }

        /// <summary>
        ///     Add specific behaviour for the selected entity.
        /// </summary>
        private void AddBehaviour (
            BehaviourType type) {
            selector.Selected?.Behaviours.Add (factory.Create (type));
        }

        /// <summary>
        ///     Remove specific behaviour for the selected entity.
        /// </summary>
        private void RemoveBehaviour (
            BehaviourType type) {
            selector.Selected?.Behaviours.Remove (type);
        }
    }

}