// dllmain.h : Declaration of module class.

class CChronosDotNetAgentModule : public ATL::CAtlDllModuleT< CChronosDotNetAgentModule >
{
public :
	DECLARE_LIBID(LIBID_ChronosDotNetAgentLib)
	DECLARE_REGISTRY_APPID_RESOURCEID(IDR_CHRONOSDOTNETAGENT, "{5AC9F386-DB04-45EF-AD6B-B653D21A1A67}")
};

extern class CChronosDotNetAgentModule _AtlModule;
