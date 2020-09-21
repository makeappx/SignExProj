using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Collections.Generic;

public class SignEX
{
	IntPtr handle;
    Process proc;
    const int maxsize = int.MaxValue >> 1;
	public SignEX(string ProcessName) { proc = GetProcess(ProcessName); handle = GetHandle(ProcessName); }
	public SignEX(int pID) { proc = Process.GetProcessById(pID); handle = OpenProcess(2035711, false, pID); }
    public SignEX(Process process) { proc = process; handle = process.Handle; }
	public struct MEMORY_BASIC_INFORMATION
	{
		public IntPtr BaseAddress;
		public IntPtr AllocationBase;
		public uint AllocationProtect;
		public IntPtr RegionSize;
		public uint State;
		public uint Protect;
		public uint Type;
	}
	private IntPtr GetHandle(string name) 
    {
		Process[] p = Process.GetProcessesByName(name);
		return p.Length > 0 ? p[0].Handle : (IntPtr)(-1);
    }
    private Process GetProcess(string name)
    {
        Process[] p = Process.GetProcessesByName(name);
        return p.Length > 0 ? p[0] : null;
    }
    public List<long> SignScan(byte[] bytes_scan) 
    {
        List<long> result = new List<long>();
        MEMORY_BASIC_INFORMATION zero = new MEMORY_BASIC_INFORMATION();
        long baseAddress = 0; int num = 0;
		List<byte[]> numArray;
        while (baseAddress <= (long)proc.Modules[proc.Modules.Count - 1].BaseAddress)
        {
            VirtualQueryEx(handle, (IntPtr)baseAddress, out zero, (uint)Marshal.SizeOf(typeof(MEMORY_BASIC_INFORMATION)));
            baseAddress = (long)zero.BaseAddress + (long)zero.RegionSize;
            if (zero.Type == 0 || zero.Protect == 1 || zero.Protect == 2 || zero.Protect == 0x100 ||
                zero.State == 0x10000 || zero.State == 0x2000) continue;
            int bytesnum = (int)Math.Ceiling((double)zero.RegionSize / maxsize);
            numArray = new List<byte[]>();
            for (int i = 0; i < bytesnum; i++)
            {
                if (i == bytesnum - 1)
                {
                    numArray.Add(new byte[(long)zero.RegionSize % maxsize + 1]);
                    ReadProcessMemory(handle, zero.BaseAddress + i * maxsize, numArray[i], (long)zero.RegionSize % maxsize, out _);
                }
                else
                {
                    numArray.Add(new byte[maxsize]);
                    ReadProcessMemory(handle, zero.BaseAddress + i * maxsize, numArray[i], maxsize, out _);
                }
            }
            long x = -1;
            while (++x < (long)zero.RegionSize)
                if (numArray[(int)(x / maxsize)][x % maxsize] == bytes_scan[0])
                    for (int i = 0; i < bytes_scan.Length; i++)
                        if (numArray[(int)((i + x) / maxsize)][(i + x) % maxsize] != bytes_scan[i])
                        {
                            num = 0;
                            break;
                        }
                        else
                        {
                            num++;
                            if (num == bytes_scan.Length)
                            {
                                result.Add((long)zero.BaseAddress + x);
                                num = 0;
                                break;
                            }
                        }
        
        }
        return result;
    }
    public bool WriteBytes(IntPtr address, byte[] bytes_write)
    {
        return WriteProcessMemory(handle, address, bytes_write, (uint)bytes_write.Length, out _);
    }
    public byte[] CheckValue(IntPtr address, int size)
    {
        byte[] buffer = new byte[size];
        ReadProcessMemory(handle, address, buffer, size, out _);
        return buffer;
    }
	
	#region kernel32
	[DllImport("kernel32.dll", CharSet = CharSet.None, ExactSpelling = false, SetLastError = true)]
    private static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out UIntPtr lpNumberOfBytesWritten);
	[DllImport("kernel32.dll", CharSet = CharSet.None, ExactSpelling = false)]
	public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);
	[DllImport("kernel32.dll", CharSet = CharSet.None, ExactSpelling = false, SetLastError = true)]
	private static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [Out] byte[] lpBuffer, long dwSize, out int lpNumberOfBytesRead);
	[DllImport("kernel32.dll", CharSet = CharSet.None, ExactSpelling = false, SetLastError = true)]
    private static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, IntPtr lpBuffer, uint nSize, UIntPtr lpNumberOfBytesWritten);
	[DllImport("kernel32.dll", CharSet = CharSet.None, ExactSpelling = false)]
	private static extern int VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress, out MEMORY_BASIC_INFORMATION lpBuffer, uint dwLength);
	#endregion
}
