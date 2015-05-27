# HealthBar

*HealthBar* extension for Unity. Draws in-game health bar on a transform.

Licensed under MIT license. See LICENSE file in the project root folder.   
Extensions with version below 1.0.0 are considered to be pre/alpha and may not work properly.

![HealthBar](/Resources/cover_screenshot.png?raw=true)

## Features

* Really nice looking health bar :)
* Customisable GUI settings

## Resources

* [Blog post]()

## Quick Start

1. Clone or download (with the *Download* button) the repository into the *Assets* folder.
2. Select game object in the hierarchy window and from the *Component* menu
   select *HealthBar* to add component to the selected game object.
3. Set reference to the main camera and add texture (can be all white).
4. Health bar will be visible only when health value changes. You can update health value by
   changing the `Health` property or by calling the `AssignHealthValue(int)` method.

## Help

Just create an issue and I'll do my best to help.

## Contributions

Pull requests, ideas, questions or any feedback at all are welcome.

See also: [Unity extensions as git submodules](http://wp.me/p56Vqs-6o)

## Versioning

Example: `v0.2.3f1`

- `0` Introduces breaking changes.
- `2` Major release. Adds new features.
- `3` Minor release. Bug fixes and refactoring.
- `f1` Quick fix.

## Thanks

* Big thanks to [Zero3Growlithe](https://zero3growlithe.wordpress.com/portfolio/) for making the original HealthBar component.
