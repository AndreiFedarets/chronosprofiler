#include "stdafx.h"	
#include "Chronos.Agent.h"

namespace Chronos
{
	namespace Agent
	{
		/*
			 __
			|__|_____1b Data Marker
			|  |
			|  |
			|  |
			|__|_____4b Data Size
			|  |
			|  |
			|  |
			~~~~

			~~~~
			|  |
			|  |
			|__|_____Nb Data

		*/

		GatewayPackage::GatewayPackage(GatewayPackage* package)
		{
			//get size of source buffer
			__uint bufferSize = package->GetBufferSize();
			//allocate memory for our buffer
			_bufferBegin = new __byte[bufferSize];
			//init end of the buffer
			_bufferEnd = _bufferBegin + bufferSize;
			//get source buffer pointer
			__byte* buffer = package->GetBuffer();
			//copy source buffer to our buffer
			memcpy(_bufferBegin, buffer, bufferSize);
			//init pointer to data size in our buffer
			_dataSizePointer = (__uint*)(_bufferBegin + Marshaler::ByteSize);

			_staticPackage = package->_staticPackage;
			if (_staticPackage)
			{
				_cursor = _bufferBegin + HeaderSize;
			}
			else
			{
				*_dataSizePointer = package->GetDataSize();
				_cursor = _bufferBegin + *_dataSizePointer;
			}
			_bufferPageSize = package->_bufferPageSize;
		}

		GatewayPackage::GatewayPackage(__byte dataMarker)
		{
			Initialize(dataMarker, MemoryStream::PageDefaultSize, false);
		}
		
		GatewayPackage::GatewayPackage(__byte dataMarker, __uint bufferSize)
		{
			Initialize(dataMarker, bufferSize, true);
		}

		GatewayPackage::~GatewayPackage()
		{
			__FREEOBJ(_bufferBegin);
		}

		__uint GatewayPackage::Write(void* data, __uint size)
		{
			if (_staticPackage)
			{
				__ASSERT(false, L"GatewayPackage::Write: attempt to write to static package");
				return 0;
			}
			//resize buffer if cursor is out of buffer
			if (_cursor + size > _bufferEnd)
			{
				Resize(size);
			}
			//write data to current position (cursor)
			memcpy(_cursor, data, size);
			//move cursor
			_cursor += size;
			//update data size in header
			*_dataSizePointer += size;
			return size;
		}

		__bool GatewayPackage::Initialized()
		{
			return true;
		}
		
		__uint GatewayPackage::GetDataSize()
		{
			__uint dataSize;
			if (_staticPackage)
			{
				dataSize = *_dataSizePointer;
			}
			else
			{
				dataSize = _cursor - _bufferBegin;
			}
			return dataSize;
		}
		
		void GatewayPackage::SetDataSize(__uint dataSize)
		{
			if (_staticPackage)
			{
				*_dataSizePointer = dataSize;
			}
			else
			{
				_cursor = _bufferBegin + dataSize;
			}
		}

		__uint GatewayPackage::GetBufferSize()
		{
			__uint bufferSize = _bufferEnd - _bufferBegin;
			return bufferSize;
		}

		__uint GatewayPackage::GetPayloadSize()
		{
			return GetDataSize() + HeaderSize;
		}

		__byte* GatewayPackage::GetBuffer()
		{
			return _bufferBegin;
		}

		__byte* GatewayPackage::GetData()
		{
			return _bufferBegin + HeaderSize;
		}

		void GatewayPackage::Resize(__uint needSize)
		{
			//each resize we double page size
			_bufferPageSize += _bufferPageSize;
			//get current buffer size
			__uint bufferSize = GetBufferSize();
			//increate buffer size on resize-step
			if (needSize > bufferSize + _bufferPageSize)
			{
				bufferSize = needSize + _bufferPageSize;
			}
			else
			{
				bufferSize = bufferSize + _bufferPageSize;
			}

			//get current data size
			__uint dataSize = GetDataSize();
			//reallocate buffer
			_bufferBegin = (__byte*)realloc(_bufferBegin, bufferSize);
			//calculate end buffer value
			_bufferEnd = _bufferBegin + bufferSize;
			//update cursor position: begin of new buffer + dataSize
			_cursor = _bufferBegin + dataSize;
			//update pointer on data size in header
			_dataSizePointer = (__uint*)(_bufferBegin + Marshaler::ByteSize);
		}

		void GatewayPackage::Initialize(__byte dataMarker, __uint bufferSize, __bool staticPackage)
		{
			_staticPackage = staticPackage;
			_bufferPageSize = MemoryStream::PageDefaultSize;
			//append package header size to whole buffer size
			bufferSize += HeaderSize;
			//allocate buffer
			_bufferBegin = new __byte[bufferSize];
			//clear memory
			memset(_bufferBegin, 0, bufferSize);
			//calculate end buffer value
			_bufferEnd = _bufferBegin + bufferSize;
			//set data marker
			*_bufferBegin = dataMarker;
			//setup cursor position (right after header)
			_cursor = _bufferBegin + HeaderSize;
			//get pointer on data size in header
			_dataSizePointer = (__uint*)(_bufferBegin + Marshaler::ByteSize);
			//update data size in header according package type
			if (staticPackage)
			{
				*_dataSizePointer = bufferSize;
			}
			else
			{
				*_dataSizePointer = 0;
			}
		}

		GatewayPackage* GatewayPackage::CreateClone(GatewayPackage* package)
		{
			return new GatewayPackage(package);
		}

		GatewayPackage* GatewayPackage::CreateDynamic(__byte dataMarker)
		{
			return new GatewayPackage(dataMarker);
		}

		GatewayPackage* GatewayPackage::CreateStatic(__byte dataMarker, __uint dataSize)
		{
			return new GatewayPackage(dataMarker, dataSize);
		}

		void GatewayPackage::ReadPackage(IStream* stream, __byte* dataMarker, __byte** data, __uint* dataSize)
		{
			__ASSERT(HeaderSize <= Marshaler::LongSize, L"GatewayPackage::ReadPackage: HeaderSize is bigger that sizeof(__long)");
			//use long as header - long size is 8 bytes, header size is 5 bytes
			__long temp = 0;
			__byte* header = (__byte*)&temp;
			//DataMarker is 1 byte with offset 0 in the header
			__byte* headerDataMarkerPointer = header;
			//DataSize is 4 bytes with offset 1 in the header
			__uint* headerDataSizePointer = (__uint*)(header + Marshaler::ByteSize);
			__uint readBytes = stream->Read(header, HeaderSize);
			//looks like stream was closed - we received empty data block, just ignore it
			if (readBytes == 0)
			{
				__ASSERT(true, L"GatewayPackage::ReadPackage: empty header");
				*dataMarker = 0;
				*data = null;
				*dataSize = 0;
				return;
			}
			__ASSERT(readBytes == HeaderSize, L"GatewayPackage::ReadPackage: actual size of header is not equal expected (HeaderSize)");
			//set value to dataMarker out parameter
			*dataMarker = *headerDataMarkerPointer;
			//set value to dataMarker out parameter
			*dataSize = *headerDataSizePointer;
			if (*dataSize == 0)
			{
				__ASSERT(true, L"GatewayPackage::ReadPackage: empty data");
				return;
			}
			if (*dataSize > MaxDataSize)
			{
				__ASSERT(true, L"GatewayPackage::ReadPackage: too big data");
				return;
			}
			//set value to data out parameter
			*data = new __byte[*dataSize];
			readBytes = stream->Read(*data, *dataSize);
			__ASSERT(readBytes == *dataSize, L"GatewayPackage::ReadPackage: actual size of data is not equal expected");
		}

		const __uint GatewayPackage::HeaderSize = Marshaler::ByteSize + Marshaler::IntSize;
		const __uint GatewayPackage::MaxDataSize = 3 * 1024 * 1024;
	}
}