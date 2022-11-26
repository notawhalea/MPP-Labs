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
**THIRD LAB**:kissing_closed_eyes:

**DIRECTORY SCANNER**

[Link on technical task](https://bsuir.ishimko.me/mpp-dotnet/3-directory-scanner)

Here i implement a graphical utility using WPF to analyze the size ratio of files and directories within a selected directory in multi-threaded mode.

**File and directory size analysis**

* Analysis of the size of files and directories must be performed in multi-threaded mode using the system's thread pool and queue.
* Each directory is processed on a separate thread. Processing includes summing the sizes of nested files and queuing all nested directories for similar processing.
* The maximum number of involved threads should be limited (it can be a constant in the code) without changing the settings of the system thread pool (ThreadPool.SetMaxThreads is not allowed).

The thread processing a directory does not need to wait for all nested directories to be processed, but only queue their processing. Otherwise, if the nesting level is high, the threads will sit idle waiting for threads running for nested directories to finish.

Also, when the limit on the number of involved threads is set to a value that is less than the level of nesting of directories in the scanned directory, the program may "hang" due to infinite waiting (mutual blocking), when all threads are busy waiting for the completion of traversal of nested directories, for which there are no threads to start processing. remains.

To solve this problem, the recalculation of the sizes of directories, taking into account nested directories, must be performed separately, after the sizes of all files have been calculated.
**********
**FIFTH LAB**:astonished:

**STRING FORMATTER**

[Link on technical task](https://bsuir.ishimko.me/mpp-dotnet/5-string-formatter)

Here i implement a StringFormatter class with a single Format method that should do a simplified "string interpolation".

For simplified access to a ready-made formatter instance, it is recommended to declare a static field in the class implementation with the created "default" instance

Full string interpolation has been available since C# 6.0 and allows you to perform in-place substitution of variables, fields, properties, and expressions in string literals:
int a = 2021;
string = "spp";
string result = $"{s.ToUpper()}-{a+1}"; // SPP-2022

In this lab, you need to implement a simplified string interpolation, when only the fields and properties of the passed object are substituted.

Expressions should only be compiled once and stored as delegates.
The cache must be thread-safe because the Format method can be called simultaneously from multiple threads.
The operation of the Formatter should be fully verified with unit tests. The use of an auxiliary console program for this is prohibited.
**********
I just relax and gained experience here.

[Link on technical task for all labs](https://bsuir.ishimko.me/mpp-dotnet)

Still in progress? Convert to draft!:wink:

>And no one ever knows.She's on my mind when I'm on yours, oh.I'm not here for games.I told you what it is, you chose to stay, oh.Baby, you chose the pain.Cause you don't know me, you just know my name.:microphone:Renegade - Aaryan Shah
