using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierlMaterial : MonoBehaviour
{
    MeshRenderer Matrenderer;
    // Start is called before the first frame update
    void Start()
    {
        Matrenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Matrenderer.material.mainTextureOffset = new Vector2(Time.time * 0.2f, 0);
    }
}
