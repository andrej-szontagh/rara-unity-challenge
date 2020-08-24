namespace Entities.Behaviours {

    /// <summary>
    ///     Provides explicit behaviour functionality for the entity.
    /// </summary>
    public interface IBehaviour {

        /// <summary>
        ///     Type that identifies this specific behaviour as unique.
        /// </summary>
        BehaviourType Type { get; }

        /// <summary>
        ///     Run behaviour action
        /// </summary>
        void Do (
            IEntity target);
    }

}