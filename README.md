
<p align="center">
  <img width="640" height="320" src="https://raw.githubusercontent.com/Timmoth/Aptacode.TaskPlex/master/Resources/Images/TaskPlexBanner.png">
</p>

A simple library for changing / interpolating .Net properties over time

NuGet package:

https://www.nuget.org/packages/Aptacode.TaskPlex/

[![Codacy Badge](https://api.codacy.com/project/badge/Grade/d25f0cea83384aacada81fa9790679c8)](https://www.codacy.com/manual/Timmoth/AptacodeTaskPlex?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=Timmoth/AptacodeTaskPlex&amp;utm_campaign=Badge_Grade)

## Why
I needed to synchronize the animation of properties triggered by user interaction in a project I was working on. Multiple animations could be applied to a single property at any given time causing it to behave erratically. 
In order to simplify the application and synchronization of transformations on properties I created TaskPlex.

I hope you find some use in it!

## User Guide

### TaskCoordinator


-  The task coordinator determines in which order tasks are executed.

```csharp

//Initialise the task coordinator
TaskCoordinator taskCoordinator = new TaskCoordinator();

//Create tasks

//Move myRectangle to x position 100 over 250ms, wait for 100ms then set myRectangle's opacity to 0.0 over 250 ms
var transformation1 = TaskFactory.Create(myRectangle, "X", 100, TimeSpan.FromMilliseconds(250));
var waitTask        = TaskFactory.Wait(TimeSpan.FromMilliseconds(100));
var transformation2 = TaskFactory.Create(myRectangle, "Opacity", 0.0, TimeSpan.FromMilliseconds(250));
var hideRectangle   = TaskFactory.Sequential(transformation1, transformation2});

//Set myRectangles width & height to 50 over 100ms at the SAME time
var transformation3 = TaskFactory.Create(myRectangle, "Width", 50, TimeSpan.FromMilliseconds(100));
var transformation4 = TaskFactory.Create(myRectangle, "Height", 50, TimeSpan.FromMilliseconds(100));
var shrinkRectangle = TaskFactory.Parallel(transformation1, transformation2});

...

//Apply task's whenever necessary
taskCoordinator.Apply(hideRectangle);
taskCoordinator.Apply(shrinkRectangle);

...

//Clean up
taskCoordinator.Dispose();

```

### Easers
<p align="center">
  <img width="700" height="600" src="https://raw.githubusercontent.com/Timmoth/Aptacode.TaskPlex/master/Resources/Images/easers.png">
</p>
