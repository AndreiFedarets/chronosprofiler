#include "stdafx.h"
#include "Chronos.DotNet.Agent.Internal.h"

namespace Chronos
{
	namespace Agent
	{
		namespace DotNet
		{
			FunctionExceptionTracer::FunctionExceptionTracer(RuntimeProfilingEvents* profilingEvents, FunctionInfoCollection* functions)
			{
				_profilingEvents = profilingEvents;
				_functions = functions;
				_currentException = 0;
				_currentFunction = null;
				_catcherFunction = null;
				_exceptionStack = null;
			}

			FunctionExceptionTracer::~FunctionExceptionTracer(void)
			{
				__FREEOBJ(_exceptionStack);
			}

			void FunctionExceptionTracer::ExceptionThrown(ObjectID exceptionId)
			{
				_currentException = exceptionId;
				if (_exceptionStack != null)
				{
					__FREEOBJ(_exceptionStack);
				}
				_exceptionStack = new std::queue<FunctionInfo*>();
				_currentFunction = null;
				_catcherFunction = null;
			}

			void FunctionExceptionTracer::ExceptionSearchFunctionEnter(FunctionID functionId)
			{
				FunctionInfo* function = _functions->FindFunction(functionId);
				_exceptionStack->push(function);
			}

			void FunctionExceptionTracer::ExceptionSearchFunctionLeave()
			{
			}

			void FunctionExceptionTracer::ExceptionSearchCatcherFound(FunctionID functionId)
			{
				FunctionInfo* function = _exceptionStack->back();
				__ASSERT(function->FunctionId == functionId, L"FunctionExceptionTracer::ExceptionSearchCatcherFound: actual function and function from local stack are different");
				_catcherFunction = function;
			}

			void FunctionExceptionTracer::ExceptionUnwindFunctionEnter(FunctionID functionId)
			{
				FunctionInfo* function = _exceptionStack->front();
				_exceptionStack->pop();
				__ASSERT(function->FunctionId == functionId, L"FunctionExceptionTracer::ExceptionUnwindFunctionEnter: actual function and function from local stack are different");
				_currentFunction = function;
			}

			void FunctionExceptionTracer::ExceptionUnwindFunctionLeave()
			{
				//current function is not catcher and its marker as 'to hook',
				//so we should notify that it was exited
				if (_currentFunction != _catcherFunction && _currentFunction->HookFunction)
				{
					RaiseFunctionExceptionHook(_currentFunction);
				}
				_currentFunction = null;
			}

			void FunctionExceptionTracer::ExceptionCatcherEnter(FunctionID functionId, ObjectID objectId)
			{

			}

			void FunctionExceptionTracer::ExceptionCatcherLeave()
			{
			}

			void FunctionExceptionTracer::RaiseFunctionExceptionHook(FunctionInfo* function)
			{
				FunctionExceptionEventArgs eventArgs(function->FunctionId, function->ClientData, _currentException);
				_profilingEvents->RaiseFunctionEvent(Chronos::Agent::DotNet::RuntimeProfilingEvents::FunctionException, &eventArgs);
			}
		}
	}
}