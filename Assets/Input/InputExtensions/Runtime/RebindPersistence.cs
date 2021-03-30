using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Erethan.InputExtensions
{
    public class RebindPersistence : MonoBehaviour
    {
        public const string PlayerPrefsKey = "BindOverrides";

        /// <summary>
        /// Private wrapper class for json serialization of the overrides
        /// </summary>
        [System.Serializable]
        class BindingWrapperClass
        {
            public List<BindingSerializable> bindingList = new List<BindingSerializable>();
        }

        /// <summary>
        /// internal struct to store an id overridepath pair for a list
        /// </summary>
        [System.Serializable]
        private struct BindingSerializable
        {
            public string id;
            public string path;

            public BindingSerializable(string bindingId, string bindingPath)
            {
                id = bindingId;
                path = bindingPath;
            }
        }

        /// <summary>
        /// stores the active control overrides to player prefs
        /// </summary>
        public static void StoreControlOverrides(InputActionAsset control)
        {
            //saving
            BindingWrapperClass bindingList = new BindingWrapperClass();
            foreach (var map in control.actionMaps)
            {
                foreach (var binding in map.bindings)
                {
                    if (!string.IsNullOrEmpty(binding.overridePath))
                    {
                        bindingList.bindingList.Add(new BindingSerializable(binding.id.ToString(), binding.overridePath));
                    }
                }
            }

            PlayerPrefs.SetString(PlayerPrefsKey, JsonUtility.ToJson(bindingList));
            PlayerPrefs.Save();
        }

        /// <summary>
        /// Loads control overrides from playerprefs
        /// </summary>
        public static void LoadControlOverrides(InputActionAsset control)
        {
            if (PlayerPrefs.HasKey(PlayerPrefsKey))
            {
                BindingWrapperClass bindingList = JsonUtility.FromJson(PlayerPrefs.GetString(PlayerPrefsKey), typeof(BindingWrapperClass)) as BindingWrapperClass;

                //create a dictionary to easier check for existing overrides
                Dictionary<System.Guid, string> overrides = new Dictionary<System.Guid, string>();
                foreach (var item in bindingList.bindingList)
                {
                    overrides.Add(new System.Guid(item.id), item.path);
                }

                //walk through action maps check dictionary for overrides
                foreach (var map in control.actionMaps)
                {
                    var bindings = map.bindings;
                    for (var i = 0; i < bindings.Count; ++i)
                    {
                        if (overrides.TryGetValue(bindings[i].id, out string overridePath))
                        {
                            //if there is an override apply it
                            map.ApplyBindingOverride(i, new InputBinding { overridePath = overridePath });
                        }
                    }
                }
            }
        }


        [SerializeField] private InputActionAsset actionAsset;

        private void Start()
        {
            LoadControlOverrides(actionAsset);
        }
    }
}