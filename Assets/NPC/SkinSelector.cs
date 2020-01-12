using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Skins
{
    public Material[] materials;
    public int key;
}

public class SkinSelector : MonoBehaviour
{
    public Skins[] skins;

    // Start is called before the first frame update
    void Start()
    {
        SelectRandom();
    }

    public void SelectRandom() {
        SkinnedMeshRenderer renderer = this.GetComponentInChildren<SkinnedMeshRenderer>();

        Material[] newMaterials = new Material[renderer.materials.Length];
        Array.Copy(renderer.materials, 0, newMaterials, 0, renderer.materials.Length);

        foreach (Skins skin in skins)
        {
            newMaterials[skin.key] = skin.materials[UnityEngine.Random.Range(0, skin.materials.Length)];
        }

        renderer.materials = newMaterials;
    }
}
