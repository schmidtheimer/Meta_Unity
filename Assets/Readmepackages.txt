Packages in use:
Meta All In One
Meta XR Simulator
Unity XR Core + Plugin Management 

Notes:

Quest 3 crashes trying to run OVRSceneManager
Quest Pro can run fine with no issues simulating physics


TO DO 11-15 dec:
Why tf does quest 3 crash so much. If cannot figure out why, Inform affi omegalul
gain access to quest developer hub

UPDATE: I FIXED IT WAAAOWWWWWW. I reset the Q3 completely and re-paired it and now i can run passthrough in playmode with no issue (besides lag but that happens on quest pro too)


To Do 18-22:
Add debugging to scene
EXPAND THE BALL SCENE RAAAAAHHHH
	Details:
		- Figure out how to tie virtual details to physical anchors (top prio)
		- Either add or elaborate available furniture in scene
		- Add hand gestures (lowest prio)
		- Layering issues	(Depth issue, More or less resolved with materials - with varying degrees of success)		

FURTNITURE SPAWNER LOGIC
retrieves data from its own SceneAnchor component
checks if its own classification is a table/couch then assigns the prefab transform variables as a child 

How am I going to do virtual obj, physical anchors? 
	SceneAnchor component is crucial. If I can use the same logic of getting every SceneAnchor with a table classification,
	I can get its transform.information
	I need to store that info somewhere, then retrieve it when spawning other items relevant to it.
	I tried finding links and containers in SceneManager, Contains QueryForExistingAnchorsTransform. with a SceneAnchorsLIst
	Furthermore has UpdateAllSceneAnchors() method
	BUT the OVRSceneAnchor in scenemanager is a weird variable. no references ANYWHERE. Using OVRSceneAnchor somwehere else
	returns a class.

	I need to run it in editor and actually look at wtf is happening  in the scene. fuckin impossible otherwise. legit.

LOGIC:
Start > Spawn Scene Room > Classifications are all children of scene room > these get updated 3 times per frame

THEORY:
Sub to SceneLoaded event (wtv its called) then retrieve the scene room GO, and the classifications that contain table GO
 - Retrieve scemantic labels instead. They will all have a public string label field. Thats how I get my table

 Result:
 Doing a Contains method is how I find tables. 
 Now I have tables. tf do I do

 To Do:
 Find a way to add physics objects (after scene has been constructed)
 Stencil shading to find virtual objs in a real env
 Either add or elaborate available furniture in scene
 Add interactions for virtual objects 