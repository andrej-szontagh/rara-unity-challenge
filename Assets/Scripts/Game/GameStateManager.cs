using System.Collections.Generic;
using System.Linq;
using Entities;
using Entities.Behaviours;
using Entities.Specific;
using UnityEngine;

namespace Game {

    /// <summary>
    ///     Provides control over game state/persistent data.
    /// </summary>
    public class GameStateManager : MonoBehaviour {

        /// <summary>
        ///     Irrelevant key name to store in the PlayerPrefs
        /// </summary>
        private const string Key = "JSON";

        [Tooltip ("Wiring")]
        [SerializeField]
        private EntityManager entityManager;

        [Tooltip ("Wiring")]
        [SerializeField]
        private EntityGenerator entityGenerator;

        [Tooltip ("Wiring")]
        [SerializeField]
        private BehaviourFactory behaviourFactory;

        [Tooltip ("If true provides action feedback to console.")]
        [Space (10)]
        [SerializeField]
        private bool verbose;

        private GameState gameState;

        private void Start () {
            Load ();
        }

        /// <summary>
        ///     Clear the persistend data store and remove all saved data.
        /// </summary>
        [ContextMenu ("Clear")]
        public void Clear () {

            PlayerPrefs.SetString (
                Key,
                JsonUtility.ToJson (gameState = new GameState ())
            );

            PlayerPrefs.Save ();

            if (verbose) {
                Debug.Log ("Scene save data cleared.");
            }
        }

        /// <summary>
        ///     Restore game state from the previously stored records in
        ///     persistend data store.
        /// </summary>
        [ContextMenu ("Load")]
        public void Load () {

            gameState = JsonUtility.FromJson <GameState> (
                PlayerPrefs.GetString (Key)
            );

            entityManager.Clear ();

            foreach (var entity in gameState.entities) {

                var e = entityGenerator.Generate (entity.type);

                e.ID = entity.id;

                var t = e.GameObject.transform;

                t.position = new Vector3 (
                    entity.positionX,
                    entity.positionY,
                    entity.positionZ
                );

                t.localScale = new Vector3 (
                    entity.scale,
                    entity.scale,
                    entity.scale
                );

                e.Behaviours.Clear ();

                foreach (var behaviour in entity.behaviours) {
                    e.Behaviours.Add (behaviourFactory.Create (behaviour.type));
                }
            }

            if (verbose) {
                Debug.Log ("Scene loaded.");
            }
        }

        /// <summary>
        ///     Stores the current game state into persistent data store.
        /// </summary>
        [ContextMenu ("Save")]
        public void Save () {

            var entities = new List <GameState.Entity> ();

            foreach (var entity in entityManager.Entities) {

                EntityType type;

                // NOTE: this is obviously not ideal solution, but it's
                // sufficient for the prupose of this demo.
                switch (entity) {

                    case EntityCube _:

                        type = EntityType.Cube;

                        break;

                    case EntitySphere _:

                        type = EntityType.Sphere;

                        break;

                    default:

                        Debug.LogError ("Unknown entity type!");

                        continue;
                }

                var t = entity.GameObject.transform;

                var position = t.position;

                entities.Add (
                    new GameState.Entity {
                        id        = entity.ID,
                        type      = type,
                        positionX = position.x,
                        positionY = position.y,
                        positionZ = position.z,
                        scale     = t.localScale.x,
                        behaviours = entity.Behaviours.Behaviours.Select (
                                behaviour => new GameState.Entity.Behaviour {
                                    type = behaviour.Type
                                }
                            ).
                            ToArray ()
                    }
                );
            }

            gameState = new GameState {
                entities = entities.ToArray ()
            };

            PlayerPrefs.SetString (Key, JsonUtility.ToJson (gameState));
            PlayerPrefs.Save ();

            if (verbose) {
                Debug.Log ("Scene saved.");
            }
        }

        /// <summary>
        ///     Prints the current content of the presistent data
        ///     store in JSON format.
        /// </summary>
        [ContextMenu ("Print")]
        private void Print () {

            Debug.Log (PlayerPrefs.GetString (Key));
        }
    }

}