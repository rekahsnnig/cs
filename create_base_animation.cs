using UnityEngine;
using UnityEditor;
using System;
using System.Collections;


public class create_base_animation : MonoBehaviour
{
    public string gameobject_name = "none";
    public string topBone_name = "Armature";
    private string[] xyz = { "x", "y", "z" };
    string path;
    // Use this for initialization

    void Start()
    {
        var animclip = new AnimationClip();
        SkinnedMeshRenderer renderer;
        EditorCurveBinding curveBinding;// = new EditorCurveBinding();
                                        //try
                                        //{
        renderer = GameObject.Find(gameobject_name).GetComponent<SkinnedMeshRenderer>();
        //}
        //catch(NullReferenceException)
        //{
        //    Debug.Log("not found");
        //    //return;
        //}

        // curveBinding.propertyName = typeof(localEulerAnglesRaw.x);
        //curveBinding.propertyName = "localPosition.x";

        AnimationCurve curve = new AnimationCurve();
        Vector3 defaultBone;
        foreach (Transform bone in renderer.bones)
        {
            curveBinding = new EditorCurveBinding();
            path = GetGameObjectPath(bone);
            curveBinding.path = path.Remove(0, (path.IndexOf(topBone_name)));
            curveBinding.type = typeof(Transform);
            Debug.Log(curveBinding.path);

            for (int i = 0; i < 3; i++)
            {
                defaultBone = GameObject.Find(bone.name).transform.localPosition;
                // defaultBone = transform.InverseTransformDirection(defaultBone);

                //Debug.Log(Vector2float(i, defaultBone));
                curveBinding.propertyName = ("m_LocalPosition." + xyz[i]);
                curve.AddKey(0.0f, Vector2float(i, defaultBone));
                curve.AddKey(5.0f, Vector2float(i, defaultBone));
                AnimationUtility.SetEditorCurve(animclip, curveBinding, curve);
                Debug.Log(bone.name + Vector2float(i, defaultBone));
                curve = new AnimationCurve();

                defaultBone = GameObject.Find(bone.name).transform.localEulerAngles;
                // defaultBone = transform.InverseTransformDirection(defaultBone);

                curveBinding.propertyName = ("localEulerAnglesRaｗ." + xyz[i]);
                curve.AddKey(0.0f, Vector2float(i, defaultBone));
                curve.AddKey(5.0f, Vector2float(i, defaultBone));

                AnimationUtility.SetEditorCurve(animclip, curveBinding, curve);

                curve = new AnimationCurve();

                defaultBone = GameObject.Find(bone.name).transform.localScale;
                // defaultBone = transform.InverseTransformDirection(defaultBone);

                curveBinding.propertyName = ("m_LocalScale." + xyz[i]);
                curve.AddKey(0.0f, Vector2float(i, defaultBone));
                curve.AddKey(5.0f, Vector2float(i, defaultBone));

                AnimationUtility.SetEditorCurve(animclip, curveBinding, curve);

                curve = new AnimationCurve();
            }
        }
        AssetDatabase.CreateAsset(
                animclip,
                AssetDatabase.GenerateUniqueAssetPath("Assets/" + gameobject_name + "_basic_anm" + ".anim")
            );
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    public static float Vector2float(int i, Vector3 vec3)
    {
        return (i < 1) ? vec3.x : (i < 2) ? vec3.y : vec3.z;
    }

    private static string GetGameObjectPath(Transform transform)
    {
        string path = transform.name;
        while (transform.parent != null)
        {
            transform = transform.parent;
            path = transform.name + "/" + path;
        }
        return path;
    }

}
