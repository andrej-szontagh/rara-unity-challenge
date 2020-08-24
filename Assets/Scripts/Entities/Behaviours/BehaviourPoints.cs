using GUI;
using UnityEngine;

namespace Entities.Behaviours {

    [CreateAssetMenu (
        fileName = "BehaviourPoints",
        menuName = "Behaviours/BehaviourPoints",
        order    = 1
    )]
    public class BehaviourPoints : Behaviour {

        /// <inheritdoc />
        public override BehaviourType Type { get; protected set; } =
            BehaviourType.Points;

        /// <inheritdoc cref="IBehaviour" />
        public override void Do (
            IEntity target) {

            GUIManager.Instance.AddPoints (100);

            Debug.Log ("Giving points ..");
        }
    }

}