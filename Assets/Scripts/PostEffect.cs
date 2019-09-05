using System.Collections;
using UnityEngine;

public class PostEffect : MonoBehaviour
{
    Camera AttachedCamera;
    public Shader Post_Outline;
    public Shader DrawSimple;
    Camera TempCam;
    Material Post_Mat;

    void Start()
    {
        AttachedCamera = GetComponent<Camera>();
        TempCam = new GameObject().AddComponent<Camera>();
        TempCam.enabled = false;
        Post_Mat = new Material(Post_Outline);
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        // Set up a temporary camera
        TempCam.CopyFrom(AttachedCamera);
        TempCam.clearFlags = CameraClearFlags.Color;
        TempCam.backgroundColor = Color.black;

        // Cull any layer that isn't the outline
        TempCam.cullingMask = 1 << LayerMask.NameToLayer("Outline");

        // Make the temporary rendertexture
        RenderTexture tempRT = new RenderTexture(source.width, source.height, 0, RenderTextureFormat.R8);

        // Put it to video memory
        tempRT.Create();

        // Set the camera's target texture when rendering
        TempCam.targetTexture = tempRT;

        // Render all objects this camera can render, but with our custom shader.
        TempCam.RenderWithShader(DrawSimple, "");

        // Copy the temporary RT to the final image
        Graphics.Blit(tempRT, destination, Post_Mat);

        // Release the temporary RT
        tempRT.Release();
    }
}
