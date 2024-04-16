## ImageSorter

This is a tool to sort through a lot of images at a time.

All you need to do is to open a folder full of images, and then press various keys on the keyboard to send the images to corresponding subfolders.

Once you press the key and the image is moved, you automatically get presented with the next image.

By default, there's only a preset for "spacebar" moving images to a subfolder called "NSFW" (the original purpose of this program was to simply sort out images of such character from piles of random images), but new keys are as easily added as needed, simply by pressing something on your keyboard that produces a character.

The "produces a character" part is more or less the only limitation - it can pretty much bind to anything that you can type in as one character (as long as it fits into UTF-16, I guess).

That means not only different actions for upper/lowercase, but also different alphabets, or even dead key entry methods such as umlauts.

The program in action:

![screenshot](https://raw.githubusercontent.com/htmlcoderexe/ImageSorter/main/screenshot.png)

The key presets are saved every time anything changes, in the folder currently worked on. This means closing and reopening the application will restore the bindings, as well as that each folder can have its own set, customised for a specific purpose - once you've sorted everything into generic "screenshots", "memes", "photos" and whatever else, you can open the subfolders with this program and get specific.

It is also possible to copy a particularly useful bindings file to the same directory as you're running the EXE from - it will use those as default. This may become a feature - allow to overwrite the default preset with current preset.

If you accidentally pressed the wrong button and sent an image the wrong way, you can now undo this with the ``Ctrl+Z`` shortcut. Actions can be undone as far back as the beginning of the specific folder session - quitting the program or opening a different folder clears the undo history.

Pressing F2 now allows to specify a custom filename to be used for the image when it gets moved to target folder - as a consequence of this, the program checks if a file by that name already exists in that folder, attempts to create a useable filename and offers the user the option of using that as the destination filename to complete the move.

F5 reloads the folder but keeps progress and Undo states.
