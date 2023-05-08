﻿/**
 * Main file for client
 * Copyright Florian Pfeiffer
 * Author Florian Pfeiffer
 **/

using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System.Drawing;
using VoxPopuliLibrary.client.network;

namespace Vox_Populi
{
    public static class Program
    {
        public static VoxPopuliLibrary.client.Window? window;
        public static NativeWindowSettings nativeWindowSettings = new NativeWindowSettings()
        {
                Size = new Vector2i(1000, 800),
                Title = "VoxPopuli",
                Flags = ContextFlags.ForwardCompatible,
        };
        private static void Main()
        {
            using (window = new VoxPopuliLibrary.client.Window(GameWindowSettings.Default, nativeWindowSettings))
            {
                window.VSync = VSyncMode.On;
                window.Run();
            }
            Network.DeConnect();
        }
    }
}