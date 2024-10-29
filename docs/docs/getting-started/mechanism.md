# Mechanism

In this section of the documentation, you will learn how this tool works in general. Let's give it a go!

## What are VariableBanks?

**VariableBanks** are simple [scriptable objects](https://docs.unity3d.com/Manual/class-ScriptableObject.html). There are only two properties important. **'Guid'** and **'ForExternalUse'**. I will be covering them in detail in a second.

## How to use VariableBanks?

### Cloning
---

So, because of the intended way of using scriptable objects, the system itself clones some of the banks created (we will be covering which ones are included in that *some*) right before the **splash screen**. This cloning process is handled by **VariableBanksCloningHandler**.

>[!WARNING]
>This cloning process is **async** when using **Addressables**.

>[!INFORMATION]
>VariableBanks with **ForExternalUse** property set to true **won't get cloned and won't get shown on the list: 'VariableBankDatabase.GetBankNameList()'**.

>[!TIP]
>You can use **'VariableBanksCloningHandler.AddOnCloningCompleteCallbackOrInvoke(...)'** to get notified when the cloning process is ended, or just get notified instantly if the banks are already cloned.

### Referencing (Runtime)
---

Here, I must get into Unity's asset management procedure for clarity. When you build a game, these happen:

1. All of the files that has a **direct reference** on a game object contained in a scene gets packed with the game itself.

2. All of the files inside the **'Resouces'** folder gets packed in a single, big bundle.

3. If you're using **Addressables**, any of the assets referenced in the addressables window gets packed with its asset group (you can see asset group in Addressables window).

So, to avoid **asset duplication** you must use only of these way of referencing banks for each bank. You can use Resources for some of the banks, while using Addressables for some other (you need to implement this kind of mixed logic on your own) and etc. But you should be aware of **asset duplication** as I said.

### VariableBankReference

For this, there is a class named: **'VariableBankReference'**. It looks like a direct reference when serialized in the editor but the only thing it holds is the **Guid** of the selected variable bank. Because of this, **it does not cause duplication on build**.

You can use one of these classes to select a bank, and then in runtime, you can get its **'Bank'** property to find the cloned bank **runtime** with that guid (after ensuring the cloning process is completed successfully).

When a bank gets cloned, the **Guid remains the same on the cloned one**. That's way the system above works.

>[!CAUTION]
>Before trying to get the **'Bank'** property of a reference, ensure that the cloning process has ended successfully. If you're in editor, use **'reference.TargetGuid'** to get the original bank's guid.

>[!TIP]
> If you're using direct references and handle the cloning process independently, you can **avoid using VariableBankReference class.** Ensuring there are no asset duplications.

### VariableBank.GetInstance(...);

Or as an alternative way, you can use **'VariableBank.GetInstance(...)'** method to get a cloned bank with a specific Guid **runtime**.

>[!CAUTION]
>You cannot use this method in editor. It only works runtime. Also make sure that the cloning process has ended successfully before calling it.

### Referencing (In Editor)
---

Everything is much simpler when you're in editor. You can just call:

```c#
VariableBankDatabase.GetBankIfExists(string targetGuid);
```

and you have the bank.

>[!CAUTION]
>Remember, the system uses Guids instead of direct references to avoid any **asset duplications**. So be aware of this while writing editor code.

## How To use Manipulators (Comparers & Setters)

**Comparers** and **Setters** are you best friend when you want to manipulate any variable stored in a VariableBank **runtime**.

They are simply classes which contains a string for the **Guid** of a bank, a string for the target variable name, an enum for processing type and a value which represents the new value.

### Fixed Manipulators

**Fixed Comparers** and the **Fixed Setters** are a little different than normal ones. They don't have a bank selector in editor, instead you give them the target bank's Guid manually with the **'SetFixedBank(...)'** function. This is an example of it:

```c#
private void OnValidate() 
{
    if (Application.isPlaying) return; // Not needed.
    
    m_comparers.ForEach(comparer => comparer.SetFixedBank(m_reference.TargetGuid));
    m_setters.ForEach(setter => setter.SetFixedBank(m_reference.TargetGuid));
}
```

The code above sets the bank Guids of manipulators of a component every time a change in the inspector occurs. This is the most practical way of using fixed manipulators with components.

If you are not working with lists, you can also use **'OnEnable()'** instead of using **'OnValidate()'**

>[!TIP]
>You can also create your own manipulators via deriving from the **BaseVariableComparer** or **BaseVariableSetter** classes. You can find an example [here](https://github.com/b1lodHand/absent-dialogues/tree/main/absent-dialogues/Assets/Plugins/absencee_/absent-dialogues/Internal/Graph/Extensions). 
>
>In this example, I used **direct references**. But I mark all **BlackboardBank**s as **ForExternalUse** banks automatically on creation, so this does not cause any duplications.

## What's Next?

This section is ended. Go to [Mechanism](https://b1lodhand.github.io/absent-variablebanks/docs/getting-started/components.html) to continue.