# Before Installing

In this section of the documentation, you will learn how to set up this package properly and get it ready to use. Let's start.

First of all, this package needs at least one **asset management** (for loading/unloading variablebanks) package in order to work properly. By default, it supports [Addressables](https://docs.unity3d.com/Manual/com.unity.addressables.html) and Unity's default package [Resources](https://docs.unity3d.com/Manual/UnderstandingPerformanceResourcesFolder.html).

Because of this, there are **three ways** of setting up this package:

1.  Setup for [Addressables](https://docs.unity3d.com/Manual/com.unity.addressables.html) (This is the way recommended for production).
2. Setup for [Resources](https://docs.unity3d.com/Manual/UnderstandingPerformanceResourcesFolder.html) (this is the way recommended for fast-prototyping).
3. Setup for any other **third-party** asset management package (or maybe your own package for it).

For the **first two** ways, there are seperate files in every release for both of them. You can pick the right one for you while installing the package.

But in **any of these ways**, the only thing you that changes is the **VariableBanksCloningHandler** class actually. 

So, if you are interested in the **third** way, you can just install any of the **.unitypackage** file in the release you've selected and override that class to work with your needs.

## What's Next?

This section is ended. Go to [Installing](https://b1lodhand.github.io/absent-variablebanks/docs/introduction/installing.html) to continue.