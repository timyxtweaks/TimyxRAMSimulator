using System;
using System.Windows.Forms;

namespace TimyxRAMSimulator
{
    internal static class Program
    {
        /// <summary>
        /// Glavna tacka ulaska u aplikaciju
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new GlavnaForma());
        }
    }
}