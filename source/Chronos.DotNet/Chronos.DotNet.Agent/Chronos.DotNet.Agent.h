#pragma once
#include <cor.h>
#include <corprof.h>
#include <corhlpr.h>
#include <MetaHost.h>
#include <Chronos/Chronos.Agent.h>

#ifdef CHRONOS_DOTNET_EXPORT_API
#define CHRONOS_DOTNET_API __declspec(dllexport) 
#else
#define CHRONOS_DOTNET_API __declspec(dllimport) 
#endif

namespace Chronos
{
	namespace Agent
	{
		namespace DotNet
		{
			namespace Reflection
			{
				class RuntimeMetadataProvider;
				
// ==================================================================================================================================================
				struct CHRONOS_DOTNET_API ThreadMetadata
				{
					public:
						ThreadMetadata(ICorProfilerInfo2* corProfilerInfo, RuntimeMetadataProvider* provider, ThreadID threadId);
						~ThreadMetadata();
						ThreadID GetId();
						__string* GetName();
						HANDLE GetThreadHandle();
						__uint GetOsThreadId();
					private:
						void Initialize();
						ICorProfilerInfo2* _corProfilerInfo;
						RuntimeMetadataProvider* _provider;
						ThreadID _threadId;
						HANDLE _threadHandle;
						__uint _osThreadId;
						__string* _name;
				};

// ==================================================================================================================================================
				struct CHRONOS_DOTNET_API AppDomainMetadata
				{
					public:
						AppDomainMetadata(ICorProfilerInfo2* corProfilerInfo, RuntimeMetadataProvider* provider, AppDomainID appDomainId);
						~AppDomainMetadata();
						AssemblyID GetId();
						__string* GetName();
					private:
						void Initialize();
						ICorProfilerInfo2* _corProfilerInfo;
						RuntimeMetadataProvider* _provider;
						AppDomainID _appDomainId;
						__string* _name;
				};

// ==================================================================================================================================================
				struct CHRONOS_DOTNET_API AssemblyMetadata
				{
					public:
						AssemblyMetadata(ICorProfilerInfo2* corProfilerInfo, RuntimeMetadataProvider* provider, AssemblyID assemblyId);
						~AssemblyMetadata();
						AssemblyID GetId();
						AppDomainID GetAppDomainId();
						ModuleID GetManifestModuleId();
						__string* GetName();
					private:
						void Initialize();
						ICorProfilerInfo2* _corProfilerInfo;
						RuntimeMetadataProvider* _provider;
						AssemblyID _assemblyId;
						AppDomainID _appDomainId;
						ModuleID _manifestModuleId;
						__string* _name;
				};

// ==================================================================================================================================================
				struct CHRONOS_DOTNET_API ModuleMetadata
				{
					public:
						ModuleMetadata(ICorProfilerInfo2* corProfilerInfo, RuntimeMetadataProvider* provider, ModuleID moduleId);
						~ModuleMetadata();
						ModuleID GetId();
						AssemblyID GetAssemblyId();
						LPCBYTE GetBaseLoadAddress();
						__string* GetName();
						IMetaDataImport2* GetMetadataImport();
					private:
						void Initialize();
						IMetaDataImport2* _metaDataImport;
						ICorProfilerInfo2* _corProfilerInfo;
						RuntimeMetadataProvider* _provider;
						ModuleID _moduleId;
						AssemblyID _assemblyId;
						LPCBYTE _baseLoadAddress;
						__string* _name;
				};

// ==================================================================================================================================================
				struct CHRONOS_DOTNET_API TypeMetadata
				{
					public:
						TypeMetadata(ICorProfilerInfo2* corProfilerInfo, RuntimeMetadataProvider* provider, ClassID classId);
						~TypeMetadata();
						ClassID GetId();
						mdTypeDef GetTypeToken();
						ModuleID GetModuleId();
						__string* GetName();
					private:
						void Initialize();
						ICorProfilerInfo2* _corProfilerInfo;
						RuntimeMetadataProvider* _provider;
						ClassID _classId;
						ModuleID _moduleId;
						mdTypeDef _typeToken;
						__string* _name;
				};
			
// ==================================================================================================================================================
				struct CHRONOS_DOTNET_API ParameterMetadata
				{
					public:
						ParameterMetadata(IMetaDataImport2* metaDataImport, PCCOR_SIGNATURE* corSignature);
						~ParameterMetadata();
					private:
						void Initialize(IMetaDataImport2* metaDataImport, PCCOR_SIGNATURE* corSignature);
						PCCOR_SIGNATURE ParseSignature(PCCOR_SIGNATURE signature, IMetaDataImport* metaDataImport);
						void ParseType(PCCOR_SIGNATURE& signature, IMetaDataImport* metaDataImport);
						__string* _name;
						__bool _byRef;
						CorElementType _elementType;
						__string* _typeName;
						__bool _isArray;
						mdTypeDef _typeToken;
				};

// ==================================================================================================================================================
				struct CHRONOS_DOTNET_API MethodMetadata
				{
					public:
						MethodMetadata(ICorProfilerInfo2* corProfilerInfo, RuntimeMetadataProvider* provider, FunctionID functionId);
						~MethodMetadata();
						FunctionID GetId();
						mdMethodDef GetMethodToken();
						mdTypeDef GetTypeToken();
						ClassID GetTypeId();
						ModuleID GetModuleId();
						AssemblyID GetAssemblyId();
						__string* GetName();
					private:
						void Initialize();
						ICorProfilerInfo2* _corProfilerInfo;
						RuntimeMetadataProvider* _provider;
						FunctionID _functionId;
						ClassID _classId;
						mdTypeDef _typeToken;
						ModuleID _moduleId;
						AssemblyID _assemblyId;
						mdMethodDef _methodToken;
						CorCallingConvention _callingConvention;
						__string* _name;
						__vector<ParameterMetadata*>* _parameters;
						ParameterMetadata* _returnParameter;
				};

// ==================================================================================================================================================
				struct CHRONOS_DOTNET_API FieldMetadata
				{
					public:
						FieldMetadata(ICorProfilerInfo2* corProfilerInfo, TypeMetadata* type, mdFieldDef fieldToken);
						~FieldMetadata();
						mdFieldDef GetFieldToken();
					private:
						ICorProfilerInfo2* _corProfilerInfo;
						IMetaDataImport2* _metaDataImport;
						TypeMetadata* _type;
						mdFieldDef _fieldToken;
				};

// ==================================================================================================================================================
				template<typename T>
				class MetadataCollection
				{
					public:
						MetadataCollection<T>()
						{
							_elements = new std::map<UINT_PTR, T*>();
						}
						~MetadataCollection<T>()
						{
							for (std::map<UINT_PTR, T*>::iterator i = _elements->begin(); i != _elements->end(); ++i)
							{
								T* item = i->second;
								__FREEOBJ(item);
							}
							_elements->clear();
							__FREEOBJ(_elements);
						}
						T* Find(UINT_PTR id)
						{
							Lock lock(&_criticalSection);
							std::map<UINT_PTR, T*>::iterator i = _elements->find(id);
							if (i == _elements->end())
							{
								return null;
							}
							return i->second;
						}
						__bool Add(UINT_PTR id, T* element)
						{
							Lock lock(&_criticalSection);
							return _elements->insert(std::pair<UINT_PTR, T*>(id, element)).second;
						}
					private:
						CriticalSection _criticalSection;
						std::map<UINT_PTR, T*>* _elements;
				};

// ==================================================================================================================================================
				static const wchar_t RuntimeMetadataProviderServiceToken[] = L"{3A1A67DA-8696-459C-AD51-BFD141571B5F}";
				class CHRONOS_DOTNET_API RuntimeMetadataProvider
				{
					public:
						RuntimeMetadataProvider();
						~RuntimeMetadataProvider();
						static HRESULT Initialize(IUnknown* corProfilerInfoUnk);
						
						HRESULT GetAppDomain(AppDomainID appDomainId, AppDomainMetadata** metadata);
						HRESULT GetAssembly(AssemblyID assemblyId, AssemblyMetadata** metadata);
						HRESULT GetModule(ModuleID moduleId, ModuleMetadata** metadata);
						HRESULT GetType(ClassID classId, TypeMetadata** metadata);
						HRESULT GetMethod(FunctionID functionId, MethodMetadata** metadata);
						HRESULT GetThread(ThreadID threadId, ThreadMetadata** metadata);
						HRESULT GetClassFromObject(ObjectID objectId, ClassID* classId);
						//ObjectMetadata* GetObject(ObjectID objectId);

						HRESULT GetRuntimeInfo(ICLRRuntimeInfo** runtimeInfo);
						HRESULT GetCorProfilerInfo2(ICorProfilerInfo2** profilerInfo);
						HRESULT GetCorProfilerInfo3(ICorProfilerInfo3** profilerInfo);
						HRESULT SetEventMask(DWORD eventsMask);
						HRESULT GetCurrentThreadId(ThreadID* threadId);
						HRESULT GetHandleFromThread(ThreadID threadId, HANDLE* threadHandle);
						
						const static __guid ServiceToken;
						
					private:
						ICorProfilerInfo2* GetCorProfilerInfo2();
						ICorProfilerInfo3* GetCorProfilerInfo3();

					private:
						MetadataCollection<AppDomainMetadata>* _appDomain;
						MetadataCollection<AssemblyMetadata>* _assemblies;
						MetadataCollection<ModuleMetadata>* _modules;
						MetadataCollection<TypeMetadata>* _types;
						MetadataCollection<MethodMetadata>* _methods;
						MetadataCollection<ThreadMetadata>* _threads;
						ICorProfilerInfo2* _corProfilerInfo2;
						ICorProfilerInfo3* _corProfilerInfo3;
						ICLRRuntimeInfo* _runtimeInfo;
						static IUnknown* ÑorProfilerInfoUnk;
				};


// ==================================================================================================================================================

				namespace Emit
				{
					/*class CHRONOS_DOTNET_API ILHelper
					{
						public:
							static IMAGE_COR_ILMETHOD* ConvertTinyMethodToFat(COR_ILMETHOD_TINY* sourceMethod, IMethodMalloc* methodMalloc);
							static IMAGE_COR_ILMETHOD* ConvertTinyMethodToFat(COR_ILMETHOD_TINY* sourceMethod);

							static IMAGE_COR_ILMETHOD* CloneFatMethod(COR_ILMETHOD_FAT* sourceMethod);
							static IMAGE_COR_ILMETHOD* CloneFatMethod(COR_ILMETHOD_FAT* sourceMethod, IMethodMalloc* methodMalloc);
							static void UpdateEHSectionsOffset(COR_ILMETHOD_FAT* method, int offsetDelta);
						private:
							static BYTE* GetMethodSectionsData(COR_ILMETHOD_FAT* method, unsigned long* dataSize);
							static void CopyTinyToFat(COR_ILMETHOD_TINY* sourceMethod, COR_ILMETHOD_FAT* targetMethod);
							static void CopyFatToFat(COR_ILMETHOD_FAT* sourceMethod, COR_ILMETHOD_FAT* targetMethod);
							static unsigned long GetMethodFullSize(COR_ILMETHOD_TINY* method);
							static unsigned long GetMethodFullSize(COR_ILMETHOD_FAT* method);
					};*/

// ==================================================================================================================================================
					typedef __byte (*GetOpCodeValueSize)(__byte* code);
					struct CHRONOS_DOTNET_API OpCode
					{
						public:
							char* Name;
							union 
							{
								__ushort Token;
								__byte TokenData[2];
							};
							__byte TokenSize;
							__byte ValueSize;
							GetOpCodeValueSize GetValueSize;
					};

// ==================================================================================================================================================
					class CHRONOS_DOTNET_API OpCodes
					{
						public:
							//0x0*
							static OpCode* Nop;
							static OpCode* Break;
							static OpCode* Ldarg_0;
							static OpCode* Ldarg_1;
							static OpCode* Ldarg_2;
							static OpCode* Ldarg_3;
							static OpCode* Ldloc_0;
							static OpCode* Ldloc_1;
							static OpCode* Ldloc_2;
							static OpCode* Ldloc_3;
							static OpCode* Stloc_0;
							static OpCode* Stloc_1;
							static OpCode* Stloc_2;
							static OpCode* Stloc_3;
							static OpCode* Ldarg_S;
							static OpCode* Ldarga_S;
							//0x1*
							static OpCode* Starg_S;
							static OpCode* Ldloc_S;
							static OpCode* Ldloca_S;
							static OpCode* Stloc_S;
							static OpCode* Ldnull;
							static OpCode* Ldc_I4_M1;
							static OpCode* Ldc_I4_0;
							static OpCode* Ldc_I4_1;
							static OpCode* Ldc_I4_2;
							static OpCode* Ldc_I4_3;
							static OpCode* Ldc_I4_4;
							static OpCode* Ldc_I4_5;
							static OpCode* Ldc_I4_6;
							static OpCode* Ldc_I4_7;
							static OpCode* Ldc_I4_8;
							static OpCode* Ldc_I4_S;
							//0x2*
							static OpCode* Ldc_I4;
							static OpCode* Ldc_I8;
							static OpCode* Ldc_R4;
							static OpCode* Ldc_R8;
							static OpCode* Dup;
							static OpCode* Pop;
							static OpCode* Jmp;
							static OpCode* Call;
							static OpCode* Calli;
							static OpCode* Ret;
							static OpCode* Br_S;
							static OpCode* Brfalse_S;
							static OpCode* Brtrue_S;
							static OpCode* Beq_S;
							static OpCode* Bge_S;
							//0x3*
							static OpCode* Bgt_S;
							static OpCode* Ble_S;
							static OpCode* Blt_S;
							static OpCode* Bne_Un_S;
							static OpCode* Bge_Un_S;
							static OpCode* Bgt_Un_S;
							static OpCode* Ble_Un_S;
							static OpCode* Blt_Un_S;
							static OpCode* Br;
							static OpCode* Brfalse;
							static OpCode* Brtrue;
							static OpCode* Beq;
							static OpCode* Bge;
							static OpCode* Bgt;
							static OpCode* Ble;
							static OpCode* Blt;
							//0x4*
							static OpCode* Bne_Un;
							static OpCode* Bge_Un;
							static OpCode* Bgt_Un;
							static OpCode* Ble_Un;
							static OpCode* Blt_Un;
							static OpCode* Switch;
							static OpCode* Ldind_I1;
							static OpCode* Ldind_U1;
							static OpCode* Ldind_I2;
							static OpCode* Ldind_U2;
							static OpCode* Ldind_I4;
							static OpCode* Ldind_U4;
							static OpCode* Ldind_I8;
							static OpCode* Ldind_I;
							static OpCode* Ldind_R4;
							static OpCode* Ldind_R8;
							//0x5*
							static OpCode* Ldind_Ref;
							static OpCode* Stind_Ref;
							static OpCode* Stind_I1;
							static OpCode* Stind_I2;
							static OpCode* Stind_I4;
							static OpCode* Stind_I8;
							static OpCode* Stind_R4;
							static OpCode* Stind_R8;
							static OpCode* Add;
							static OpCode* Sub;
							static OpCode* Mul;
							static OpCode* Div;
							static OpCode* Div_Un;
							static OpCode* Rem;
							static OpCode* Rem_Un;
							static OpCode* And;
							//0x6*
							static OpCode* Or;
							static OpCode* Xor;
							static OpCode* Shl;
							static OpCode* Shr;
							static OpCode* Shr_Un;
							static OpCode* Neg;
							static OpCode* Not;
							static OpCode* Conv_I1;
							static OpCode* Conv_I2;
							static OpCode* Conv_I4;
							static OpCode* Conv_I8;
							static OpCode* Conv_R4;
							static OpCode* Conv_R8;
							static OpCode* Conv_U4;
							static OpCode* Conv_U8;
							static OpCode* Callvirt;
							//0x7*
							static OpCode* Cpobj;
							static OpCode* Ldobj;
							static OpCode* Ldstr;
							static OpCode* Newobj;
							static OpCode* Castclass;
							static OpCode* Isinst;
							static OpCode* Conv_R_Un;
							static OpCode* Unbox;
							static OpCode* Throw;
							static OpCode* Ldfld;
							static OpCode* Ldflda;
							static OpCode* Stfld;
							static OpCode* Ldsfld;
							static OpCode* Ldsflda;
							//0x8*
							static OpCode* Stsfld;
							static OpCode* Stobj;
							static OpCode* Conv_Ovf_I1_Un;
							static OpCode* Conv_Ovf_I2_Un;
							static OpCode* Conv_Ovf_I4_Un;
							static OpCode* Conv_Ovf_I8_Un;
							static OpCode* Conv_Ovf_U1_Un;
							static OpCode* Conv_Ovf_U2_Un;
							static OpCode* Conv_Ovf_U4_Un;
							static OpCode* Conv_Ovf_U8_Un;
							static OpCode* Conv_Ovf_I_Un;
							static OpCode* Conv_Ovf_U_Un;
							static OpCode* Box;
							static OpCode* Newarr;
							static OpCode* Ldlen;
							static OpCode* Ldelema;
							//0x9*
							static OpCode* Ldelem_I1;
							static OpCode* Ldelem_U1;
							static OpCode* Ldelem_I2;
							static OpCode* Ldelem_U2;
							static OpCode* Ldelem_I4;
							static OpCode* Ldelem_U4;
							static OpCode* Ldelem_I8;
							static OpCode* Ldelem_I;
							static OpCode* Ldelem_R4;
							static OpCode* Ldelem_R8;
							static OpCode* Ldelem_Ref;
							static OpCode* Stelem_I;
							static OpCode* Stelem_I1;
							static OpCode* Stelem_I2;
							static OpCode* Stelem_I4;
							static OpCode* Stelem_I8;
							//0xA*
							static OpCode* Stelem_R4;
							static OpCode* Stelem_R8;
							static OpCode* Stelem_Ref;
							static OpCode* Ldelem;
							static OpCode* Stelem;
							static OpCode* Unbox_Any;
							//0xB*
							static OpCode* Conv_Ovf_I1;
							static OpCode* Conv_Ovf_U1;
							static OpCode* Conv_Ovf_I2;
							static OpCode* Conv_Ovf_U2;
							static OpCode* Conv_Ovf_I4;
							static OpCode* Conv_Ovf_U4;
							static OpCode* Conv_Ovf_I8;
							static OpCode* Conv_Ovf_U8;
							//0xC*
							static OpCode* Refanyval;
							static OpCode* Ckfinite;
							static OpCode* Mkrefany;
							//0xD*
							static OpCode* Ldtoken;
							static OpCode* Conv_U2;
							static OpCode* Conv_U1;
							static OpCode* Conv_I;
							static OpCode* Conv_Ovf_I;
							static OpCode* Conv_Ovf_U;
							static OpCode* Add_Ovf;
							static OpCode* Add_Ovf_Un;
							static OpCode* Mul_Ovf;
							static OpCode* Mul_Ovf_Un;
							static OpCode* Sub_Ovf;
							static OpCode* Sub_Ovf_Un;
							static OpCode* Endfinally;
							static OpCode* Leave;
							static OpCode* Leave_S;
							static OpCode* Stind_I;
							//0xE*
							static OpCode* Conv_U;
							//0xFE*
							static	OpCode* Arglist;
							static OpCode* Ceq;
							static OpCode* Cgt;
							static OpCode* Cgt_Un;
							static OpCode* Clt;
							static OpCode* Clt_Un;
							static OpCode* Ldftn;
							static OpCode* Ldvirtftn;
							static OpCode* Ldarg;
							static OpCode* Ldarga;
							static OpCode* Starg;
							static OpCode* Ldloc;
							static OpCode* Ldloca;
							static OpCode* Stloc;
							static OpCode* Localloc;
							static OpCode* Endfilter;
							static OpCode* Unaligned;
							static OpCode* Volatile;
							static OpCode* Tailcall;
							static OpCode* Initobj;
							static OpCode* Constrained;
							static OpCode* Cpblk;
							static OpCode* Initblk;
							static OpCode* Rethrow;
							static OpCode* Sizeof;
							static OpCode* Readonly;

							static OpCode* Read(__byte* data);
							
							static OpCode* Define(char* name, __ushort token, __ushort tokenSize, __byte valueSize, GetOpCodeValueSize getValueSize);

						private:
							static __map<__ushort, OpCode*>* Items;
					};

// ==================================================================================================================================================
					struct CHRONOS_DOTNET_API Instruction
					{
						OpCode* OpCode;
						Instruction* Previous;
						Instruction* Next;
						__uint ValueSize;
						union{
							__byte ByteValue;
							__ushort ShortValue;
							__uint IntValue;
							__ulong LongValue;
							//just definition of array, it could have bigger size
							__byte ArrayValue[8];
						};
						__uint GetSize() { return OpCode->TokenSize + ValueSize; }
					};
					
// ==================================================================================================================================================
					struct CHRONOS_DOTNET_API ExceptionHandler
					{
						CorExceptionFlag Flags;
						mdToken ClassToken;
						Instruction* TryBegin;
						Instruction* TryEnd;
						Instruction* Filter;
						Instruction* HandlerBegin;
						Instruction* HandlerEnd;
						ExceptionHandler* Next;
					};

// ==================================================================================================================================================
					struct CHRONOS_DOTNET_API SignatureElement
					{
						CorElementType ElementType;
						mdToken ClassToken;
						SignatureElement* ChildElement;
						SignatureElement* ArgumentElement;
						SignatureElement* Next;
					};

// ==================================================================================================================================================
					struct CHRONOS_DOTNET_API Signature
					{
						CorCallingConvention CallingConvention;
						SignatureElement* Front;
					};

// ==================================================================================================================================================
					struct CHRONOS_DOTNET_API Method
					{
						__uint MaxStack;
						mdSignature LocalVarSigTok;
						Instruction* FrontInstruction;
						ExceptionHandler* FrontHandler;
					};
					
// ==================================================================================================================================================
					class CHRONOS_DOTNET_API InstructionManager
					{
						public:
							static Instruction* Alloc();
							static Instruction* Alloc(__uint valueSize);
							static void ReleaseChain(Instruction* chain);
							static Instruction* ByOffset(Instruction* chain, __uint offset);
							static Instruction* ByOffset(Instruction* chain, __uint offset, __uint length);
							static __uint GetRangeSize(Instruction* instructionFrom, Instruction* instructionTo);
							static Instruction* ReadChain(__byte* ilCode, __uint ilCodeSize);
							static Instruction* Create(OpCode* opCode, __byte* valueData);
							static Instruction* Create(OpCode* opCode);
							static Instruction* Create(OpCode* opCode, __byte value);
							static Instruction* Create(OpCode* opCode, __ushort value);
							static Instruction* Create(OpCode* opCode, __uint value);
							static Instruction* Create(OpCode* opCode, __ulong value);
							static __uint GetChainSize(Instruction* instruction);
							static __uint WriteTo(Instruction* instruction, __byte* data);
							static __uint WriteChainTo(Instruction* instruction, __byte* data);
							static __uint GetOffset(Instruction* instruction);
							static Instruction* MoveToFront(Instruction* chain);
							static Instruction* MoveToFinal(Instruction* chain);
							static Instruction* LookForward(Instruction* startFrom, OpCode* targetOpCode);
							static Instruction* LookBackward(Instruction* startFrom, OpCode* targetOpCode);
							static Instruction* InsertChainBefore(Instruction* instruction, Instruction* chain);
							static Instruction* InsertChainAfter(Instruction* instruction, Instruction* chain);
						private:
							static void Release(Instruction* instruction);
							static Instruction* Read(__byte* ilCode);

					};
					class CHRONOS_DOTNET_API ExceptionHandlerManager
					{
						public:
							static ExceptionHandler* Alloc();
							static void Release(ExceptionHandler* handler);
							static void ReleaseChain(ExceptionHandler* chain);
							static ExceptionHandler* ReadChain(const COR_ILMETHOD_SECT* ilSection, Instruction* instructionChain);
							static __uint GetChainSize(ExceptionHandler* chain);
							static __uint GetChainCount(ExceptionHandler* chain);
							static void WriteChainTo(ExceptionHandler* chain, __byte* ilSection);
							static ExceptionHandler* DefineTryFinally(Method* method, Instruction* tryBegin, Instruction* tryEnd, Instruction* handlerBegin, Instruction* handlerEnd);
						private:
							static ExceptionHandler* ReadChainSmall(COR_ILMETHOD_SECT_EH_SMALL* ilSection, Instruction* instructionChain);
							static ExceptionHandler* ReadChainFat(COR_ILMETHOD_SECT_EH_FAT* ilSection, Instruction* instructionChain);
							static ExceptionHandler* ReadSmall(IMAGE_COR_ILMETHOD_SECT_EH_CLAUSE_SMALL* ilClause, Instruction* instructionChain);
							static ExceptionHandler* ReadFat(IMAGE_COR_ILMETHOD_SECT_EH_CLAUSE_FAT* ilClause, Instruction* instructionChain);
							static __uint WriteTo(ExceptionHandler* handler, __byte* ilClauseData);
							static void InsertHandler(Method* method, ExceptionHandler* handler);
					};
					class CHRONOS_DOTNET_API SignatureManager
					{
						public:
							static Signature* Alloc();
							static SignatureElement* AllocElement();
							static void Release(Signature* signature);
							static void ReleaseElement(SignatureElement* element);
							static void ReleaseElementChain(SignatureElement* chain);
							static Signature* Read(mdSignature corSignatureToken, IMetaDataImport* metadataImport);
							static Signature* Read(PCCOR_SIGNATURE corSignature);
							static mdSignature Write(Signature* signature, IMetaDataEmit* metadataEmit);
							static __uint InsertElement(Signature* signature, SignatureElement* element);
						private:
							static __uint GetChainCount(SignatureElement* chain);
							static __uint GetRawSize(Signature* signature);
							static __uint GetRawSizeRecurcive(SignatureElement* element);
							static SignatureElement* ReadElement(PCCOR_SIGNATURE* corSignatureRef);
							static __byte* WriteElement(SignatureElement* element, __byte* corSignatureData);
							//static PCCOR_SIGNATURE Parse(SignatureElement* element, PCCOR_SIGNATURE corSignature);
					};
					class CHRONOS_DOTNET_API MethodManager
					{
						public:
							static Method* Alloc();
							static void Release(Method* method);
							static Method* Read(IMAGE_COR_ILMETHOD* ilMethod);
							static __uint GetSize(Method* method);
							static void WriteTo(Method* method, BYTE* methodData);
							static Instruction* InsertChainBefore(Method* method, Instruction* instruction, Instruction* chain);
							static Instruction* InsertChainAfter(Method* method, Instruction* instruction, Instruction* chain);
							static void WriteDebug(Method* method);
						private:
							static Method* ReadTiny(COR_ILMETHOD_TINY* ilMethod);
							static Method* ReadFat(COR_ILMETHOD_FAT* ilMethod);
							static __uint AlignCodeSize(__uint size);
					};
				}

			}

// APPLICATION DOMAIN EVENTS ------------------------------------------------------------------------------------------------------------------------
// ==================================================================================================================================================
			struct AppDomainCreationStartedEventArgs
			{
				AppDomainCreationStartedEventArgs(AppDomainID appDomainId) : AppDomainId(appDomainId) { }
				AppDomainID AppDomainId;
			};

			struct AppDomainCreationFinishedEventArgs
			{
				AppDomainCreationFinishedEventArgs(AppDomainID appDomainId, HRESULT result) : AppDomainId(appDomainId), Result(result) { }
				AppDomainID AppDomainId;
				HRESULT Result;
			};

			struct AppDomainShutdownStartedEventArgs
			{
				AppDomainShutdownStartedEventArgs(AppDomainID appDomainId) : AppDomainId(appDomainId) { }
				AppDomainID AppDomainId;
			};

			struct AppDomainShutdownFinishedEventArgs
			{
				AppDomainShutdownFinishedEventArgs(AppDomainID appDomainId, HRESULT result) : AppDomainId(appDomainId), Result(result) { }
				AppDomainID AppDomainId;
				HRESULT Result;
			};

// ASSEMBLY EVENTS ----------------------------------------------------------------------------------------------------------------------------------
// ==================================================================================================================================================
			struct AssemblyLoadStartedEventArgs
			{
				AssemblyLoadStartedEventArgs(AssemblyID assemblyId) : AssemblyId(assemblyId) { }
				AssemblyID AssemblyId;
			};
			
			struct AssemblyLoadFinishedEventArgs
			{
				AssemblyLoadFinishedEventArgs(AssemblyID assemblyId, HRESULT result) : AssemblyId(assemblyId), Result(result) { }
				AssemblyID AssemblyId;
				HRESULT Result;
			};

			struct AssemblyUnloadStartedEventArgs
			{
				AssemblyUnloadStartedEventArgs(AssemblyID assemblyId) : AssemblyId(assemblyId) { }
				AssemblyID AssemblyId;
			};
			
			struct AssemblyUnloadFinishedEventArgs
			{
				AssemblyUnloadFinishedEventArgs(AssemblyID assemblyId, HRESULT result) : AssemblyId(assemblyId), Result(result) { }
				AssemblyID AssemblyId;
				HRESULT Result;
			};

// MODULE EVENTS  -----------------------------------------------------------------------------------------------------------------------------------
// ==================================================================================================================================================
			struct ModuleLoadStartedEventArgs
			{
				ModuleLoadStartedEventArgs(ModuleID moduleId) : ModuleId(moduleId) { }
				ModuleID ModuleId;
			};

			struct ModuleLoadFinishedEventArgs
			{
				ModuleLoadFinishedEventArgs(ModuleID moduleId, HRESULT result) : ModuleId(moduleId), Result(result) { }
				ModuleID ModuleId;
				HRESULT Result;
			};
			
			struct ModuleUnloadStartedEventArgs
			{
				ModuleUnloadStartedEventArgs(ModuleID moduleId) : ModuleId(moduleId) { }
				ModuleID ModuleId;
			};
			
			struct ModuleUnloadFinishedEventArgs
			{
				ModuleUnloadFinishedEventArgs(ModuleID moduleId, HRESULT result) : ModuleId(moduleId), Result(result) { }
				ModuleID ModuleId;
				HRESULT Result;
			};
			
			struct ModuleAttachedToAssemblyEventArgs
			{
				ModuleAttachedToAssemblyEventArgs(ModuleID moduleId, AssemblyID assemblyId) : ModuleId(moduleId), AssemblyId(assemblyId) { }
				ModuleID ModuleId;
				AssemblyID AssemblyId;
			};

// CLASS EVENTS  ------------------------------------------------------------------------------------------------------------------------------------
// ==================================================================================================================================================
			struct ClassLoadStartedEventArgs
			{
				ClassLoadStartedEventArgs(ClassID classId) : ClassId(classId) { }
				ClassID ClassId;
			};
			
			struct ClassLoadFinishedEventArgs
			{
				ClassLoadFinishedEventArgs(ClassID classId, HRESULT result) : ClassId(classId), Result(result) { }
				ClassID ClassId;
				HRESULT Result;
			};
			
			struct ClassUnloadStartedEventArgs
			{
				ClassUnloadStartedEventArgs(ClassID classId) : ClassId(classId) { }
				ClassID ClassId;
			};
			
			struct ClassUnloadFinishedEventArgs
			{
				ClassUnloadFinishedEventArgs(ClassID classId, HRESULT result) : ClassId(classId), Result(result) { }
				ClassID ClassId;
				HRESULT Result;
			};
			
// JIT EVENTS  --------------------------------------------------------------------------------------------------------------------------------------
// ==================================================================================================================================================
			struct JITCompilationStartedEventArgs
			{
				JITCompilationStartedEventArgs(FunctionID functionId, BOOL isSafeToBlock) : FunctionId(functionId), IsSafeToBlock(isSafeToBlock) { }
				FunctionID FunctionId;
				BOOL IsSafeToBlock;
			};

			struct JITCompilationFinishedEventArgs
			{
				JITCompilationFinishedEventArgs(FunctionID functionId, HRESULT status, BOOL isSafeToBlock) : FunctionId(functionId), Status(status), IsSafeToBlock(isSafeToBlock) { }
				FunctionID FunctionId;
				HRESULT Status;
				BOOL IsSafeToBlock;
			};

			struct JITCachedFunctionSearchStartedEventArgs
			{
				JITCachedFunctionSearchStartedEventArgs(FunctionID functionId, BOOL useCachedFunction) : FunctionId(functionId),  UseCachedFunction(useCachedFunction) { }
				FunctionID FunctionId;
				BOOL UseCachedFunction;
			};

			struct JITCachedFunctionSearchFinishedEventArgs
			{
				JITCachedFunctionSearchFinishedEventArgs(FunctionID functionId, COR_PRF_JIT_CACHE result) : FunctionId(functionId), Result(result) { }
				FunctionID FunctionId;
				COR_PRF_JIT_CACHE Result;
			};

			struct JITFunctionPitchedEventArgs
			{
				JITFunctionPitchedEventArgs(FunctionID functionId) : FunctionId(functionId) { }
				FunctionID FunctionId;
			};
			
			struct JITInliningEventArgs
			{
				JITInliningEventArgs(FunctionID callerId, FunctionID calleeId, BOOL shouldInline) : CallerId(callerId), CalleeId(calleeId), ShouldInline(shouldInline) { }
				FunctionID CallerId;
				FunctionID CalleeId;
				BOOL ShouldInline;
			};

// FUNCTION EVENTS  ---------------------------------------------------------------------------------------------------------------------------------
// ==================================================================================================================================================
			struct FunctionLoadStartedEventArgs
			{
				FunctionLoadStartedEventArgs(FunctionID functionId, bool hookFunction, void* clientData) 
					: FunctionId(functionId), HookFunction(hookFunction), ClientData(clientData) { }
				FunctionID FunctionId;
				bool HookFunction;
				void* ClientData;
			};

			struct FunctionUnloadStartedEventArgs
			{
				FunctionUnloadStartedEventArgs(FunctionID functionId) : FunctionId(functionId) { }
				FunctionID FunctionId;
			};
			
			struct FunctionEnterEventArgs
			{
				FunctionEnterEventArgs(FunctionID functionId, UINT_PTR clientData) : FunctionId(functionId), ClientData(clientData) { }
				FunctionID FunctionId;
				UINT_PTR ClientData;
			};
			
			struct FunctionLeaveEventArgs
			{
				FunctionLeaveEventArgs(FunctionID functionId, UINT_PTR clientData) : FunctionId(functionId), ClientData(clientData) { }
				FunctionID FunctionId;
				UINT_PTR ClientData;
			};
			
			struct FunctionTailcallEventArgs
			{
				FunctionTailcallEventArgs(FunctionID functionId, UINT_PTR clientData) : FunctionId(functionId), ClientData(clientData) { }
				FunctionID FunctionId;
				UINT_PTR ClientData;
			};
			
			struct FunctionExceptionEventArgs
			{
				FunctionExceptionEventArgs(FunctionID functionId, UINT_PTR clientData, ObjectID exceptionId)
					: FunctionId(functionId), ClientData(clientData), ExceptionId(exceptionId) { }
				FunctionID FunctionId;
				UINT_PTR ClientData;
				ObjectID ExceptionId;
			};

// THREAD EVENTS  -----------------------------------------------------------------------------------------------------------------------------------
// ==================================================================================================================================================
			struct ThreadCreatedEventArgs
			{
				ThreadCreatedEventArgs(ThreadID threadId) : ThreadId(threadId) { }
				ThreadID ThreadId;
			};

			struct ThreadDestroyedEventArgs
			{
				ThreadDestroyedEventArgs(ThreadID threadId) : ThreadId(threadId) { }
				ThreadID ThreadId;
			};

			struct ThreadAssignedToOSThreadEventArgs
			{
				ThreadAssignedToOSThreadEventArgs(ThreadID threadId, __uint osThreadId) : ThreadId(threadId), OsThreadId(osThreadId) { }
				ThreadID ThreadId;
				__uint OsThreadId;
			};

			struct ThreadNameChangedEventArgs
			{
				ThreadNameChangedEventArgs(ThreadID threadId, std::wstring name) : ThreadId(threadId), Name(name) { }
				ThreadID ThreadId;
				std::wstring Name;
			};
			
// EXCEPTION EVENTS (Exception creation) ------------------------------------------------------------------------------------------------------------
// ==================================================================================================================================================
			struct ExceptionThrownEventArgs
			{
				ExceptionThrownEventArgs(ObjectID exceptionId) : ExceptionId(exceptionId) { }
				ObjectID ExceptionId;
			};

// EXCEPTION EVENTS (Search phase) ------------------------------------------------------------------------------------------------------------------
// ==================================================================================================================================================
			struct ExceptionSearchFunctionEnterEventArgs
			{
				ExceptionSearchFunctionEnterEventArgs(FunctionID functionId) : FunctionId(functionId) { }
				FunctionID FunctionId;
			};

			struct ExceptionSearchFunctionLeaveEventArgs
			{
				ExceptionSearchFunctionLeaveEventArgs() {}
			};
			
			struct ExceptionSearchFilterEnterEventArgs
			{
				ExceptionSearchFilterEnterEventArgs(FunctionID functionId) : FunctionId(functionId) { }
				FunctionID FunctionId;
			};
			
			struct ExceptionSearchFilterLeaveEventArgs
			{
				ExceptionSearchFilterLeaveEventArgs() {}
			};
			
			struct ExceptionSearchCatcherFoundEventArgs
			{
				ExceptionSearchCatcherFoundEventArgs(FunctionID functionId) : FunctionId(functionId) { }
				FunctionID FunctionId;
			};
			
// EXCEPTION EVENTS (Unwind phase) ------------------------------------------------------------------------------------------------------------------
// ==================================================================================================================================================
			struct ExceptionUnwindFunctionEnterEventArgs
			{
				ExceptionUnwindFunctionEnterEventArgs(FunctionID functionId) : FunctionId(functionId) { }
				FunctionID FunctionId;
			};

			struct ExceptionUnwindFunctionLeaveEventArgs
			{
				ExceptionUnwindFunctionLeaveEventArgs() { }
			};
			
			struct ExceptionUnwindFinallyEnterEventArgs
			{
				ExceptionUnwindFinallyEnterEventArgs(FunctionID functionId) : FunctionId(functionId) { }
				FunctionID FunctionId;
			};
			
			struct ExceptionUnwindFinallyLeaveEventArgs
			{
				ExceptionUnwindFinallyLeaveEventArgs() { }
			};
			
			struct ExceptionCatcherEnterEventArgs
			{
				ExceptionCatcherEnterEventArgs(FunctionID functionId, ObjectID exceptionId) : FunctionId(functionId), ExceptionId(exceptionId) { }
				FunctionID FunctionId;
				ObjectID ExceptionId;
			};
			
			struct ExceptionCatcherLeaveEventArgs
			{
				ExceptionCatcherLeaveEventArgs() { }
			};

			
// CONTEXT EVENTS -----------------------------------------------------------------------------------------------------------------------------------
// ==================================================================================================================================================
			struct UnmanagedToManagedTransitionEventArgs
			{
				UnmanagedToManagedTransitionEventArgs(FunctionID functionId, COR_PRF_TRANSITION_REASON reason) : FunctionId(functionId), Reason(reason) { }
				FunctionID FunctionId;
				COR_PRF_TRANSITION_REASON Reason;
			};

			struct ManagedToUnmanagedTransitionEventArgs
			{
				ManagedToUnmanagedTransitionEventArgs(FunctionID functionId, COR_PRF_TRANSITION_REASON reason) : FunctionId(functionId), Reason(reason) { }
				FunctionID FunctionId;
				COR_PRF_TRANSITION_REASON Reason;
			};

// SUSPENSION EVENTS --------------------------------------------------------------------------------------------------------------------------------
// ==================================================================================================================================================
			struct RuntimeSuspendStartedEventArgs
			{
				RuntimeSuspendStartedEventArgs(COR_PRF_SUSPEND_REASON suspendReason) : SuspendReason(suspendReason) { }
				COR_PRF_SUSPEND_REASON SuspendReason;
			};

			struct RuntimeSuspendFinishedEventArgs
			{
				RuntimeSuspendFinishedEventArgs() { }
			};

			struct RuntimeSuspendAbortedEventArgs
			{
				RuntimeSuspendAbortedEventArgs() { }
			};

			struct RuntimeResumeStartedEventArgs
			{
				RuntimeResumeStartedEventArgs() { }
			};

			struct RuntimeResumeFinishedEventArgs
			{
				RuntimeResumeFinishedEventArgs() { }
			};

			struct RuntimeThreadSuspendedEventArgs
			{
				RuntimeThreadSuspendedEventArgs(ThreadID threadId) : ThreadId(threadId) { }
				ThreadID ThreadId;
			};

			struct RuntimeThreadResumedEventArgs
			{
				RuntimeThreadResumedEventArgs(ThreadID threadId) : ThreadId(threadId) { }
				ThreadID ThreadId;
			};
			
// GC EVENTS ----------------------------------------------------------------------------------------------------------------------------------------
// ==================================================================================================================================================
			/*struct MovedReferencesEventArgs
			{
				MovedReferencesEventArgs(ULONG cmovedObjectIDRanges, ObjectID oldObjectIDRangeStart[], ObjectID newObjectIDRangeStart[], ULONG cObjectIDRangeLength[]) : ThreadId(threadId) { }
				ULONG cmovedObjectIDRanges;
				ObjectID oldObjectIDRangeStart[];
				ObjectID newObjectIDRangeStart[];
			};*/

// ==================================================================================================================================================
			typedef void (*FunctionEventCallback)(void*);

			static const wchar_t RuntimeProfilingEventsServiceToken[] = L"{95EBE89E-9ACE-4EEA-9FC9-FD8E9965995E}";
			class CHRONOS_DOTNET_API RuntimeProfilingEvents
			{
				public:
					RuntimeProfilingEvents(void);
					~RuntimeProfilingEvents(void);

					void SetAdditionalEventsMask(__int eventsMask);
					__int GetProfilingEvents();

					ICallback* SubscribeEvent(__uint eventId, ICallback* callback);
					FunctionEventCallback SubscribeFunctionEvent(__uint eventId, FunctionEventCallback callback);

					void RaiseEvent(__uint eventId, void* eventArgs);
					void RaiseFunctionEvent(__uint eventId, void* eventArgs);

					__bool HookEvent(__uint eventId);
					
					/*FunctionEventCallback SusbcribeFunctionEnterEvent(FunctionEventCallback callback);
					__bool HookFunctionEnterEvent();
					void RaiseFunctionEnterEvent(FunctionEnterEventArgs* eventArgs);
					
					FunctionEventCallback SusbcribeFunctionLeaveEvent(FunctionEventCallback callback);
					__bool HookFunctionLeaveEvent();
					void RaiseFunctionLeaveEvent(FunctionLeaveEventArgs* eventArgs);
					
					FunctionEventCallback SusbcribeFunctionTailcallEvent(FunctionEventCallback callback);
					__bool HookFunctionTailcallEvent();
					void RaiseFunctionTailcallEvent(FunctionTailcallEventArgs* eventArgs);
					
					FunctionEventCallback SusbcribeFunctionExceptionEvent(FunctionEventCallback callback);
					__bool HookFunctionExceptionEvent();
					void RaiseFunctionExceptionEvent(FunctionExceptionEventArgs* eventArgs);*/

					const static __guid ServiceToken;
				private:
					__bool IsFunctionEvent(__uint eventId);

				private:
					__int _additionalEvents;
					ICallback* _events[0xFF];
					FunctionEventCallback _functionEvents[0xFF];
					/*FunctionEventCallback _functionEnterCallback;
					FunctionEventCallback _functionLeaveCallback;
					FunctionEventCallback _functionTailcallCallback;
					FunctionEventCallback _functionExceptionCallback;*/

				public:
					enum ProfilingEventId
					{
						// APPLICATION DOMAIN EVENTS
						AppDomainCreationStarted = 1,
						AppDomainCreationFinished = 2,
						AppDomainShutdownStarted = 3,
						AppDomainShutdownFinished = 4,

						// ASSEMBLY EVENTS
						AssemblyLoadStarted = 20,
						AssemblyLoadFinished = 21,
						AssemblyUnloadStarted = 22,
						AssemblyUnloadFinished = 23,

						// MODULE EVENTS
						ModuleLoadStarted = 40,
						ModuleLoadFinished = 41,
						ModuleUnloadStarted = 42,
						ModuleUnloadFinished = 43,
						ModuleAttachedToAssembly = 44,

						// CLASS EVENTS
						ClassLoadStarted = 60,
						ClassLoadFinished = 61,
						ClassUnloadStarted = 62,
						ClassUnloadFinished = 63,

						// JIT EVENTS
						JITCompilationStarted = 80,
						JITCompilationFinished = 81,
						JITCachedFunctionSearchStarted = 82,
						JITCachedFunctionSearchFinished = 83,
						JITFunctionPitched = 84,
						JITInlining = 85,


						// FUNCTION EVENTS
						FunctionLoadStarted = 100,
						FunctionUnloadStarted = 101,
						FunctionEnter = 102,
						FunctionLeave = 103,
						FunctionTailcall = 104,
						FunctionException = 105,

						// THREAD EVENTS
						ThreadCreated = 120,
						ThreadDestroyed = 121,
						ThreadAssignedToOSThread = 122,
						ThreadNameChanged = 123,

						// EXCEPTION EVENTS (Exception creation)
						ExceptionThrown = 140,

						// EXCEPTION EVENTS (Search phase)
						ExceptionSearchFunctionEnter = 160,
						ExceptionSearchFunctionLeave = 161,
						ExceptionSearchFilterEnter = 162,
						ExceptionSearchFilterLeave = 163,
						ExceptionSearchCatcherFound = 164,

						// EXCEPTION EVENTS (Unwind phase)
						ExceptionUnwindFunctionEnter = 180,
						ExceptionUnwindFunctionLeave = 181,
						ExceptionUnwindFinallyEnter = 182,
						ExceptionUnwindFinallyLeave = 183,
						ExceptionCatcherEnter = 184,
						ExceptionCatcherLeave = 185,

						// CONTEXT EVENTS
						UnmanagedToManagedTransition = 200,
						ManagedToUnmanagedTransition = 201,

						// SUSPENSION EVENTS
						RuntimeSuspendStarted = 220,
						RuntimeSuspendFinished = 221,
						RuntimeSuspendAborted = 222,
						RuntimeResumeStarted = 223,
						RuntimeResumeFinished = 224,
						RuntimeThreadSuspended = 225,
						RuntimeThreadResumed = 226,

						// GC EVENTS
						MovedReferences = 240,
						ObjectAllocated = 241,
						ObjectsAllocatedByClass = 242,
						ObjectReferences = 243,
						RootReferences = 244,
						GarbageCollectionStarted = 245,
						SurvivingReferences = 246,
						GarbageCollectionFinished = 247,
						FinalizeableObjectQueued = 248,
						RootReferences2 = 249,
						HandleCreated = 250,
						HandleDestroyed = 251

					};
			};

// ==================================================================================================================================================
			template<typename T>
			class ProfilingEventsSubscription
			{
				public:
					ProfilingEventsSubscription<T>(T* thisObject, RuntimeProfilingEvents* profilingEvents)
					{
						_thisObject = thisObject;
						_profilingEvents = profilingEvents;
						memset(_subscriptions, 0, sizeof(_subscriptions));
					}
					~ProfilingEventsSubscription<T>()
					{
					}
					void SubscribeEvent(__uint eventId, void (T::*callbackFunction)(void*))
					{
						ICallback* callback = new ThisCallback<T>(_thisObject, callbackFunction);
						_subscriptions[eventId] = _profilingEvents->SubscribeEvent(eventId, callback);
					}
					void RaiseNextEvent(__uint eventId, void* eventArgs)
					{
						ICallback* callback = _subscriptions[eventId];
						if (callback != null)
						{
							callback->Call(eventArgs);
						}
					}
				private:
					ICallback* _subscriptions[0xFF];
					RuntimeProfilingEvents* _profilingEvents;
					T* _thisObject;
			};

// ==================================================================================================================================================
			class CHRONOS_DOTNET_API FunctionJitEvent
			{
				private:
					class FunctionCollection
					{
						public:
							void Add(mdToken functionToken);
							__bool Contains(mdToken functionToken);
							void Remove(mdToken functionToken);
						private:
							CriticalSection _criticalSection;
							std::map<mdToken, mdToken> _functions;
					};
					class ModuleCollection
					{
						public:
							~ModuleCollection();
							FunctionCollection* Add(ModuleID moduleId);
							FunctionCollection* Get(ModuleID moduleId);
							void Remove(ModuleID moduleId);
						private:
							CriticalSection _criticalSection;
							std::map<ModuleID, FunctionCollection*> _modules;
					};
					class AssemblyCollection
					{
						public:
							~AssemblyCollection();
							ModuleCollection* Add(AssemblyID assemblyId);
							ModuleCollection* Get(AssemblyID assemblyId);
							void Remove(AssemblyID assemblyId);
						private:
							CriticalSection _criticalSection;
							std::map<AssemblyID, ModuleCollection*> _assemblies;
					};

				public:
					FunctionJitEvent(__string assemblyName, __string className, __string functionName, __vector<__string> arguments, ICallback* callback);
					~FunctionJitEvent();
					void Initialize(RuntimeProfilingEvents* profilingEvents, Reflection::RuntimeMetadataProvider* metadataProvider);
					void Subscribe();
					
				private:
					void OnAssemblyLoadStarted(void* eventArgs);
					void OnAssemblyUnloadStarted(void* eventArgs);
					void OnModuleAttachedToAssembly(void* eventArgs);
					void OnJITCompilationStarted(void* eventArgs);

				private:
					__string* _assemblyName;
					__string* _className;
					__string* _functionName;
					__vector<__string>* _arguments;
					AssemblyCollection* _assemblies;
					ICallback* _callback;
					Reflection::RuntimeMetadataProvider* _metadataProvider;
					ProfilingEventsSubscription<FunctionJitEvent>* _subscription;
			};

// ==================================================================================================================================================
			template<typename T>
			class DotNetUnitCollectionBase : public UnitCollectionBase<T>
			{
				public:
					DotNetUnitCollectionBase<T>()
					{
					}

					~DotNetUnitCollectionBase<T>()
					{
					}
					
					void Initialize(ProfilingTimer* profilingTimer, Reflection::RuntimeMetadataProvider* metadataProvider)
					{
						_profilingTimer = profilingTimer;
						_metadataProvider = metadataProvider;
					}

				protected:
					Reflection::RuntimeMetadataProvider* _metadataProvider;
			};
			
// ==================================================================================================================================================
			class CHRONOS_DOTNET_API FrameworkAdapter : public IFrameworkAdapter
			{
				public:
					FrameworkAdapter();
					~FrameworkAdapter();
					HRESULT BeginInitialize(FrameworkSettings* frameworkSettings, ProfilingTargetSettings* profilingTargetSettings);
					HRESULT ExportServices(ServiceContainer* container);
					HRESULT ImportServices(ServiceContainer* container);
					HRESULT EndInitialize();
					HRESULT SubscribeEvents();
					HRESULT FlushData();
					const static __guid FrameworkUid;
				private:
					Reflection::RuntimeMetadataProvider* _metadataProvider;
					RuntimeProfilingEvents* _profilingEvents;
					FrameworkSettings* _frameworkSettings;
			};
			
// ==================================================================================================================================================
			class CHRONOS_DOTNET_API MethodInjector
			{
				public:
					MethodInjector();
					~MethodInjector();
					HRESULT Initialize(Reflection::RuntimeMetadataProvider* metadataProvider, ModuleID moduleId, std::wstring pinvokeModuleName, std::wstring injectedClassName, std::wstring prologMethodName, std::wstring epilogMethodName);
					HRESULT InjectByToken(mdMethodDef methodToken);
					HRESULT InjectById(FunctionID functionId);
				private:
					mdMethodDef _prologMethod;
					mdMethodDef _epilogMethod;
					IMetaDataEmit* _metadataEmit;
					IMetaDataImport* _metadataImport;
					IMethodMalloc* _methodAlloc;
					ICorProfilerInfo2* _corProfilerInfo;
					ModuleID _moduleId;
					HRESULT _initializeResult;
			};
// ==================================================================================================================================================
// ==================================================================================================================================================
		}
	}
}