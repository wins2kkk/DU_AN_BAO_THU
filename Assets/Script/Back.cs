using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Back : MonoBehaviour
{
    
    [Range(-1f, 1f)]
    public float scrollingSPeed = 0.5f;
    private float offset;
    private Material mat;

    // Update is called once per frame
    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }
    void Update()
    {
        offset += (Time.deltaTime * scrollingSPeed / 10f);
        mat.SetTextureOffset("MainTex", new Vector2(offset, 0));
    }

}
