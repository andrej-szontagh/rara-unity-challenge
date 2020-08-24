using Entities.Behaviours;
using UnityEngine;

namespace Entities {

    /// <summary>
    ///     Entity configuration is like DNA of the entity,
    ///     it provides a recipe that specifies the properties of
    ///     this particular entity and provides static data for it's operation.
    /// </summary>
    [CreateAssetMenu (
        fileName = "EntityConfig",
        menuName = "Entities/EntityConfig",
        order    = 0
    )]
    public class EntityConfig : ScriptableObject {

        [Tooltip (
            "This prefab will be instantiated as a "
          + "representation of entity."
        )]
        [SerializeField]
        private GameObject prefab;

        [Tooltip (
            "This type will be assigned to this "
          + "configuration as an identification."
        )]
        [SerializeField]
        private EntityType type;

        public EntityType Type => type;

        [Tooltip ("Initial behaviour prototype added to this entity!")]
        [SerializeField]
        private Behaviours.Behaviour initialBehaviour;

        /// <summary>
        ///     Instantiate the entity game object at specified position.
        /// </summary>
        public IEntity Spawn (
            Vector3   position,
            Transform parent) {

            var gameObject = Instantiate (
                prefab,
                position,
                Random.rotationUniform,
                parent
            );

            // Random scale to spice it up ..
            var scale = Random.Range (0.5f, 1.0f);

            gameObject.transform.localScale =
                new Vector3 (scale, scale, scale);

            var entity = gameObject.
                GetComponentInChildren <IEntity> ();

            // Initialize configuration for this instance to
            // provide data at run-time but keep immutable.

            if (entity != null) {
                entity.Config = this;

                if (initialBehaviour != null) {

                    entity.Behaviours.Add (
                        BehaviourFactory.Create (initialBehaviour)
                    );
                }

            }

            return entity;
        }
    }

}