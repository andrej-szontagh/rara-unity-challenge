using UnityEngine;
using UnityEngine.Events;

namespace Game {

    /// <summary>
    ///     Controls the game mode and switching the state.
    /// </summary>
    public class GameModeManager : MonoBehaviour {

        private const GameMode DefaultMode = GameMode.Editor;

        [Tooltip ("Current mode.")]
        [SerializeField]
        private GameMode mode = DefaultMode;

        public GameMode Mode => mode;

        [Space (10)]
        [Tooltip ("Invokes after mode state has changed.")]
        [SerializeField]
        private UnityEvent onStateChange =
            new UnityEvent ();

        public UnityEvent OnStateChange => onStateChange;

        private void Start () {
            SetMode (mode);
        }

        /// <summary>
        ///     Restarts the game mode to default.
        /// </summary>
        public void Restart () {
            SetMode (DefaultMode);
        }

        /// <summary>
        ///     Provides ability to switch the mode.
        /// </summary>
        public void SetMode (
            GameMode newMode) {
            mode = newMode;

            OnStateChange?.Invoke ();
        }
    }

}