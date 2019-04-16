using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace cpGames.core
{
    public interface IRandomRoll
    {
        #region Properties
        float Chance { get; }
        #endregion
    }

    public static class CpUnityExtensions
    {
        #region Methods
        public static Transform FindOrAddChild(this Transform transform, string name)
        {
            var child = transform.Find(name);
            if (child != null)
            {
                return child;
            }
            return AddChild(transform, name).transform;
        }

        public static bool IsOppositeSign(float a, float b)
        {
            if (a < 0 && b > 0)
            {
                return true;
            }
            if (a > 0 && b < 0)
            {
                return true;
            }
            return false;
        }

        public static string GetFullPathName(this GameObject gob)
        {
            var fullPathName = "";
            if (gob.transform.parent != null)
            {
                fullPathName = gob.transform.parent.gameObject.GetFullPathName() + "/";
            }
            fullPathName += gob.name;
            return fullPathName;
        }

        public static bool IsRelated<T>(this Type tSelf)
        {
            var tOther = typeof(T);
            if (tSelf == tOther)
            {
                return true;
            }
            if (tSelf.IsSubclassOf(tOther))
            {
                return true;
            }
            return false;
        }

        public static Match RegexMatch(this string text, string regexExp)
        {
            var regex = new Regex(regexExp);
            return regex.Match(text);
        }

        public static Toggle GetActive(this ToggleGroup aGroup)
        {
            return aGroup.ActiveToggles().FirstOrDefault();
        }

        public static void SetLayerRecursively(this GameObject go, int layerNumber)
        {
            if (go == null)
            {
                return;
            }
            foreach (var trans in go.GetComponentsInChildren<Transform>(true))
            {
                trans.gameObject.layer = layerNumber;
            }
        }

        public static string GetSceneName(Type sceneType)
        {
            var sceneName = sceneType.Name;
            if (sceneName.EndsWith("view", StringComparison.CurrentCultureIgnoreCase))
            {
                sceneName = sceneName.Substring(0, sceneName.Length - 4);
            }
            else if (sceneName.EndsWith("model", StringComparison.CurrentCultureIgnoreCase))
            {
                sceneName = sceneName.Substring(0, sceneName.Length - 5);
            }
            return sceneName;
        }

        public static string GetSceneName<T>()
        {
            return GetSceneName(typeof(T));
        }

        public static bool LoadLevelAdditive<T>()
        {
            return LoadLevelAdditive(typeof(T));
        }

        public static bool LoadLevelAdditive(Type sceneType)
        {
            return LoadLevelAdditive(GetSceneName(sceneType));
        }

        public static bool LoadLevelAdditive(string sceneName)
        {
            if (!SceneManager.GetSceneByName(sceneName).isLoaded)
            {
                SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
                return true;
            }
            return false;
        }

        public static bool UnloadLevelAdditive<T>()
        {
            return UnloadLevelAdditive(typeof(T));
        }

        public static bool UnloadLevelAdditive(Type sceneType)
        {
            var sceneName = GetSceneName(sceneType);
            if (SceneManager.GetSceneByName(sceneName).isLoaded)
            {
                SceneManager.UnloadSceneAsync(sceneName);
                return true;
            }
            return false;
        }

        public static T AddChild<T>(this GameObject parent,
            T prefab,
            Vector3 position,
            Quaternion rotation,
            Vector3 scale) where T : Component
        {
            var gob = Object.Instantiate(prefab, parent?.transform);

            if (gob != null && parent != null)
            {
                var t = gob.transform;
                t.localPosition = position;
                t.localRotation = rotation;
                t.localScale = scale;
            }
            return gob;
        }

        public static T AddChild<T>(this GameObject parent,
            Vector3 position,
            Quaternion rotation,
            Vector3 scale) where T : Component
        {
            var gob = new GameObject(typeof(T).Name);

            if (parent != null)
            {
                var t = gob.transform;
                t.SetParent(parent.transform);
                t.localPosition = position;
                t.localRotation = rotation;
                t.localScale = scale;
            }
            return gob.AddComponent<T>();
        }

        public static T AddChild<T>(this GameObject parent, T prefab) where T : Component
        {
            return AddChild(parent, prefab, Vector3.zero, Quaternion.identity, Vector3.one);
        }

        public static T AddChild<T>(this Transform parent, T prefab) where T : Component
        {
            return AddChild(parent.gameObject, prefab, Vector3.zero, Quaternion.identity,
                Vector3.one);
        }

        public static T AddChild<T>(this GameObject parent) where T : Component
        {
            return AddChild<T>(parent, Vector3.zero, Quaternion.identity, Vector3.one);
        }

        public static T AddChild<T>(this Transform parent) where T : Component
        {
            return AddChild<T>(parent.gameObject, Vector3.zero, Quaternion.identity, Vector3.one);
        }

        public static GameObject AddChild(this GameObject parent,
            GameObject prefab,
            Vector3 position,
            Quaternion rotation,
            Vector3 scale)
        {
            var go = Object.Instantiate(prefab);

            if (go != null && parent != null)
            {
                var t = go.transform;
                t.SetParent(parent.transform);

                t.localPosition = position;
                t.localRotation = rotation;
                t.localScale = scale;
            }
            return go;
        }

        public static GameObject AddChild(this GameObject parent,
            string name,
            Vector3 position,
            Quaternion rotation,
            Vector3 scale)
        {
            var go = new GameObject(name);

            if (parent != null)
            {
                var t = go.transform;
                t.SetParent(parent.transform);

                t.localPosition = position;
                t.localRotation = rotation;
                t.localScale = scale;
            }
            return go;
        }

        public static GameObject AddChild(this GameObject parent, string name)
        {
            return AddChild(parent, name, Vector3.zero, Quaternion.identity, Vector3.one);
        }

        public static GameObject AddChild(this Transform parent, string name)
        {
            return AddChild(parent.gameObject, name, Vector3.zero, Quaternion.identity,
                Vector3.one);
        }

        public static GameObject AddChild(this GameObject parent, GameObject prefab)
        {
            return AddChild(parent, prefab, Vector3.zero, Quaternion.identity, Vector3.one);
        }

        public static GameObject AddChild(this Transform parent, GameObject prefab)
        {
            return AddChild(parent.gameObject, prefab, Vector3.zero, Quaternion.identity,
                Vector3.one);
        }

        public static List<T> FindAllChildren<T>(this Transform current)
        {
            return
                current.Cast<Transform>()
                    .Select(child => child.GetComponent<T>())
                    .Where(component => component != null)
                    .ToList();
        }

        public static Transform FindChild(this Transform transform, string name)
        {
            for (var i = 0; i < transform.childCount; ++i)
            {
                var child = transform.GetChild(i);
                if (child.name == name)
                {
                    return child;
                }
            }
            return null;
        }

        public static T FindChild<T>(this Transform transform, string name) where T : Component
        {
            for (var i = 0; i < transform.childCount; ++i)
            {
                var child = transform.GetChild(i);
                if (child.name == name)
                {
                    var component = child.GetComponent<T>();
                    if (component != null)
                    {
                        return component;
                    }
                }
            }
            return null;
        }

        public static T FindChild<T>(this Transform transform) where T : Component
        {
            for (var i = 0; i < transform.childCount; ++i)
            {
                var child = transform.GetChild(i);
                var component = child.GetComponent<T>();
                if (component != null)
                {
                    return component;
                }
            }
            return null;
        }

        public static Transform FindChildRecursively(this Transform current, string name, bool partialMatch = false)
        {
            if (partialMatch && current.name.Contains(name) || current.name == name)
            {
                return current;
            }

            for (var i = 0; i < current.childCount; ++i)
            {
                var found = FindChildRecursively(current.GetChild(i), name, partialMatch);

                if (found != null)
                {
                    return found;
                }
            }
            return null;
        }

        public static T FindChildRecursively<T>(this Transform current, string name)
        {
            if (current.name == name)
            {
                return current.GetComponent<T>();
            }

            for (var i = 0; i < current.childCount; ++i)
            {
                var found = FindChildRecursively(current.GetChild(i), name);

                if (found != null)
                {
                    return found.GetComponent<T>();
                }
            }
            return default;
        }

        public static T FindChildRecursively<T>(this Transform current)
        {
            var result = current.GetComponent<T>();

            if (result != null)
            {
                return result;
            }

            foreach (Transform child in current)
            {
                result = FindChildRecursively<T>(child);

                if (result != null)
                {
                    return result;
                }
            }
            return default;
        }

        public static List<T> FindFirstChildrenRecursively<T>(this Transform current)
            where T : Component
        {
            var result = new List<T>();

            var component = current.GetComponent<T>();
            if (component != null)
            {
                result.Add(component);
            }
            else
            {
                foreach (Transform child in current)
                {
                    result.AddRange(FindFirstChildrenRecursively<T>(child));
                }
            }

            return result;
        }

        public static List<T> FindAllChildrenRecursively<T>(this Transform current)
        {
            var result = new List<T>();

            var component = current.GetComponent<T>();
            if (component != null)
            {
                result.Add(component);
            }

            foreach (Transform child in current)
            {
                result.AddRange(FindAllChildrenRecursively<T>(child));
            }

            return result;
        }

        public static List<T> FindAllChildrenRecursively<T>(this Transform current, string name)
            where T : Component
        {
            var result = new List<T>();

            foreach (Transform child in current)
            {
                result.AddRange(FindAllChildrenRecursively<T>(child, name));
            }

            if (current.name == name)
            {
                var component = current.GetComponent<T>();
                if (component != null)
                {
                    result.Add(component);
                }
            }

            return result;
        }

        public static List<Transform> FindAllChildrenRecursively(this Transform current, string name, bool partialMatch = false)
        {
            var result = new List<Transform>();

            foreach (Transform child in current)
            {
                result.AddRange(FindAllChildrenRecursively<Transform>(child, name));
            }

            if (current && current.name.Contains(name) || current.name == name)
            {
                result.Add(current);
            }

            return result;
        }

        public static void DeleteChildren(this Transform transform)
        {
            var toDestroy = new List<GameObject>();
            foreach (Transform t in transform)
            {
                toDestroy.Add(t.gameObject);
            }
            toDestroy.ForEach(Object.Destroy);
        }

        public static void DeleteChildren(this GameObject gameObject)
        {
            var toDestroy = new List<GameObject>();
            foreach (Transform t in gameObject.transform)
            {
                toDestroy.Add(t.gameObject);
            }
            toDestroy.ForEach(Object.Destroy);
        }

        public static T FindFirstParentInterface<T>(this Transform current)
        {
            if (current == null)
            {
                return default;
            }

            while (true)
            {
                var components = current.gameObject.GetComponents(typeof(Component));
                var result = components.FirstOrDefault(x => x is T);
                if (result != null)
                {
                    return (T)(object)result;
                }
                current = current.parent;
                if (current == null)
                {
                    return default;
                }
            }
        }

        public static T FindFirstParent<T>(this Transform current)
        {
            if (current == null)
            {
                return default;
            }

            while (true)
            {
                var result = current.GetComponent<T>();
                if (result != null)
                {
                    return result;
                }
                current = current.parent;
                if (current == null)
                {
                    return default;
                }
            }
        }

        public static T FindNextParent<T>(this Transform current)
        {
            if (current == null || current.parent == null)
            {
                return default;
            }
            return current.parent.FindFirstParent<T>();
        }

        public static T FindLastParent<T>(this Transform current) where T : Component
        {
            if (current == null || current.parent == null)
            {
                return null;
            }

            current = current.parent;
            T lastParent = null;

            while (true)
            {
                var result = current.GetComponent<T>();
                if (result != null)
                {
                    lastParent = result;
                }
                current = current.parent;
                if (current == null)
                {
                    return lastParent;
                }
            }
        }

        public static Bounds GetBoundsRecursively(Transform t)
        {
            var bounds = new Bounds(Vector3.zero, Vector3.zero);

            foreach (Transform child in t)
            {
                var childBounds = GetBoundsRecursively(child);
                if (childBounds.extents.sqrMagnitude != 0)
                {
                    if (bounds.extents.sqrMagnitude == 0)
                    {
                        bounds = childBounds;
                    }
                    else
                    {
                        bounds.Encapsulate(childBounds);
                    }
                }
            }

            var r = t.GetComponent<Renderer>();
            if (r != null)
            {
                if (bounds.extents.sqrMagnitude == 0)
                {
                    bounds = r.bounds;
                }
                else
                {
                    bounds.Encapsulate(r.bounds);
                }
            }

            return bounds;
        }

        public static bool Compare(int[] a, int[] b)
        {
            if (a == null || b == null)
            {
                return a == b;
            }

            if (a.Length != b.Length)
            {
                return false;
            }

            for (var i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i])
                {
                    return false;
                }
            }

            return true;
        }

        public static bool Compare(float a, float b, float precision)
        {
            if (a == b)
            {
                return true;
            }
            if (Mathf.Abs(a - b) < precision)
            {
                return true;
            }
            return false;
        }

        public static bool Compare(Vector3 a, Vector3 b, float precision)
        {
            return Compare(a.x, b.x, precision) &&
                Compare(a.y, b.y, precision) &&
                Compare(a.z, b.z, precision);
        }

        public static bool Compare(Quaternion a, Quaternion b, float precision)
        {
            var rot = Quaternion.Inverse(a) * b;
            float angle;
            Vector3 axis;
            rot.ToAngleAxis(out angle, out axis);
            return angle < precision;
        }

        public static float CalcVerticalAngle(Vector2 start, Vector2 finish, float bulletVelocity)
        {
            var g = -Physics.gravity.y;
            var x = Mathf.Abs(finish.x - start.x);
            var y = finish.y - start.y;
            var v2 = bulletVelocity * bulletVelocity;
            var v4 = v2 * v2;
            var D = v4 - g * (g * x * x + 2 * y * v2);
            var sqrt = Mathf.Sqrt(D);
            var reqAngle1 = Mathf.Atan((v2 - sqrt) / (g * x)) * Mathf.Rad2Deg;
            return reqAngle1;
        }

        public static float ClampAngle(float angle)
        {
            angle = angle % 360;

            if (angle < -180)
            {
                angle += 360;
            }
            if (angle > 180)
            {
                angle -= 360;
            }

            return angle;
        }

        public static float ClampAngle(float angle, float min, float max)
        {
            return Mathf.Clamp(ClampAngle(angle), min, max);
        }

        public static Vector3 RelativePosition(Vector3 point, Transform t, Transform relativeTo)
        {
            var pos = t.TransformPoint(point);
            var relativePos = relativeTo.InverseTransformPoint(pos);
            return relativePos;
        }

        public static Vector2 ToVector2(this Vector3 input)
        {
            return new Vector2(input.x, input.y);
        }

        public static IRandomRoll PickRandom(List<IRandomRoll> collection)
        {
            float total = 0;
            foreach (var x in collection)
            {
                total += x.Chance;
            }

            var random = Random.Range(0, total);
            foreach (var x in collection)
            {
                random -= x.Chance;
                if (random <= 0)
                {
                    return x;
                }
            }
            return null;
        }

        public static List<B> CovariantCast<B, D>(List<D> derivedList) where D : B
        {
            var baseList = new List<B>();
            derivedList.ForEach(x => baseList.Add(x));
            return baseList;
        }

        public static string ToHex(this Color32 color)
        {
            var hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
            return hex;
        }
        #endregion

#if UNITY_EDITOR
        public static bool IsObjectSelected(GameObject gob, int levels)
        {
            foreach (var selected in Selection.gameObjects)
            {
                if (selected == gob)
                {
                    return true;
                }

                var current = gob.transform.parent;
                while (current != null && levels > 0)
                {
                    if (current.gameObject == selected)
                    {
                        return true;
                    }
                    current = current.parent;
                    levels--;
                }
            }
            return false;
        }

        public static bool AreSiblingsSelected(GameObject gob, int levels)
        {
            var parent = gob.transform.parent;

            while (parent != null && levels > 0)
            {
                foreach (Transform t in parent)
                {
                    foreach (var selected in Selection.gameObjects)
                    {
                        if (t.gameObject == selected)
                        {
                            return true;
                        }
                    }
                }

                parent = parent.parent;
                levels--;
            }

            return false;
        }

        public static void DrawTextWithOutlines(string text,
            Vector3 position,
            Transform cameraTransform,
            float width,
            Color textColor,
            Color borderColor,
            int fontSize = 12,
            float maxDist = 100)
        {
            DrawTextWithOutlines(text,
                position,
                cameraTransform.up,
                cameraTransform.right,
                Vector3.Distance(cameraTransform.position, position),
                width,
                textColor,
                borderColor,
                fontSize,
                maxDist);
        }

        public static void DrawTextWithOutlines(string text,
            Vector3 position,
            Vector3 cameraUp,
            Vector3 cameraRight,
            float cameraDistance,
            float width,
            Color textColor,
            Color borderColor,
            int fontSize = 12,
            float maxDist = 100)
        {
            if (maxDist < cameraDistance)
            {
                return;
            }

            var up = cameraUp * width * cameraDistance;
            var right = cameraRight * width * cameraDistance;

            var colorOld = GUI.color;
            var oldFontSize = GUI.skin.label.fontSize;
            GUI.skin.label.fontSize = fontSize;

            GUI.color = borderColor;
            Handles.Label(position + up, text);
            Handles.Label(position - up, text);
            Handles.Label(position + right, text);
            Handles.Label(position - right, text);

            GUI.color = textColor;
            Handles.Label(position, text);

            GUI.skin.label.fontSize = oldFontSize;
            GUI.color = colorOld;
        }
#endif
    }
}