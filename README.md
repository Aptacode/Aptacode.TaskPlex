
<p align="center">
  <img width="640" height="320" src="https://raw.githubusercontent.com/Timmoth/Aptacode.TaskPlex/master/Resources/Images/TaskPlexBanner.png">
</p>

TaskPlex is a lightweight cross platform .net tweening library with a goal of simplifying the creation and use of complex animations.

NuGet package:

https://www.nuget.org/packages/Aptacode.TaskPlex/

[![Codacy Badge](https://api.codacy.com/project/badge/Grade/d25f0cea83384aacada81fa9790679c8)](https://www.codacy.com/manual/Timmoth/AptacodeTaskPlex?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=Timmoth/AptacodeTaskPlex&amp;utm_campaign=Badge_Grade)

## User Guide

### TaskCoordinator

-  The task coordinator is used to manage and apply each task.

### Code Example

```csharp

//Initialise the task coordinator
TaskCoordinator taskCoordinator = new TaskCoordinator();
taskCoordinator.Start();

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

//Apply task's whenever necessary
taskCoordinator.Apply(hideRectangle);
taskCoordinator.Apply(shrinkRectangle);

//Managing running tasks

//Pause and resume all running tasks
taskCoordinator.Pause();
taskCoordinator.Resume();

//Pause & Resume a single running task
taskCoordinator.Pause(transformation3);
taskCoordinator.Resume(transformation3);

//Cancel all running tasks and restart the task coordinator
taskCoordinator.Restart();

//Cancel all running tasks and release all resources
taskCoordinator.Stop();

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


### Easers
Easer functions are used to determine the rate at which a transformations value changes. The graphs below show the rate at which an interpolator affected by each easer approaches its end value over time.

<p align="center">
  <img width="700" height="600" src="https://raw.githubusercontent.com/Timmoth/Aptacode.TaskPlex/master/Resources/Images/easers.png">
</p>

### Easer demos

#### Linear

![Alt Text](https://raw.githubusercontent.com/Timmoth/Aptacode.TaskPlex/master/Resources/demos/Linear.gif)

#### EaseIn

![Alt Text](https://raw.githubusercontent.com/Timmoth/Aptacode.TaskPlex/master/Resources/demos/EaseIn.gif)

#### EaseOut

![Alt Text](https://raw.githubusercontent.com/Timmoth/Aptacode.TaskPlex/master/Resources/demos/EaseOut.gif)

#### EaseInOut

![Alt Text](https://raw.githubusercontent.com/Timmoth/Aptacode.TaskPlex/master/Resources/demos/EaseInOut.gif)

#### Elastic

![Alt Text](https://raw.githubusercontent.com/Timmoth/Aptacode.TaskPlex/master/Resources/demos/Elastic.gif)


