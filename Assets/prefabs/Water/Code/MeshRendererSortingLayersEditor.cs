using UnityEngine;
using System.Collections;

// 이 줄을 추가: 유니티 에디터 상태일 때만 UnityEditor를 사용
#if UNITY_EDITOR
using UnityEditor;
#endif

// 이 줄을 추가: 에디터일 때만 아래 클래스를 컴파일
#if UNITY_EDITOR
[CustomEditor(typeof(MeshRenderer))]
public class MeshRendererSortingLayersEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MeshRenderer renderer = target as MeshRenderer;

        EditorGUILayout.BeginHorizontal();

        EditorGUI.BeginChangeCheck();

        string name = EditorGUILayout.TextField("Sorting Layer Name", renderer.sortingLayerName);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(renderer, "Change Sorting Layer Name"); // 변경 사항 저장(Undo) 지원 추가 추천
            renderer.sortingLayerName = name;
            EditorUtility.SetDirty(renderer); // 변경 사항 즉시 반영
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();

        EditorGUI.BeginChangeCheck();

        int order = EditorGUILayout.IntField("Sorting Order", renderer.sortingOrder);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(renderer, "Change Sorting Order");
            renderer.sortingOrder = order;
            EditorUtility.SetDirty(renderer);
        }

        EditorGUILayout.EndHorizontal();
    }
}
#endif // 여기서 닫아줌