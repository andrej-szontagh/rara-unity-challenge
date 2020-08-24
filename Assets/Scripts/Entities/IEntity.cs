using System;
using Entities.Behaviours;
using UnityEngine;
using UnityEngine.Events;

namespace Entities {

    /// <summary>
    ///     Represents interactable game object in the scene with
    ///     particular functionality.
    /// </summary>
    public interface IEntity {

        /// <summary>
        ///     Unique ID
        /// </summary>
        string ID { get; set; }

        // TODO: find a better way how to provide required
        // functionality without exposing everything.
        /// <summary>
        ///     Reference to the game object.
        /// </summary>
        GameObject GameObject { get; }

        /// <summary>
        ///     Events invoked when the entity gets destroyed
        /// </summary>
        UnityEvent <IEntity> OnDestory { get; }

        /// <summary>
        ///     Persistent entity configuration.
        /// </summary>
        EntityConfig Config { get; set; }

        /// <summary>
        ///     Provides entity behaviours.
        /// </summary>
        IBehaviourManager Behaviours { get; }

        /// <summary>
        ///     Highlight this entity
        /// </summary>
        void Highlight (
            bool   on,
            Action callback = null);

        /// <summary>
        ///     Destorys the entity
        /// </summary>
        void Destroy ();
    }

}