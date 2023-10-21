# unity-simple-console
[![Unity 2021.3+](https://img.shields.io/badge/unity-2021.3.13+-blue)](https://unity3d.com/get-unity/download)
[![License: MIT](https://img.shields.io/badge/License-MIT-brightgreen.svg)](https://github.com/tayfagames/DateTimeManager/blob/master/LICENSE)

A simple Unity in-game command console.

## Installation 

Simply import the .unitypackage file in the [Releases](https://github.com/enisbt/unity-simple-console/releases/) page to your project. Console object is in the `Prefabs` folder.

## Usage

Add the `Simple Console` object in the Prefabs folder to your canvas. Console will be activate/deactivate when you press the `` ` `` key by default. This can change in the Simple Console object.

![Component Setup](https://i.imgur.com/hNK0edq.png)

Add `[ConsoleCommand]` attribute to the functions you want to use in the console.

![Console Command Setup](https://i.imgur.com/IIBuzft.png)

With this set up, `DealDamage` function can be used in the console like this: `DealDamage 15`. This command will substract 15 health from the object.

Functions can have aliases for ease of use in the console.

![Alias Setup](https://i.imgur.com/8Oy3Awv.png)

To invoke `DealDamage` function in the console you need to give `hit 15` command.

## Notes

- Only C# base data types are supported to use as parameters. For more information: https://learn.microsoft.com/en-us/dotnet/api/system.convert?view=net-7.0
- This asset requires `TextMeshPro` to work.

## License

Distributed under the MIT License. See `LICENSE` for more information.
