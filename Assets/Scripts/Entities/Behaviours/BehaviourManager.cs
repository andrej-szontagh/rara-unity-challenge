using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Entities.Behaviours {

    /// <inheritdoc />
    public class BehaviourManager : IBehaviourManager {

        // This behaviour manager is tied to specific entity!
        private readonly IEntity entity;

        public BehaviourManager (
            IEntity entity) {
            this.entity = entity;
        }

        // NOTE: we don't support more then one behaviour of the same type.
        private readonly Dictionary <BehaviourType, IBehaviour> behaviours =
            new Dictionary <BehaviourType, IBehaviour> ();

        /// <inheritdoc />
        public IEnumerable <IBehaviour> Behaviours => behaviours.Values;

        /// <inheritdoc />
        public void Add (
            IBehaviour behaviour) {

            if (Contains (behaviour.Type)) {

                Debug.Log (
                    "Behaviour of this type is already added : " + behaviour
                );

                return;
            }

            behaviours.Add (behaviour.Type, behaviour);
        }

        /// <inheritdoc />
        public void Remove (
            BehaviourType type) {

            if (behaviours.TryGetValue (type, out var behaviour)) {
                behaviours.Remove (type);

                Object.Destroy (behaviour as Behaviour);
            }
        }

        /// <inheritdoc />
        public void RunAll () {
            foreach (var behaviour in behaviours.Values) {
                behaviour.Do (entity);
            }
        }

        /// <inheritdoc />
        public bool Contains (
            BehaviourType type) {
            return behaviours.ContainsKey (type);
        }

        /// <inheritdoc />
        public void Clear () {

            var tmp = behaviours.Values.ToArray ();

            foreach (var behaviour in tmp) {

                Object.Destroy (behaviour as Behaviour);
            }

            behaviours.Clear ();
        }
    }

}