using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Collections.Generic;

public class SceneEditorWindow : EditorWindow
{
    private string[] sceneNames;
    private int selectedSceneIndex = -1;

    [MenuItem("Window/Scene Editor")]
    public static void ShowWindow()
    {
        GetWindow<SceneEditorWindow>("Scene Editor");
    }

    private void OnEnable()
    {
        RefreshSceneNames();
    }

    private void OnGUI()
    {
        GUILayout.Label("Scene Manager", EditorStyles.boldLabel);

        if (sceneNames == null || sceneNames.Length == 0)
        {
            GUILayout.Label("No scenes available.");
            return;
        }

        selectedSceneIndex = EditorGUILayout.Popup("Select Scene", selectedSceneIndex, sceneNames);

        if (GUILayout.Button("Load Scene"))
        {
            if (selectedSceneIndex >= 0 && selectedSceneIndex < sceneNames.Length)
            {
                LoadScene(sceneNames[selectedSceneIndex]);
            }
        }

        if (GUILayout.Button("Save Scene State"))
        {
            SaveSceneState();
        }
    }

    private void RefreshSceneNames()
    {
        string[] scenes = new string[SceneManager.sceneCountInBuildSettings];
        for (int i = 0; i < scenes.Length; i++)
        {
            scenes[i] = System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
        }
        sceneNames = scenes;
    }

    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    private void SaveSceneState()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        string defaultFileName = sceneName + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".json";

        // Пользователь выбирает место сохранения файла
        string saveFilePath = EditorUtility.SaveFilePanel("Save Scene State", "", defaultFileName, "json");

        if (!string.IsNullOrEmpty(saveFilePath))
        {
            SceneData sceneData = new SceneData();

            GameObject[] sceneObjects = SceneManager.GetActiveScene().GetRootGameObjects();
            foreach (GameObject obj in sceneObjects)
            {
                foreach (Transform child in obj.GetComponentsInChildren<Transform>())
                {
                    if (child.gameObject.activeSelf)
                    {
                        sceneData.objects.Add(new ObjectData(child));
                    }
                }
            }

            string json = JsonUtility.ToJson(sceneData, true);
            File.WriteAllText(saveFilePath, json);

            Debug.Log("Scene state saved to: " + saveFilePath);
        }
    }

}

[Serializable]
public class ObjectData
{
    public string name;
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;
    public string tag; 
    public string[] components; 

    public ObjectData(Transform transform)
    {
        name = transform.name;
        position = transform.position;
        rotation = transform.rotation;
        scale = transform.localScale;
        tag = transform.tag;

        Component[] allComponents = transform.GetComponents<Component>();
        components = new string[allComponents.Length];
        for (int i = 0; i < allComponents.Length; i++)
        {
            components[i] = allComponents[i].GetType().Name;
        }
    }
}

[Serializable]
public class SceneData
{
    public List<ObjectData> objects = new List<ObjectData>();
    public string sceneDescription;
    public int objectCount; 
    public string[] usedResources; 
    public Vector3 cameraPosition;
    public Quaternion cameraRotation;
    public Light[] lights; 
}