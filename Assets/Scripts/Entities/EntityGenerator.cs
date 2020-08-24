using UnityEngine;

namespace Entities {

    /// <summary>
    ///     Generates entity instances.
    /// </summary>
    public class EntityGenerator : MonoBehaviour {

        [Tooltip ("Wiring")]
        [SerializeField]
        private EntityFactory entityFactory;

        [Tooltip (
            "Entities are spawned in the volume of the "
          + "box collider at random positions."
        )]
        [SerializeField]
        private BoxCollider spawnArea;

        [Tooltip (
            "All generated entities will be "
          + "parented under this transform."
        )]
        [SerializeField]
        private Transform entityParent;

        /// <summary>
        ///     Generate entity at implicit (random) position of
        ///     specific type.
        /// </summary>
        public IEntity Generate (
            EntityType type) {

            var entity = entityFactory.Create (type);

            var t = entity.GameObject.transform;

            t.position = EvalPosition (spawnArea);
            t.parent   = entityParent;

            return entity;
        }

        /// <summary>
        ///     Evaluate random position in the volume of box collider.
        /// </summary>
        private Vector3 EvalPosition (
            BoxCollider area) {

            var center = area.center;
            var hsize  = area.size * 0.5f;

            var boundsMin = center - hsize;
            var boundsMax = center + hsize;

            var position = new Vector3 (
                Random.Range (boundsMin.x, boundsMax.x),
                Random.Range (boundsMin.y, boundsMax.y),
                Random.Range (boundsMin.z, boundsMax.z)
            );

            return spawnArea.transform.TransformPoint (position);
        }
    }

}