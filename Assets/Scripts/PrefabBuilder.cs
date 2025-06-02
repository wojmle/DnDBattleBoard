#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;

public class PrefabBuilder
{
    [MenuItem("Tools/Generate Enemy Prefabs")]
    public static void CreateEnemyPrefabs()
    {
        string fbxFolder = "Assets/FBX/Adversary"; // adjust as needed
        string prefabOutputFolder = "Assets/Prefabs/Adversary";

        if (!Directory.Exists(prefabOutputFolder))
            Directory.CreateDirectory(prefabOutputFolder);

        string[] guids = AssetDatabase.FindAssets("t:Model", new[] { fbxFolder });

        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            GameObject fbx = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
            GameObject instance = GameObject.Instantiate(fbx);
            instance.transform.localScale = new Vector3(1000f, 1000f, 1000f); // Adjust scale as needed

            // Add required components
            var rb = instance.GetComponent<Rigidbody>();
            if (rb == null)
                rb = instance.AddComponent<Rigidbody>();

            rb.useGravity = false;
            rb.isKinematic = true;

            var mc = instance.GetComponent<MeshCollider>();
            if (mc == null)
            {
                MeshFilter mf = instance.GetComponentInChildren<MeshFilter>();
                if (mf != null)
                {
                    mc = instance.AddComponent<MeshCollider>();
                    mc.sharedMesh = mf.sharedMesh;
                }
            }

            // Configure MeshCollider
            if (mc != null)
            {
                mc.convex = true;
                mc.isTrigger = true;
            }

            if (instance.GetComponent<CheckPlacement>() == null)
                instance.AddComponent<CheckPlacement>();

            var enemyCtrl = instance.GetComponent<EnemyController>();

            if (enemyCtrl == null)
                enemyCtrl = instance.AddComponent<EnemyController>();

            // Save as prefab
            string filename = Path.GetFileNameWithoutExtension(assetPath);
            string prefabPath = $"{prefabOutputFolder}/{filename}.prefab";
            PrefabUtility.SaveAsPrefabAsset(instance, prefabPath);

            GameObject.DestroyImmediate(instance);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Enemy prefabs generated.");
    }

    [MenuItem("Tools/Generate Ally Prefabs")]
    public static void CreateAllyPrefabs()
    {
        string fbxRootFolder = "Assets/FBX/Ally";
        string prefabRootFolder = "Assets/Prefabs/Ally";

        if (!Directory.Exists(prefabRootFolder))
            Directory.CreateDirectory(prefabRootFolder);

        // Iterate over each subfolder (race) in FBX/Ally
        foreach (var raceDir in Directory.GetDirectories(fbxRootFolder))
        {
            string raceName = Path.GetFileName(raceDir);
            string prefabRaceDir = Path.Combine(prefabRootFolder, raceName);

            if (!Directory.Exists(prefabRaceDir))
                Directory.CreateDirectory(prefabRaceDir);

            // Find all FBX models in this race directory
            string[] guids = AssetDatabase.FindAssets("t:Model", new[] { raceDir });

            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                GameObject fbx = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
                GameObject instance = GameObject.Instantiate(fbx);

                // Add required components
                var rb = instance.GetComponent<Rigidbody>();
                if (rb == null)
                    rb = instance.AddComponent<Rigidbody>();

                rb.useGravity = false;
                rb.isKinematic = true;

                var mc = instance.GetComponent<MeshCollider>();
                if (mc == null)
                {
                    MeshFilter mf = instance.GetComponentInChildren<MeshFilter>();
                    if (mf != null)
                    {
                        mc = instance.AddComponent<MeshCollider>();
                        mc.sharedMesh = mf.sharedMesh;
                    }
                }

                if (mc != null)
                {
                    mc.convex = true;
                    mc.isTrigger = true;
                }

                if (instance.GetComponent<CheckPlacement>() == null)
                    instance.AddComponent<CheckPlacement>();

                var enemyCtrl = instance.GetComponent<AllyController>();
                if (enemyCtrl == null)
                    enemyCtrl = instance.AddComponent<AllyController>();

                // Save as prefab in the corresponding output subfolder
                string filename = Path.GetFileNameWithoutExtension(assetPath);
                string prefabPath = Path.Combine(prefabRaceDir, filename + ".prefab").Replace("\\", "/");
                PrefabUtility.SaveAsPrefabAsset(instance, prefabPath);

                GameObject.DestroyImmediate(instance);
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Ally prefabs generated.");
    }
}
#endif
