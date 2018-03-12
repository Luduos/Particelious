using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class MobileBloom : MonoBehaviour
{
    public float intensity = 0.7f;
    public float threshhold = 0.75f;
    public float blurWidth = 1.0f;

    public bool extraBlurry = false;

    // image effects materials for internal use

    public Material bloomMaterial = null;

    private bool supported = false;

    private RenderTexture tempRtA  = null;
    private RenderTexture tempRtB  = null;


    bool Supported() {
	    if(supported) return true;
	    supported = (SystemInfo.supportsImageEffects && bloomMaterial.shader.isSupported);
	    return supported;
    }

    void CreateBuffers()
    {
        //if (!tempRtA)
        //{
            tempRtA = RenderTexture.GetTemporary(Screen.width / 4, Screen.height / 4, 0);
            tempRtA.hideFlags = HideFlags.DontSave;
        //}
        //if (!tempRtB)
        //{
            tempRtB = RenderTexture.GetTemporary(Screen.width / 4, Screen.height / 4, 0);
            tempRtB.hideFlags = HideFlags.DontSave;
        //}
    }

    void OnDisable()
    {
        if (tempRtA)
        {
            //RenderTexture.ReleaseTemporary(tempRtA);
            //DestroyImmediate(tempRtA);
            tempRtA = null;
        }
        if (tempRtB)
        {
            //RenderTexture.ReleaseTemporary(tempRtB);
            // DestroyImmediate(tempRtB);
            tempRtB = null;
        }
    }

    bool EarlyOutIfNotSupported(RenderTexture source, RenderTexture destination) {
	    if (!Supported()) {
		    enabled = false;
		    Graphics.Blit(source, destination);	
		    return true;
	    }	
	    return false;
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        CreateBuffers();
        if (EarlyOutIfNotSupported(source, destination))
            return;

        // prepare data

        bloomMaterial.SetVector("_Parameter", new Vector4(0.0f, 0.0f, threshhold, intensity / (1.0f - threshhold)));

        // ds & blur
        float oneOverW  = 1.0f / (source.width);
        float oneOverH = 1.0f / (source.height);

        bloomMaterial.SetVector("_OffsetsA", new Vector4(1.5f * oneOverW, 1.5f * oneOverH, -1.5f * oneOverW, 1.5f * oneOverH));
        bloomMaterial.SetVector("_OffsetsB", new Vector4(-1.5f * oneOverW, -1.5f * oneOverH, 1.5f * oneOverW, -1.5f * oneOverH));

        Graphics.Blit(source, tempRtB, bloomMaterial, 1);

        oneOverW *= 4.0f * blurWidth;
        oneOverH *= 4.0f * blurWidth;

        bloomMaterial.SetVector("_OffsetsA", new Vector4(1.5f * oneOverW, 0.0f, -1.5f * oneOverW, 0.0f));
        bloomMaterial.SetVector("_OffsetsB", new Vector4(0.5f * oneOverW, 0.0f, -0.5f * oneOverW, 0.0f));
        Graphics.Blit(tempRtB, tempRtA, bloomMaterial, 2);

        bloomMaterial.SetVector("_OffsetsA", new Vector4(0.0f, 1.5f * oneOverH, 0.0f, -1.5f * oneOverH));
        bloomMaterial.SetVector("_OffsetsB", new Vector4(0.0f, 0.5f * oneOverH, 0.0f, -0.5f * oneOverH));
        Graphics.Blit(tempRtA, tempRtB, bloomMaterial, 2);

        if (extraBlurry)
        {
            bloomMaterial.SetVector("_OffsetsA", new Vector4(1.5f * oneOverW, 0.0f, -1.5f * oneOverW, 0.0f));
            bloomMaterial.SetVector("_OffsetsB", new Vector4(0.5f * oneOverW, 0.0f, -0.5f * oneOverW, 0.0f));
            Graphics.Blit(tempRtB, tempRtA, bloomMaterial, 2);

            bloomMaterial.SetVector("_OffsetsA", new Vector4(0.0f, 1.5f * oneOverH, 0.0f, -1.5f * oneOverH));
            bloomMaterial.SetVector("_OffsetsB", new Vector4(0.0f, 0.5f * oneOverH, 0.0f, -0.5f * oneOverH));
            Graphics.Blit(tempRtA, tempRtB, bloomMaterial, 2);
        }

        // bloomMaterial

        bloomMaterial.SetTexture("_Bloom", tempRtB);
        Graphics.Blit(source, destination, bloomMaterial, 0);

        RenderTexture.ReleaseTemporary(tempRtA);
        RenderTexture.ReleaseTemporary(tempRtB);
    }
}
*/

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class MobileBloom : MonoBehaviour {

    [SerializeField]
    private float m_Threshold = 0.5f;

    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float m_FinalResolutionScaler = 0.8f;

    [SerializeField]
    [Range(1,8)]
    private int m_Precision = 4;

    [SerializeField]
    private Shader BloomShader = null;
    private Material m_BloomMaterial = null;

    private int m_WidthOfRenderTarget = 0;
    private int m_HeightOfRenderTarget = 0;

    private float[] m_HorizontalTexelSizes;
    private float[] m_VerticelTexelSizes;


    private RenderTexture m_TemporaryOriginalScene = null;
    private Camera m_CurrentCamera = null;

	// Use this for initialization
	void Start () {
        m_CurrentCamera = GetComponent<Camera>();

        if (BloomShader)
        {
            m_BloomMaterial = new Material(BloomShader);
            m_BloomMaterial.SetVector("_ReturnValue", new Vector4(0, 0, 0, 0));
        }


        if (m_WidthOfRenderTarget < 1)
        {
            m_WidthOfRenderTarget = m_CurrentCamera.pixelWidth / m_Precision;
        }
        m_HorizontalTexelSizes = new float[] { 1.0f / m_WidthOfRenderTarget, 0.0f };
        if (m_HeightOfRenderTarget < 1)
        {
            m_HeightOfRenderTarget = m_CurrentCamera.pixelHeight / m_Precision;
        }
        m_VerticelTexelSizes = new float[] { 0.0f, 1.0f / m_HeightOfRenderTarget };
    }

    void OnPreRender()
    {
        m_TemporaryOriginalScene = RenderTexture.GetTemporary((int) (m_CurrentCamera.pixelWidth * m_FinalResolutionScaler), (int) (m_CurrentCamera.pixelHeight * m_FinalResolutionScaler));
        m_CurrentCamera.targetTexture = m_TemporaryOriginalScene;
    }


    void OnPostRender()
    {
        m_CurrentCamera.targetTexture = null;
        RenderTexture temp_LowQuality = RenderTexture.GetTemporary(m_WidthOfRenderTarget, m_HeightOfRenderTarget);
        RenderTexture temp_Bloom = RenderTexture.GetTemporary(m_WidthOfRenderTarget, m_HeightOfRenderTarget);

        m_BloomMaterial.SetFloat("_Threshold", m_Threshold);
        m_BloomMaterial.SetFloatArray("_SingleStepOffset", m_HorizontalTexelSizes);
        // downscale
        Graphics.Blit(m_TemporaryOriginalScene, temp_LowQuality, m_BloomMaterial, 2);
        
        // First Horizontal Blur
        Graphics.Blit(temp_LowQuality, temp_Bloom, m_BloomMaterial, 0);
        temp_LowQuality.DiscardContents();
        // Vertical Blur and addition to original Scene
        m_BloomMaterial.SetFloatArray("_SingleStepOffset", m_VerticelTexelSizes);
        m_BloomMaterial.SetTexture("_OriginalScene", temp_LowQuality);
        Graphics.Blit(temp_Bloom, temp_LowQuality, m_BloomMaterial, 1);
        temp_Bloom.DiscardContents();
        // Second Horizontal Blur
        m_BloomMaterial.SetFloat("_Threshold", m_Threshold);
        m_BloomMaterial.SetFloatArray("_SingleStepOffset", m_HorizontalTexelSizes);
        Graphics.Blit(temp_LowQuality, temp_Bloom, m_BloomMaterial, 0);

        // Second Vertical Blur and addition to original Scene
        m_BloomMaterial.SetFloatArray("_SingleStepOffset", m_VerticelTexelSizes);
        m_BloomMaterial.SetTexture("_OriginalScene", m_TemporaryOriginalScene);
        Graphics.Blit(temp_Bloom, null as RenderTexture, m_BloomMaterial, 1);

        RenderTexture.ReleaseTemporary(temp_LowQuality);
        RenderTexture.ReleaseTemporary(temp_Bloom);
        RenderTexture.ReleaseTemporary(m_TemporaryOriginalScene);
        m_TemporaryOriginalScene = null;
    }
}