using UnityEngine;
using System.Collections;
using UnityEditor;


//General class for handling misc menus, maybe more later
public class Voxwell : Editor {

	[MenuItem("Assets/Create/Voxel Pallette")]
	public static void CreateVoxelPalletteAsset ()
	{
		ScriptableObjectUtility.CreateAsset<VoxelPallette> ();
	}



}
