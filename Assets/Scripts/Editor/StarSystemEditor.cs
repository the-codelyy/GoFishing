using System;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;

//[CustomEditor(typeof(StarSystemData))]
public class StarSystemEditor : Editor
{
    private SerializedProperty _systemName;
    private SerializedProperty _bodyList;
    
    private GUIStyle _headerStyle;
    private GUIStyle _sectionStyle;
    private GUIStyle _centeredStyle;

    private void OnEnable()
    {
        _systemName = serializedObject.FindProperty("Name");
        _bodyList = serializedObject.FindProperty("Bodies");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        SetupStyles();
        
        // -- Header -- 
        EditorGUILayout.LabelField("Star System", _headerStyle);
        DrawSeparator(Color.gray);
        
        // -- Name --
        EditorGUILayout.LabelField("System Name", _sectionStyle);
        EditorGUILayout.Space();
        _systemName.stringValue = EditorGUILayout.TextField(_systemName.stringValue);
        DrawSeparator(Color.gray);
        
        // -- Stars --
        EditorGUILayout.LabelField("Bodies", _sectionStyle);
        DrawSeparator(Color.gray);

        for (int i = 0; i < _bodyList.arraySize; i++)
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);
            SerializedProperty bodies = _bodyList.GetArrayElementAtIndex(i);
            SerializedProperty body = bodies.FindPropertyRelative("Body");
            
            DrawArrayLabel("Star: " + (i + 1), body);
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(body, GUIContent.none);
            if (body.objectReferenceValue != null)
            {
                CelestialBodyData bodyData = body.objectReferenceValue as CelestialBodyData;
                if (bodyData != null && bodyData.Type != CelestialBodyType.Star)
                {
                    EditorUtility.DisplayDialog("Invalid Type", "This Stellar Body isn't a Star", "OK");
                    body.objectReferenceValue = null;
                }
            }

            if (GUILayout.Button("Remove Star"))
            {
                _bodyList.arraySize--;
                serializedObject.ApplyModifiedProperties();
            }
            
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            // -- Size Property --
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Size: ");
            SerializedProperty size = bodies.FindPropertyRelative("Size");
            size.floatValue = EditorGUILayout.FloatField(size.floatValue);
            EditorGUILayout.EndHorizontal();
            
            SerializedProperty satellites = bodies.FindPropertyRelative("Satellites");
            if (GUILayout.Button("Add Satellite"))
            {
                AddToArray(satellites);
            }

            if (satellites.arraySize > 0)
            {
                EditorGUILayout.LabelField("Satellites", _sectionStyle);
                DrawSeparator(Color.gray);
                EditorGUI.indentLevel++;

                for (int j = 0; j < satellites.arraySize; j++)
                {
                    SerializedProperty satellite = satellites.GetArrayElementAtIndex(j).FindPropertyRelative("Body");
                    SerializedProperty satelliteBodies = satellites.GetArrayElementAtIndex(j).FindPropertyRelative("Satellites");
                    
                    DrawArrayLabel("Satellite: " + (j + 1), satellite);
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(satellite, GUIContent.none);

                    if (GUILayout.Button("Add Satellite"))
                    {
                        AddToArray(satelliteBodies);
                    }

                    if (GUILayout.Button("Remove"))
                    {
                        RemoveFromArray(satellites);
                    }
                    
                    EditorGUILayout.EndHorizontal();

                    DrawBodyProperties(satellites.GetArrayElementAtIndex(j), j);
                    DrawSatellites(satelliteBodies);
                    DrawSeparator(Color.gray);
                }
                
                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
            }

            EditorGUILayout.EndVertical();
            DrawSeparator(Color.gray);
        }
        
        if (GUILayout.Button("Add Body"))
        {
            _bodyList.arraySize++;
            serializedObject.ApplyModifiedProperties();
        }
        
        serializedObject.ApplyModifiedProperties();
    }

    private void DrawBodyProperties(SerializedProperty body, int index)
    {
        SerializedProperty distance = body.FindPropertyRelative("DistanceFromPrimary");
        SerializedProperty size = body.FindPropertyRelative("Size");
        
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Distance From Primary");
        if (distance.floatValue == 0)
        {
            distance.floatValue = index + 1;
        }
        distance.floatValue = EditorGUILayout.FloatField(distance.floatValue);
        EditorGUILayout.EndHorizontal();
            
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Size");
        size.floatValue = EditorGUILayout.FloatField(size.floatValue);
        EditorGUILayout.EndHorizontal();
    }
    
    private void DrawSatellites(SerializedProperty property)
    {
        if (property.isArray && property.arraySize > 0)
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.Space();
            EditorGUI.indentLevel++;

            for (int i = 0; i < property.arraySize; i++)
            {
                SerializedProperty orbitingBody = property.GetArrayElementAtIndex(i).FindPropertyRelative("Body");
                    
                DrawArrayLabel("Satellite: " + (i + 1), orbitingBody);
                    
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(orbitingBody, GUIContent.none);
                        
                if (GUILayout.Button("Remove Satellite"))
                    RemoveFromArray(property);
                        
                EditorGUILayout.EndHorizontal();
            }
                
            EditorGUILayout.EndVertical();
        }
    }
    
    private void AddToArray(SerializedProperty property)
    {
        property.arraySize++;
        SerializedProperty newProperty = property.GetArrayElementAtIndex(property.arraySize - 1).FindPropertyRelative("Satellites");
        newProperty.objectReferenceValue = null;
            
        serializedObject.ApplyModifiedProperties();
    }
    
    private void RemoveFromArray(SerializedProperty property)
    {
        property.arraySize--;
        serializedObject.ApplyModifiedProperties();
    }
    
    private void DrawArrayLabel(string initialString, SerializedProperty property)
    {
        string labelName = initialString;
        if (property.objectReferenceValue != null)
        {
            labelName = property.objectReferenceValue.name.Replace("_", " ");
        }
            
        EditorGUILayout.LabelField(labelName, _sectionStyle);
    }
    
    private void DrawSeparator(Color color)
    {
        EditorGUILayout.Space();
            
        GUIStyle separatorStyle = new GUIStyle(GUI.skin.box);
        separatorStyle.normal.background = EditorGUIUtility.whiteTexture;
        separatorStyle.margin = new RectOffset(0, 0, 4, 4);
        EditorGUI.DrawRect(GUILayoutUtility.GetRect(1, 1), color);
            
        EditorGUILayout.Space();
    }
    
    private void SetupStyles()
    {
        _headerStyle = new GUIStyle(EditorStyles.boldLabel);
        _headerStyle.alignment = TextAnchor.MiddleCenter;
        _headerStyle.fontSize = 20;
            
        _sectionStyle = new GUIStyle(EditorStyles.boldLabel);
        _sectionStyle.alignment = TextAnchor.MiddleCenter;
        _sectionStyle.fontSize = 14;
            
        _centeredStyle = new GUIStyle(GUI.skin.box);
        _centeredStyle.alignment = TextAnchor.MiddleCenter;
    }
}
