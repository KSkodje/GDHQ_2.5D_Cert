using UnityEditor;

[CustomEditor(typeof(ControlPanel))]
public class ControlPanelEditor : Editor
{
    private SerializedProperty _onPlatform;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        //https://answers.unity.com/questions/1630615/how-acess-private-variable-inside-custom-editor-an.html
        var controlPanel = target as ControlPanel;
        _onPlatform = serializedObject.FindProperty("_onTheElevator");
    }
}
