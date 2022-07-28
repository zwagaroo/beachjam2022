#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GradientSkybox
{
    #if UNITY_EDITOR
    public class CircularTwoColorGradientSkyboxGUI : ShaderGUI
    {
        public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
        {
            base.OnGUI(materialEditor, properties);
        }
    }
    #endif
}
