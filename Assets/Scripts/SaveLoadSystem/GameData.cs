using System;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public List<SceneData> scenes = new();
    public SceneData defaultScene = new()
    {
        sceneName = "DefaultScene",
    };
}

[Serializable]
public class SceneData
{
    public string sceneName; 
    public List<SceneAlly> allies = new();
    public List<SceneAdversary> enemies = new();
    public List<Ally> definedAllies = new();
}

[Serializable]
public class SerializableTransform
{
    public float posX, posY, posZ;
    public float rotX, rotY, rotZ, rotW;
    public float scaleX, scaleY, scaleZ;

    public SerializableTransform() { }

    public SerializableTransform(Transform t)
    {
        posX = t.position.x;
        posY = t.position.y;
        posZ = t.position.z;
        rotX = t.rotation.x;
        rotY = t.rotation.y;
        rotZ = t.rotation.z;
        rotW = t.rotation.w;
        scaleX = t.localScale.x;
        scaleY = t.localScale.y;
        scaleZ = t.localScale.z;
    }

    public Vector3 GetPosition()
    {
        return new Vector3(posX, posY, posZ);
    }

    public Quaternion GetRotation()
    {
        return new Quaternion(rotX, rotY, rotZ, rotW);
    }

    public Vector3 GetScale()
    {
        return new Vector3(scaleX, scaleY, scaleZ);
    }
}

[Serializable]
public class SceneAlly
{
    public SceneAlly(Ally ally, Transform transform)
    {
        this.ally = ally;
        this.transform = new SerializableTransform(transform);
    }

    public Ally ally;
    public SerializableTransform transform;
}

[Serializable]
public class SceneAdversary
{
    public SceneAdversary(Adversary adversary, Transform transform)
    {
        this.adversary = adversary;
        this.transform = new SerializableTransform(transform);
    }
    public Adversary adversary;
    public SerializableTransform transform;
}