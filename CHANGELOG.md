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
- "Merge images from same Pdf page" is now unchecked by default
- The "N images for M pages!" warning now also appears when the images were merged back into pages (it says "Merged into N images"), so sliced-up PDFs are always flagged
- Only `.cbr`, `.cbz` and `.pdf` files are picked up; other files (including `.rar` and `.zip`, which the original also converted) are ignored
- New, cleaner layout: Source / Output / Convert / Progress sections, a proper Start button, and the window can be dragged to any size
- Drag a file or folder onto the window (or onto the .exe) to select it
- Output is now "Same folder as source" or "Other folder", with a Browse button
- The confusing "P/Img" checkbox is gone; "Merge sliced images back into whole pages" now sits under the PDF options, which it belongs to
- New outputs: CBR/CBZ → image folder and PDF → image folder (alongside CBR/CBZ → PDF and PDF → CBZ; several can be ticked at once)
- Removed "Compress images to reduce file size": images are always kept at their original resolution and quality
- Folders are no longer processed recursively: only the files directly inside the selected folder are converted
- New "Subfolders → CBZ" option: each subfolder of the selected folder is treated as one comic, and its images are packed into a CBZ named after it; subfolders without images are skipped with a warning
- Fixed: pages in different subfolders of an archive (e.g. one folder per chapter) with the same file name overwrote each other, losing pages
- Images that can't go in a PDF (e.g. .webp) are reported in the log instead of silently left out
- Fixed: pressing Stop during CBR/CBZ → PDF left a half-written PDF behind
- Progress shows "File 3 of 12"; the "Overall" bar moves steadily and counts only the files that will actually be converted (it used to count only PDFs)
- The log shows only warnings and errors, with the time and type; warnings and errors are colour-coded; a line under the column headings; a Clear button
- Fixed: one unreadable CBR/CBZ stopped a whole folder conversion; now it's logged and the next file is converted
- Fixed: clicking the link in the About window crashed
- Settings are locked while a conversion runs, so they can't be changed halfway through
- Typo fixes ("process recursively", "ouput")
- GitHub Actions builds a ready-to-run Windows .exe on every push
