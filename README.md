# Simple Localization
Lightweight string translation.

## Setup
### Dictionary Loading
To load a dictionary, create a `LocalizationDictionary`.

Call `SetActive()` on it to apply it as the active language dicionary.

### Language change event
You can react to when the active dictionary is changed by subscribing to `Localization.LanguageUpdated`.

## Usage
Create a serialized `LocalizedString` field for every to-be-translated string.

The key contained in it can have **named arguments** in this format: `{{apple count}}`.
You can use **formatting** to replace them with any string.

### Applying without formatting
When there are no arguments to replace, apply the text with the implicit conversion:
```cs
label.text = myLocalizedString;
```

### Applying with formatting
Use the `Format` function to replace arguments with values.

If there is only one argument, you can call `Format` with a single `object` parameter:
```cs
// The key is something like "You have {{apple count}} apples".
label.text = myLocalizedString.Format(appleCount);
```

If there are multiple arguments, you can pass a `Dictionary<string, string`.
```cs
// The key is something like "You have {{apple count}} apples and {{banana count}} bananas.".
label.text = myLocalizedString.Format(new()
{
    { "apple count", appleCount },
    { "banana count", bananaCount }
});
```
**Please note** that you shouldn't constantly create dictionaries and produce garbage,
so only use this option if you expect the arguments to almost never change.
In any _other_ case, please consider using an `AutoTranslation` instance.

### AutoTranslation
The `AutoTranslation` class can be used to automatically update a text whenever
either the language or one/some of the arguments change.

The consructor takes in an `Action<string>` that is invoked when the language changes or `Apply()` is called.
```cs
translation = new AutoTranslation(localizedString, s => label.text = s, true).Apply();
```
Note: `Apply()` returns the object to allow for method call chaining as seen above.<br/>
Note: The boolean parameter in the constructor sets `UpdateOnLanguageChange`.
If this property is `true`, the object will `Apply` whenever the active language changes.

The `Apply()` method becomes more important when there are arguments involved.<br/>
The `AutoTranslation` instance stores arguments in order to be able
to instantly apply its string when the language changes.<br/>
Thus, the dictionary that is used for formatting is stored inside the instance
and is updated through `SetArgument(string, string)`.<br/>
To allow for batch-updating argument values, `Apply()` must be called
after updating arguments.<br/>
```cs
translation = new AutoTranslation(localizedString, s => label.text = s, true)
    .SetArgument("apple count", appleCount)
    .SetArgument("banana count", bananaCount)
    .Apply();
```
```cs
translation.SetArgument("apple count", appleCount)
    .SetArgument("banana count", bananaCount)
    .Apply();
```

Note: These call chains are safe because the `AutoTranslation` has an internal `dirty` flag.<br/>
`Apply()` will re-use the previous output if previous argument updates didn't set that flag.

#### Dispose
`Dispose()` must be called to have the `AutoTranslation` unsubscribe from the language changed event.

## Installation

In Unity: **Window > Package Manager > + > Add package from git URL**, then enter:

```
https://github.com/tea-spoons/simple-localization.git
```

Pin a release by appending a tag, for example `#v0.5.0`.

## Change plan

See [CHANGE-PLAN.md](CHANGE-PLAN.md) for what changed before publishing and what is planned next.

## License

Copyright (c) 2026 Bigpoint. Authored by Muhammad Tarek Abdou.

Available for research, education and other noncommercial use under the [PolyForm Noncommercial 1.0.0](LICENSE.md)
license. Commercial use is not permitted.
