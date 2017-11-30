#pragma once
#include "Chronos.Java.Agent.Internal.h"

namespace Chronos
{
	namespace Agent
	{
		namespace Java
		{
			class ProfilerEntryPoint
			{
			public:
				ProfilerEntryPoint();
				~ProfilerEntryPoint();
				HRESULT OnLoad(JavaVM* javaVM);
				HRESULT OnAttach(JavaVM* javaVM);

			private:
				HRESULT InitializeInternal(JavaVM* javaVM);

			private:
				Chronos::Agent::Application* _application;
				Chronos::Agent::Java::Reflection::RuntimeMetadataProvider* _metadataProvider;
			};
		}
	}
}