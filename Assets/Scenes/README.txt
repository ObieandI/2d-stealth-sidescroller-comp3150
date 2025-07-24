
notes for the level designer: 

all the scripts have some comments in them. they are at the very top. please read them first before implementing if it is your first time doing so. 

when something is not function correctly, please check: 

1. is the object(s) in the correct layer(s) if it(they) is going to collide with other objects in the scene? 
2. are the colliders actually colliding, if there are colliders present. 
3. does the gameObject have all of its public variable assigned? if a variable is public, it should not be left as null or 0 
4. if all the public variables are assigned, are they correctly assigned? Especially the ones where certain sprite need to be assigned 
5. do you have an ui? Enemy scripts are dependent on the ui, so if there is no ui or an ui manager, then it's not going to work
6. do you have all the manager in the scene? this should an easy one to figure out, look at the errors and it should tell which manager is missing 

some other notes: 
1. checkpoints
    Checkpoint needs to be assigned in the game manager for them to work. you need at least 2 checkpoints (reason explained in the checkpoint script). 
    The order MATTERS! very important. if the player is going to hit checkpoint A first, then checkpoint A should be first on the list. 

2. the restart button
    for the restart button in the victoty and defeat screen to function, you need a eventSystem present in the hinerarchy to function. unity will create an eventSystem for you if you  
    create a canves. i recommand you create a canves youself then replace the canves with the canves prefab (or just drag whatever we have in the canves into the canves you created)
3. audio sources 
    if hear a sound being played at launch, that because all of the audio sources default to play on awake. there is a box called "play on awake", untick said box to stop this from happening 

4. Ladder 
     Stop re-pruposing the ladder into branches! it's not going to work! if u want something to be a ladder, make sure it has the tag "ladder" and on the defaualt layer. 
     if you dont want something to be a ladder, simply do the opposite. 

5. Background should be scaled to 2x - 2.5x to be large enough to cover the trees, it should be seamless with itself to cover further distances.

6. The "player stuck on the wall" bug. There is a fix available. Go to the PhysicsMaterial folder and apply the NoFricition material in the RigidBody of the ground grid. 
    Player need to have this physics material applied to it as well, please double check. 
    