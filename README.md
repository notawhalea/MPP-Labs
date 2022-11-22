# HELLO WORLD! :earth_africa:
**********
Modern Platfors of Programming BSUIR(СПП БГУИР 5 сем)

It's my *vision* in some task.
**********
**What I've done**:question:

This is my solutions of five labs.
**********
**FIRST LAB**:heart_eyes:

**TRACER**

[Link on technical task](https://bsuir.ishimko.me/mpp-dotnet/1-tracer)

Here i implement a method execution time meter using the StackTrace system class.

The total execution time of the analyzed methods in one thread should also be calculated. To do this, it is enough to calculate the sum of the times of the "root" methods called from the thread.
The trace results of nested methods should be presented in the appropriate place in the results tree.
For time measurements, you should use the Stopwatch class.

The measurement result must be presented in three formats: JSON, XML and YAML. When implementing plugins, you should use ready-made libraries for working with these formats.
**********
**SECOND LAB**:frowning:

**FAKER**

[Link on technical task](https://bsuir.ishimko.me/mpp-dotnet/2-faker)

Here i implement an object generator with random test data.

When creating an object, you should use the constructor, and also fill in public fields and properties with public setters that were not filled in the constructor. Consider scenarios where a class has only a private constructor, multiple constructors, a constructor with parameters, and public fields/properties.
If there are multiple constructors, the one with more parameters should be preferred, but if an exception occurs while trying to use it, the others should be tried.
Note that user-defined value types, which are structures declared with the struct keyword, always have a parameterless constructor, but a user-defined constructor can be declared in addition to it (which should be tried first, guided by the logic of preference). constructor with many parameters).
The padding must be recursive (if the field is another object, then it must also be created using Faker).

Creating collections should be done in the same way as creating other types that have generators.
**********
I just relax and gained experience here.

[Link on technical task for all labs](https://bsuir.ishimko.me/mpp-dotnet)

Still in progress? Convert to draft!:wink:
**********
