using System;
using System.Runtime.InteropServices;
using System.Diagnostics;

public class SignEX
{
	IntPtr handle;
	public SignEX(string ProcessName) { handle = GetHandle(ProcessName); }
	public SignEX(int pID) { handle = OpenProcess(2035711, false, pID); }
	public SignEX(IntPtr pHandle) { handle = pHandle; }
    public SignEX(Process process) { handle = process.Handle; }
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
    private unsafe int SignScan(byte[] bytes_scan) 
    {
        MEMORY_BASIC_INFORMATION zero = new MEMORY_BASIC_INFORMATION();
        int baseAddress = 0, num = 0;
		byte* numArray;
		while (baseAddress <= Int32.MaxValue)
        {
            VirtualQueryEx(handle, (IntPtr)baseAddress, out zero, (uint)Marshal.SizeOf(typeof(MEMORY_BASIC_INFORMATION)));
			if (baseAddress != (int)zero.BaseAddress + (int)zero.RegionSize)
			{
				baseAddress = (int)zero.BaseAddress + (int)zero.RegionSize;
				try
				{
                    numArray = (byte*)Marshal.AllocHGlobal((int)zero.RegionSize);
                    //numArray = new byte[(int)zero.RegionSize];
				}
				catch { return -1; }
				ReadProcessMemory(handle, zero.BaseAddress, numArray, (int)zero.RegionSize, out _);
				int x = -1;
				while (++x < (int)zero.RegionSize)
					if (numArray[x] == bytes_scan[0])
						for (int i = 0; i < bytes_scan.Length; i++)
							if (numArray[i + x] != bytes_scan[i])
							{
								num = 0;
								break;
							}
							else
							{
								num++;
								if (num == bytes_scan.Length)
									return (int)zero.BaseAddress + x;
							}
			}
			else break;
        }
        return -1;
    }

	/// <summary>
	/// Returns True or False whether code was injected or not
	/// </summary>
	/// <param name="bytes_scan"></param>
	/// <param name="bytes_write"></param>
	/// <returns></returns>
	public bool WriteBytes(byte[] bytes_scan, byte[] bytes_write, bool forAll) 
    {
		int num = SignScan(bytes_scan);
        if (num != -1)
			return WriteProcessMemory(handle, (IntPtr)num, bytes_write, (uint)bytes_write.Length, out _);
		return false;
    }
	
	#region kernel32
	[DllImport("kernel32.dll", CharSet = CharSet.None, ExactSpelling = false, SetLastError = true)]
    private static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out UIntPtr lpNumberOfBytesWritten);
	[DllImport("kernel32.dll", CharSet = CharSet.None, ExactSpelling = false)]
	public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);
	[DllImport("kernel32.dll", CharSet = CharSet.None, ExactSpelling = false, SetLastError = true)]
	private unsafe static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [Out] byte* lpBuffer, int dwSize, out int lpNumberOfBytesRead);
	[DllImport("kernel32.dll", CharSet = CharSet.None, ExactSpelling = false, SetLastError = true)]
    private static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, IntPtr lpBuffer, uint nSize, UIntPtr lpNumberOfBytesWritten);
    private void WriteProcessMemory(IntPtr handle, IntPtr addr, object bytes_write, uint length, out UIntPtr dammy)
    {
        throw new NotImplementedException();
    }
	[DllImport("kernel32.dll", CharSet = CharSet.None, ExactSpelling = false)]
	private static extern int VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress, out MEMORY_BASIC_INFORMATION lpBuffer, uint dwLength);
	#endregion
}
