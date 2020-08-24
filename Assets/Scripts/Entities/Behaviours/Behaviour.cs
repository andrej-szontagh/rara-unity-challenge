using UnityEngine;

namespace Entities.Behaviours {

    /// <inheritdoc cref="IBehaviour" />
    public abstract class Behaviour
        : ScriptableObject,
            IBehaviour {

        /// <inheritdoc />
        public abstract BehaviourType Type { get; protected set; }

        // TODO: find a better way how to inject
        // behaviour specific dependencies

        /// <inheritdoc />
        public abstract void Do (
            IEntity target);
    }

}