﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSPspEmu.Hle.Vfs;

namespace CSPspEmu.Hle.Modules.iofilemgr
{
	unsafe public partial class IoFileMgrForUser
	{
		/// <summary>
		/// Open or create a file for reading or writing (asynchronous)
		/// </summary>
		/// <param name="FileName">Pointer to a string holding the name of the file to open</param>
		/// <param name="Flags">Libc styled flags that are or'ed together</param>
		/// <param name="Mode">File access mode.</param>
		/// <returns>A non-negative integer is a valid fd, anything else an error</returns>
		[HlePspFunction(NID = 0x89AA9906, FirmwareVersion = 150)]
		public int sceIoOpenAsync(string FileName, HleIoFlags Flags, SceMode Mode)
		{
			var FileHandle = sceIoOpen(FileName, Flags, Mode);
			var File = HleState.HleIoManager.HleIoDrvFileArgPool.Get(FileHandle);
			File.AsyncLastResult = FileHandle;
			return FileHandle;
		}

		/// <summary>
		/// Reposition read/write file descriptor offset (asynchronous)
		/// </summary>
		/// <param name="fd">Opened file descriptor with which to seek</param>
		/// <param name="offset">Relative offset from the start position given by whence (32 bits)</param>
		/// <param name="whence">
		///		Set to SEEK_SET to seek from the start of the file, SEEK_CUR
		///		seek from the current position and SEEK_END to seek from the end.
		/// </param>
		/// <returns>
		///		Less than 0 on error.
		///		Actual value should be passed returned by the ::sceIoWaitAsync call.
		/// </returns>
		[HlePspFunction(NID = 0x1B385D8F, FirmwareVersion = 150)]
		public int sceIoLseek32Async(SceUID fd, uint offset, int whence)
		{
			throw (new NotImplementedException());
			/*
			SceOff offsetAfterSeek = sceIoLseek(fd, offset, whence);
			auto fileHandle = uniqueIdFactory.get!FileHandle(fd);
			fileHandle.lastOperationResult = cast(long)offsetAfterSeek;
			return 0;
			*/
		}

		/// <summary>
		/// Reposition read/write file descriptor offset (asynchronous)
		/// </summary>
		/// <param name="fd">Opened file descriptor with which to seek</param>
		/// <param name="offset">Relative offset from the start position given by whence</param>
		/// <param name="whence">
		///		Set to SEEK_SET to seek from the start of the file, SEEK_CUR
		///		seek from the current position and SEEK_END to seek from the end.
		/// </param>
		/// <returns>
		///		Less than 0 on error.
		///		Actual value should be passed returned by the ::sceIoWaitAsync call.
		/// </returns>
		[HlePspFunction(NID = 0x71B19E77, FirmwareVersion = 150)]
		public int sceIoLseekAsync(SceUID fd, SceOff offset, int whence)
		{
			throw(new NotImplementedException());
			/*
			SceOff offsetAfterSeek = sceIoLseek(fd, offset, whence);
			auto fileHandle = uniqueIdFactory.get!FileHandle(fd);
			fileHandle.lastOperationResult = cast(long)offsetAfterSeek;
			return 0;
			*/
		}

		/// <summary>
		/// Delete a descriptor (asynchronous)
		/// </summary>
		/// <param name="FileHandle">File descriptor to close</param>
		/// <returns>Less than 0 on error</returns>
		[HlePspFunction(NID = 0xFF5940B6, FirmwareVersion = 150)]
		public int sceIoCloseAsync(int FileHandle)
		{
			var File = HleState.HleIoManager.HleIoDrvFileArgPool.Get(FileHandle);
			File.AsyncLastResult = 0;
			sceIoClose(FileHandle);

			//HleState.HleIoManager.HleIoDrvFileArgPool.Remove(FileHandle);
			
			return 0;
		}

		/// <summary>
		/// Read input (asynchronous)
		/// </summary>
		/// <example>
		/// bytes_read = sceIoRead(fd, data, 100);
		/// </example>
		/// <param name="FileHandle">Opened file descriptor to read from</param>
		/// <param name="OutputPointer">Pointer to the buffer where the read data will be placed</param>
		/// <param name="OutputSize">Size of the read in bytes</param>
		/// <returns>
		///		Less than 0 on error.
		///	</returns>
		[HlePspFunction(NID = 0xA0B5A7C2, FirmwareVersion = 150)]
		public int sceIoReadAsync(int FileHandle, byte* OutputPointer, int OutputSize)
		{
			var File = HleState.HleIoManager.HleIoDrvFileArgPool.Get(FileHandle);
			File.AsyncLastResult = sceIoRead(FileHandle, OutputPointer, OutputSize);
			return 0;
		}

		/// <summary>
		/// Change the priority of the asynchronous thread.
		/// </summary>
		/// <param name="FileHandle">The opened fd on which the priority should be changed.</param>
		/// <param name="Priority">The priority of the thread.</param>
		/// <returns>Less than 0 on error.</returns>
		[HlePspFunction(NID = 0xB293727F, FirmwareVersion = 150)]
		public int sceIoChangeAsyncPriority(SceUID FileHandle, int Priority)
		{
			throw(new NotImplementedException());
			/*
			unimplemented_notice();
			//return -1;
			return 0;
			*/
		}

		/*
		public int _sceIoWaitAsyncCB(SceUID fd, SceInt64* res, bool callbacks)
		{
			logInfo("_sceIoWaitAsyncCB(fd=%d, callbacks=%d)", fd, callbacks);
			FileHandle fileHandle = uniqueIdFactory.get!FileHandle(fd);
			*res = fileHandle.lastOperationResult;
			if (callbacks) {
				hleEmulatorState.callbacksHandler.executeQueued(currentThreadState);
			}
			currentRegisters.LO = fd;
			return fd;
		}
		*/

		/// <summary>
		/// Wait for asyncronous completion.
		/// </summary>
		/// <param name="fd">The file descriptor which is current performing an asynchronous action.</param>
		/// <param name="res">The result of the async action.</param>
		/// <returns>The given fd or a negative value on error.</returns>
		[HlePspFunction(NID = 0xE23EEC33, FirmwareVersion = 150)]
		public int sceIoWaitAsync(SceUID fd, long* res)
		{
			throw(new NotImplementedException());
			//return _sceIoWaitAsyncCB(fd, res, false);
		}

		/// <summary>
		/// Wait for asyncronous completion.
		/// </summary>
		/// <param name="fd">The file descriptor which is current performing an asynchronous action.</param>
		/// <param name="res">The result of the async action.</param>
		/// <returns>The given fd or a negative value on error.</returns>
		[HlePspFunction(NID = 0x35DBD746, FirmwareVersion = 150)]
		public int sceIoWaitAsyncCB(SceUID fd, long* res)
		{
			throw (new NotImplementedException());
			//return _sceIoWaitAsyncCB(fd, res, true);
		}

		/// <summary>
		/// Poll for asyncronous completion.
		/// </summary>
		/// <param name="FileHandle">The file descriptor which is current performing an asynchronous action.</param>
		/// <param name="Result">The result of the async action.</param>
		/// <returns>
		///		Return 1 on busy.
		///		Return 0 on ready.
		/// </returns>
		[HlePspFunction(NID = 0x3251EA56, FirmwareVersion = 150)]
		public int sceIoPollAsync(int FileHandle, long* Result)
		{
			var File = HleState.HleIoManager.HleIoDrvFileArgPool.Get(FileHandle);
			*Result = File.AsyncLastResult;

			return 0;
		}

		/// <summary>
		/// Write output (asynchronous)
		/// </summary>
		/// <param name="fd">Opened file descriptor to write to</param>
		/// <param name="data">Pointer to the data to write</param>
		/// <param name="size">Size of data to write</param>
		/// <returns>Less than 0 on error</returns>
		[HlePspFunction(NID = 0x0FACAB19, FirmwareVersion = 150)]
		public int sceIoWriteAsync(SceUID fd, void* data, SceSize size)
		{
			throw(new NotImplementedException());
			/*
			unimplemented();
			return -1;
			*/
		}
	}
}