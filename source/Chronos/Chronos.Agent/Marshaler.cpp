#include "stdafx.h"
#include "Chronos.Agent.h"

namespace Chronos
{
	namespace Agent
	{
		void Marshaler::MarshalBool(__bool value, IStreamWriter* stream)
		{
			stream->Write(&value, BoolSize);
		}

		void Marshaler::MarshalByte(__byte value, IStreamWriter* stream)
		{
			stream->Write(&value, ByteSize);
		}

		void Marshaler::MarshalInt(__int value, IStreamWriter* stream)
		{
			stream->Write(&value, IntSize);
		}

		void Marshaler::MarshalUInt(__uint value, IStreamWriter* stream)
		{
			stream->Write(&value, IntSize);
		}

		void Marshaler::MarshalLong(__long value, IStreamWriter* stream)
		{
			stream->Write(&value, LongSize);
		}

		void Marshaler::MarshalULong(__ulong value, IStreamWriter* stream)
		{
			stream->Write(&value, LongSize);
		}

		void Marshaler::MarshalSize(__size value, IStreamWriter* stream)
		{
			stream->Write(&value, SizeSize);
		}
		
		void Marshaler::MarshalString(__string* value, IStreamWriter* stream)
		{
			if (value == null)
			{
				MarshalUInt(0, stream);
			}
			else
			{
				__wchar* nativeValue = const_cast<__wchar*>(value->c_str());
				__uint size = (__uint)((value->size()) * CharSize);
				MarshalUInt(size, stream);
				stream->Write(nativeValue, size);
			}
		}

		void Marshaler::MarshalGuid(__guid* value, IStreamWriter* stream)
		{
			stream->Write(value, GuidSize);
		}

		void Marshaler::MarshalBuffer(Buffer* value, IStreamWriter* stream)
		{
			MarshalInt(value->Size, stream);
			if (value->Size > 0)
			{
				stream->Write(value->Data, value->Size);
			}
		}
		
		__bool Marshaler::DemarshalBool(IStreamReader* stream)
		{
			__bool value = false;
			stream->Read(&value, BoolSize);
			return value;
		}

		__byte Marshaler::DemarshalByte(IStreamReader* stream)
		{
			__byte value = 0;
			stream->Read(&value, ByteSize);
			return value;
		}

		__int Marshaler::DemarshalInt(IStreamReader* stream)
		{
			__int value = 0;
			stream->Read(&value, IntSize);
			return value;
		}

		__uint Marshaler::DemarshalUInt(IStreamReader* stream)
		{
			__uint value = 0;
			stream->Read(&value, IntSize);
			return value;
		}

		__long Marshaler::DemarshalLong(IStreamReader* stream)
		{
			__long value = 0;
			stream->Read(&value, LongSize);
			return value;
		}
		
		__ulong Marshaler::DemarshalULong(IStreamReader* stream)
		{
			__ulong value = 0;
			stream->Read(&value, LongSize);
			return value;
		}

		__size Marshaler::DemarshalSize(IStreamReader* stream)
		{
			__size value = 0;
			stream->Read(&value, SizeSize);
			return value;
		}

		__string Marshaler::DemarshalString(IStreamReader* stream)
		{
			__uint size = DemarshalUInt(stream);
			__uint valueLength = size / CharSize;
			valueLength += 1; // 1 for zero-wchar
			__uint valueBufferSize = valueLength * CharSize;
			__wchar* valueBuffer = new __wchar[valueLength];
			memset(valueBuffer, 0, valueBufferSize);
			stream->Read(valueBuffer, size);
			__string value(valueBuffer);
			__FREEARR(valueBuffer);
			return value;
		}

		__guid Marshaler::DemarshalGuid(IStreamReader* stream)
		{
			__guid value;
			stream->Read(&value, GuidSize);
			return value;
		}

		Buffer* Marshaler::DemarshalBuffer(IStreamReader* stream)
		{
			__int size = DemarshalInt(stream);
			Buffer* value = new Buffer(size);
			if (size > 0)
			{
				stream->Read(value->Data, value->Size);
			}
			return value;
		}

		const __uint Marshaler::BoolSize = sizeof(__bool);
		const __uint Marshaler::ByteSize = sizeof(__byte);
		const __uint Marshaler::CharSize = sizeof(__wchar);
		const __uint Marshaler::IntSize = sizeof(__int);
		const __uint Marshaler::SizeSize = sizeof(__size);
		const __uint Marshaler::LongSize = sizeof(__long);
		const __uint Marshaler::GuidSize = sizeof(__guid);
	}
}