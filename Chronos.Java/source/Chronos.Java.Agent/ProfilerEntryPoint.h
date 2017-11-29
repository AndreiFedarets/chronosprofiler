#pragma once
#include "Chronos.Java.Agent.Internal.h"

class ProfilerEntryPoint
{
	public:
		ProfilerEntryPoint();
		~ProfilerEntryPoint();

	private:
		Chronos::Agent::Application* _application;
};