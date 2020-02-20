
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

//Add tasks as they occur
taskCoordinator.Apply(transformation1);
...
taskCoordinator.Apply(transformation2);

//Clean up
taskCoordinator.Dispose();

```

### Tasks


-  A task is a unit of work to be executed over a duration.
Each task has an started and finished event.
'OnStarted' is triggered when the task coordinator decides to run the task.
'OnFinished' is triggered just after the task finishes.

```csharp

//Create a DoubleTransformation on the 'Width' property of 'myObject'.
//Transform the Width property from its current value to the 10.5
//The transformation will occur over 100ms

  var transformation = new DoubleTransformation(
      myObject,
      "Width",
      10.5,
      TimeSpan.FromMilliseconds(100));

//When applied the TaskCoordinator checks if there are currently no running tasks which conflict with the given task
//If there is a confliction the given task is added to a list of 'Pending tasks' and executed when there are no coflicts

  taskCoordinator.Apply(transformation);
  
 ```

## Built in Tasks

### PropertyTransformation


-  Incrementally transition a property from its initil value to the specified end value.

```csharp

PropertyTransformation xTransformation = new DoubleTransformation(
                testObject,
                "X",
                100,
                TimeSpan.FromMilliseconds(100));
                                
PropertyTransformation widthTransformation = new IntTransformation(
                testObject,
                "Width",
                100,
                TimeSpan.FromMilliseconds(100),
                RefreshRate.Highest);
                
PropertyTransformation backgroundTransformation = new ColorTransformation(
                testObject,
                "BackgroundColor",
                Color.FromARGB(255,100,150,200),
                TimeSpan.FromMilliseconds(100));
                
PropertyTransformation titleTransformation = new StringTransformation(
                testObject,
                "Title",
                "Hello, World!",
                TimeSpan.FromMilliseconds(100));
                
```

### GroupTasks


-  Group together tasks to be executed sequentially or in parallel

```csharp

GroupTask animation1 = new SequentialGroupTask(new List<BaseTask>() { transformation1 , wait1, transformation3 });

GroupTask animation2 = new ParallelGroupTask(new List<BaseTask>() { transformation4, transformation5});

```

### WaitTask


-  Wait for the specified amount of time

```csharp

WaitTask wait = new WaitTask(TimeSpan.FromMilliseconds(100));

```
