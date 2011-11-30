﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using CSPspEmu.Hle.Vfs;
using CSPspEmu.Core;

namespace CSPspEmu.Hle.Managers
{
	public struct ParsePathInfo
	{
		public HleIoDrvFileArg HleIoDrvFileArg;
		public string LocalPath;
		public IHleIoDriver HleIoDriver
		{
			get
			{
				return HleIoDrvFileArg.HleIoDriver;
			}
		}
	}

	public class HleIoManager : PspEmulatorComponent
	{
		protected Dictionary<string, IHleIoDriver> Drivers = new Dictionary<string, IHleIoDriver>();

		public HleUidPool<HleIoDrvFileArg> HleIoDrvFileArgPool = new HleUidPool<HleIoDrvFileArg>();

		public override void InitializeComponent()
		{
		}

		/// <summary>
		/// Returns a tuple of Driver/Index/path.
		/// </summary>
		/// <param name="FullPath"></param>
		/// <returns></returns>
		public ParsePathInfo ParsePath(string FullPath)
		{
			if (FullPath.IndexOf(':') == -1)
			{
				FullPath = CurrentDirectoryPath + "/" + FullPath;
			}
			var Match = new Regex(@"^(\w+)(\d+):(.*)$").Match(FullPath);
			var DriverName = Match.Groups[1].Value + ":";
			int FileSystemNumber = 0;
			IHleIoDriver HleIoDriver = null;
			Int32.TryParse(Match.Groups[2].Value, out FileSystemNumber);
			if (!Drivers.TryGetValue(DriverName, out HleIoDriver))
			{
				throw(new KeyNotFoundException("Can't find HleIoDriver '" + DriverName + "'"));
			}

			return new ParsePathInfo()
			{
				HleIoDrvFileArg = new HleIoDrvFileArg()
				{
					HleIoDriver = HleIoDriver,
					FileSystemNumber = FileSystemNumber,
					FileArgument = null,
				},
				LocalPath = Match.Groups[3].Value,
			};
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="Name"></param>
		/// <param name="Driver"></param>
		public void SetDriver(string Name, IHleIoDriver Driver)
		{
			//Drivers.Add(Name, Driver);
			Drivers[Name] = Driver;
			try
			{
				Driver.IoInit();
			}
			catch (Exception Exception)
			{
				Console.Error.WriteLine(Exception);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="DeviceName"></param>
		/// <returns></returns>
		public ParsePathInfo ParseDeviceName(string DeviceName)
		{
			var Match = new Regex(@"^([a-zA-Z]+)(\d*):$").Match(DeviceName);
			int FileSystemNumber = 0;
			Int32.TryParse(Match.Groups[2].Value, out FileSystemNumber);

			var BaseDeviceName = Match.Groups[1].Value + ":";

			//Drivers[
			if (!Drivers.ContainsKey(BaseDeviceName))
			{
				throw(new NotImplementedException(String.Format("Unknown device '{0}'", BaseDeviceName)));
			}

			return new ParsePathInfo()
			{
				HleIoDrvFileArg = new HleIoDrvFileArg()
				{
					HleIoDriver = Drivers[BaseDeviceName],
					FileSystemNumber = FileSystemNumber,
					FileArgument = null,
				},
				LocalPath = "",
			};
		}

		/// <summary>
		/// 
		/// </summary>
		public string CurrentDirectoryPath = "";

		/// <summary>
		/// Changes the current directory.
		/// </summary>
		/// <param name="DirectoryPath"></param>
		public void Chdir(string DirectoryPath)
		{
			CurrentDirectoryPath = DirectoryPath;
		}
	}
}
