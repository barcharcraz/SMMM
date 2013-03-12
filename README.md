SMMM
====
Sane Minecraft Mod Manager

SMMM is the result of frustration with current minecraft mod managers.
Because minecraft mods typically modify the minecraft bytecode in order to function and in particular load
they can be organised in any way the author sees fit. For example if I wanted to write I minecraft mod that loaded
requiered art assets from C:\somepathwithstrangeunicode\art I could do that.

This causes problems with mod management software as mods can be packed in a variety of wildly different ways.
Furthermore current mod management software does not make it clear what happens when you install a mod and where
the files actually go. Because of this it is hard to tell if the manager installed the mod correctly.

SMMM installs mods be following a user defined set of actions, such as move-file, rename, etc that can be specified
in a config file or by the user using a tree-based drag-n-drop interface.


This project was also a chance for me to become more comfortable with windows development in .net.
I ended up learning how to interact with XML from .net using linq, as well as how to use WPF.

Honestly if I rewrote this today I would probably use powershell scripts to install mods and include
a few default scripts with the application instead of using the System.net.IO classes and confguring
the install behavior with XML. Also I would use mvvm all the way through as I only really realized the
rationale behaind it half way through this project.
