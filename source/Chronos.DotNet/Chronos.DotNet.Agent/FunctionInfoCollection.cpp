#include "stdafx.h"
#include "Chronos.DotNet.Agent.Internal.h"

namespace Chronos
{
	namespace Agent
	{
		namespace DotNet
		{
			FunctionInfoCollection::FunctionInfoCollection()
			{
				_functions = new DynamicBlockFactory<FunctionInfo>(true);
			}

			FunctionInfoCollection::~FunctionInfoCollection()
			{
				__FREEOBJ(_functions);
			}

			FunctionInfo* FunctionInfoCollection::FindFunction(FunctionID functionId)
			{
				DynamicBlockFactoryEnumerator<FunctionInfo> enumerator(_functions);
				FunctionInfo* function = null;
				while ((function = enumerator.Current) != null)
				{
					if (function->FunctionId == functionId)
					{
						return function;
					}
					enumerator.MoveNext();
				}
				return null;
			}

			void FunctionInfoCollection::RemoveFunction(FunctionID functionId)
			{
				FunctionInfo* function = FindFunction(functionId);
				function->FunctionId = 0;
			}

			FunctionInfo* FunctionInfoCollection::CreateFunction(FunctionID functionId, UINT_PTR clientData, __bool hookFunction)
			{
				FunctionInfo* function = _functions->Next();
				function->FunctionId = functionId;
				function->ClientData = clientData;
				function->HookFunction = hookFunction;
				return function;
			}

		}
	}
}