﻿using System;
using CSPspEmu.Core.Cpu;
using CSPspEmu.Hle.Interop;

namespace CSPspEmu.Hle
{
    public sealed class HleCallback : IHleUidPoolClass, IDisposable
    {
        public string Name { get; private set; }
        public uint Function { get; private set; }
        public object[] Arguments { get; private set; }
        public Action ExecutedNotify;

        private HleCallback()
        {
        }

        public HleCallback Clone()
        {
            return new HleCallback()
            {
                Name = Name,
                Function = Function,
                Arguments = Arguments,
                ExecutedNotify = ExecutedNotify,
            };
        }

        public HleCallback PrependArguments(params object[] Arguments)
        {
            this.Arguments = Arguments.Concat(this.Arguments);
            return this;
        }

        public static HleCallback Create(string Name, uint Function, params object[] Arguments)
        {
            return new HleCallback() {Name = Name, Function = Function, Arguments = Arguments};
        }

        public void SetArgumentsToCpuThreadState(CpuThreadState CpuThreadState)
        {
            HleInterop.SetArgumentsToCpuThreadState(CpuThreadState, Function, Arguments);
        }

        public void Dispose()
        {
        }

        public override string ToString()
        {
            return $"HleCallback(Name='{Name}', Function=0x{Function:X})";
        }
    }
}