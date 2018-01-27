using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Collections.Generic;

[ExecuteInEditMode]
[CustomEditor(typeof(VoxelPallette))]
public class VoxelPalletteEditor : Editor
{

    public int selectedTemplate = 0;


    public bool shouldshow = false;
    void Awake()
    {
    }

    public override void OnInspectorGUI()
    {

        var voxelPallette = (VoxelPallette)target;
		 

        #if UNITY_5
        if (voxelPallette.Unity5AtlasMaterial == null && voxelPallette.AtlasMaterial != null)
        {
            voxelPallette.Unity5AtlasMaterial = voxelPallette.AtlasMaterial;
        }

        voxelPallette.Unity5AtlasMaterial = EditorGUILayout.ObjectField("Atlas Material", voxelPallette.Unity5AtlasMaterial, typeof(Material), false) as Material;
        voxelPallette.AtlasMaterial = voxelPallette.Unity5AtlasMaterial;

        #else

        if (voxelPallette.Unity4AtlasMaterial == null && voxelPallette.AtlasMaterial != null){
            voxelPallette.Unity4AtlasMaterial = voxelPallette.AtlasMaterial;
        }
        voxelPallette.Unity4AtlasMaterial = EditorGUILayout.ObjectField("Atlas Material", voxelPallette.Unity4AtlasMaterial, typeof(Material), false) as Material;
        voxelPallette.AtlasMaterial = voxelPallette.Unity4AtlasMaterial;

        #endif


        if (voxelPallette.AtlasMaterial == null || voxelPallette.AtlasMaterial.mainTexture == null)
        {
            EditorGUILayout.HelpBox("The material you have selected (or not selected) does not have a mainTexture. You'll need to fix that.", MessageType.Warning);
            return;
        }
        else
        {

            ///TODO: Only make this check when the material has changed, resulting lists should be cached in the voxelpallette
            var sprites = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(voxelPallette.AtlasMaterial.mainTexture)).OfType<Sprite>().ToList<Sprite>();
            if (sprites.Count == 0)
            {
                EditorGUILayout.HelpBox("The selected material's mainTexture does not have any sprites associated with it. You'll need to generate the sprites for that texture.", MessageType.Warning);
                return;
            }


            var spriteGuis = new List<GUIContent>();
            for (var i = 0; i < sprites.Count; i++)
            {
                spriteGuis.Add(new GUIContent(sprites[i].name, AssetPreview.GetAssetPreview(sprites[i])));
            }







            var basicText = new GUIStyle(GUI.skin.label);
            basicText.alignment = TextAnchor.UpperLeft;

            var col1Style = new GUIStyle(basicText);
            var col2Style = new GUIStyle(basicText);
            var col3Style = new GUIStyle(basicText);

            var defaultBGColor = GUI.backgroundColor;

            if (voxelPallette.voxelTemplatesSorted != null)
            {

                //for each of the templates, draw their editor ui
                for (var i = 0; i < voxelPallette.voxelTemplatesSorted.Count; i++)
                {



                    var theTemplate = voxelPallette.ConvertSortedIndexToTemplate(i);

                    if (theTemplate)
                    {
                        GUILayout.Box("", new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(1) });

                        //display the preview texture of the template
                        var theTemplateGui = new GUIContent(theTemplate.name, theTemplate.DisplayTexture);

                        GUI.backgroundColor = Color.white;



                        //kind of a hack to allow for custom 'foldout' behavior, how could this be better?
                        theTemplate.shouldShowInInspector = (EditorGUILayout.Foldout(theTemplate.shouldShowInInspector, theTemplateGui));

                        GUI.backgroundColor = defaultBGColor;





                        if (theTemplate.voxelPallette == null)
                        {
                            theTemplate.voxelPallette = voxelPallette;
                        }
                        var mychildren = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(voxelPallette));

                        ///TODO: Cleanup language/instructions
                        if (!mychildren.Contains(theTemplate) || (theTemplate.voxelPallette != null && theTemplate.voxelPallette != voxelPallette))
                        {
                            EditorGUILayout.HelpBox("Warning: This Template is not a child of this Pallette, or is being used by another Pallette.", MessageType.Warning);
                            if (GUILayout.Button("Copy this Template as a child of this Pallette"))
                            {
                                var newTemplate = CopyVoxelTemplate(voxelPallette, theTemplate);
                                voxelPallette.voxelTemplateList[voxelPallette.ConvertSortedIndexToActualIndex(i)] = newTemplate;

                                ///TODO: Generating asset preview here doesn't work??
                                newTemplate.GenerateAssetPreview();
                            }
                        }




                        else if (theTemplate.shouldShowInInspector)
                        {

                            theTemplate.name = EditorGUILayout.TextField("Name", theTemplate.name);

                            theTemplate.RandomBetweenTwoColors = GUILayout.Toggle(theTemplate.RandomBetweenTwoColors, " Random between two colors?");
                            theTemplate.color = EditorGUILayout.ColorField(theTemplate.color);
                            if (theTemplate.RandomBetweenTwoColors)
                            {
                                theTemplate.color2 = EditorGUILayout.ColorField(theTemplate.color2);
                            }

                            EditorGUILayout.Separator();

                            EditorGUILayout.LabelField("Face Configuration");
                            theTemplate.drawFacesInCenter = GUILayout.Toggle(theTemplate.drawFacesInCenter, " Draw Faces in Center");

                            theTemplate.useFrontUvsForAllFaces = GUILayout.Toggle(theTemplate.useFrontUvsForAllFaces, " Use Front UVs for all Faces?");

                            var col1Width = (Screen.width / 5f);
                            var col2Width = (Screen.width / 2f);
                            var col3Width = (Screen.width / 6f);

                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField("Position", col1Style, GUILayout.Width(col1Width));
                            EditorGUILayout.LabelField("Sprite", col2Style, GUILayout.Width(col2Width));
                            EditorGUILayout.LabelField("Draw?", col3Style, GUILayout.Width(col3Width));
                            EditorGUILayout.EndHorizontal();




                            //FRONT FACE
                            EditorGUILayout.BeginHorizontal();

                            var sf = theTemplate.SpriteFront;
                            var currentIndex = sprites.IndexOf(sf);

                            EditorGUILayout.LabelField("Front", col1Style, GUILayout.Width(col1Width));
                            var newIndex = EditorGUILayout.Popup(currentIndex, spriteGuis.ToArray(), GUILayout.Width(col2Width));

                            theTemplate.drawFront = EditorGUILayout.Toggle(theTemplate.drawFront);

                            if (newIndex >= 0 && newIndex <= sprites.Count)
                                theTemplate.SpriteFront = sprites[newIndex];

                            EditorGUILayout.EndHorizontal();
                            //END FRONT FACE




                            if (!theTemplate.useFrontUvsForAllFaces)
                            {

                                //BACK FACE
                                EditorGUILayout.BeginHorizontal();

                                var sb = theTemplate.SpriteBack;
                                currentIndex = sprites.IndexOf(sb);

                                EditorGUILayout.LabelField("Back", col1Style, GUILayout.Width(col1Width));
                                newIndex = EditorGUILayout.Popup(currentIndex, spriteGuis.ToArray(), GUILayout.Width(col2Width));

                                theTemplate.drawBack = EditorGUILayout.Toggle(theTemplate.drawBack);

                                if (newIndex >= 0 && newIndex <= sprites.Count)
                                    theTemplate.SpriteBack = sprites[newIndex];

                                EditorGUILayout.EndHorizontal();
                                //END BACK FACE






                                //TOP FACE
                                EditorGUILayout.BeginHorizontal();

                                var st = theTemplate.SpriteTop;
                                currentIndex = sprites.IndexOf(st);

                                EditorGUILayout.LabelField("Top", col1Style, GUILayout.Width(col1Width));
                                newIndex = EditorGUILayout.Popup(currentIndex, spriteGuis.ToArray(), GUILayout.Width(col2Width));

                                theTemplate.drawTop = EditorGUILayout.Toggle(theTemplate.drawTop);

                                if (newIndex >= 0 && newIndex <= sprites.Count)
                                    theTemplate.SpriteTop = sprites[newIndex];

                                EditorGUILayout.EndHorizontal();
                                //END TOP FACE






                                //BOTTOM FACE
                                EditorGUILayout.BeginHorizontal();

                                var sbt = theTemplate.SpriteBottom;
                                currentIndex = sprites.IndexOf(sbt);

                                EditorGUILayout.LabelField("Bottom", col1Style, GUILayout.Width(col1Width));
                                newIndex = EditorGUILayout.Popup(currentIndex, spriteGuis.ToArray(), GUILayout.Width(col2Width));

                                theTemplate.drawBottom = EditorGUILayout.Toggle(theTemplate.drawBottom);

                                if (newIndex >= 0 && newIndex <= sprites.Count)
                                    theTemplate.SpriteBottom = sprites[newIndex];

                                EditorGUILayout.EndHorizontal();
                                //END BOTTOM FACE




                                //RIGHT FACE
                                EditorGUILayout.BeginHorizontal();

                                var sr = theTemplate.SpriteRight;
                                currentIndex = sprites.IndexOf(sr);

                                EditorGUILayout.LabelField("Right", col1Style, GUILayout.Width(col1Width));
                                newIndex = EditorGUILayout.Popup(currentIndex, spriteGuis.ToArray(), GUILayout.Width(col2Width));

                                theTemplate.drawRight = EditorGUILayout.Toggle(theTemplate.drawRight);

                                if (newIndex >= 0 && newIndex <= sprites.Count)
                                    theTemplate.SpriteRight = sprites[newIndex];

                                EditorGUILayout.EndHorizontal();
                                //END RIGHT FACE






                                //LEFT FACE
                                EditorGUILayout.BeginHorizontal();

                                var sl = theTemplate.SpriteLeft;
                                currentIndex = sprites.IndexOf(sl);

                                EditorGUILayout.LabelField("Left", col1Style, GUILayout.Width(col1Width));
                                newIndex = EditorGUILayout.Popup(currentIndex, spriteGuis.ToArray(), GUILayout.Width(col2Width));


                                theTemplate.drawLeft = EditorGUILayout.Toggle(theTemplate.drawLeft);


                                if (newIndex >= 0 && newIndex <= sprites.Count)
                                    theTemplate.SpriteLeft = sprites[newIndex];

                                EditorGUILayout.EndHorizontal();
                                //END LEFT FACE





                            }






                            EditorGUILayout.Separator();

                            EditorGUILayout.BeginHorizontal();

                            if (GUILayout.Button("Move Up"))
                            {
                                var initialOrder = theTemplate.DisplayOrder;
                                var newOrder = theTemplate.DisplayOrder - 1;
                                var templateToSwapWith = voxelPallette.voxelTemplateList.Find(v => v != null && v.DisplayOrder == newOrder);


                                if (templateToSwapWith)
                                {
                                    templateToSwapWith.DisplayOrder = initialOrder;
                                    EditorUtility.SetDirty(templateToSwapWith);
                                }



                                theTemplate.DisplayOrder = newOrder;
                                EditorUtility.SetDirty(theTemplate);
                                voxelPallette.ArrangeVoxelTemplates();
                                EditorUtility.SetDirty(voxelPallette);

                                Repaint();
                            }
                            if (GUILayout.Button("Move Down"))
                            {
                                var initialOrder = theTemplate.DisplayOrder;
                                var newOrder = theTemplate.DisplayOrder + 1;
                                var templateToSwapWith = voxelPallette.voxelTemplateList.Find(v => v != null && v.DisplayOrder == newOrder);


                                if (templateToSwapWith)
                                {
                                    templateToSwapWith.DisplayOrder = initialOrder;
                                    EditorUtility.SetDirty(templateToSwapWith);
                                }

                                theTemplate.DisplayOrder = newOrder;
                                EditorUtility.SetDirty(theTemplate);
                                voxelPallette.ArrangeVoxelTemplates();
                                EditorUtility.SetDirty(voxelPallette);

                                Repaint();
                            }
                            EditorGUILayout.EndHorizontal();







                            //----------------------------------------------------------
                            //   DELETE BUTTON
                            //----------------------------------------------------------

                            EditorGUILayout.BeginHorizontal();

                            GUI.backgroundColor = Color.red;

                            if (GUILayout.Button("Destroy Voxel Template"))
                            {

                                var fullpath = AssetDatabase.GetAssetPath(theTemplate) + "/" + theTemplate.name + ".asset";
                                if (EditorUtility.DisplayDialog("Delete Voxel Template?", "Are you sure you want to move this Voxel Template to the trash?", "Delete", "Cancel"))
                                {
                                    voxelPallette.voxelTemplateList[voxelPallette.ConvertSortedIndexToActualIndex(i)] = null;
                                    AssetDatabase.MoveAssetToTrash(fullpath);
                                }


                            }
                            GUI.backgroundColor = defaultBGColor;

                            EditorGUILayout.EndHorizontal();

                            //----------------------------------------------------------






                            if (GUILayout.Button("Generate Asset Preview"))
                            {
                                theTemplate.GenerateAssetPreview();
                            }



                            GUILayout.Box("", new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(1) });


                            theTemplate.UpdateUVOffsets();

                        }
                    }


                }


            }






            EditorGUILayout.Separator();


            if (GUILayout.Button("+ New Voxel Template"))
            {
                AddNewVoxelTemplate(voxelPallette);

            }

            EditorGUILayout.Separator();
            if (GUILayout.Button("Generate Voxel Templates from Spritesheet"))
            {
                GenerateVoxelTemplates();
            }
            EditorGUILayout.Separator();


            //----------------------------------------------------------
            // Super Secret Debug Buttons, shhhhhh
            //----------------------------------------------------------

            /*
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Reveal All Child Templates"))
            {
                var theTemplates = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(voxelPallette)).OfType<VoxelTemplate>().ToArray();
                for (var i = 0; i < theTemplates.Length; i++)
                {

                    theTemplates[i].hideFlags = HideFlags.None;
                    AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(theTemplates[i]));
                    //				AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(voxelPallette));
                }

            }
            if (GUILayout.Button("Hide All Child Templates"))
            {
                var theTemplates = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(voxelPallette)).OfType<VoxelTemplate>().ToArray();
                for (var i = 0; i < theTemplates.Length; i++)
                {
                    theTemplates[i].hideFlags = HideFlags.HideInHierarchy;

                    AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(theTemplates[i]));
                    //				AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(voxelPallette));

                }

            }
            EditorGUILayout.EndHorizontal();
            */



        }





    }



    void GenerateVoxelTemplates()
    {

        var voxelPallette = (VoxelPallette)target;

        //Get all of the sprites
        Sprite[] sprites = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(voxelPallette.AtlasMaterial.mainTexture)).OfType<Sprite>().ToArray();


        //if there aren't anything, log an error, we can't do anything!
        if (sprites.Length == 0)
        {
            Debug.LogError("There are no sprites associated with the selected Texture. Voxel Templates cannot be generated");
        }
        else if (EditorUtility.DisplayDialog("Generate voxel templates?", "You are about to create " + sprites.Length + " voxel templates. That could take some time. You sure?", "Yes, Create the Templates", "Cancel"))
        {

            //get the path to the voxel pallette we're working with
            string path = AssetDatabase.GetAssetPath(voxelPallette);
            path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(voxelPallette)), "");



            if (voxelPallette.voxelTemplateList == null || voxelPallette.voxelTemplateList.Count == 0)
            {
                voxelPallette.voxelTemplateList = new List<VoxelTemplate>();
                voxelPallette.voxelTemplateList.Add(null);
            }


            
            for (var i = 0; i < sprites.Length; i++)
            {
                var sprite = sprites[i];
                AddNewVoxelTemplate(voxelPallette, sprite);

            }
            EditorUtility.SetDirty(voxelPallette);
            AssetDatabase.SaveAssets();

        }







    }

    void AddNewVoxelTemplate(VoxelPallette voxelPallette)
    {

        VoxelTemplate newVoxelTemplate = ScriptableObject.CreateInstance<VoxelTemplate>();
        newVoxelTemplate.shouldDraw = true;
        newVoxelTemplate.name = "New Voxel Template";
        newVoxelTemplate.voxelPallette = voxelPallette;
        newVoxelTemplate.DisplayOrder = voxelPallette.voxelTemplatesSorted.Count();


        AssetDatabase.AddObjectToAsset(newVoxelTemplate, voxelPallette);
        voxelPallette.voxelTemplateList.Add(newVoxelTemplate);

        newVoxelTemplate.hideFlags = HideFlags.HideInHierarchy;


        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(newVoxelTemplate));
        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(voxelPallette));

        newVoxelTemplate.GenerateAssetPreview();
        EditorUtility.SetDirty(newVoxelTemplate);
    }

    void AddNewVoxelTemplate(VoxelPallette voxelPallette, Sprite sprite)
    {

        VoxelTemplate newVoxelTemplate = ScriptableObject.CreateInstance<VoxelTemplate>();
        newVoxelTemplate.shouldDraw = true;
        newVoxelTemplate.name = sprite.name;

        newVoxelTemplate.atlasScale = new Vector2(sprite.rect.width / voxelPallette.AtlasMaterial.mainTexture.width, sprite.rect.height / voxelPallette.AtlasMaterial.mainTexture.height);
        newVoxelTemplate.voxelPallette = voxelPallette;



        newVoxelTemplate.SpriteFront = sprite;
        newVoxelTemplate.SpriteBack = sprite;
        newVoxelTemplate.SpriteLeft = sprite;
        newVoxelTemplate.SpriteRight = sprite;
        newVoxelTemplate.SpriteTop = sprite;
        newVoxelTemplate.SpriteBottom = sprite;

        var offset = new Vector2(sprite.rect.x / voxelPallette.AtlasMaterial.mainTexture.width, sprite.rect.y / voxelPallette.AtlasMaterial.mainTexture.height);
        newVoxelTemplate.UVOffsetFront = offset;
        newVoxelTemplate.UVOffsetBack = offset;
        newVoxelTemplate.UVOffsetTop = offset;
        newVoxelTemplate.UVOffsetBottom = offset;
        newVoxelTemplate.UVOffsetLeft = offset;
        newVoxelTemplate.UVOffsetRight = offset;


        AssetDatabase.AddObjectToAsset(newVoxelTemplate, voxelPallette);
        voxelPallette.voxelTemplateList.Add(newVoxelTemplate);

        newVoxelTemplate.hideFlags = HideFlags.HideInHierarchy;


        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(newVoxelTemplate));
        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(voxelPallette));

        newVoxelTemplate.GenerateAssetPreview();
        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(newVoxelTemplate));
        EditorUtility.SetDirty(newVoxelTemplate);

    }


    VoxelTemplate CopyVoxelTemplate(VoxelPallette voxelPallette, VoxelTemplate original)
    {

        VoxelTemplate newVoxelTemplate = ScriptableObject.CreateInstance<VoxelTemplate>();
        newVoxelTemplate.shouldDraw = original.shouldDraw;
        newVoxelTemplate.name = original.name;

        newVoxelTemplate.atlasScale = original.atlasScale;
        newVoxelTemplate.voxelPallette = voxelPallette;


        newVoxelTemplate.drawFacesInCenter = original.drawFacesInCenter;
        newVoxelTemplate.color = original.color;

        newVoxelTemplate.UVOffsetFront = original.UVOffsetFront;
        newVoxelTemplate.UVOffsetBack = original.UVOffsetBack;
        newVoxelTemplate.UVOffsetTop = original.UVOffsetTop;
        newVoxelTemplate.UVOffsetBottom = original.UVOffsetBottom;
        newVoxelTemplate.UVOffsetLeft = original.UVOffsetLeft;
        newVoxelTemplate.UVOffsetRight = original.UVOffsetRight;


        newVoxelTemplate.SpriteFront = original.SpriteFront;
        newVoxelTemplate.SpriteBack = original.SpriteBack;
        newVoxelTemplate.SpriteTop = original.SpriteTop;
        newVoxelTemplate.SpriteBottom = original.SpriteBottom;
        newVoxelTemplate.SpriteLeft = original.SpriteLeft;
        newVoxelTemplate.SpriteRight = original.SpriteRight;


        newVoxelTemplate.drawFront = original.drawFront;
        newVoxelTemplate.drawBack = original.drawBack;
        newVoxelTemplate.drawLeft = original.drawLeft;
        newVoxelTemplate.drawRight = original.drawRight;
        newVoxelTemplate.drawTop = original.drawTop;
        newVoxelTemplate.drawBottom = original.drawBottom;



        MakeTemplateChildOfPallette(voxelPallette, newVoxelTemplate);
        newVoxelTemplate.GenerateAssetPreview();


        EditorUtility.SetDirty(newVoxelTemplate);

        return newVoxelTemplate;
    }
    void MakeTemplateChildOfPallette(VoxelPallette targetPallette, VoxelTemplate voxelTemplate)
    {
        AssetDatabase.AddObjectToAsset(voxelTemplate, targetPallette);
        voxelTemplate.hideFlags = HideFlags.HideInHierarchy;

        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(voxelTemplate));
        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(targetPallette));

    }

}
