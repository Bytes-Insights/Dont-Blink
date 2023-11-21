using System;
using System.Runtime.InteropServices;

public class FacialLandmarksPlugin
{
    // --- STRUCT DEFINITIONS ---

    [StructLayout(LayoutKind.Sequential)]
    public struct Result
    {
        public bool successful;
        public ulong size;
        public ulong faceAmount;
        public IntPtr data;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct OutPoint
    {
        public int x;
        public int y;
    }

    // --- NATIVE METHODS ---

    [DllImport("FacialLandmarksPlugin")]
    public static extern int TestMethod();
    [DllImport("FacialLandmarksPlugin")]
    public static extern void InitializeDLIB(string s);
    [DllImport("FacialLandmarksPlugin")]
    public static extern Result UpdateFacialLandmarks(byte[] bytes, int width, int height);

    // Prevent instantiation.
    private FacialLandmarksPlugin()
    {
    }
}
