cropped-voxel-fireplace.png 

Voxwell: A Quick Voxel Toolkit for Unity


Introduction
Getting started
Keyboard Shortcuts and Mouse Interactions
Voxel Structure Settings
Voxel Pallette
Current Voxel Type
The Mouse Action Dropdowns
Dimensions
Selectively Enable/Disable Faces
Generate Secondary UV Set
The Buttons
Voxel Pallette
Atlas Material
Voxel Templates[]
+ New Voxel Template
Generate Voxel Templates from Spritesheet - New!
Voxel Templates
The Material & Texture
Version History
0.1 - February 10, 2014
0.2 - July 20, 2014
0.2.2 - January 6, 2015 Current
0.4 - Coming Soon
Roadmap
“Chunking”
Procedural Generation Tools
Slopes/Stairs/Half Blocks
Better UI/UX
More Resources
Contact me
Thanks!


________________


Introduction


Voxwell is a small tool for Unity3D, aimed at enabling rapid game design and development. It uses voxels and a simple point and click interface to get you making game assets as quickly as possible. Other tools might result in more complicated, better looking, or more realistic assets, but Voxwell will be faster to learn and use, and will have a much smaller footprint on your project and workflow.


It is not intended to be an all in one package. 
It won’t force you to change the way you structure your code.
It won’t take you hours to figure out.


It is quick to setup, learn, and use.
It requires no technical knowledge, just the ability to point and click.
It will increase the speed that you design levels.
It will allow you to quickly test out ideas and refine them as soon as inspiration strikes.


I’ve included some additional resources (youtube tutorial/demo, contact info, link to the Unity Asset store) at the end of this document.


________________
Getting started


Once you’ve imported the Voxwell package, you can either open a brand new scene, or start with one of the demo scenes provided in /Assets/Voxwell/Demo Scenes/


For the purpose of this document, we’ll open the Empty Demo Scene, which just contains a directional light (with shadows) and a camera with a basic movement component.


Locate the “Basic Voxel Structure” prefab located in /Assets/Voxwell/ and drag that into your scene.


This prefab is simply a gameobject with the VoxelStructure.cs component. This component requires a Mesh Renderer, Mesh Collider, and Mesh Filter. It uses a Transparent/Cutout/Diffuse shader, and has the VoxelAtlas.psd texture applied to it.


To get started, go ahead and click the “Room with Back” button in the inspector. Now try left clicking and right clicking on the generated mesh. That’s really all there is to it, but read on to learn what else there is.


The two Scripts that do all of the work are /Voxwell/Scripts/VoxelStructure.cs and /Voxwell/Editor/VoxelStructureEditor.cs - If you want to customize this to suit your needs, most of the work will be done in VoxelStructure.cs, but exposing those functions to the editor will be in VoxelStructureEditor.cs


Keyboard Shortcuts and Mouse Interactions
If you hold down the shift key while dragging with the LMB or RMB, the current Action will continuously ‘fire.’ This allows for quicker addition or removal of voxels. 


You can use the numbers 1-5 to quickly select the desired action for the LMB.
1: Add
2: Max Pull
3: Paint
4: Erase
5: Flood
________________
Voxel Structure Settings


In the inspector, you should see some dropdown menus, input fields, checkboxes, and a whole bunch of buttons. Here’s the breakdown:
Voxel Pallette
The Voxel Pallette contains all of the definitions for the Voxel Templates. More information on Pallettes and Templates below.
Current Voxel Type
The Current Voxel Type determines which voxel template to use when performing any action. You’ll probably change this more frequently than anything else. 
The Mouse Action Dropdowns
The LMB Action and RMB Action which determines what to do with you click on the voxel structure with either the Left Mouse Button or Right Mouse button. Both dropdowns contain the same options, and are there to allow you to configure your mouse clicks however you’d like.


Add adds a new voxel wherever you click
Erase removes the voxel that you click
MaxPull creates a column/row of blocks from where you clicked until it finds another occupied cell
Flood takes the Y value of wherever you clicked, and sets every cell at that height to the current voxel type.
None is there in case you don’t want any actions interfering with the normal mouse clicks.


In the future, I will be adding more actions like surface extrusion and marquee selection.
Dimensions
Below those dropdowns, you’ll see a place to specify the Width, Height, and Depth of your voxel structure. The current max is 32x32x32 - though in the future, there will be no upper limit, and new gameobjects will be spawned automatically whenever the limit is reached.
Selectively Enable/Disable Faces
Next, we see some checkboxes. These determine whether or not it will generate a certain face for each voxel. 


For example, if you are creating a sidescroller, and are certain that the camera will never see the back of your level, simple uncheck Draw Back, hit the Draw button (below) and those faces won’t be created, saving precious verts and tris.
Generate Secondary UV Set
Because the voxel structures are already UV-mapped, if we want lightmapping to work, we must generate a secondary UV Set for the lightmap to use. Enabling this will slow down the time it takes to redraw a mesh, but will allow for lightmapping.
The Buttons 
Most of these buttons are simply a quick way to easily call utility functions from the inspector. As with the LMB and RMB actions, these buttons (with the exception of the Draw and Export Mesh buttons) use the Current Voxel Type field to determine what blocks to place. 




The Clear All button set’s every cell in the Voxel Structure to empty.
Solid Cube sets every cell in the Voxel Structure to the current voxel type.


Left Wall, Right Wall, Floor, Ceiling, Front Wall, and Back Wall all fill the specified wall with the current voxel type.


Set All Non-Empties is kind of weird, but can be useful at times. It basically looks for every voxel that is not empty, and sets them all to the current voxel type. Good if you want to test out a few options for the ‘main’ bits of your level - ie should this room be made of wood, or stone?


Grow Grass looks for every dirt voxel, checks to see if the voxel above it is empty, and if it is, it turns into a ‘dirt with grass’ voxel.


Pillars Front Eight is intended to be a starting point/example for you to create your own buttons. Search through the VoxelStructure.cs and VoxelStructureEditor.cs files to see how it works, and modify to suit your needs.


The Draw button just tells the Voxel Structure mesh to redraw itself. This is useful if you’ve somehow changed the Voxel Structure (for instance by enabling or disabling one of the ‘draw face’ checkboxes) and you want the mesh to be redrawn.






And finally, the Export Mesh button, which allows you to export the mesh of the Voxel Structure to use however you like. There are a number of Unity Extensions that let you take this mesh and export as a .obj, .fbx, or .dae, in case you want to take the mesh and fool around with it in another program.


Voxel Pallette
Voxel Pallettes are collections of Voxel Templates. Each Voxel Structure can only utilize the Voxel Templates from one Voxel Pallette.


Each Voxel Structure references a Voxel Pallette, and uses the information in it’s Voxel Templates to determine what to draw.


To create a new Voxel Pallette, go to Assets > Create > Voxel Pallette or right click in the Project view and select Create > Voxel Pallette.


In the inspector, you will see:
Atlas Material
Because the Voxel Templates rely on the material used by the Voxel Structure to properly render, the Voxel Pallette must provide the material to the Voxel Structure when it is drawn. Whatever material is selected here will be applied to the Mesh Renderer of the Voxel Structure whenever it is drawn.


Read more about the Material and Texture below.


Voxel Templates[]
This is where we keep references to all of the Voxel Templates that we want to use. Whenever you place a voxel in your structure, what you are really doing is setting an integer value. That integer value corresponds to the Voxel Template with that index in the Voxel Pallette.


For example, if you place a stone voxel somewhere, you are really setting that voxel value to 1. When the voxel structure is drawn, it finds the Voxel Template at index 1 in the Voxel Pallette and uses that data to figure out what to draw.


+ New Voxel Template
This button allows you to create a new Voxel Template for you to configure manually.
Generate Voxel Templates from Spritesheet - New!
If the texture referenced by the material used by the palette contains any sprites, you can press this button to automatically turn the sprites into voxel templates. Be warned, because I need to do some trickery to get a sensible voxel template preview image, this can take some time. If anyone knows of a faster way to generate a preview image, please let me know!
Voxel Templates
A Voxel Template is a scriptable object that describes how to draw each voxel. You can define UV-mapping, vertex colors, toggle which faces to draw, determine whether or not to draw the faces in the center of the voxel, and determine the display order in Voxel selection menus.


Defining your Voxel Template: First, expand the voxel template in the Voxel Pallette inspector. In the inspector you’ll see everything you need to customize your voxels.


Name - The name of your Voxel Template, for display purposes only.


Random Between Two Colors - If this checkbox is enabled, it allows you to choose two colors that will randomly be chosen from to determine vertex colors. If this is not enabled, only one color will be chooseable, and that will be used for the vertex colors. This is nice for adding some subtle variation to your structures.


Color - Use this to set the vertex color for your voxel. Whichever color you choose will be multiplied with the texture colors. NOTE: You must use a shader that supports vertex colors for this to work. Use the materials provided, Voxwell/Standard (Vertex Color) for Unity 5, or Voxwell/CutoutVertexColor.shader for Unity 4, or make your own. The default Unity Shaders do not support vertex colors.


Draw Faces in Center - Normally, all of the faces of the voxel will be drawn on the ‘outside of the cube.’ However, if you wanted to create something like a fence or flower, you can enable this option. If enabled, each of the faces will be drawn in the center of the voxel, and neighboring voxels will draw adjacent faces.


Use Front UVs for All Faces - Most of the time, you’ll only want one texture for each side of your voxel. Enabling this option means you will only need to configure the UVs for the front face, which will be copied to all other faces.


Voxel Face Configuration - Here, you can configure each face of your voxel. For each face, you can select a sprite from the texture used by the voxel pallette and determine whether or not to draw that face. If you have Use Front UVs for All Faces enabled, there will only be one face to configure. If it is disabled, you will be able to configure all 6 faces.








The Material & Texture
In ../Voxwell/Materials & Textures/ you will find two materials: Atlas (for Unity 4) and Atlas - Unity 5 (for Unity 5), both of which use the atlas_psd.psd. 


Atlas - Unity 5 uses the Voxwell/Standard (Vertex Color) shader, which is similar to the Unity 5 Standard shader, but multiplies vertex colors with the albedo texture.


Atlas uses the CutoutVertexColor shader, which is similar to the Transparent/Cutout/Diffuse shader, but multiplies vertex colors with the diffuse texture.


In order for face selection to work properly, you must use a texture with sprites. You can learn more about generating spritesheets on the Unity Website, like here for example: https://unity3d.com/learn/tutorials/modules/beginner/2d/sprite-editor


IMPORTANT: Playing around with the import settings on your texture can have unexpected results, if things are rendering oddly, check to make sure your import settings match mine:


Texture Type: Sprite (2d and UI)
Sprite Mode: Multiple
Generate Mip Maps: false, but you should feel free to play around with this depending on your scene/needs
Filter Mode: Point






________________
Version History
0.1 - February 10, 2014
Initial Release. 

0.2 - July 20, 2014 
Introduced Voxel Pallettes and Voxel Templates
Added support for Vertex colors
“Spray Paint” Mouse Actions

0.2.2 - January 6, 2015
Greatly Improved Shader. 
DirectX11 Fix.
Preview of ‘Voxel Complex Feature’

0.4 - September 1, 2015 - Current
Support for Unity5!
Can generate voxel templates from spritesheet
Preview Images for Voxel Templates
Improved UI for Voxel Templates, Voxel Pallettes, and Voxel Structures
Can copy and paste voxel structures without their meshes remaining ‘linked’


________________
Roadmap
The following items are features that I plan on adding in the future. They are roughly ordered based on feedback from users, but the order that they are implemented in are subject to change.
“Chunking”
Instead of needing to create a new Voxel Structure manually, and needing many structures to build something large, each structure will automatically break itself down into “chunks” to allow infinitely large structures.
Procedural Generation Tools
Create procedurally generated dungeons and terrains with the push of a button.
Slopes/Stairs/Half Blocks
Better UI/UX














________________




More Resources
Unity Asset Store: https://www.assetstore.unity3d.com/#/content/14989
YouTube demo/tutorial: http://www.youtube.com/watch?v=Z-9Q3GPFbkI


Contact me
E-mail: DarkMeatGames@gmail.com
Twitter: https://twitter.com/DarkMeatGames
Thanks!
Thanks to Paul Hocker for being an awesome tester, and giving me the idea to generate templates from a sprite sheet. You can follow him on Twitter @sP0CkEr2, or view his website at http://sdnllc.com/


Thanks to Mark Swick for some great improvements to the UI and some refactoring. You can follow him on Twitter @Riv_Roy


Thanks to Christian Flodihn for some awesome refactoring, some of which still needs to be fully incorporated :) - You view Christian’s website at www.flodihn.se


Thanks to reddit and all of their help and words of encouragement.
Extra special thanks to /u/Rancarable for coming up with the name Voxwell!