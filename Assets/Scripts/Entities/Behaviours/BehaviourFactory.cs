using System.Collections.Generic;
using UnityEngine;

namespace Entities.Behaviours {

    /// <summary>
    ///     Create behaviour instances.
    /// </summary>
    public class BehaviourFactory : MonoBehaviour {

        [Tooltip (
            "List of behaviour scriptable object prototypes that "
          + "will be instantiated/cloned for each entity."
        )]
        [SerializeField]
        private List <Behaviour> behaviours;

        // NOTE: we support only one instance of the same type!

        private readonly Dictionary <BehaviourType, IBehaviour> map =
            new Dictionary <BehaviourType, IBehaviour> ();

        private void Awake () {

            // Filling the map dictionary to efficiently
            // approach behaviour prototypes by type ..

            foreach (var behaviour in behaviours) {

                if (map.ContainsKey (behaviour.Type)) {

                    Debug.Log (
                        "Behaviour of the type : '"
                      + behaviour.Type
                      + "' has been already added. Skipping : '"
                      + behaviour
                      + "'"
                    );

                    continue;
                }

                map.Add (behaviour.Type, behaviour);
            }
        }

        /// <summary>
        ///     Create behaviour instance of the specific type.
        /// </summary>
        public IBehaviour Create (
            BehaviourType type) {

            // Look up for the appropriate prototype ..

            if (map.TryGetValue (type, out var behaviour)) {

                // Instantiate prototype for this particular instance.

                return Create (behaviour);
            }

            Debug.LogError ("Cannot create behaviour of the type : " + type);

            return null;
        }

        /// <summary>
        ///     Create behaviour instance using the
        ///     original prototype reference.
        /// </summary>
        public static IBehaviour Create (
            IBehaviour prototype) {

            // Instantiate prototype for this particular instance.
            if (ScriptableObject.CreateInstance (prototype.GetType ()) is
                IBehaviour b) {

                return b;
            }

            Debug.LogError ("Cannot create behaviour : '" + prototype + "'.");

            return null;
        }
    }

}