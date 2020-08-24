using System;
using DG.Tweening;
using UnityEngine;

namespace Entities.Behaviours {

    [CreateAssetMenu (
        fileName = "BehaviourExplode",
        menuName = "Behaviours/BehaviourExplode",
        order    = 0
    )]
    public class BehaviourExplode : Behaviour {

        /// <inheritdoc />
        public override BehaviourType Type { get; protected set; } =
            BehaviourType.Explode;

        [Tooltip (
            "Object scale will be animated uniformly to reach this "
          + "value to create 'balloon' like effect."
        )]
        [SerializeField]
        private float explosionScale = 2f;

        [Tooltip ("Duration of the explosion 'balloon' expansion animation.")]
        [SerializeField]
        private float explosionDuration = 1f;

        private Tweener tweener;

        /// <inheritdoc cref="IBehaviour" />
        public override void Do (
            IEntity target) {

            BlowUp (
                target.GameObject.transform,
                explosionScale,
                explosionDuration,
                () => {
                    target.Destroy ();
                    Debug.Log ("Boom!");
                }
            );
        }

        /// <summary>
        ///     Animate entity scale to achieve 'balloon' expansion like effect.
        /// </summary>
        /// <param name="transform">
        ///     Reference to the object transform.
        /// </param>
        /// <param name="scale">
        ///     Target uniform scale to animate to.
        /// </param>
        /// <param name="duration">
        ///     Duration of the animation.
        /// </param>
        /// <param name="callback">
        ///     Callback is called after the animation gets completed.
        /// </param>
        private void BlowUp (
            Transform transform,
            float     scale,
            float     duration,
            Action    callback = null) {

            tweener?.Kill ();

            tweener = transform.DOScale (
                new Vector3 (scale, scale, scale),
                duration
            );

            if (callback != null) {
                tweener.onComplete += () => {
                    callback ();
                };
            }
        }
    }

}