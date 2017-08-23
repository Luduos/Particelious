using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MobileBloom : MonoBehaviour {

    public float Threshold = 0.5f;

    [SerializeField]
    private Shader ThresholdingShader = null;
    [SerializeField]
    private Shader GaussianBlurShader = null;

    private Material ThresholdMaterial;
    private Material BlurMaterial;

    void Awake()
    {
        ThresholdMaterial = new Material(ThresholdingShader);
        BlurMaterial = new Material(GaussianBlurShader);
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (Threshold == 0)
        {
            Graphics.Blit(source, destination);
            return;
        }

        //RenderTexture ThresholdTex = RenderTexture.GetTemporary(source.width, source.height);

        //ThresholdMaterial.SetFloat("_threshold", Threshold);
        //Graphics.Blit(source, ThresholdTex, ThresholdMaterial);
        Graphics.Blit(source, destination, BlurMaterial);

       // RenderTexture.ReleaseTemporary(ThresholdTex);
    }
}
