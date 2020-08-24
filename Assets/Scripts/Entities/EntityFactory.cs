using System.Collections.Generic;
using UnityEngine;

namespace Entities {

    /// <summary>
    ///     Is responsible for providing entity instances.
    /// </summary>
    public class EntityFactory : MonoBehaviour {

        [Tooltip ("Wiring")]
        [SerializeField]
        private EntityManager entityManager;

        [Tooltip ("List of available entity configurations to choose from.")]
        [SerializeField]
        private List <EntityConfig> configurations;

        // NOTE: we support only one instance of the same type!

        private readonly Dictionary <EntityType, EntityConfig> map =
            new Dictionary <EntityType, EntityConfig> ();

        private void Awake () {

            // Store configuration in the dictionary for efficient
            // approach and to rule out invalid duplicates ..

            foreach (var config in configurations) {

                if (map.ContainsKey (config.Type)) {

                    Debug.LogError (
                        "Configuration for this type ('"
                      + config.Type
                      + "') has been already added! "
                    );

                    continue;
                }

                map.Add (config.Type, config);
            }
        }

        /// <summary>
        ///     Create new entity instance by specified entity type.
        /// </summary>
        public IEntity Create (
            EntityType type) {

            if (!map.TryGetValue (type, out var config)) {

                Debug.Log (
                    "Cannot find entity configuration for the type : '"
                  + type
                  + "'"
                );

                return null;
            }

            return Create (config);
        }

        /// <summary>
        ///     Create new entity instance by specified configuration.
        /// </summary>
        private IEntity Create (
            EntityConfig config) {

            var entity = config.Spawn (Vector3.zero, null);

            entityManager.Add (entity);

            entity.OnDestory.AddListener (
                e => {
                    entityManager.Remove (e);
                }
            );

            return entity;
        }
    }

}