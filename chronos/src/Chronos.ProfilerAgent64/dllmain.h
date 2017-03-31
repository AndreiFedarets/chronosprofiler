// dllmain.h : Declaration of module class.

class CChronosProfilerAgent64Module : public ATL::CAtlDllModuleT< CChronosProfilerAgent64Module >
{
public :
	DECLARE_LIBID(LIBID_ChronosProfilerAgent64Lib)
	DECLARE_REGISTRY_APPID_RESOURCEID(IDR_CHRONOSPROFILERAGENT64, "{590C1550-365D-41C5-A061-374FAC5A84F8}")
};

extern class CChronosProfilerAgent64Module _AtlModule;
