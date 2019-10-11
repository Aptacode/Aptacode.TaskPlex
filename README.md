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

### Built in Tasks:
- IntInterpolation          - Animate between a integer property value and a given destination value over a set time
- DoubleInterpolation       - Animate between a double property value and a given destination value over a set time
- StringTransformation      - Change a string property to a given value at a set time
- WaitTask                  - Wait for the specified time
- LinearGroupTask           - Chain together a set of Tasks such that the completion of the first task triggers the start of the next and so on
- ParallelGroupTask         - Run a set of tasks in parallel
