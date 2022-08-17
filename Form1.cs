using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CsRead_WriteProcessMemory
{
    public partial class Form1 : Form
    {
        public IntPtr basemodul = IntPtr.Zero;
        public int PID;
        #region DllImport
        [DllImport("Kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);
        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(IntPtr hHandle);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint dwSize, out UIntPtr lpNumberOfBytesRead);

        #endregion
        public Form1()
        {
            InitializeComponent();
        }
        public void GetProcess(string name)
        {
            var processName = Process.GetProcesses();
            if (processName.Count() != 0)
            {
                foreach (var process in processName)
                {
                    if (process.ProcessName == name)
                    {
                        PID = process.Id;

                        return;
                    }
                   
                }
            }
            return;
        }
        
        public void WriteByte()
        {
            
            var BaseAddress = 0x00860438;
            byte[] ammobyte = { 0x20, 0x00 };
            var size = ammobyte.Length;
            var dum = new UIntPtr();
            var handle = OpenProcess(0x001F0FFF, false, PID);
            WriteProcessMemory(handle,(IntPtr)BaseAddress, ammobyte, (uint)size, out dum);
            CloseHandle(handle);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            GetProcess("ac_client");
            WriteByte();
            
        }
    }
}
