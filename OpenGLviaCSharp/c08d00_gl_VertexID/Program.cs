﻿using CSharpGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace c08d00_gl_VertexID
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new FormMain());
        }
    }
}
