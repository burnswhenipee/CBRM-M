# Changelog

Running notes for the README's "What's new" section, to be written up when the modernization is done.

## Fixed: data-loss bug (important for anyone using the original)

The original program could permanently delete a folder in your output location.

When converting a file such as `Comic.cbz`, it unpacked the images into a working folder named `Comic` next to the output file. If a folder with that name already existed there, the program deleted it and everything in it without asking, then deleted its own working folder when done. Existing output files (`Comic.pdf` / `Comic.cbz`) were also overwritten silently.

Now:
- Images are unpacked into a private folder in the Windows temp directory, which is cleaned up afterward. Nothing in your folders is touched except the new output file.
- If the output file already exists, the new one is saved as `Comic (1).pdf`, `Comic (2).pdf`, etc., and the log says so.

## Other changes so far

- Ported to .NET 8; updated SharpCompress (0.26 → 0.50.7) and iTextSharp (5.5.13.4); replaced the deprecated DotNetZip with built-in zip support
- Window is resizable and maximizable; the log is always visible and fills the remaining space
- Removed the show/hide log arrow
- Long log messages can be read by widening the window or scrolling sideways; hover a row for the full message; Ctrl+A / Ctrl+C copy messages
- Standard Windows file and folder dialogs (separate File... and Folder... buttons) that reopen at the last location used (Windows also remembers it between sessions)
- Picking a source no longer resets the conversion options; options that don't apply are greyed out instead
- The "N images for M pages!" warning now also appears when the images were merged back into pages (it says "Merged into N images"), so sliced-up PDFs are always flagged
- `.rar` and `.zip` files are treated as comics, like `.cbr` / `.cbz`
- Typo fixes ("process recursively", "ouput")
- GitHub Actions builds a ready-to-run Windows .exe on every push

## Known issues still to fix

- When converting a folder, the "Total" progress bar only counts PDFs, so it's wrong for folders containing CBR/CBZ files
