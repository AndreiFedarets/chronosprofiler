#pragma once
#include <cor.h>
#include <corprof.h>
#include <corhlpr.h>
#include <string>
#include <map>

namespace Reflection
{
	namespace Emit
	{

#define UINT32 unsigned __int32
#define UINT64 unsigned __int64

		// ==================================================================================================================================================
		typedef BYTE(*GetOpCodeValueSize)(BYTE* code);
		struct OpCode
		{
		public:
			char* Name;
			union
			{
				WORD Token;
				BYTE TokenData[2];
			};
			BYTE TokenSize;
			BYTE ValueSize;
			GetOpCodeValueSize GetValueSize;
		};

		// ==================================================================================================================================================
		class OpCodes
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

			static OpCode* Read(BYTE* data);

			static OpCode* Define(char* name, WORD token, WORD tokenSize, BYTE valueSize, GetOpCodeValueSize getValueSize);

		private:
			static std::map<WORD, OpCode*>* Items;
		};

		// ==================================================================================================================================================
		struct Instruction
		{
			OpCode* OpCode;
			Instruction* Previous;
			Instruction* Next;
			UINT32 ValueSize;
			union {
				BYTE ByteValue;
				WORD ShortValue;
				UINT32 IntValue;
				UINT64 LongValue;
				//just definition of array, it could have bigger size
				BYTE ArrayValue[8];
			};
			UINT32 GetSize() { return OpCode->TokenSize + ValueSize; }
		};

		// ==================================================================================================================================================
		struct ExceptionHandler
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
		struct SignatureElement
		{
			CorElementType ElementType;
			mdToken ClassToken;
			SignatureElement* ChildElement;
			SignatureElement* ArgumentElement;
			SignatureElement* Next;
		};

		// ==================================================================================================================================================
		struct Signature
		{
			CorCallingConvention CallingConvention;
			SignatureElement* Front;
		};

		// ==================================================================================================================================================
		struct Method
		{
			UINT32 MaxStack;
			mdSignature LocalVarSigTok;
			Instruction* FrontInstruction;
			ExceptionHandler* FrontHandler;
		};

		// ==================================================================================================================================================
		class InstructionManager
		{
		public:
			static Instruction* Alloc();
			static Instruction* Alloc(UINT32 valueSize);
			static void ReleaseChain(Instruction* chain);
			static Instruction* ByOffset(Instruction* chain, UINT32 offset);
			static Instruction* ByOffset(Instruction* chain, UINT32 offset, UINT32 length);
			static UINT32 GetRangeSize(Instruction* instructionFrom, Instruction* instructionTo);
			static Instruction* ReadChain(BYTE* ilCode, UINT32 ilCodeSize);
			static Instruction* Create(OpCode* opCode, BYTE* valueData);
			static Instruction* Create(OpCode* opCode);
			static Instruction* Create(OpCode* opCode, BYTE value);
			static Instruction* Create(OpCode* opCode, WORD value);
			static Instruction* Create(OpCode* opCode, UINT32 value);
			static Instruction* Create(OpCode* opCode, UINT64 value);
			static UINT32 GetChainSize(Instruction* instruction);
			static UINT32 WriteTo(Instruction* instruction, BYTE* data);
			static UINT32 WriteChainTo(Instruction* instruction, BYTE* data);
			static UINT32 GetOffset(Instruction* instruction);
			static Instruction* MoveToFront(Instruction* chain);
			static Instruction* MoveToFinal(Instruction* chain);
			static Instruction* LookForward(Instruction* startFrom, OpCode* targetOpCode);
			static Instruction* LookBackward(Instruction* startFrom, OpCode* targetOpCode);
			static Instruction* InsertChainBefore(Instruction* instruction, Instruction* chain);
			static Instruction* InsertChainAfter(Instruction* instruction, Instruction* chain);
		private:
			static void Release(Instruction* instruction);
			static Instruction* Read(BYTE* ilCode);

		};

		// ==================================================================================================================================================
		class ExceptionHandlerManager
		{
		public:
			static ExceptionHandler* Alloc();
			static void Release(ExceptionHandler* handler);
			static void ReleaseChain(ExceptionHandler* chain);
			static ExceptionHandler* ReadChain(const COR_ILMETHOD_SECT* ilSection, Instruction* instructionChain);
			static UINT32 GetChainSize(ExceptionHandler* chain);
			static UINT32 GetChainCount(ExceptionHandler* chain);
			static void WriteChainTo(ExceptionHandler* chain, BYTE* ilSection);
			static ExceptionHandler* DefineTryFinally(Method* method, Instruction* tryBegin, Instruction* tryEnd, Instruction* handlerBegin, Instruction* handlerEnd);
		private:
			static ExceptionHandler* ReadChainSmall(COR_ILMETHOD_SECT_EH_SMALL* ilSection, Instruction* instructionChain);
			static ExceptionHandler* ReadChainFat(COR_ILMETHOD_SECT_EH_FAT* ilSection, Instruction* instructionChain);
			static ExceptionHandler* ReadSmall(IMAGE_COR_ILMETHOD_SECT_EH_CLAUSE_SMALL* ilClause, Instruction* instructionChain);
			static ExceptionHandler* ReadFat(IMAGE_COR_ILMETHOD_SECT_EH_CLAUSE_FAT* ilClause, Instruction* instructionChain);
			static UINT32 WriteTo(ExceptionHandler* handler, BYTE* ilClauseData);
			static void InsertHandler(Method* method, ExceptionHandler* handler);
		};

		// ==================================================================================================================================================
		class SignatureManager
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
			static UINT32 InsertElement(Signature* signature, SignatureElement* element);
		private:
			static UINT32 GetChainCount(SignatureElement* chain);
			static UINT32 GetRawSize(Signature* signature);
			static UINT32 GetRawSizeRecurcive(SignatureElement* element);
			static SignatureElement* ReadElement(PCCOR_SIGNATURE* corSignatureRef);
			static BYTE* WriteElement(SignatureElement* element, BYTE* corSignatureData);
		};

		// ==================================================================================================================================================
		class MethodManager
		{
		public:
			static Method* Alloc();
			static void Release(Method* method);
			static Method* Read(IMAGE_COR_ILMETHOD* ilMethod);
			static UINT32 GetSize(Method* method);
			static void WriteTo(Method* method, BYTE* methodData);
			static Instruction* InsertChainBefore(Method* method, Instruction* instruction, Instruction* chain);
			static Instruction* InsertChainAfter(Method* method, Instruction* instruction, Instruction* chain);
		private:
			static Method* ReadTiny(COR_ILMETHOD_TINY* ilMethod);
			static Method* ReadFat(COR_ILMETHOD_FAT* ilMethod);
			static UINT32 AlignCodeSize(UINT32 size);
		};

		// ==================================================================================================================================================
		class ILCodeInjector
		{
		public:
			ILCodeInjector();
			~ILCodeInjector();
			HRESULT Initialize(ICorProfilerInfo2* corProfilerInfo, ModuleID moduleId, std::wstring pinvokeModuleName, std::wstring injectedClassName, std::wstring prologMethodName, std::wstring epilogMethodName);
			HRESULT Inject(FunctionID functionId);

		private:
			HRESULT FindFieldToken(ClassID classId, std::wstring commandTextFieldName, mdFieldDef* commandTextFieldToken);
			HRESULT GetMscorlibToken(ModuleID sampleModuleId, mdAssemblyRef* mscorlibToken);
			mdMethodDef _prologMethod;
			mdMethodDef _epilogMethod;
			IMetaDataEmit* _metadataEmit;
			IMetaDataImport* _metadataImport;
			IMethodMalloc* _methodAlloc;
			ICorProfilerInfo2* _corProfilerInfo;
			ModuleID _moduleId;
			HRESULT _initializeResult;
		};
	}
}
