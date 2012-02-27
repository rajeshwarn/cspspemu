﻿using CSPspEmu.Hle.Formats;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using CSPspEmu.Hle.Vfs.Iso;
using CSharpUtils.Extensions;

namespace CSPspEmu.Core.Tests
{
	[TestClass()]
	public class IsoTest
	{
		[TestMethod()]
		public void IsoConstructorTest()
		{
			var CsoName = "../../../TestInput/test.cso";
			var Cso = new Cso(File.OpenRead(CsoName));
			var Iso = new IsoFile(new CompressedIsoProxyStream(Cso), CsoName);
			var ContentNode = Iso.Root.Locate("path/content.txt");
			var Lines = ContentNode.Open().ReadAllContentsAsString().Split('\n');
			foreach (var Line in Lines)
			{
				Iso.Root.Locate(Line);
			}
		}
	}
}
