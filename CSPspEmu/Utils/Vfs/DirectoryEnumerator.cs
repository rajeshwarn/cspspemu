﻿using System;

namespace CSPspEmu.Hle.Vfs
{
    internal class DirectoryEnumerator<TType> : IDisposable
    {
        int Index = -1;
        TType[] List;

        public DirectoryEnumerator(TType[] List)
        {
            this.List = List;
        }

        public bool MoveNext()
        {
            Index++;
            return GetLeft() > 0;
        }

        public TType Current => List[Index];

        public int GetLeft() => List.Length - Index;

        public void Dispose()
        {
        }
    }
}