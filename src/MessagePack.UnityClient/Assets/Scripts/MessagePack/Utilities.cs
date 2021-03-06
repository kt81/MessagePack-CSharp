﻿// Copyright (c) All contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Buffers;
using MessagePack.Formatters;
using Microsoft;
using Nerdbank.Streams;

namespace MessagePack
{
    /// <summary>
    /// Internal utilities and extension methods for various external types.
    /// </summary>
    internal static class Utilities
    {
        internal delegate void GetWriterBytesAction<TArg>(ref MessagePackWriter writer, TArg argument);

        internal static byte[] GetWriterBytes<TArg>(TArg arg, GetWriterBytesAction<TArg> action)
        {
            using (var sequenceRental = SequencePool.Shared.Rent())
            {
                var writer = new MessagePackWriter(sequenceRental.Value);
                action(ref writer, arg);
                writer.Flush();
                return sequenceRental.Value.AsReadOnlySequence.ToArray();
            }
        }
    }
}
