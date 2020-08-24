using System.Collections.Generic;
using UnityEngine;

namespace Entities {

    /// <summary>
    ///     Manages the collection of created entities.
    /// </summary>
    public class EntityManager : MonoBehaviour {

        /// <summary>
        ///     Holds the list of all created entities.
        /// </summary>
        private readonly List <IEntity> entities =
            new List <IEntity> ();

        /// <summary>
        ///     Provides all the created entities.
        /// </summary>
        public IEnumerable <IEntity> Entities => entities;

        /// <summary>
        ///     Registers entity in the system.
        /// </summary>
        public void Add (
            IEntity entity) {
            entities.Add (entity);
        }

        /// <summary>
        ///     Removes entity from the registry.
        /// </summary>
        public void Remove (
            IEntity entity) {
            entities.Remove (entity);
        }

        /// <summary>
        ///     Destroys all entities and clean up.
        /// </summary>
        public void Clear () {

            var tmp = entities.ToArray ();

            foreach (var entity in tmp) {
                entity.Destroy ();
            }

            entities.Clear ();
        }
    }

}