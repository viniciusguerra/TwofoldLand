using UnityEngine;
using UnityEditor;
using System.Collections;

public static class ScriptableObjectUtility
{
    /// <summary>
    //	This makes it easy to create, name and place unique new ScriptableObject asset files.
    /// </summary>
    public static void CreateAsset<T>() where T : ScriptableObject
    {
        T asset = ScriptableObject.CreateInstance<T>();

        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (path == "")
        {
            path = "Assets";
        }
        else if (System.IO.Path.GetExtension(path) != "")
        {
            path = path.Replace(System.IO.Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
        }        

        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/New " + typeof(T).ToString() + ".asset");

        AssetDatabase.CreateAsset(asset, assetPathAndName);

        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
}

//public class ScriptableObjectCreator : Editor
//{
//    [MenuItem("Assets/Create/Type")]
//    public static void CreateAsset()
//    {
//        //ScriptableObjectUtility.CreateAsset<Type>();
//    }
//}

//public class InterfaceContainerCreator : Editor
//{
//    [MenuItem("Assets/Create/Interface Container")]
//    public static void CreateAsset()
//    {
//        ScriptableObjectUtility.CreateAsset<InterfaceContainer>();
//    }
//}

//public class UnlockableInterfaceContainerCreator : Editor
//{
//    [MenuItem("Assets/Create/Interface Containers/IUnlockable")]
//    public static void CreateAsset()
//    {
//        ScriptableObjectUtility.CreateAsset<UnlockableInterfaceContainer>();
//    }
//}

//public class VulnerableInterfaceContainerCreator : Editor
//{
//    [MenuItem("Assets/Create/Interface Containers/IVulnerable")]
//    public static void CreateAsset()
//    {
//        ScriptableObjectUtility.CreateAsset<VulnerableInterfaceContainer>();
//    }
//}

public class SkillDataCreator : Editor
{
    [MenuItem("Assets/Create/Skill Data")]
    public static void CreateAsset()
    {
        ScriptableObjectUtility.CreateAsset<SkillData>();
    }
}