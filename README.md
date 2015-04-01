2015-WSU-Hackathon
==================

CREATORS:
----------------------------------------------------------------------------
Carson Tolleshaug,
Eliza Dolecki


PROJECT:
----------------------------------------------------------------------------
"Gravity" - A physics based puzzle platformer made using XNA and C#


OVERVIEW:
----------------------------------------------------------------------------
This project was created for the 3rd Annual WSU EECS Hackathon. The point 
of the hackathon was to make a piece of software in 24 hours. Our team 
(Team Gravity) decided to make a game.

For the game, we had to initially develop to physics engine to handle
object interaction. We did this from scratch. No part of the project was
created before hand. XNA and .NET obviously have some pre-existing libraries
which are used in the game, but other than a few graphics related classes
and the XmlReader class the code was all written within a 24 hour time 
period.

The game itself involves the player moving blocks with a "gravity gun" to
block lasers to make it to the end.


WHAT WAS ACCOMPLISHED:
-----------------------------------------------------------------------------
Physics Engine
  - Broadphase Collision Detection
  - AABB Collision Detection
  - Impulse Resolution
  - Simulate Friction
  - Mass/Force/Velocity
  - Allowing for infinite mass objects (such as floor)


The Game
  - Player object that is moved by WASD
  - Cursor object that is moved by Arrow Keys and lights up when over a
    "pullable" object.
  - Box object that is just a simple gravity-bound object
  - Floor Segment object that has infinite mass (not affected by gravity)
  - Pulling objects to the player (Player movement is disabled while this is
    happening)
  - Thowing an object in the direction of the cursor
  - Lasers that extend until they collide with an object and will cause the
    player to die if they come in contact with the player
  - Level parsing from XML files for easy level design (XML files use a tile
    system for size and position of objects to make estimating easier)


WHAT WAS NOT ACCOMPLISHED:
-----------------------------------------------------------------------------
The sprites are quite poor in quality due to time restrictions, and 
everything exists in Axis-Aligned Bounding Boxes. If I have some time in 
the future I might continue to work on this project to improve the sprite 
graphics, add circles and possibly allow for rotation and non-rectangle 
polygons.
