using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI {

    /// <summary>
    ///     Is responsible for provifing raycast data at the 'cursor' for
    ///     multiple sub-systems.
    /// </summary>
    public class UIRaycaster : MonoBehaviour {

        [Tooltip ("Camera used for raycasts.")]
        [SerializeField]
        private Camera raycastCamera;

        [Tooltip ("Specifies which object layers are considered.")]
        [SerializeField]
        private LayerMask raycastMask;

        [Tooltip ("Maximum distance of the ray-cast from the screen.")]
        [SerializeField]
        private float raycastDistance = 100f;

        /// <summary>
        ///     Ray of the raycast.
        /// </summary>
        public Ray Ray { get; private set; }

        // NOTE: we have to evaluate this at the spot to
        // solve a Update () order issue in the selector.
        // This is a temporary solution.
        /// <summary>
        ///     Returns true if the finger is over GUI element.
        /// </summary>
        public bool IsOverGUI =>
            Input.touchCount == 1
         && EventSystem.current.IsPointerOverGameObject (
                Input.GetTouch (0).fingerId
            );

        private RaycastHit [] hits;

        private void Update () {

            if (Input.touchCount != 1) {
                return;
            }

            // // Check if the touch isn't over GUI ..
            // IsOverGUI = EventSystem.current.IsPointerOverGameObject (
            //     Input.GetTouch (0).fingerId
            // );

            hits = Physics.RaycastAll (
                Ray = raycastCamera.ScreenPointToRay (
                    Input.GetTouch (0).position
                ),
                raycastDistance,
                raycastMask
            );
        }

        /// <summary>
        ///     Returns the closest hit satisfying the provided mask.
        /// </summary>
        public RaycastHit? GetHit (
            LayerMask mask) {

            if (hits == null) {
                return null;
            }

            var list = new List <RaycastHit> ();

            foreach (var h in hits) {

                if (h.collider                                  != null
                 && h.collider.gameObject                       != null
                 && (mask & (1 << h.collider.gameObject.layer)) > 0) {

                    list.Add (h);
                }
            }

            if (list.Count == 0) {

                return null;
            }

            list.Sort (
                (
                    hit1,
                    hit2) => {

                    var d1 = hit1.distance;
                    var d2 = hit2.distance;

                    if (d1 < d2) {
                        return 1;
                    }

                    if (d1 > d2) {
                        return -1;
                    }

                    return 0;
                }
            );

            return list [0];
        }
    }

}