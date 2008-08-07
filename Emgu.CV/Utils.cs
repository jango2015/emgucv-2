using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;

namespace Emgu.CV
{
    /// <summary>
    /// Utilities class
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// The ColorPalette of Grayscale for Bitmap Format8bppIndexed
        /// </summary>
        public static readonly System.Drawing.Imaging.ColorPalette GrayscalePalette = GenerateGrayscalePalette();

        private static System.Drawing.Imaging.ColorPalette GenerateGrayscalePalette()
        {
            using (Bitmap image = new Bitmap(1, 1, System.Drawing.Imaging.PixelFormat.Format8bppIndexed))
            {
                System.Drawing.Imaging.ColorPalette palette = image.Palette;
                for (int i = 0; i < 256; i++)
                {
                    palette.Entries[i] = System.Drawing.Color.FromArgb(i, i, i);
                }
                return palette;
            }
        }

        /*
        public static void GetModuleInfo()
        {
                        IntPtr version = IntPtr.Zero;
        IntPtr plugin_info = IntPtr.Zero;
        CvInvoke.cvGetModuleInfo(0, ref version, ref plugin_info);
        return (plugin_info != 0 && strstr(plugin_info, "ipp") != 0);
        }
        */ 

        /// <summary>
        /// Enable or diable IPL optimization for opencv
        /// </summary>
        /// <param name="enable">true to enable optimization, false to disable</param>
        public static void OptimizeCV(bool enable)
        {            
            CvInvoke.cvUseOptimized(enable);
        }

        /// <summary>
        /// Determine if a small convex polygon is inside a larger convex polygon
        /// </summary>
        /// <typeparam name="T">The type of the depth</typeparam>
        /// <param name="smallPolygon">the smaller polygon</param>
        /// <param name="largePolygon">the larger polygon</param>
        /// <returns>
        /// true if the small convex polygon is inside the larger convex polygon;
        /// false otherwise.
        /// </returns>
        public static bool IsConvexPolygonInConvexPolygon<T>(IConvexPolygon<T> smallPolygon, IConvexPolygon<T> largePolygon) where T : IComparable, new()
        {
            Point2D<T>[] vertices = smallPolygon.Vertices;
            return Array.TrueForAll<Point2D<T>>(vertices, delegate(Point2D<T> v) { return v.InConvexPolygon(largePolygon); });
        }

        /// <summary>
        /// Calculates disparity for stereo-pair 
        /// </summary>
        /// <param name="leftImage">Left image of stereo pair, rectified grayscale 8-bit image</param>
        /// <param name="rightImage">Right image of stereo pair, rectified grayscale 8-bit image</param>
        /// <param name="maxDisparity">Maximum possible disparity. The closer the objects to the cameras, the larger value should be specified here. Too big values slow down the process significantly</param>
        /// <returns></returns>
        public static Image<Gray, Byte> FindStereoCorrespondence(Image<Gray, Byte> leftImage, Image<Gray, Byte> rightImage, int maxDisparity)
        {
            Image<Gray, Byte> disparity = new Image<Gray, byte>(leftImage.Width, leftImage.Height);
            CvInvoke.cvFindStereoCorrespondence(
                leftImage.Ptr,
                rightImage.Ptr,
                0, //CV_DISPARITY_BIRCHFIELD
                disparity,
                maxDisparity,
                25,
                5,
                12,
                15,
                25);
            return disparity;
        }
    }
}
