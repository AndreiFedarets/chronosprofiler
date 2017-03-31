// dllmain.h : Declaration of module class.

class CChronosProfilerAgent32Module : public ATL::CAtlDllModuleT< CChronosProfilerAgent32Module >
{
public :
	DECLARE_LIBID(LIBID_ChronosProfilerAgent32Lib)
	DECLARE_REGISTRY_APPID_RESOURCEID(IDR_CHRONOSPROFILERAGENT32, "{636C0826-D48E-442E-8B32-B58C38F0E29D}")
};

extern class CChronosProfilerAgent32Module _AtlModule;
