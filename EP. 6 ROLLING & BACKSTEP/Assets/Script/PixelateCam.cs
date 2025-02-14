using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PixelateCam : MonoBehaviour
{
    [Range(1, 100)] public int pixelate = 1; // 기본값 설정

    [SerializeField] private Material material; // 픽셀화 효과를 위한 머테리얼

    private void Awake()
    {
        // 머테리얼이 없다면 기본 머테리얼 생성
        if (material == null)
        {
            material = new Material(Shader.Find("Hidden/Internal-Colored"));
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        int width = source.width / pixelate;
        int height = source.height / pixelate;
        
        RenderTexture resultTexture = RenderTexture.GetTemporary(width, height);
        resultTexture.filterMode = FilterMode.Point;
        
        Graphics.Blit(source, resultTexture);
        Graphics.Blit(resultTexture, destination);
        
        RenderTexture.ReleaseTemporary(resultTexture);
    }
}