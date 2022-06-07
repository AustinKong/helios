Project "Helios": N-body gravity simulation
A gravity simulation using Newton's universal law of gravitation and Kepler's 1st law of planetary motion.
Made in May-June 2022 by Austin Kong :>

Controls:
-Space: Opens celestial body creation menu
-Middle Mouse Button: Click and drag to navigate, scroll to zoom in/out
-Esc/Alt+F4: Quit
-Left Click: Snap onto a body, displaying its info
-F: Focus camera onto a body, zooming in/out
-Up/Down Arrow Keys: Change orbital ellipse eccentricity
-R: Reset sandbox
-T: Toggle trajectory lines
-Q: Randomly select a body (Used to focus onto satelites cause they really are too tiny to click on :P)
-Delete: Delete selected body

Info:
G = 0.6
constant time step = 0.02s

Authors notes:
-Note that the orbital trajectory defaults to the current selected body as its central body
-There might be a 1 in 10000000000000000 chance of breaking the game when 2 bodies of the same mass collide, just Alt+F4 IF that happens :P
-Originally there was going to be procedural planet texture generation, I had coded a "gas giant texture" with perlin noise and "star texture" with voronoid noise
 But the scope was too large and I wanted to finish this project asap
-Note that the trajectory shown when creating a planet IS NOT an absolute prediction of the body's trajectory, it is merely the trajectory that would be taken
 in the case of 2 bodies orbiting, m1 >> m2, in an ideal simulation
-Also note that the units used are not accurate to reality, so are the conversions and representations - this is merely a fun project to observe the effect of
 Newton's universal law
-Have fun with this project!

-If you've come here to rip off my code, feel free to do so, but try not to Ctrl+C Ctrl+V, cause honestly speaking, I gave up trying to write clean code halfway through