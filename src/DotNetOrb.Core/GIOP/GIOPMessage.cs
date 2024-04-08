// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Compression;
using DotNetty.Buffers;
using DotNetty.Common;
using DotNetty.Common.Utilities;

namespace DotNetOrb.Core.GIOP
{
    public class GIOPMessage : IByteBufferHolder
    {

        public ORB ORB { get; }
        public GIOPHeader Header { get; protected set; }
        public CompressorIdLevel Compressor { get; }

        protected CompositeByteBuffer content;

        public IByteBuffer Content
        {
            get
            {
                if (content.ReferenceCount <= 0)
                {
                    throw new IllegalReferenceCountException(content.ReferenceCount);
                }

                return content;
            }
        }

        public int ReferenceCount => content.ReferenceCount;

        public GIOPMessage(ORB orb)
        {
            ORB = orb;
            Compressor = new CompressorIdLevel()
            {
                CompressorId = 0,
                CompressionLevel = 0
            };
            content = PooledByteBufferAllocator.Default.CompositeBuffer();
        }

        public GIOPMessage(ORB orb, IByteBuffer content)
        {
            ORB = orb;
            Header = new GIOPHeader(content);
            Compressor = new CompressorIdLevel()
            {
                CompressorId = 0,
                CompressionLevel = 0
            };
            this.content = PooledByteBufferAllocator.Default.CompositeBuffer();
            this.content.AddComponent(content);
            this.content.SetWriterIndex(this.content.WriterIndex + content.ReadableBytes);
        }

        public virtual void InsertMsgSize()
        {
            content.SetInt(8, content.WriterIndex - GIOPHeader.HeaderSize);
        }

        public IByteBufferHolder Copy() => Replace(content.Copy());

        public IByteBufferHolder Duplicate() => Replace(content.Duplicate());

        public IByteBufferHolder RetainedDuplicate() => Replace(content.RetainedDuplicate());

        public virtual IByteBufferHolder Replace(IByteBuffer content) => new DefaultByteBufferHolder(content);

        public IReferenceCounted Retain()
        {
            content.Retain();
            return this;
        }

        public IReferenceCounted Retain(int increment)
        {
            content.Retain(increment);
            return this;
        }

        public IReferenceCounted Touch()
        {
            content.Touch();
            return this;
        }

        public IReferenceCounted Touch(object hint)
        {
            content.Touch(hint);
            return this;
        }

        public bool Release() => content.Release();

        public bool Release(int decrement) => content.Release(decrement);

        public virtual void Write(IByteBuffer buf)
        {
            InsertMsgSize();
            buf.WriteBytes(content, 0, content.WriterIndex);
        }
    }
}
