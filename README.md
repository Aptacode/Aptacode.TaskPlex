# AptacodeTaskPlex
A simple library for changing / interpolating .Net properties over time

### Why?
I needed to synchronize the animation of .Net properties triggered by user interaction in a project I was working on. Multiple animations could be applied to a single property at any given time causing it behave erratically. 
In order simplify the application and synchronization of transformations on properties I created TaskPlex.
I hope you find some use in it!

## User Guide

### Instantiation
```
TaskCoordinator taskCoordinator = new TaskCoordinator();
taskCoordinator.Start();
```

### Applying Tasks
```
  PropertyTransformation transformation = new IntInterpolation(
      myObject,
      myObject.GetType().GetProperty("Width"),
      () =>
      {
          return 10;
      },
      TimeSpan.FromMilliseconds(100));
  transformation.Interval = TimeSpan.FromMilliseconds(10);

  taskCoordinator.Apply(transformation);
  
 ```
