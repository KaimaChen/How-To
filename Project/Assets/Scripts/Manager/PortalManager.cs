using UnityEngine;

public class PortalManager : MonoBehaviour
{
    private const string kShaderName = "Unlit/ScreenCutoutShader";

    public Renderer mPortalRender1;
    public Camera mPortalCamera1;
    public Renderer mPortalRender2;
    public Camera mPortalCamera2;

    void Start()
    {
        Shader screenShader = Shader.Find(kShaderName);

        //RenderTexture的大小要和屏幕分辨率一致，否则会有模糊等问题
        RenderTexture rt1 = new RenderTexture(Screen.width, Screen.height, 24) { name = "RT1", };
        mPortalCamera1.targetTexture = rt1;

        RenderTexture rt2 = new RenderTexture(Screen.width, Screen.height, 24) { name = "RT2" };
        mPortalCamera2.targetTexture = rt2;
        
        Material mat1 = new Material(screenShader)
        {
            mainTexture = mPortalCamera1.targetTexture
        };
        Material mat2 = new Material(screenShader)
        {
            mainTexture = mPortalCamera2.targetTexture,
        };

        mPortalRender1.sharedMaterial = mat2;
        mPortalRender2.sharedMaterial = mat1;
    }
}
