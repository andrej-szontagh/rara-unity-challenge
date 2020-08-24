using Entities;
using UnityEngine;

namespace Game {

    /// <summary>
    ///     Manages top-most main game logic and
    ///     interaction of the sub-systems.
    /// </summary>
    public class GameManager : MonoBehaviour {

        [SerializeField]
        private GameStateManager stateManager;

        [SerializeField]
        private GameModeManager modeManager;

        [SerializeField]
        private EntityManager entityManager;

        /// <summary>
        ///     Restarts the game completely.
        /// </summary>
        public void Restart () {
            modeManager.Restart ();
            stateManager.Clear ();
            entityManager.Clear ();
        }
    }

}