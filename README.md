# AptacodeTaskPlex
A library for changing .Net properties over time

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
