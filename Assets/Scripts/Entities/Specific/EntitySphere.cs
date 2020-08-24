using UnityEngine;

namespace Entities.Specific {

    public class EntitySphere : Entity {

        protected override void Awake () {

            base.Awake ();

            // Modulate color for the spheres ..
            OriginalColor = Renderer.material.color = Color.Lerp (
                OriginalColor,
                Random.ColorHSV (0f, 1f, 1f, 1f, 0.5f, 1f),
                0.5f
            );
        }
    }

}