using System;

namespace ParTech.ImageLibrary.Core.Classes
{
    public static class BitmapProperties
    {
        /// <summary>
        /// Indicates the Microsoft Windowsbitmap (BMP) format.
        /// </summary>
        public static readonly Guid ImageFormatBmp = new Guid("B96B3CAB-0728-11D3-9D7B-0000F81EF32E");

        /// <summary>
        /// Indicates the Enhanced Metafile (EMF) format.
        /// </summary>
        public static readonly Guid ImageFormatEmf = new Guid("B96B3CAC-0728-11D3-9D7B-0000F81EF32E");

        /// <summary>
        /// Indicates the Exif (Exchangeable Image File) format.
        /// </summary>
        public static readonly Guid ImageFormatExif = new Guid("B96B3CB2-0728-11D3-9D7B-0000F81EF32E");

        /// <summary>
        /// Indicates the Graphics Interchange Format (GIF) format.
        /// </summary>
        public static readonly Guid ImageFormatGif = new Guid("B96B3CB0-0728-11D3-9D7B-0000F81EF32E");

        /// <summary>
        /// Indicates the Icon format.
        /// </summary>
        public static readonly Guid ImageFormatIcon = new Guid("B96B3CB5-0728-11D3-9D7B-0000F81EF32E");

        /// <summary>
        /// Indicates the JPEG format.
        /// </summary>
        public static readonly Guid ImageFormatJpeg = new Guid("B96B3CAE-0728-11D3-9D7B-0000F81EF32E");

        /// <summary>
        /// Indicates that the image was constructed from a memory bitmap.
        /// </summary>
        public static readonly Guid ImageFormatMemoryBmp = new Guid("B96B3CAB-0728-11D3-9D7B-0000F81EF32E");

        /// <summary>
        /// Indicates the Portable Network Graphics (PNG) format.
        /// </summary>
        public static readonly Guid ImageFormatPng = new Guid("B96B3CAF-0728-11D3-9D7B-0000F81EF32E");

        /// <summary>
        /// Indicates the Tagged Image File Format (TIFF) format.
        /// </summary>
        public static readonly Guid ImageFormatTiff = new Guid("B96B3CB1-0728-11D3-9D7B-0000F81EF32E");

        /// <summary>
        /// Indicates that Windows GDI+ is unable to determine the format.
        /// </summary>
        public static readonly Guid ImageFormatUndefined = new Guid("B96B3CA9-0728-11D3-9D7B-0000F81EF32E");

        /// <summary>
        /// Indicates the Windows Metafile Format (WMF) format.
        /// </summary>
        public static readonly Guid ImageFormatWmf = new Guid("B96B3CAD-0728-11D3-9D7B-0000F81EF32E");
    }
}
