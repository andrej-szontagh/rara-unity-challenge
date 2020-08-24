using Entities;
using UnityEngine;
using UnityEngine.Events;

namespace UI {

    /// <summary>
    ///     Is responsible for basic entity selection mechanics.
    /// </summary>
    public class UISelector : MonoBehaviour {

        [Tooltip ("Wiring")]
        [SerializeField]
        private UIRaycaster raycaster;

        [Tooltip ("Specifies which object layers are considered in selection.")]
        [SerializeField]
        private LayerMask raycastMask;

        [Space (10)]
        [Tooltip ("Invokes after selection change.")]
        [SerializeField]
        private UnityEvent onSelected =
            new UnityEvent ();

        public UnityEvent OnSelected => onSelected;

        public IEntity Selected { get; private set; }
        public IEntity Previous { get; private set; }

        private void Start () {

            Select (null, true);
        }

        private void Update () {

            if (Input.touchCount         != 1
             || Input.GetTouch (0).phase != TouchPhase.Began) {
                return;
            }

            if (raycaster.IsOverGUI) {
                return;
            }

            var prev = Selected;

            // Deselect previous selection
            Select (null);

            var hit = raycaster.GetHit (raycastMask);

            if (hit != null) {

                var entity =
                    hit.Value.collider.GetComponentInParent <IEntity> ();

                if (entity != prev) {

                    Select (entity);
                }
            }
        }

        /// <summary>
        ///     Set the selection state to the provided entity reference.
        /// </summary>
        /// <param name="entity">
        ///     Entity to be selected.
        /// </param>
        /// <param name="force">
        ///     Force selection process even if the
        ///     selection state doesn't change.
        /// </param>
        public void Select (
            IEntity entity,
            bool    force = false) {

            if (!force && Selected == entity) {
                return;
            }

            Previous?.OnDestory.RemoveListener (OnDestroyEntity);

            Previous = Selected;
            Selected = entity;

            entity?.OnDestory.RemoveListener (OnDestroyEntity);
            entity?.OnDestory.AddListener (OnDestroyEntity);

            OnSelected?.Invoke ();
        }

        /// <summary>
        ///     This even listener is invoked when the selected
        ///     entity gets destroyed so we can remove the invalid
        ///     references and set correct selection state.
        /// </summary>
        private void OnDestroyEntity (
            IEntity entity) {

            if (Selected == entity) {
                Selected = null;
                Select (null, true);

                return;
            }

            if (Previous == entity) {
                Previous = null;
            }
        }
    }

}