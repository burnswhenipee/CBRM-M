# Third-party notices

The source code in this repository is under the MIT License (see `LICENSE`).

The ready-to-run `CBRM-M.exe` also contains the libraries below. Most are under permissive licenses, but **iTextSharp is under the GNU Affero General Public License v3 (AGPL-3.0)**, so the `.exe` as a whole is distributed under the terms of the AGPL-3.0. The complete source code for it is this repository. If you build the program yourself without iTextSharp, only the permissive licenses apply.

| Library | Version | License | Used for |
|---|---|---|---|
| [iTextSharp](https://github.com/itext/itextsharp) | 5.5.13.4 | AGPL-3.0 (full text in `LICENSES/AGPL-3.0.md`) | Reading and writing PDFs |
| [BouncyCastle.Cryptography](https://github.com/bcgit/bc-csharp) | 2.4.0 | MIT | Used by iTextSharp for encrypted PDFs |
| [PdfPig](https://github.com/UglyToad/PdfPig) | 0.1.16 | Apache-2.0 | Reading PDFs that iTextSharp can't open |
| [SharpCompress](https://github.com/adamhathcock/sharpcompress) | 0.50.7 | MIT | Reading CBR/CBZ archives |
| [.NET runtime](https://github.com/dotnet/runtime) and [Windows Forms](https://github.com/dotnet/winforms) | 8.0 | MIT | Included in the self-contained .exe |
