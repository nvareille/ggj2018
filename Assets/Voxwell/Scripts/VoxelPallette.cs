using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Linq;

[System.Serializable]
public class VoxelPallette : ScriptableObject {
	
	[SerializeField]
	public Material AtlasMaterial;

    [SerializeField]
    public Material Unity4AtlasMaterial;

    [SerializeField]
    public Material Unity5AtlasMaterial;

	[SerializeField]
	public VoxelTemplate[] voxelTemplates;
	[SerializeField]
	public List<VoxelTemplate> voxelTemplateList;

    [SerializeField]
    public List<KeyValuePair<int, int>> lookup = new List<KeyValuePair<int, int>>();
	public List<string> voxelNames = new List<string>();
	public List<Texture> voxelImages = new List<Texture>();
    public List<GUIContent> voxelGuiContent = new List<GUIContent>();
    public List<VoxelTemplate> voxelTemplatesSorted = new List<VoxelTemplate>();
	
	
	//For display/UX purposes, we want to have a sorted, filtered list.
	//To do this, we sort voxel templates by their display order, while 
	//constructing a "lookup table" to refer to later when we need to 
	//get the "real" voxel template index in our voxelTemplates[] array
	public void ArrangeVoxelTemplates(){
		//wipe the list
		voxelNames = new List<string>();
		voxelImages = new List<Texture>();
		voxelGuiContent = new List<GUIContent>();
        voxelTemplatesSorted = new List<VoxelTemplate>();
		//sort the templates by their display order
		var sorted = voxelTemplateList
			.Select((x, i) => new KeyValuePair<VoxelTemplate, int>(x, i))
			.OrderBy( x => { 
					if(x.Key != null) 
						return x.Key.DisplayOrder; 
					else
						return 100000;
					
				} )
			.ToList();
		
		//wipe the lookup
		lookup = new List<KeyValuePair<int,int>>();

		//construct the lookup table and the display list (voxelNames)
		for(var i = 0; i<sorted.Count;i++){

            
			if(sorted[i].Key!=null){
                sorted[i].Key.DisplayOrder = i;



                lookup.Add(new KeyValuePair<int, int>(i, sorted[i].Value));

                voxelTemplatesSorted.Add(sorted[i].Key);

                voxelNames.Add(sorted[i].Key.name);
				var newGuiDisplay = new GUIContent(sorted[i].Key.name, sorted[i].Key.DisplayTexture);
				voxelGuiContent.Add(newGuiDisplay);



			}
		}

		
		
	}


	void ConvertVoxelArrayToList(){
		voxelTemplateList = new List<VoxelTemplate>();
		for(var i = 0;i<this.voxelTemplates.Length;i++){
			voxelTemplateList.Add(this.voxelTemplates[i]);
		}
		
	} 

	void OnEnable(){
        if (voxelTemplateList == null)
        {
            voxelTemplateList = new List<VoxelTemplate>();
        }
        CheckForUpdate();

        //always ensure templates belong to me
        for (var i = 0; i < this.voxelTemplateList.Count(); i++)
        {
            var t = this.voxelTemplateList[i];
            if (t && t.voxelPallette == null)
            {
                t.voxelPallette = this;
            }
        }

        //if we have any voxel templates, but the sorted list is empty, sort them now
        if (voxelTemplateList.Count > 0 && voxelTemplatesSorted.Count == 0)
        {
            ArrangeVoxelTemplates();
        }
		
	}
    void CheckForUpdate() 
    {
        if (voxelTemplates != null && voxelTemplates.Length > 0 && (voxelTemplateList == null || voxelTemplateList.Count == 0))
        {
            Debug.Log("Updating Voxel Pallette: " + name);
            ConvertVoxelArrayToList();
            #if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            #endif
        }
    }


    public int ConvertSortedIndexToActualIndex(int index)
    {
        if (lookup.Count==0)
        {
            ArrangeVoxelTemplates();
        }
        if (index < 0 || index > lookup.Count())
        {
            index = 0;
        }
        if (lookup.Count > 0)
            return lookup[index].Value;
        else
            return 0;
    }
    public VoxelTemplate ConvertSortedIndexToTemplate(int index)
    {
        var actualIndex = ConvertSortedIndexToActualIndex(index);
        if (actualIndex >= 0 && actualIndex < voxelTemplateList.Count())
            return voxelTemplateList[actualIndex];
        else
            return null;

    }



}
