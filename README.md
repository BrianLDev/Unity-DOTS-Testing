# Unity DOTS / ECS / Job System / Burst Compiler Testing

## Description

Unity is rebuilding the entire core of their platform using DOTS (Data-Oriented Technology Stack) which allows for multi-threading and performance by default.  There are a lot of advantages to this rebuild but the primary one is *MASSIVE* performance increases.  



## [Data-Oriented Technology Stack](https://unity.com/dots)
There is a lot that goes into the DOTS system but these are the main categories

- [ECS System (Entities, Components, Systems)](https://docs.unity3d.com/Packages/com.unity.entities@latest/index.html)
    - New programming concept which differs from standard object oriented programming and shifts towards grouping similar (from a data perspective) things together in memory.
        - Entities = objects / things in the program (excluding data and functions)
        - Components = the entity's data
        - Systems = Action, functions, logic, processing
- [C# Job System](https://docs.unity3d.com/Manual/JobSystem.html)
    - Multithreading for high performance and efficiency
- [Burst Compiler](https://docs.unity3d.com/Packages/com.unity.burst@latest/index.html)
    - Converts job system code into highly optimized native code and enables huge performance increases
- [Unity's new Mathematics Library](https://docs.unity3d.com/Packages/com.unity.mathematics@latest/index.html)
    - Highly efficient C# math library that uses the burst compiler


## [DOTS Packages](https://unity.com/dots/packages#getting-started-dots)  

New packages are being added (and constantly changing) over time.  The main ones that I'l be working with are:
- [Entities](https://docs.unity3d.com/Packages/com.unity.entities@0.10/manual/index.html)
- [C# Job System](https://docs.unity3d.com/Packages/com.unity.entities@0.10/manual/ecs_job_extensions.html)
- [Burst Compiler](https://github.com/Unity-Technologies/GettingStartedWithBurst-Unite2019)  
- [Unity's New Physics Engine](https://unity.com/unity/physics)
- [Havok Physics](https://docs.unity3d.com/Packages/com.havok.physics@latest/index.html?_ga=2.122600203.145847635.1589180611-1058733522.1588214681)
- [Unity NetCode (for multiplayer)](https://www.youtube.com/watch?v=P_-FoJuaYOI)
***
