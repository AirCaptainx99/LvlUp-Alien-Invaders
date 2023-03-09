using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class SetupButton : MonoBehaviour
{
    public enum ButtonFunction { Menu, Restart, Game };
    public ButtonFunction goToScene;

    [HideInInspector]
    public int buildIdx;
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => {
            switch (goToScene)
            {
                case ButtonFunction.Menu:
                    SceneChanger.Instance.MenuScene();
                    break;
                case ButtonFunction.Restart:
                    SceneChanger.Instance.RestartScene();
                    break;
                case ButtonFunction.Game:
                    SceneChanger.Instance.ChangeScene(buildIdx);
                    break;
                default:
                    break;
            }
        });
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(SetupButton))]
public class SetupButtonInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        // cast target
        SetupButton setupButtonScript = target as SetupButton;

        // make so that buildIdx only shows in Inspector when it's ButtonFunction.Game
        switch (setupButtonScript.goToScene)
        {
            case SetupButton.ButtonFunction.Game:
                SerializedProperty buildIdxProperty = serializedObject.FindProperty("buildIdx");
                EditorGUILayout.PropertyField(buildIdxProperty);

                setupButtonScript.buildIdx = buildIdxProperty.intValue;
                break;
            default:
                break;
        }
    }
}
#endif