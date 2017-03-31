#include "stdafx.h"
#include "Chronos.Agent.h"

namespace Chronos
{
	namespace Agent
	{
		UnitBase::UnitBase()
			: _beginLifetime(0), _endLifetime(0)
		{
			Uid = 0;
			Id = 0;
			_name = null;
		}

		UnitBase::~UnitBase()
		{
			__FREEOBJ(_name);
		}

		void UnitBase::Initialize(__uint uid, __uptr id, __uint beginLifetime)
		{
			Uid = uid;
			Id = id;
			_beginLifetime = beginLifetime;
		}

		void UnitBase::Close(__uint endLifetime)
		{
			_endLifetime = endLifetime;
		}
		
		__uint UnitBase::GetBeginLifetime()
		{
			return _beginLifetime;
		}

		__uint UnitBase::GetEndLifetime()
		{
			return _endLifetime;
		}

		__string* UnitBase::GetName()
		{
			return _name;
		}

		__bool UnitBase::GetIsAlive()
		{
			return _endLifetime <= 0;
		}
	}
}