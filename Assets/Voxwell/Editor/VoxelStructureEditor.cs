using UnityEngine;
using System.Collections;
using UnityEditor;
using System;

[ExecuteInEditMode]
[CanEditMultipleObjects]
[CustomEditor(typeof(VoxelStructure))]
public class VoxelStructureEditor : Editor {
	
	//used for determining a 'mouse up' event
	bool wasLeftMouseDown = false;
	bool wasRightMouseDown = false;
	
	int sprayPaintThrottle = 100;
	DateTime lastSprayPaintAction;
	
	Vector2 currentMousePosition;

	//Modify and Add Buttons
	private GUIContent solidCubeButtonContent = new GUIContent();
	private GUIContent clearButtonContent = new GUIContent();
	private GUIContent floorButtonContent = new GUIContent();
	private GUIContent ceilingButtonContent = new GUIContent();
	private GUIContent leftWallButtonContent = new GUIContent();
	private GUIContent rightWallButtonContent = new GUIContent();
	private GUIContent frontWallButtonContent = new GUIContent();
	private GUIContent backWallButtonContent = new GUIContent();

	//Selected Voxel Type Display
	GUIContent currentSelectedVoxelTypeContent = new GUIContent();
	
	//GUIStyle for Voxel Buttons and Labels
	private int selectorButtonSize = 80;
	private static GUIStyle voxelSelectorLabelGUIStyle;
	private static GUIStyle voxelSelectorButtonGUIStyle;

	//GUI was setup
	bool isGUIStyleInit = false;

	//GUI mode
	bool updateHide = true;
	bool hideOtherComponents = false;

	//Scrollhandle Positions
	public static Vector2 voxelSelectionScrollPosition = Vector2.zero;
	public static Vector2 voxel2SelectionScrollPosition = Vector2.zero;

	//VoxelStructure
	private static VoxelStructure voxelStructure;

	void OnEnable(){
		solidCubeButtonContent.image = (Texture)EditorGUIUtility.Load("Voxwell/UI/solidCube_png.png");
		clearButtonContent.image = (Texture)EditorGUIUtility.Load("Voxwell/UI/clear_png.png");
		floorButtonContent.image = (Texture)EditorGUIUtility.Load("Voxwell/UI/floor_png.png");
		ceilingButtonContent.image = (Texture)EditorGUIUtility.Load("Voxwell/UI/ceiling_png.png");
		leftWallButtonContent.image = (Texture)EditorGUIUtility.Load("Voxwell/UI/left_png.png");
		rightWallButtonContent.image = (Texture)EditorGUIUtility.Load("Voxwell/UI/right_png.png");
		frontWallButtonContent.image = (Texture)EditorGUIUtility.Load("Voxwell/UI/front_png.png");
		backWallButtonContent.image = (Texture)EditorGUIUtility.Load("Voxwell/UI/back_png.png");

		//cache Voxel Structure
		if ( !CacheVoxelStructure() ){
			Debug.LogError("Voxel Structure cache failed for: " + voxelStructure.name);
			return;
		}

		if ( !voxelStructure.CacheComponents() ){
			Debug.LogError("Voxel Structure Component cache failed for: " + voxelStructure.name);
			return;
		}

	}

	void OnDisable (){
		//Should probly unload EDitorGUIUtility Loaded Assets by calling Object.Destroy() and EditorUtility.UnloadUnusedAssets
	}

	void Update () {
		var view = SceneView.currentDrawingSceneView; 
		if(view!=null && Camera.current!=null){
			if(wasLeftMouseDown){
				var currentTime = DateTime.Now;
				var dTime = currentTime.Subtract(lastSprayPaintAction);
				
				if(dTime.Milliseconds>sprayPaintThrottle){
					lastSprayPaintAction=currentTime;
					HandleClick(currentMousePosition, VoxelStructure.LMB_Action, 0);
				}
			}
			if(wasRightMouseDown){
				var currentTime = DateTime.Now;
				var dTime = currentTime.Subtract(lastSprayPaintAction);
				
				if(dTime.Milliseconds>sprayPaintThrottle){
					lastSprayPaintAction=currentTime;
					HandleClick(currentMousePosition, VoxelStructure.RMB_Action, 1);
				}
			}
		}



	}

	private bool CacheVoxelStructure ()
	{	
		voxelStructure = (VoxelStructure)target;
		voxelStructure.ClampDimensions ();

		if( voxelStructure == null ){
			return false;
		}
		else{
			return true;
		}
	
	}

	private  bool CacheVoxelStructurePallette ()
	{
		voxelStructure.pallette = (VoxelPallette)EditorGUILayout.ObjectField ("Pallette", voxelStructure.pallette, typeof(VoxelPallette), false);
		if( voxelStructure.pallette == null ){
			return false;
		}
		else{
			return true;
		}	
	}

	public override void OnInspectorGUI() {

		EditorGUI.BeginChangeCheck();

		if ( !isGUIStyleInit || voxelSelectorLabelGUIStyle ==null || voxelSelectorButtonGUIStyle == null){
			isGUIStyleInit = true; 

			voxelSelectorLabelGUIStyle = new GUIStyle( GUI.skin.label );
			voxelSelectorButtonGUIStyle = new GUIStyle( GUI.skin.button );

			solidCubeButtonContent.tooltip = "Solid Fill";
			clearButtonContent.tooltip = "Clear All Voxels";
			floorButtonContent.tooltip = "Add Floor";
			ceilingButtonContent.tooltip = "Add Ceiling";
			leftWallButtonContent.tooltip = "Add Left Wall";
			rightWallButtonContent.tooltip = "Add Right Wall";
			frontWallButtonContent.tooltip = "Add Front Wall";
			backWallButtonContent.tooltip = "Add Back Wall";

			currentSelectedVoxelTypeContent.tooltip = "Currently Selected Voxel";
			currentSelectedVoxelTypeContent.text = "Current Voxel Type";	
			
			voxelSelectorButtonGUIStyle.alignment = TextAnchor.UpperCenter;
			voxelSelectorButtonGUIStyle.imagePosition = ImagePosition.ImageAbove;
			voxelSelectorButtonGUIStyle.fixedWidth = selectorButtonSize;
			voxelSelectorButtonGUIStyle.fixedHeight = selectorButtonSize;
			voxelSelectorButtonGUIStyle.stretchHeight = true;
			voxelSelectorButtonGUIStyle.wordWrap = true;
			
			voxelSelectorLabelGUIStyle.alignment = TextAnchor.UpperCenter;
			voxelSelectorLabelGUIStyle.imagePosition = ImagePosition.ImageAbove;
			voxelSelectorLabelGUIStyle.fixedWidth = selectorButtonSize;
			voxelSelectorLabelGUIStyle.fixedHeight = selectorButtonSize;
			voxelSelectorLabelGUIStyle.stretchHeight = true;
			voxelSelectorLabelGUIStyle.wordWrap = true;

		}

		//updates state of hiding other components
		if( updateHide ){
			updateHide = false;
			
			if( hideOtherComponents ){
				voxelStructure.mr.hideFlags = HideFlags.HideInInspector;
				voxelStructure.mc.hideFlags = HideFlags.HideInInspector;
				voxelStructure.mf.hideFlags = HideFlags.HideInInspector;
			}
			else{
				voxelStructure.mr.hideFlags = HideFlags.None;
				voxelStructure.mc.hideFlags = HideFlags.None;
				voxelStructure.mf.hideFlags = HideFlags.None;
			}
			
		}




		//Make sure we actually have a pallette, 
		//otherwise do nothing and display a message
		if ( !CacheVoxelStructurePallette() ){
			EditorGUILayout.LabelField("You must select a Voxel Pallette before you can do anything!");
			Debug.LogWarning("No Pallette selected for: " + voxelStructure.name);
			return;
		}
	
		
		EditorGUILayout.Separator();



        ///TODO: ONLY DO THIS WHEN NECESSARY
        if (voxelStructure.pallette.voxelTemplateList.Count > 0 && voxelStructure.pallette.lookup.Count == 0)
        {
            voxelStructure.pallette.ArrangeVoxelTemplates();

        }


		if(voxelStructure.pallette.lookup.Count<=0){
			EditorGUILayout.LabelField("The Selected Pallette has no Voxel Templates. You must define some templates before you can do anything!");
			Debug.LogWarning("The Selected Pallette has no Voxel Templates.");
			return;
		}








		var currentVoxelType = EditorPrefs.GetInt("CurrentVoxelType");

		if (currentVoxelType > voxelStructure.pallette.voxelTemplateList.Count)
			currentVoxelType = 0;


		var voxelGuis = voxelStructure.pallette.voxelGuiContent.ToArray();



		EditorGUILayout.Separator();

		//Current Selected Voxel Type
		//could be refactored for the image to be set on change of voxel type
		currentSelectedVoxelTypeContent.image = voxelGuis[currentVoxelType].image;

		//null check due to the possibly the content may be null before the OnGUI
		if( currentSelectedVoxelTypeContent != null ){
			GUILayout.Label( currentSelectedVoxelTypeContent, voxelSelectorLabelGUIStyle );
		}

		EditorGUILayout.Separator();

		voxelSelectionScrollPosition = EditorGUILayout.BeginScrollView(voxelSelectionScrollPosition, GUILayout.ExpandWidth(true), GUILayout.Height (200), GUILayout.ExpandHeight(true) );

		var numXButtons = Mathf.FloorToInt(Screen.width/(selectorButtonSize+(voxelSelectorButtonGUIStyle.padding.right + voxelSelectorButtonGUIStyle.padding.left)));

		currentVoxelType = GUILayout.SelectionGrid(currentVoxelType, voxelGuis, numXButtons, voxelSelectorButtonGUIStyle);
        EditorPrefs.SetInt("CurrentVoxelType", currentVoxelType);
		EditorGUILayout.EndScrollView();
	






		//This is the tricky bit. 
		//We've figured out which item in the list has been selected, 
		//but that list is an altered, sorted version of the array in our VoxelPallette.
		//If we want to know what the REAL index of the voxel template in that array,
		//we have to consult the pallette.lookup, which is constructed by pallette.ArrangeVoxelTemplates()
		VoxelStructure.SelectedVoxelIndex = voxelStructure.pallette.ConvertSortedIndexToActualIndex(currentVoxelType);

	
		
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		
		EditorPrefs.SetInt("LMBAction", (int)(VoxelStructure.Action)EditorGUILayout.EnumPopup("LMB Action", (VoxelStructure.Action)EditorPrefs.GetInt("LMBAction")));
		VoxelStructure.LMB_Action = (VoxelStructure.Action)EditorPrefs.GetInt("LMBAction");
		
		EditorPrefs.SetInt("RMBAction", (int)(VoxelStructure.Action)EditorGUILayout.EnumPopup("RMB Action", (VoxelStructure.Action)EditorPrefs.GetInt("RMBAction")));
		VoxelStructure.RMB_Action = (VoxelStructure.Action)EditorPrefs.GetInt("RMBAction");

		EditorGUILayout.Separator();
		
		voxelStructure.Width = EditorGUILayout.IntField("Width:", voxelStructure.Width);
		voxelStructure.Height = EditorGUILayout.IntField("Height:", voxelStructure.Height);
		voxelStructure.Depth = EditorGUILayout.IntField("Depth:", voxelStructure.Depth);
		
		EditorGUILayout.Separator();
		
		voxelStructure.DrawFront = EditorGUILayout.Toggle("Draw Front", voxelStructure.DrawFront);
		voxelStructure.DrawBack  = EditorGUILayout.Toggle("Draw Back", voxelStructure.DrawBack);
		voxelStructure.DrawLeft = EditorGUILayout.Toggle("Draw Left", voxelStructure.DrawLeft);
		voxelStructure.DrawRight = EditorGUILayout.Toggle("Draw Right", voxelStructure.DrawRight);
		voxelStructure.DrawTop = EditorGUILayout.Toggle("Draw Top", voxelStructure.DrawTop);
		voxelStructure.DrawBottom = EditorGUILayout.Toggle("Draw Bottom", voxelStructure.DrawBottom);
		
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		voxelStructure.GenerateSecondaryUvSet = EditorGUILayout.Toggle("Generate Secondary UV Set", voxelStructure.GenerateSecondaryUvSet);
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.Separator();
		
	
	
		
		//Here we expose various Voxel Structure functions to the Unity Editor
		#region macro command buttons




        EditorGUILayout.Separator();

		EditorGUILayout.BeginHorizontal();
	    if(GUILayout.Button( solidCubeButtonContent) ){
			if( EditorUtility.DisplayDialog( "Filling All Voxels","Are you sure you want to Fill this voxel structure?","Yes, Fill it", "No, Wait") ){
			 	Undo.RecordObject(voxelStructure, "Solid Cube");
				voxelStructure.SolidCube();
				voxelStructure.Draw();
			}
		}
		if(GUILayout.Button( clearButtonContent )){
			if( EditorUtility.DisplayDialog( "Clearing All Voxels","Are you sure you want to Clear this voxel structure?","Yes, Clear it", "No, Wait") ){
				Undo.RecordObject(voxelStructure, "Clear All");
				voxelStructure.ClearAll();
				voxelStructure.Draw();
			}
		}

		EditorGUILayout.EndHorizontal();
		

		
		EditorGUILayout.Separator();



		EditorGUILayout.Separator();

		EditorGUILayout.BeginHorizontal();
		if(GUILayout.Button(  leftWallButtonContent )){
			Undo.RecordObject(voxelStructure, "Left");
			voxelStructure.LeftWall();
			voxelStructure.Draw();
		}
		if(GUILayout.Button( rightWallButtonContent )){
			Undo.RecordObject(voxelStructure, "Right");
			voxelStructure.RightWall();
			voxelStructure.Draw();
		}
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.BeginHorizontal();
		if(GUILayout.Button( floorButtonContent )){
			Undo.RecordObject(voxelStructure, "Floor");
			voxelStructure.Floor();
			voxelStructure.Draw();
		}
		if(GUILayout.Button( ceilingButtonContent )){
			Undo.RecordObject(voxelStructure, "Ceiling");
			voxelStructure.Ceiling();
			voxelStructure.Draw();
		}
		EditorGUILayout.EndHorizontal();
		
		
		EditorGUILayout.BeginHorizontal();
		if(GUILayout.Button( frontWallButtonContent  )){
			Undo.RecordObject(voxelStructure, "Front Wall");
			voxelStructure.FrontWall();
			voxelStructure.Draw();
		}
		if(GUILayout.Button(  backWallButtonContent )){
			Undo.RecordObject(voxelStructure, "Back Wall");
			voxelStructure.BackWall();
			voxelStructure.Draw();
		}
		EditorGUILayout.EndHorizontal(); 
		
		EditorGUILayout.Separator();

		if(GUILayout.Button("Set All Non-Empties")){
			Undo.RecordObject(voxelStructure, "Set All Non-Empties");
			voxelStructure.SetAllNonEmpties();
			voxelStructure.Draw();
		}

		if(GUILayout.Button("Pillars Front Eight")){
			Undo.RecordObject(voxelStructure, "Pillars Front Eight");
			voxelStructure.PillarsFrontEight();
			voxelStructure.Draw();
		}
		
		EditorGUILayout.Separator();	
	    if(GUILayout.Button("Draw")){
			voxelStructure.Draw();
		}
	   
		EditorGUILayout.Separator();	
		EditorGUILayout.Separator();	    
		if(GUILayout.Button("Export Mesh")){
			voxelStructure.ExportMesh();
		}	
		EditorGUILayout.Separator();
		if(GUILayout.Button("Hide other Components")){
			updateHide = true;
			hideOtherComponents = !hideOtherComponents;
		}

		#endregion



		if ( EditorGUI.EndChangeCheck() ) EditorUtility.SetDirty(target);


	}

	void HandleClick (Vector2 mousePosition, VoxelStructure.Action action, int button)
	{
		
		VoxelStructure voxelStructure = (VoxelStructure)target;
		//Strange for the editor, but can sometimes happen)
		if(Camera.current==null){
			Debug.Log("Strange, Camera.current was null. Please try again in a moment.");	
		}else{
			
		
			RaycastHit hit;
			Ray ray = HandleUtility.GUIPointToWorldRay(currentMousePosition);
	
			//If we hit something
			if (Physics.Raycast (ray, out hit, 100f)){
				//get the position of the voxel that was clicked
				var targetVoxelPositionPreScale = (hit.point - (hit.normal*.01f)) - voxelStructure.transform.position;
				var targetVoxelPosition = new Vector3(
					targetVoxelPositionPreScale.x/voxelStructure.transform.localScale.x,
					targetVoxelPositionPreScale.y/voxelStructure.transform.localScale.y,
					targetVoxelPositionPreScale.z/voxelStructure.transform.localScale.z
					);
				//and the neighbor on the side it was clicked on
				var normalNeighborVoxelPositionPreScale = (hit.point + (hit.normal*.01f)) - voxelStructure.transform.position;
				var normalNeighborVoxelPosition = new Vector3(
					normalNeighborVoxelPositionPreScale.x / voxelStructure.transform.localScale.x,
					normalNeighborVoxelPositionPreScale.y / voxelStructure.transform.localScale.y,
					normalNeighborVoxelPositionPreScale.z / voxelStructure.transform.localScale.z
					);

				//Use the currently selected action to determine which function to call on the Voxel Structure
				switch(action){
					case VoxelStructure.Action.MaxPull:
						Undo.RecordObject(voxelStructure, "Max Pull");
						voxelStructure.MaxExtrude(targetVoxelPosition, hit.normal);
						voxelStructure.Draw();
						EditorUtility.SetDirty(target);
						break;
					case VoxelStructure.Action.Paint:
						Undo.RecordObject(voxelStructure, "Paint");
						if(button == 0)
						voxelStructure.SetVoxel(targetVoxelPosition, VoxelStructure.SelectedVoxelIndex);
						else if(button ==1)
						voxelStructure.SetVoxel(targetVoxelPosition, VoxelStructure.SelectedVoxelIndex);
						
						voxelStructure.Draw();
						EditorUtility.SetDirty(target);
						break;
					case VoxelStructure.Action.Add:
						Undo.RecordObject(voxelStructure, "Add");
						if(button == 0){
						voxelStructure.SetVoxel(normalNeighborVoxelPosition, VoxelStructure.SelectedVoxelIndex);
                        }
						else if(button ==1)
						voxelStructure.SetVoxel(normalNeighborVoxelPosition, VoxelStructure.SelectedVoxelIndex);

						voxelStructure.Draw();
						EditorUtility.SetDirty(target);
						break;
					case VoxelStructure.Action.Erase: 	
						
						Undo.RecordObject(voxelStructure, "Erase");
                        voxelStructure.SetVoxel(targetVoxelPosition, 0);
						voxelStructure.Draw();
						EditorUtility.SetDirty(target);
						
						break;
					case VoxelStructure.Action.Flood:
						Undo.RecordObject(voxelStructure, "Flood");
						voxelStructure.FloodBasic((int)Mathf.Floor(hit.point.y));
						voxelStructure.Draw();
						EditorUtility.SetDirty(target);
						break;
					
					}
				
			
			}
		}
		
		
	}


	
	//Here is where we handle any user interactions with the scene view
	void OnSceneGUI()
	{
		VoxelStructure voxelStructure = (VoxelStructure)target;
	    //If we are here, we must have received some event. Grab it for later usage.
		Event e = Event.current;
		currentMousePosition = e.mousePosition;




		//small hack to ensure the mouse events aren't overridden by the default unity actions
		if (e.shift && e.type == EventType.Layout){
			HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
		}
		
		//If it's an undo or redo, just redraw.
		//This doesn't always seem to fire, I'm think there must be a race condition
		if (e.commandName == "UndoRedoPerformed") {
	    	voxelStructure.Draw();
			Repaint ();
	    	return;
	    }



		//if shift && mousedown, do that spraypaint thang
		if( e.shift && (e.type == EventType.MouseDrag || e.type == EventType.MouseDown) ){
			var currentTime = DateTime.Now;
			var dTime = currentTime.Subtract(lastSprayPaintAction);
			if(dTime.Milliseconds>sprayPaintThrottle){
				lastSprayPaintAction=currentTime;
				if(e.button == 0){
					HandleClick (e.mousePosition, VoxelStructure.LMB_Action, e.button);
				}
				else if(e.button == 1){
					HandleClick (e.mousePosition, VoxelStructure.RMB_Action, e.button);
				}
				e.Use ();
			}
			
		}

		else if(e.type == EventType.MouseDown ) {
			if(e.button == 0){
		    	wasLeftMouseDown = true;
				HandleClick (currentMousePosition, VoxelStructure.LMB_Action, e.button);
			}else if(e.button == 1){
		    	wasRightMouseDown = true;
				HandleClick (currentMousePosition, VoxelStructure.RMB_Action, e.button);
			}
		}


		
		
		
		
		
		

		
		
		//Keyboard shortcuts for tools
		if(e.type == EventType.KeyUp && e.keyCode == KeyCode.Alpha1){
			EditorPrefs.SetInt("LMBAction", (int)VoxelStructure.Action.Add);
			VoxelStructure.LMB_Action = VoxelStructure.Action.Add;
			Repaint();
			e.Use ();
		}
		if(e.type == EventType.KeyUp && e.keyCode == KeyCode.Alpha2){
			EditorPrefs.SetInt("LMBAction", (int)VoxelStructure.Action.MaxPull);
			VoxelStructure.LMB_Action = VoxelStructure.Action.MaxPull;	
			Repaint();
			e.Use ();
		}		
		if(e.type == EventType.KeyUp && e.keyCode == KeyCode.Alpha3){
			EditorPrefs.SetInt("LMBAction", (int)VoxelStructure.Action.Paint);
			VoxelStructure.LMB_Action = VoxelStructure.Action.Paint;	
			Repaint();
			e.Use ();
		}
		if(e.type == EventType.KeyUp && e.keyCode == KeyCode.Alpha4){
			EditorPrefs.SetInt("LMBAction", (int)VoxelStructure.Action.Erase);
			VoxelStructure.LMB_Action = VoxelStructure.Action.Erase;	
			Repaint();
			e.Use ();
		}
		if(e.type == EventType.KeyUp && e.keyCode == KeyCode.Alpha5){
			EditorPrefs.SetInt("LMBAction", (int)VoxelStructure.Action.Flood);
			VoxelStructure.LMB_Action = VoxelStructure.Action.Flood;	
			Repaint();
			e.Use ();
		}
	}
	
	

	
	
	
	
	
	
	
	

}