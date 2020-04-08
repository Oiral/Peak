using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class renderTexture : MonoBehaviour
{
    private Texture2D eyedropperTexture;
    private Rect eyeRect;
    private bool firstGetPixel = true;
    private renderTexture eyedropperRenderTexture;

    public IEnumerator GetPixel(bool saveImage)
    {
        yield return new WaitForEndOfFrame();

        if (firstGetPixel)
        {
            firstGetPixel = false;

            eyedropperTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, false);
            eyeRect = new Rect(0, 0, Screen.width, Screen.height);

            eyedropperRenderTexture = new renderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
            eyedropperRenderTexture.Create();

            eyedropperCamera.targetTexture = eyedropperRenderTexture;
            eyedropperCamera.enabled = false;
        }

        renderTexture currentRT = renderTexture.active;
        try
        {
            renderTexture.active = eyedropperCamera.targetTexture;
            eyedropperCamera.backgroundColor = Camera.main.backgroundColor;
            eyedropperCamera.transform.position = Camera.main.transform.position;
            eyedropperCamera.transform.rotation = Camera.main.transform.rotation;
            eyedropperCamera.Render();
            eyedropperTexture.ReadPixels(new Rect(0, 0, eyedropperCamera.targetTexture.width, eyedropperCamera.targetTexture.height), 0, 0);
        }
        finally
        {
            renderTexture.active = currentRT;
        }
    }
}
