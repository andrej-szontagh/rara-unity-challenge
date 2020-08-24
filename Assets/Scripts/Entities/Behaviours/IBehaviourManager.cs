using System.Collections.Generic;

namespace Entities.Behaviours {

    /// <summary>
    ///     Is responsible for managing behaviour instances for
    ///     the particular entity instance.
    /// </summary>
    public interface IBehaviourManager {

        /// <summary>
        ///     Returns contained behaviours.
        /// </summary>
        IEnumerable <IBehaviour> Behaviours { get; }

        /// <summary>
        ///     Add new unique behaviour for this entity.
        /// </summary>
        void Add (
            IBehaviour behaviour);

        /// <summary>
        ///     Remove all the behaviours of a type.
        /// </summary>
        void Remove (
            BehaviourType type);

        /// <summary>
        ///     Returns true if contains behaviour of the specified type.
        /// </summary>
        bool Contains (
            BehaviourType type);

        /// <summary>
        ///     Run all the behaviour actions for this entity.
        /// </summary>
        void RunAll ();

        /// <summary>
        ///     Remove all the behaviours.
        /// </summary>
        void Clear ();
    }

}