# VSUtils

This repository contains utils that helps you to work in Visual Studio 2022

## Projects

- [ReplaceItemTemplates](#ReplaceItemTemplates)

## Description

### <span id="ReplaceItemTemplates">ReplaceItemTemplates</span>

*[Getting started to Visual Studio templates](https://learn.microsoft.com/en-us/visualstudio/ide/creating-project-and-item-templates?view=vs-2022)*

<!--- cSpell:words recustomize --->

After each update Visual Studio resets item templates 👎  
So if you customized them once, you need to recustomize them after each update ☠️

This project helps you automate item templates customizing 👍  
You need only to run this project instead of replacing item templates one by one 🎯

#### Notes:

- Only Class and Interface templates are provided by default;
- You can customize Class and Interface templates in the [Template](./ReplaceItemTemplates/Templates/) folder;
- You can add your own templates to the [Template](./ReplaceItemTemplates/Templates/) folder;
- You can use your own templates in the [appsettings.json](./ReplaceItemTemplates/appsettings.json) file (use existing configuration as an example).
