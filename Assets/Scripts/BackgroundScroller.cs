using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [Range(-1f, 1f)]
    public float scrollSpeed = 0.5f;
    private float offset;
    private Material mat;

    private void Start()
    {
        mat = GetComponent<Renderer>().material;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        offset -= (Time.deltaTime * scrollSpeed) / 10f;
        mat.SetTextureOffset("_BaseMap", new Vector2(0f, offset));
    }
}
