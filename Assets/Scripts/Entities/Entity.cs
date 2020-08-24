using System;
using DG.Tweening;
using Entities.Behaviours;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Entities {

    /// <inheritdoc cref="Entities.IEntity" />
    public abstract class Entity
        : MonoBehaviour,
            IEntity {

        [Tooltip ("Entity Unique ID")]
        [SerializeField]
        private string id;

        /// <summary>
        ///     If true unique ID becomes immutable.
        ///     Allows for initialization with protection.
        /// </summary>
        private bool idInitialized;

        /// <inheritdoc />
        public string ID {
            get => id;
            set {

                // NOTE: prevent changing the value more than once !
                // the value is initialized right after instantiation and
                // becomes immutable!

                if (idInitialized) {

                    Debug.Log ("Entity ID already initialized!");

                    return;
                }

                idInitialized = true;

                id = value;
            }
        }

        [Tooltip ("Entity Configuration / Specification")]
        [SerializeField]
        private EntityConfig config;

        /// <summary>
        ///     If true configuration becomes immutable.
        ///     Allows for initialization with protection.
        /// </summary>
        private bool configInitialized;

        /// <inheritdoc />
        public EntityConfig Config {
            get => config;
            set {

                // NOTE: prevent changing the value more than once !
                // the value is initialized right after instantiation and
                // becomes immutable!

                if (configInitialized) {

                    Debug.Log ("Entity configuration already initialized!");

                    return;
                }

                configInitialized = true;

                config = value;
            }
        }

        private class EntityDestroyEvent : UnityEvent <IEntity> { }

        [Space (10)]
        [Tooltip ("Events invoked when the entity gets destroyed")]
        [SerializeField]
        private EntityDestroyEvent onDestory =
            new EntityDestroyEvent ();

        /// <inheritdoc />
        public UnityEvent <IEntity> OnDestory => onDestory;

        /// <inheritdoc />
        public GameObject GameObject => gameObject;

        /// <inheritdoc />
        public IBehaviourManager Behaviours { get; private set; }

        /// <summary>
        ///     Is this entity currently highlighted ?
        /// </summary>
        private bool highlight;

        /// <summary>
        ///     Renderer of this entity (supporting only one)
        /// </summary>
        protected Renderer Renderer;

        /// <summary>
        ///     Stores original material base color for animation purposes.
        /// </summary>
        protected Color OriginalColor;

        private Tweener tweener;

        protected virtual void Awake () {

            // Generate Unique ID
            id = GUID.Generate ().ToString ();

            Renderer = GetComponentInChildren <Renderer> ();

            OriginalColor = Renderer.sharedMaterial.color;

            Behaviours = new BehaviourManager (this);
        }

        /// <inheritdoc />
        public void Highlight (
            bool   on,
            Action callback = null) {

            if (highlight == on) {
                return;
            }

            SetColor (
                on ?
                    Color.white :
                    OriginalColor,
                0.5f,
                callback
            );

            highlight = on;
        }

        /// <summary>
        ///     Animate entity base color to a new specified color.
        /// </summary>
        /// <param name="color">
        ///     Target color to be set for the entity.
        /// </param>
        /// <param name="duration">
        ///     Duration of the transition animation.
        /// </param>
        /// <param name="callback">
        ///     Callback is invoked when the animation is complete.
        /// </param>
        private void SetColor (
            Color  color,
            float  duration,
            Action callback = null) {

            tweener?.Kill ();

            tweener = Renderer.material.DOColor (color, duration);

            if (callback != null) {
                tweener.onComplete += () => {
                    callback ();
                };
            }
        }

        /// <inheritdoc />
        public void Destroy () {

            OnDestory?.Invoke (this);

            Destroy (gameObject);
        }
    }

}