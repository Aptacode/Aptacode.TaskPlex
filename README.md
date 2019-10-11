# AptacodeTaskPlex
A simple library for changing / interpolating .Net properties over time

### Why?
I needed to synchronize the animation of .Net properties triggered by user interaction in a project I was working on. Multiple animations could be applied to a single property at any given time causing it behave erratically. 
In order to simplify the application and synchronization of transformations on properties I created TaskPlex.
I hope you find some use in it!

## User Guide

### Instantiation
```
//Initialise and start the task coordinator
TaskCoordinator taskCoordinator = new TaskCoordinator();
taskCoordinator.Start();
```

### Applying Tasks
```
//Create a DoubleTransformation on the 'With' property of 'myObject'.
//Animate the Width property from its current value to the result of the specified function.
//The function will be evaluated when the transformation is ran by the TaskCoordinator.
//The transformation will occur over 100ms and will update the property every 10ms.

  PropertyTransformation transformation = new DoubleTransformation(
      myObject,
      "Width",
      () =>
      {
          if(Orientation == Orientation.Horizontal)
            return 100;
          else
            return 50;
      },
      TimeSpan.FromMilliseconds(100),
      TimeSpan.FromMilliseconds(10));

  taskCoordinator.Apply(transformation);
  
 ```

## Built in Tasks:

### Interpolator         
- Transition from a start value to an end value over a set time with a given interval.
*Note you can set a custom easing function as shown bellow the default is 'LinearEaser'
e.g
```
//Interpolate between 10.0 -> 50.0 over 10ms updating the value every 1ms.
DoubleInterpolator exampleDoubleInterpolator = 
      new DoubleInterpolator(10.0, 50.0, TimeSpan.FromMilliseconds(10), TimeSpan.FromMilliseconds(1));
exampleDoubleInterpolator.SetEaser(new CubicInEaser());


IntInterpolator exampleIntInterpolator = 
      new IntInterpolator(10, 0, TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(10));         
  exampleIntInterpolator.SetEaser(new CubicOutEaser());

exampleDoubleInterpolator.OnValueChanged += (s, e) =>
{
    UpdatedValue = e.Value;
};

exampleDoubleInterpolator.StartAsync();
exampleInterpolator.StartAsync();
```

### PropertyTransformation
- Animate a property on a .Net object from its current value to a given end value over a given time and interval
```                
PropertyTransformation xTransformation = new DoubleTransformation(
                testObject,
                "X",
                100,
                TimeSpan.FromMilliseconds(100),
                TimeSpan.FromMilliseconds(10));
                                
PropertyTransformation widthTransformation = new IntTransformation(
                testObject,
                "Width",
                100,
                TimeSpan.FromMilliseconds(100),
                TimeSpan.FromMilliseconds(10));
                
PropertyTransformation backgroundTransformation = new ColorTransformation(
                testObject,
                "BackgroundColor",
                Color.FromARGB(255,100,150,200),
                TimeSpan.FromMilliseconds(100),
                TimeSpan.FromMilliseconds(10));
                
PropertyTransformation titleTransformation = new StringTransformation(
                testObject,
                "Title",
                "Hello, World!",
                TimeSpan.FromMilliseconds(100),
                TimeSpan.FromMilliseconds(10));
                
```

### GroupTasks
- Group together tasks to be executed sequentially or in parallel
```
GroupTask animation1 = new LinearGroupTask(new List<BaseTask>() { transformation1 , wait1, transformation3 });
GroupTask animation2 = new ParallelGroupTask(new List<BaseTask>() { transformation4, transformation5});
```

### WaitTask
-Wait for the specified amount of time
```
WaitTask wait = new WaitTask(TimeSpan.FromMilliseconds(100));
```
