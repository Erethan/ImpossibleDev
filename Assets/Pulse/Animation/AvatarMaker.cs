using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

//Script from austintaylorx's post at Apr 6, 2019
//https://forum.unity.com/threads/how-to-create-an-avatar-mask-for-custom-gameobject-hierarchy-from-scene.574270/
//

namespace Pulse.Editor
{
    public class AvatarMaker
    {
#if UNITY_EDITOR
        [MenuItem("CustomTools/MakeAvatarMask")]
        private static void MakeAvatarMask()
        {
            GameObject activeGameObject = Selection.activeGameObject;

            if (activeGameObject != null)
            {
                AvatarMask avatarMask = new AvatarMask();

                avatarMask.AddTransformPath(activeGameObject.transform);

                var path = string.Format("Assets/{0}.mask", activeGameObject.name.Replace(':', '_'));
                AssetDatabase.CreateAsset(avatarMask, path);
            }
        }

        [MenuItem("CustomTools/MakeAvatar")]
        private static void MakeAvatar()
        {
            GameObject activeGameObject = Selection.activeGameObject;

            if (activeGameObject != null)
            {
                UnityEngine.Avatar avatar = AvatarBuilder.BuildGenericAvatar(activeGameObject, "");
                avatar.name = activeGameObject.name;
                Debug.Log(avatar.isHuman ? "is human" : "is generic");

                var path = string.Format("Assets/{0}.ht", avatar.name.Replace(':', '_'));
                AssetDatabase.CreateAsset(avatar, path);
            }
        }
#endif
    }
}

