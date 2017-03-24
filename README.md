# Asteroid-Defence---Unity3D

Earth is being bombarded by asteroids and it’s your job to destroy them before they cause damage. You control a huge turret that can fire missiles at any asteroids that would come within a designated safety zone. 
 
• The simulation runs in a top-down environment.
• Earth (and the turret) are located at the center of the screen.
• The player can “throw" asteroids towards earth (e.g. by dragging with mouse)
• When a new asteroid is spawned, the turret checks if the trajectory comes within the safety zone.
• If the asteroid would come too close, the turret would fire a missile, which intercepts the asteroid.
 
Here are some other facts about the scenario:
• The turret can fire immediately into any direction. There is no targeting delay.
• The turret must fire immediately when a new (dangerous) asteroid is spawned.
• As there is no gravity, the velocity of an asteroid stays constant after it has been spawned.
• Missiles are always launched with a constant speed S. Only the angle of the velocity vector is calculated.