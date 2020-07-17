using UnityEngine;
using UnityEngine.Rendering;

[ExecuteAlways]
public class BendingManager : MonoBehaviour
{

    #region Public Variables

    public bool isShadeBasedOnMainLightDirection;
    [Range(1,180)]
    public float sizeOfFrustrum = 100;

    #endregion

    #region Constant

    private const string WORLD_BENDING = "FS_WB_ENABLE_WORLD_BENDING";
    private const string SHADE_ON_MAIN_LIGHT = "FS_WB_SHADE_ON_MAIN_LIGH";

    #endregion

    #region Mono Behaviour

    private void Awake(){

        if(isShadeBasedOnMainLightDirection){
            EnabledShadeBasedOnMainLightDirection();
        }else{
            DisabledShadeBasedOnMainLightDirection();
        }

        if(Application.isPlaying){
            EnableWorldBending();
        }else{
            DisableWorldBending();
        }
    }

    private void OnEnable(){

        RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
        RenderPipelineManager.endCameraRendering += OnEndCameraRendering;
    }

    private void OnDisable(){

        RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;
        RenderPipelineManager.endCameraRendering -= OnEndCameraRendering;
    }

    #endregion

    #region Configuretion

    private void OnBeginCameraRendering(
        ScriptableRenderContext t_ScriptableRenderContext,
        Camera t_Camera
    ){

        float t_NearClipingPlane    = t_Camera.nearClipPlane;
        float t_FarClipingPlane     = t_Camera.farClipPlane;
        t_Camera.cullingMatrix = Matrix4x4.Ortho(
            -sizeOfFrustrum,
            sizeOfFrustrum,
            -sizeOfFrustrum,
            sizeOfFrustrum,
            t_NearClipingPlane,
            t_FarClipingPlane) * t_Camera.worldToCameraMatrix;
    }

    private void OnEndCameraRendering(
        ScriptableRenderContext t_ScriptableRenderContext,
        Camera t_Camera
    ){

        t_Camera.ResetCullingMatrix();
    }

    #endregion

    #region Public Callback

    public void EnableWorldBending(){

        Shader.EnableKeyword(WORLD_BENDING);
    }

    public void DisableWorldBending(){

        Shader.DisableKeyword(WORLD_BENDING);
    }

    public void EnabledShadeBasedOnMainLightDirection(){

        Shader.EnableKeyword(SHADE_ON_MAIN_LIGHT);
    }

    public void DisabledShadeBasedOnMainLightDirection(){
        Shader.DisableKeyword(SHADE_ON_MAIN_LIGHT);
    }

    #endregion
}
