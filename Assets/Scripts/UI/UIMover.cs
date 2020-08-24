using DG.Tweening;
using Game;
using UnityEngine;

namespace UI {

    /// <summary>
    ///     Is responsible for the entity relocation by the user.
    /// </summary>
    public class UIMover : MonoBehaviour {

        [Tooltip ("Wiring")]
        [SerializeField]
        private GameModeManager mode;

        [Tooltip ("Wiring")]
        [SerializeField]
        private UISelector selector;

        [Tooltip ("Wiring")]
        [SerializeField]
        private UIRaycaster raycaster;

        [Tooltip ("Specifies which object layers are considered in selection.")]
        [SerializeField]
        private LayerMask raycastMask;

        private void Update () {

            if (mode.Mode != GameMode.Editor) {
                return;
            }

            if (selector.Selected == null) {
                return;
            }

            if (Input.touchCount != 1) {
                return;
            }

            var touch = Input.GetTouch (0);

            if (touch.phase != TouchPhase.Moved
             && touch.phase != TouchPhase.Stationary) {
                return;
            }

            if (raycaster.IsOverGUI) {
                return;
            }

            var hit = raycaster.GetHit (raycastMask);

            if (hit != null) {

                var offset = 0f;

                var col = selector.Selected.GameObject.
                    GetComponent <Collider> ();

                if (col != null) {

                    var bounds = col.bounds;

                    offset = Mathf.Max (
                        bounds.size.x,
                        bounds.size.y,
                        bounds.size.z
                    );
                }

                // Using animation to achieve smooth motion.

                selector.Selected.GameObject.transform.DOMove (
                    hit.Value.point - raycaster.Ray.direction * offset,
                    0.2f
                );
            }
        }
    }

}