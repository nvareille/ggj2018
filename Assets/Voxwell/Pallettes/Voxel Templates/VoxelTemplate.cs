using UnityEngine;
using System.Collections;
using System.Linq;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class VoxelTemplate : ScriptableObject {

	//Used to determine where in the display lists this template should be
	//Less = higher on the list.
	public int DisplayOrder = 0;

	//Used for vertex colors
    public bool RandomBetweenTwoColors = false;
    public Color32 color = Color.white;
    public Color32 color2 = Color.white;
	 
	//determine whether or not to use the front face uvs for all faces
	public bool useFrontUvsForAllFaces = false;

	//Determine whether or not to draw the faces in the center of the voxel.
	//As opposed to drawing 'on the surface' which happens by default
	public bool drawFacesInCenter = false;

	//the size of each tile in the atlas. 
	//If our texture atlas is 16/16 tiles, then this will be 1/16, or .0625
	//If our texture atlas is 8x8 tiles, then this will be 1/8, or .125
	public Vector2 atlasScale = new Vector2(0.0625f, 0.0625f);


	public Texture2D DisplayTexture;



	public Sprite SpriteFront;
	public Sprite SpriteBack;
	public Sprite SpriteTop;
	public Sprite SpriteBottom;
	public Sprite SpriteLeft;
	public Sprite SpriteRight;


	//The uv offsets (used for uv-mapping) for each face
	//offset.x = percentage distance from left edge of the atlas
	//offset.y = percentage distance from bottom edge of the atlas
	public Vector2 UVOffsetFront;
	public Vector2 UVOffsetBack;	
	public Vector2 UVOffsetTop;
	public Vector2 UVOffsetBottom;
	public Vector2 UVOffsetLeft;
	public Vector2 UVOffsetRight;
	

	//determine whether or not to draw this voxel
	public bool shouldDraw = true;

	//determine whether or not to draw each individual face
	public bool drawFront = true;
	public bool drawBack = true;
	public bool drawTop = true;
	public bool drawBottom = true;
	public bool drawLeft = true;
	public bool drawRight = true;



	public VoxelPallette voxelPallette;


	public bool shouldShowInInspector = false;

	//public GameObject Special; //Coming Soon!

	public void GenerateAssetPreview(){
        #if UNITY_EDITOR
        UpdateUVOffsets();
		if(voxelPallette==null){
			Debug.LogError("Voxel Template has no associated Voxel Pallette, unable to generate the asset preview");
			return;
		}


		var tempPath = "Assets/TEMP.prefab";
		var tempMeshPath = "Assets/TEMP_MESH.asset";
		
		
		GameObject tempObj = new GameObject();




		var vs = tempObj.AddComponent<VoxelStructure>();
		vs.Width = 1;
		vs.Depth = 1;
		vs.Height = 1;
		vs.pallette = voxelPallette;
		
		voxelPallette.ArrangeVoxelTemplates();
		vs.SetVoxel(0,0,0,voxelPallette.voxelTemplateList.IndexOf(this));
		vs.Draw();



		AssetDatabase.CreateAsset(vs.GetComponent<MeshFilter>().sharedMesh, tempMeshPath); 
		
		tempObj.name = this.name + " Asset Preview";

		var prefab = PrefabUtility.CreatePrefab(tempPath, tempObj);
		var newGO = PrefabUtility.ReplacePrefab(tempObj, prefab);






		var someTexture = AssetPreview.GetAssetPreview(prefab);
		var counter = 0;
		while(AssetPreview.IsLoadingAssetPreview(prefab.GetInstanceID()))
		{
			someTexture = AssetPreview.GetAssetPreview(newGO);
			counter++;
			System.Threading.Thread.Sleep(15);
		}
		someTexture = AssetPreview.GetAssetPreview(newGO);
		

		if(someTexture){
			var newTex = new Texture2D(someTexture.width, someTexture.height);
			newTex.SetPixels(someTexture.GetPixels());
			newTex.Apply();

			this.DisplayTexture = newTex;
			newTex.hideFlags = HideFlags.HideInHierarchy;
			AssetDatabase.AddObjectToAsset(newTex, this);




			AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(newTex));
			AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(this));
			AssetDatabase.Refresh();
		}else{
			Debug.Log("No Asset Preview Found");
		}

		Object.DestroyImmediate(tempObj, true);
		AssetDatabase.DeleteAsset(tempMeshPath);
		AssetDatabase.DeleteAsset(tempPath);


        #endif

    }


	public void UpdateUVOffsets(){

		if(voxelPallette != null && voxelPallette.AtlasMaterial!=null && voxelPallette.AtlasMaterial.mainTexture!=null){
			if(SpriteFront!=null){
				UVOffsetFront = new Vector2(SpriteFront.rect.x/voxelPallette.AtlasMaterial.mainTexture.width, 
				                         SpriteFront.rect.y/voxelPallette.AtlasMaterial.mainTexture.height);
			}

			if(SpriteBack!=null){
				UVOffsetBack = new Vector2(SpriteBack.rect.x/voxelPallette.AtlasMaterial.mainTexture.width, 
				                     	SpriteBack.rect.y/voxelPallette.AtlasMaterial.mainTexture.height);		
			}

			if(SpriteTop!=null){
				UVOffsetTop = new Vector2(SpriteTop.rect.x/voxelPallette.AtlasMaterial.mainTexture.width, 
				                     	SpriteTop.rect.y/voxelPallette.AtlasMaterial.mainTexture.height);		
			}

			if(SpriteBottom!=null){
				UVOffsetBottom = new Vector2(SpriteBottom.rect.x/voxelPallette.AtlasMaterial.mainTexture.width, 
				                     	SpriteBottom.rect.y/voxelPallette.AtlasMaterial.mainTexture.height);
			}

			if(SpriteLeft!=null){
				UVOffsetLeft = new Vector2(SpriteLeft.rect.x/voxelPallette.AtlasMaterial.mainTexture.width, 
				                         SpriteLeft.rect.y/voxelPallette.AtlasMaterial.mainTexture.height);
			}


			if(SpriteRight!=null){
				UVOffsetRight = new Vector2(SpriteRight.rect.x/voxelPallette.AtlasMaterial.mainTexture.width, 
				                     SpriteRight.rect.y/voxelPallette.AtlasMaterial.mainTexture.height);
			}
		}

	}
}
