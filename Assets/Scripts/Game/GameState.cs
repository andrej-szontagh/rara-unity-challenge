using System;
using Entities;
using Entities.Behaviours;

namespace Game {

    /// <summary>
    ///     Holds the serializable structure required for persistent data store.
    /// </summary>
    [Serializable]
    public class GameState {

        [Serializable]
        public class Entity {

            [Serializable]
            public class Behaviour {

                /// <summary>
                ///     The type of this behaviour.
                /// </summary>
                public BehaviourType type;

                // ..
            }

            /// <summary>
            ///     Unique ID of the entity.
            /// </summary>
            public string id;

            /// <summary>
            ///     Type of the entity.
            /// </summary>
            public EntityType type;

            /// <summary>
            ///     World space position of the entity (X coord).
            /// </summary>
            public float positionX;

            /// <summary>
            ///     World space position of the entity (Y coord).
            /// </summary>
            public float positionY;

            /// <summary>
            ///     World space position of the entity (Z coord).
            /// </summary>
            public float positionZ;

            /// <summary>
            ///     Uniform scale of the entity.
            /// </summary>
            public float scale;

            /// <summary>
            ///     Behaviours attached to this entity.
            /// </summary>
            public Behaviour [] behaviours;
        }

        /// <summary>
        ///     Entities in the scene.
        /// </summary>
        public Entity [] entities;

    }

}