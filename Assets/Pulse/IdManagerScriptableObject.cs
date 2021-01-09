using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pulse
{

    public abstract class IdManagerScriptableObject<T> : ScriptableObject
        where T : ScriptableObject
    {
        public List<T> registeredSOs;

        public T FromId(int id)
        {
            return registeredSOs[id];
        }

        public int GetId(T scriptableObject)
        {
            return registeredSOs.FindIndex(x => x.Equals(scriptableObject));
        }
    }



    /*Backup impl:
     * 
     * public abstract class IdManagerScriptableObject<T> : ScriptableObject
        where T : ScriptableObject
    {
        private static IdManagerScriptableObject<T> instance;

        private List<T> registeredSOs;

        public static int GetOrRegisterHitType(T hitType)
        {
            if (instance == null)
                return -1;

            if (instance.registeredSOs == null)
            {
                instance.registeredSOs = new List<T>();
            }
            else
            {
                instance.CleanUp();
            }

            if (instance.registeredSOs.Contains(hitType))
            {
                return instance.registeredSOs.FindIndex(x => x.Equals(hitType));
            }
            else
            {
                instance.registeredSOs.Add(hitType);
                return instance.registeredSOs.Count - 1;
            }
        }

        public static T FromId(int id)
        {
            return instance.registeredSOs[id];
        }

        private void OnEnable()
        {
            if (instance == null)
            {
                Debug.Log($"Setting IdManageableScriptableObject<{typeof(T)}> singleton");
                instance = this;
            }
            else
            {
                Debug.LogError($"More than one instance of IdManageableScriptableObject<{typeof(T)}> object is loaded");
            }
        }

        private void CleanUp()
        {
            for (int i = registeredSOs.Count - 1; i <= 0; i--)
            {
                if (registeredSOs[i] == null)
                {
                    registeredSOs.RemoveAt(i);
                }
            }
        }
    }
     * 
     */
}