﻿using CSharpUtils;
using CSPspEmu.Core.Cpu.VFpu;
using SafeILGenerator.Ast.Generators;
using SafeILGenerator.Ast.Nodes;
using System;

namespace CSPspEmu.Core.Cpu.Emitter
{
    public sealed partial class CpuEmitter
	{
		public static uint _vt4444_step(uint i0, uint i1)
		{
			uint o0 = 0;
			o0 |= ((i0 >> 4) & 15) << 0;
			o0 |= ((i0 >> 12) & 15) << 4;
			o0 |= ((i0 >> 20) & 15) << 8;
			o0 |= ((i0 >> 28) & 15) << 12;
			o0 |= ((i1 >> 4) & 15) << 16;
			o0 |= ((i1 >> 12) & 15) << 20;
			o0 |= ((i1 >> 20) & 15) << 24;
			o0 |= ((i1 >> 28) & 15) << 28;
			//Console.Error.WriteLine("{0:X8} {1:X8} -> {2:X8}", i0, i1, o0);
			//throw(new Exception("" + i0 + ";" + i1));
			return o0;
		}

		public static uint _vt5551_step(uint i0, uint i1)
		{
			uint o0 = 0;
			o0 |= ((i0 >> 3) & 31) << 0;
			o0 |= ((i0 >> 11) & 31) << 5;
			o0 |= ((i0 >> 19) & 31) << 10;
			o0 |= ((i0 >> 31) & 1) << 15;
			o0 |= ((i1 >> 3) & 31) << 16;
			o0 |= ((i1 >> 11) & 31) << 21;
			o0 |= ((i1 >> 19) & 31) << 26;
			o0 |= ((i1 >> 31) & 1) << 31;
			//throw (new Exception("" + i0 + ";" + i1));
			return o0;
		}

		public static uint _vt5650_step(uint i0, uint i1)
		{
			uint o0 = 0;
			o0 |= ((i0 >> 3) & 31) << 0;
			o0 |= ((i0 >> 10) & 63) << 5;
			o0 |= ((i0 >> 19) & 31) << 11;
			o0 |= ((i1 >> 3) & 31) << 16;
			o0 |= ((i1 >> 10) & 63) << 21;
			o0 |= ((i1 >> 19) & 31) << 27;
			//throw (new Exception("" + i0 + ";" + i1));
			return o0;
		}

		private AstNodeStm _vtXXXX_q(Func<uint, uint, uint> _vtXXXX_stepCallback)
		{
			var VectorSize = Instruction.ONE_TWO;
			if (VectorSize != 4) throw (new Exception("Not implemented _vtXXXX_q for VectorSize=" + VectorSize));
			var Dest = _Vector(VD_NoPrefix, VUInt, 2);
			var Src = _Vector(VS_NoPrefix, VUInt, 4);
			//AstLoadVfpuReg

			var Node = Dest.SetVector((Index) =>
			{
				return ast.CallStatic(
					_vtXXXX_stepCallback,
					Src[Index * 2 + 0],
					Src[Index * 2 + 1]
				);
			});

			//throw(new Exception(GeneratorCSharp.GenerateString<GeneratorCSharp>(Node)));

			return Node;
		}

		public AstNodeStm vt4444_q() { return _vtXXXX_q(_vt4444_step); }
		public AstNodeStm vt5551_q() { return _vtXXXX_q(_vt5551_step); }
		public AstNodeStm vt5650_q() { return _vtXXXX_q(_vt5650_step); }
	}
}
