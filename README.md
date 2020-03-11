
<p align="center">
  <img width="640" height="320" src="https://raw.githubusercontent.com/Timmoth/Aptacode.TaskPlex/master/Resources/Images/TaskPlexBanner.png">
</p>

### A lightweight cross platform .net animation & render engine.

NuGet package:

https://www.nuget.org/packages/Aptacode.TaskPlex/

[![Codacy Badge](https://api.codacy.com/project/badge/Grade/d25f0cea83384aacada81fa9790679c8)](https://www.codacy.com/manual/Timmoth/AptacodeTaskPlex?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=Timmoth/AptacodeTaskPlex&amp;utm_campaign=Badge_Grade)

## User Guide

### TaskCoordinator

-  The task coordinator is used to manage and apply each task.

### Code Example

```csharp

//Initialise the engine
PlexEngine plexEngine = new PlexEngine(new NullLoggerFactory(), new SystemTimerUpdater(RefreshRate.High));
plexEngine.Start();

//Creating tasks

//Move myRectangle to x position 100->200->0 over 250ms
var transformation1 = PlexFactory.Create(myRectangle, "X", TimeSpan.FromMilliseconds(250), 100, 200, 0);

//wait for 100ms
var waitTask        = PlexFactory.Wait(TimeSpan.FromMilliseconds(100));

//set myRectangle's opacity to 0.0 over 250 ms
var transformation2 = PlexFactory.Create(myRectangle, "Opacity", TimeSpan.FromMilliseconds(250), 0.0);

//Create a sequential group of the above transformations
var hideRectangle   = PlexFactory.Sequential(transformation1, waitTask, transformation2});


//Set myRectangles width & height to 50 over 100ms at the SAME time
var transformation3 = PlexFactory.Create(myRectangle, "Width", TimeSpan.FromMilliseconds(100), 50);
var transformation4 = PlexFactory.Create(myRectangle, "Height", TimeSpan.FromMilliseconds(100), 50);
var shrinkRectangle = PlexFactory.Parallel(transformation1, transformation2});

//Apply task's whenever necessary
plexEngine.Apply(hideRectangle);
plexEngine.Apply(shrinkRectangle);

//Managing running tasks

//Pause and resume all running tasks
plexEngine.Pause();
plexEngine.Resume();

//Pause & Resume a single running task
plexEngine.Pause(transformation3);
plexEngine.Resume(transformation3);

//Cancel all running tasks and restart the task coordinator
plexEngine.Restart();

//Cancel all running tasks and release all resources
plexEngine.Stop();

```

### Demos
* Find the application to run these demos in the 'WPFDemo' directory

#### SequentialTask
A group of tasks to be applied one after another
![Alt Text](https://raw.githubusercontent.com/Timmoth/Aptacode.TaskPlex/master/Resources/demos/SequentialTransformation.gif)

#### ParallelTask
A group of tasks to be applied at the same time
![Alt Text](https://raw.githubusercontent.com/Timmoth/Aptacode.TaskPlex/master/Resources/demos/ParallelTransformations.gif)

#### Composite example
An animation composed of a Parallel and Sequential groups
![Alt Text](https://raw.githubusercontent.com/Timmoth/Aptacode.TaskPlex/master/Resources/demos/ComplexTransformation.gif)
