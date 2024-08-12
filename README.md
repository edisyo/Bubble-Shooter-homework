# Bubble Shooter
 Bubble shooter 2D game for a homework in Unity

This project was developed as technical assigment for MunchMunchDev. 
The goal of the project was to create a simple hyper-casual mobile 
game prototype with a Bubble Shooter mechanic in around 4 hour time.
Included subtasks were:
	* Gameplay mechanics from the reference game
	* An input method for devices without a cursor (e.g. mobile devices)
	* Level editor (meaning, levels wouldn’t be random anymore)
	* A level selection menu
	* Juice (game feel)
	* Come up with your own subtask

-------------------------------------------------------------------------------

## The project's main gameplay mechanics are:
	* Bubble layout
	* Shooting the Bubble
	* Attaching shot Bubble to other Bubbles
	* Matching Bubbles


## Technology used
The project is made in Unity platform, using Unity 2021.3.25 LTS
All scripts are wrote in C#
Project uses URP Render Pipeline


## How to play instructions
For now there is only 1 level made and there is only 1 input - mouse button.

	**Input:**
		* Aim 			- Using mouse/ cursos
		* Shoot Bubble 	- Left Mouse Button

	**Instructions:**
		* Open the project in the specified Unity Version
		* Press Play
		* Use your mouse to aim
		* Use Left Mouse Button to Shoot Bubbles


## Challenges
	* Bubble colors
		TLDR: HEX code didnt work. Tried RGB(0-255), that didnt work. Tried the
		built-in Color.Blue and suddenly worked. Changed all my colors to
		RGB(0-1) and works as intended.
    The first big challenge, that i spent way too much time on is about
	 Balloon colors. At first I wanted to use HEX values for color, Unity
	 didn't really seem to support that, which is VERY, VERY weird to me.
	 Then I tried to do it with RGB(0-255) values - using Unity's class
	 Color, which seemed very intuitive at first. It worked only for a few
	 colors, but the color was still a little bit off. Then I spent way too
	 much time trying to understand what is happening and playing around with
	 different variables. Then I tried to aplly simple, bult-in colors, like 
	 Color.Blue etc. And it worked. Then I was very confused. I debugged the
	 new color and it printed out in RGB(0-1). Then I changed all my colors
	 to RGB(0-1) and suddenly they worked.

	* Bubble matching
		I couldn't find a way how to get the bubbles, that are around the 
		just shot bubble. I was thinking about using Raycasts, but thought it
		will be harder and ineffective. I was reading about CircleCollider2D 
		and it's methods, then I saw OverlapCollider which led me to find a
		Physics2D.OverlapCircleAll which sounded exactly what I needed. Tried
		it out and it was just what I wanted. I also thought about using arrays,
		but thought that I have to redo the layput logic.


## Bugs/ Problems
	* There is a bug, that rarely after bouncing off the wall, the bubble wont
	attach correctly where it is supposed to attach.


## Future Improvements
	* Need to improve "Floating bubble" logic, does not work correctly
	* Create a full game loop
	* While writing this Readme file and looking at the code, I dont understand
	how I didnt thought about this sooner. When creating layout, I can put each
	Bubble into an 2D array
	* Add better background image
	* Add UI elements, like score
	* Add Particle Systems and Audio/Sounds
	* Add Main menu and Level selection screen
	* Level editor would be perfect - available colors, layout size and pattern
	* Add obstacles


Hope I mentioned everything.
Looking forward to perfecting this project and my skills.


Eduards, 12.08.2024
