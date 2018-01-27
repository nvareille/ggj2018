using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Linq;

[ExecuteInEditMode]
[CustomEditor(typeof(VoxelTemplate))]
public class VoxelTemplateEditor : Editor {

	public override void OnInspectorGUI() {

		DrawDefaultInspector();
		var voxelTemplate = (VoxelTemplate)target;
		if(GUILayout.Button("Generate Asset Preview")){
			voxelTemplate.GenerateAssetPreview();
		}

	}


}
