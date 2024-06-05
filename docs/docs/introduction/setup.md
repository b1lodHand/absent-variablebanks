# Setup

In this section of the documentation, you will learn how to setup this package properly. Let's begin.

## Setup for Addressables

First of all, **I assume** that you have the Addressables package all set up before starting.

**First**, you have to find the **Addressables Groups Window**. You can open it up by: **'Window/Asset Management/Addressables/Groups'** at the toolbar above.

After opening up that window, you will see a single button on the screen (or a window like the image below if you already have it set-up). Press that button.

**Second**, right click to an empty area in that window and select **'Create New roup/Packed Assets'**. You can name it anything you want.

**Third**, drag and drop any of your folders with your **VariableBanks** inside (any bank created after in this folder after that will automatically get added, so don't worry) to the packed asset group you've just created.

**Last and the most important**, add the label **"variable-banks"** to the folders you've just dragged.

>[!TIP]
>I recommend reading more about [Addressables](https://docs.unity3d.com/Manual/com.unity.addressables.html) in order to understand the asset management better.

Now your window should look like this:


![Imgur Image](https://imgur.com/1P9GQv6.png)

As I said earlier, the folder names of the group names does not matter. **BUT** the label **MUST** be "absent-variables".

>[!INFORMATION]
>That's because the **VariableBanksCloningManager** finds the banks in asset bundles with that label, and not the folder or group names **by default**. 
>
>This tag is holded as a constant in the **Constants** class. You can find it at: **'Plugins/absencee_/absent-variablebanks/Runtime/Helpers/Constants.cs'**. 
>
>You can change the tag via changing the **K_ADDRESSABLES_TAG** property inside that file.

>[!CAUTION]
>Banks with **ForExternalUse** property true won't get cloned whether they're packed correctly with the target asset management API.

## Setup for Resources

For the Resources API, you only have one thing to do: Place your VariableBanks (the ones needs to get cloned by the internal system automatically) inside the folder: **"Resources/VariableBanks"**. The internal system will handle the rest.

>[!CAUTION]
>Banks with **ForExternalUse** property true won't get cloned whether they're packed correctly with the target asset management API.

## Setup for Third-Party Asset Management Packages

You don't have to anythin special, actually. You can just find **'Plugins/absencee_/absent-variablebanks/Runtime/Static/VariableBanksCloningManager.cs'** and modify it to fit your needs.

## What's Next?

This section is ended. Go to [Mechanism](https://b1lodhand.github.io/absent-variablebanks/docs/getting-started/mechanism.html) to continue.