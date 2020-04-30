# Unity ECS Tutorial - Simple Guide

Link to tutorial on Medium.com:  
[A simple guide to get started with Unity ECS](https://levelup.gitconnected.com/a-simple-guide-to-get-started-with-unity-ecs-b0e6a036e707)

***
## Description

Unity is rebuilding the entire core of their platform using DOTS (Data-Oriented Technology Stack) and ECS (Entities, Components, Systems).  There are a lot of advantages to this rebuild but the primary one is *MASSIVE* performance increases.  

In this demo, we'll be creating the same scene in the current standard (soon to be legacy) version of Unity, and then the same scene using DOTS / ECS and comparing the performance of the two.  A simple scene with just a ton of cubes moving around to see how well the system handles a large amount of moving objects.

Results: The current / legacy system has performance of only 4-5 FPS.  The new DOTS / ECS system has performance of 200-300 FPS!  50x to 60x better performance.

Link to video comparing current MonoBehavior vs DOTS / ECS:  
[Standard MonoBehaviour implementation vs ECS in Unity3D](https://www.youtube.com/watch?v=tMafMne-DxM&feature=emb_title)