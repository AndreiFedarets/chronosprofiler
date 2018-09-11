#include "stdafx.h"
#include "Chronos.Agent.h"

namespace Chronos
{
	namespace Agent
	{
#pragma warning(push) 
#pragma warning(disable:4244)
		void UnitMarshaler::MarshalUnit(IUnit* unit, IStreamWriter* stream)
		{
			//Uid
			Marshaler::MarshalUInt(unit->Uid, stream);
			//Id
			Marshaler::MarshalULong(unit->Id, stream);
			//BeginLifetime
			Marshaler::MarshalUInt(unit->GetBeginLifetime(), stream);
			//EndLifetime
			Marshaler::MarshalUInt(unit->GetEndLifetime(), stream);
			//Name
			Marshaler::MarshalString(unit->GetName(), stream);
		}
#pragma warning(pop)
	}
}