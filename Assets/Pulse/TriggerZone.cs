using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Pulse.Events;

namespace Pulse.Experimental
{
    public abstract class BaseTriggerZone<T1, T2> : MonoBehaviour where T1 : UnityEvent<T2>
    {
        public T1 onEnter, onExit;

        private List<T2> objectsInside;
        protected virtual bool InterestedGameObject(T2 item) => true;

        void Awake()
        {
            objectsInside = new List<T2>();
        }

        protected virtual T2 ColliderToListeningType(Collider other)
        {
            return other.GetComponent<T2>();
        }

        void OnTriggerEnter(Collider other)
        {
            T2 desiredObject = ColliderToListeningType(other);
            if (desiredObject != null && !objectsInside.Contains(desiredObject))
            {
                objectsInside.Add(desiredObject);
                onEnter.Invoke(desiredObject);
            }
        }

        void OnTriggerExit(Collider other)
        {
            T2 desiredObject = ColliderToListeningType(other);
            if (desiredObject != null && objectsInside.Contains(desiredObject))
            {
                objectsInside.Remove(desiredObject);
                onExit.Invoke(desiredObject);
            }
        }

        public virtual T2[] ObjectsInside()
        {
            return objectsInside.ToArray();
        }
    }

    public class TriggerZone : BaseTriggerZone<GameObjectUnityEvent, GameObject>
    {
        protected override GameObject ColliderToListeningType(Collider other)
        {
            return other.gameObject;
        }
    }
}