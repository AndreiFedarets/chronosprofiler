#include "stdafx.h"
#include "ILCodeInjector.h"


#define DefineTokenShortStatic(token, argSize, opCode) OpCode* OpCodes::opCode = OpCodes::Define(#opCode, token, sizeof(BYTE), argSize, NULL);
#define DefineTokenLargeStatic(token, argSize, opCode) OpCode* OpCodes::opCode = OpCodes::Define(#opCode, token, sizeof(WORD), argSize, NULL);
#define DefineTokenShortDynamic(token, getArgSize, opCode) OpCode* OpCodes::opCode = OpCodes::Define(#opCode, token, sizeof(BYTE), -1, getArgSize);


namespace Reflection
{
	namespace Emit
	{
		//================================================================================================================
		BYTE GetSwitchValueSize(BYTE* code)
		{
			UINT32 count = *code;
			return count * sizeof(UINT32);
		}

		OpCode* OpCodes::Read(BYTE* data)
		{
			WORD token = (BYTE)*data;
			if (token == 0xFE) //2-bytes OpCode first byte
			{
				token = (token << 8) | (WORD)*(data + 1);
			}
			std::map<WORD, OpCode*>::iterator i = Items->find(token);
			OpCode* opCode = NULL;
			if (i != Items->end())
			{
				opCode = i->second;
			}
			return opCode;
		}

		OpCode* OpCodes::Define(char* name, WORD token, WORD tokenSize, BYTE valueSize, GetOpCodeValueSize getValueSize)
		{
			std::map<WORD, OpCode*>::iterator i = Items->find(token);
			OpCode* opCode = NULL;
			if (i == Items->end())
			{
				opCode = new OpCode();
				opCode->Name = name;
				opCode->Token = token;
				opCode->TokenSize = tokenSize;
				opCode->ValueSize = valueSize;
				opCode->GetValueSize = getValueSize;
				Items->insert(std::pair<WORD, OpCode*>(opCode->Token, opCode));
			}
			else
			{
				opCode = i->second;
			}
			return opCode;
		}
		std::map<WORD, OpCode*>* OpCodes::Items = new std::map<WORD, OpCode*>();

		//0x0*
		DefineTokenShortStatic(0x00, 0, Nop);
		DefineTokenShortStatic(0x01, 0, Break);
		DefineTokenShortStatic(0x02, 0, Ldarg_0);
		DefineTokenShortStatic(0x03, 0, Ldarg_1);
		DefineTokenShortStatic(0x04, 0, Ldarg_2);
		DefineTokenShortStatic(0x05, 0, Ldarg_3);
		DefineTokenShortStatic(0x06, 0, Ldloc_0);
		DefineTokenShortStatic(0x07, 0, Ldloc_1);
		DefineTokenShortStatic(0x08, 0, Ldloc_2);
		DefineTokenShortStatic(0x09, 0, Ldloc_3);
		DefineTokenShortStatic(0x0A, 0, Stloc_0);
		DefineTokenShortStatic(0x0B, 0, Stloc_1);
		DefineTokenShortStatic(0x0C, 0, Stloc_2);
		DefineTokenShortStatic(0x0D, 0, Stloc_3);
		DefineTokenShortStatic(0x0E, 1, Ldarg_S);
		DefineTokenShortStatic(0x0F, 1, Ldarga_S);
		//0x1*
		DefineTokenShortStatic(0x10, 1, Starg_S);
		DefineTokenShortStatic(0x11, 1, Ldloc_S);
		DefineTokenShortStatic(0x12, 1, Ldloca_S);
		DefineTokenShortStatic(0x13, 1, Stloc_S);
		DefineTokenShortStatic(0x14, 0, Ldnull);
		DefineTokenShortStatic(0x15, 0, Ldc_I4_M1);
		DefineTokenShortStatic(0x16, 0, Ldc_I4_0);
		DefineTokenShortStatic(0x17, 0, Ldc_I4_1);
		DefineTokenShortStatic(0x18, 0, Ldc_I4_2);
		DefineTokenShortStatic(0x19, 0, Ldc_I4_3);
		DefineTokenShortStatic(0x1A, 0, Ldc_I4_4);
		DefineTokenShortStatic(0x1B, 0, Ldc_I4_5);
		DefineTokenShortStatic(0x1C, 0, Ldc_I4_6);
		DefineTokenShortStatic(0x1D, 0, Ldc_I4_7);
		DefineTokenShortStatic(0x1E, 0, Ldc_I4_8);
		DefineTokenShortStatic(0x1F, 1, Ldc_I4_S);
		//0x2*
		DefineTokenShortStatic(0x20, 4, Ldc_I4);
		DefineTokenShortStatic(0x21, 8, Ldc_I8);
		DefineTokenShortStatic(0x22, 4, Ldc_R4);
		DefineTokenShortStatic(0x23, 8, Ldc_R8);
		DefineTokenShortStatic(0x25, 0, Dup);
		DefineTokenShortStatic(0x26, 0, Pop);
		DefineTokenShortStatic(0x27, 4, Jmp);
		DefineTokenShortStatic(0x28, 4, Call);
		DefineTokenShortStatic(0x29, 4, Calli); // 4?
		DefineTokenShortStatic(0x2A, 0, Ret);
		DefineTokenShortStatic(0x2B, 1, Br_S);
		DefineTokenShortStatic(0x2C, 1, Brfalse_S);
		DefineTokenShortStatic(0x2D, 1, Brtrue_S);
		DefineTokenShortStatic(0x2E, 1, Beq_S);
		DefineTokenShortStatic(0x2F, 1, Bge_S);
		//0x3*
		DefineTokenShortStatic(0x30, 1, Bgt_S);
		DefineTokenShortStatic(0x31, 1, Ble_S);
		DefineTokenShortStatic(0x32, 1, Blt_S);
		DefineTokenShortStatic(0x33, 1, Bne_Un_S);
		DefineTokenShortStatic(0x34, 1, Bge_Un_S);
		DefineTokenShortStatic(0x35, 1, Bgt_Un_S);
		DefineTokenShortStatic(0x36, 1, Ble_Un_S);
		DefineTokenShortStatic(0x37, 1, Blt_Un_S);
		DefineTokenShortStatic(0x38, 4, Br);
		DefineTokenShortStatic(0x39, 4, Brfalse);
		DefineTokenShortStatic(0x3A, 4, Brtrue);
		DefineTokenShortStatic(0x3B, 4, Beq);
		DefineTokenShortStatic(0x3C, 4, Bge);
		DefineTokenShortStatic(0x3D, 4, Bgt);
		DefineTokenShortStatic(0x3E, 4, Ble);
		DefineTokenShortStatic(0x3F, 4, Blt);
		//0x4*
		DefineTokenShortStatic(0x40, 4, Bne_Un);
		DefineTokenShortStatic(0x41, 4, Bge_Un);
		DefineTokenShortStatic(0x42, 4, Bgt_Un);
		DefineTokenShortStatic(0x43, 4, Ble_Un);
		DefineTokenShortStatic(0x44, 4, Blt_Un);
		DefineTokenShortDynamic(0x45, GetSwitchValueSize, Switch);
		DefineTokenShortStatic(0x46, 0, Ldind_I1);
		DefineTokenShortStatic(0x47, 0, Ldind_U1);
		DefineTokenShortStatic(0x48, 0, Ldind_I2);
		DefineTokenShortStatic(0x49, 0, Ldind_U2);
		DefineTokenShortStatic(0x4A, 0, Ldind_I4);
		DefineTokenShortStatic(0x4B, 0, Ldind_U4);
		DefineTokenShortStatic(0x4C, 0, Ldind_I8);
		DefineTokenShortStatic(0x4D, 0, Ldind_I);
		DefineTokenShortStatic(0x4E, 0, Ldind_R4);
		DefineTokenShortStatic(0x4F, 0, Ldind_R8);
		//0x5*
		DefineTokenShortStatic(0x50, 0, Ldind_Ref);
		DefineTokenShortStatic(0x51, 0, Stind_Ref);
		DefineTokenShortStatic(0x52, 0, Stind_I1);
		DefineTokenShortStatic(0x53, 0, Stind_I2);
		DefineTokenShortStatic(0x54, 0, Stind_I4);
		DefineTokenShortStatic(0x55, 0, Stind_I8);
		DefineTokenShortStatic(0x56, 0, Stind_R4);
		DefineTokenShortStatic(0x57, 0, Stind_R8);
		DefineTokenShortStatic(0x58, 0, Add);
		DefineTokenShortStatic(0x59, 0, Sub);
		DefineTokenShortStatic(0x5A, 0, Mul);
		DefineTokenShortStatic(0x5B, 0, Div);
		DefineTokenShortStatic(0x5C, 0, Div_Un);
		DefineTokenShortStatic(0x5D, 0, Rem);
		DefineTokenShortStatic(0x5E, 0, Rem_Un);
		DefineTokenShortStatic(0x5F, 0, And);
		//0x6*
		DefineTokenShortStatic(0x60, 0, Or);
		DefineTokenShortStatic(0x61, 0, Xor);
		DefineTokenShortStatic(0x62, 0, Shl);
		DefineTokenShortStatic(0x63, 0, Shr);
		DefineTokenShortStatic(0x64, 0, Shr_Un);
		DefineTokenShortStatic(0x65, 0, Neg);
		DefineTokenShortStatic(0x66, 0, Not);
		DefineTokenShortStatic(0x67, 0, Conv_I1);
		DefineTokenShortStatic(0x68, 0, Conv_I2);
		DefineTokenShortStatic(0x69, 0, Conv_I4);
		DefineTokenShortStatic(0x6A, 0, Conv_I8);
		DefineTokenShortStatic(0x6B, 0, Conv_R4);
		DefineTokenShortStatic(0x6C, 0, Conv_R8);
		DefineTokenShortStatic(0x6D, 0, Conv_U4);
		DefineTokenShortStatic(0x6E, 0, Conv_U8);
		DefineTokenShortStatic(0x6F, 4, Callvirt);
		//0x7*
		DefineTokenShortStatic(0x70, 4, Cpobj);
		DefineTokenShortStatic(0x71, 4, Ldobj);
		DefineTokenShortStatic(0x72, 4, Ldstr);
		DefineTokenShortStatic(0x73, 4, Newobj);
		DefineTokenShortStatic(0x74, 4, Castclass);
		DefineTokenShortStatic(0x75, 4, Isinst);
		DefineTokenShortStatic(0x76, 0, Conv_R_Un);
		DefineTokenShortStatic(0x79, 4, Unbox);
		DefineTokenShortStatic(0x7A, 0, Throw);
		DefineTokenShortStatic(0x7B, 4, Ldfld);
		DefineTokenShortStatic(0x7C, 4, Ldflda);
		DefineTokenShortStatic(0x7D, 4, Stfld);
		DefineTokenShortStatic(0x7E, 4, Ldsfld);
		DefineTokenShortStatic(0x7F, 4, Ldsflda);
		//0x8*
		DefineTokenShortStatic(0x80, 4, Stsfld);
		DefineTokenShortStatic(0x81, 4, Stobj);
		DefineTokenShortStatic(0x82, 0, Conv_Ovf_I1_Un);
		DefineTokenShortStatic(0x83, 0, Conv_Ovf_I2_Un);
		DefineTokenShortStatic(0x84, 0, Conv_Ovf_I4_Un);
		DefineTokenShortStatic(0x85, 0, Conv_Ovf_I8_Un);
		DefineTokenShortStatic(0x86, 0, Conv_Ovf_U1_Un);
		DefineTokenShortStatic(0x87, 0, Conv_Ovf_U2_Un);
		DefineTokenShortStatic(0x88, 0, Conv_Ovf_U4_Un);
		DefineTokenShortStatic(0x89, 0, Conv_Ovf_U8_Un);
		DefineTokenShortStatic(0x8A, 0, Conv_Ovf_I_Un);
		DefineTokenShortStatic(0x8B, 0, Conv_Ovf_U_Un);
		DefineTokenShortStatic(0x8C, 4, Box);
		DefineTokenShortStatic(0x8D, 4, Newarr);
		DefineTokenShortStatic(0x8E, 0, Ldlen);
		DefineTokenShortStatic(0x8F, 4, Ldelema);
		//0x9*
		DefineTokenShortStatic(0x90, 0, Ldelem_I1);
		DefineTokenShortStatic(0x91, 0, Ldelem_U1);
		DefineTokenShortStatic(0x92, 0, Ldelem_I2);
		DefineTokenShortStatic(0x93, 0, Ldelem_U2);
		DefineTokenShortStatic(0x94, 0, Ldelem_I4);
		DefineTokenShortStatic(0x95, 0, Ldelem_U4);
		DefineTokenShortStatic(0x96, 0, Ldelem_I8);
		DefineTokenShortStatic(0x97, 0, Ldelem_I);
		DefineTokenShortStatic(0x98, 0, Ldelem_R4);
		DefineTokenShortStatic(0x99, 0, Ldelem_R8);
		DefineTokenShortStatic(0x9A, 0, Ldelem_Ref);
		DefineTokenShortStatic(0x9B, 0, Stelem_I);
		DefineTokenShortStatic(0x9C, 0, Stelem_I1);
		DefineTokenShortStatic(0x9D, 0, Stelem_I2);
		DefineTokenShortStatic(0x9E, 0, Stelem_I4);
		DefineTokenShortStatic(0x9F, 0, Stelem_I8);
		//0xA*
		DefineTokenShortStatic(0xA0, 0, Stelem_R4);
		DefineTokenShortStatic(0xA1, 0, Stelem_R8);
		DefineTokenShortStatic(0xA2, 0, Stelem_Ref);
		DefineTokenShortStatic(0xA3, 4, Ldelem);
		DefineTokenShortStatic(0xA4, 4, Stelem);
		DefineTokenShortStatic(0xA5, 4, Unbox_Any);
		//0xB*
		DefineTokenShortStatic(0xB3, 0, Conv_Ovf_I1);
		DefineTokenShortStatic(0xB4, 0, Conv_Ovf_U1);
		DefineTokenShortStatic(0xB5, 0, Conv_Ovf_I2);
		DefineTokenShortStatic(0xB6, 0, Conv_Ovf_U2);
		DefineTokenShortStatic(0xB7, 0, Conv_Ovf_I4);
		DefineTokenShortStatic(0xB8, 0, Conv_Ovf_U4);
		DefineTokenShortStatic(0xB9, 0, Conv_Ovf_I8);
		DefineTokenShortStatic(0xBA, 0, Conv_Ovf_U8);
		//0xC*
		DefineTokenShortStatic(0xC2, 4, Refanyval);
		DefineTokenShortStatic(0xC3, 0, Ckfinite);
		DefineTokenShortStatic(0xC6, 4, Mkrefany);
		//0xD*
		DefineTokenShortStatic(0xD0, 4, Ldtoken);
		DefineTokenShortStatic(0xD1, 0, Conv_U2);
		DefineTokenShortStatic(0xD2, 0, Conv_U1);
		DefineTokenShortStatic(0xD3, 0, Conv_I);
		DefineTokenShortStatic(0xD4, 0, Conv_Ovf_I);
		DefineTokenShortStatic(0xD5, 0, Conv_Ovf_U);
		DefineTokenShortStatic(0xD6, 0, Add_Ovf);
		DefineTokenShortStatic(0xD7, 0, Add_Ovf_Un);
		DefineTokenShortStatic(0xD8, 0, Mul_Ovf);
		DefineTokenShortStatic(0xD9, 0, Mul_Ovf_Un);
		DefineTokenShortStatic(0xDA, 0, Sub_Ovf);
		DefineTokenShortStatic(0xDB, 0, Sub_Ovf_Un);
		DefineTokenShortStatic(0xDC, 0, Endfinally);
		DefineTokenShortStatic(0xDD, 4, Leave);
		DefineTokenShortStatic(0xDE, 1, Leave_S);
		DefineTokenShortStatic(0xDF, 0, Stind_I);
		//0xE*
		DefineTokenShortStatic(0xE0, 0, Conv_U);
		//0xFE*
		DefineTokenLargeStatic(0xFE00, 0, Arglist);
		DefineTokenLargeStatic(0xFE01, 0, Ceq);
		DefineTokenLargeStatic(0xFE02, 0, Cgt);
		DefineTokenLargeStatic(0xFE03, 0, Cgt_Un);
		DefineTokenLargeStatic(0xFE04, 0, Clt);
		DefineTokenLargeStatic(0xFE05, 0, Clt_Un);
		DefineTokenLargeStatic(0xFE06, 4, Ldftn);
		DefineTokenLargeStatic(0xFE07, 4, Ldvirtftn);
		DefineTokenLargeStatic(0xFE09, 2, Ldarg);
		DefineTokenLargeStatic(0xFE0A, 2, Ldarga);
		DefineTokenLargeStatic(0xFE0B, 2, Starg);
		DefineTokenLargeStatic(0xFE0C, 2, Ldloc);
		DefineTokenLargeStatic(0xFE0D, 2, Ldloca);
		DefineTokenLargeStatic(0xFE0E, 2, Stloc);
		DefineTokenLargeStatic(0xFE0F, 0, Localloc);
		DefineTokenLargeStatic(0xFE11, 0, Endfilter);
		DefineTokenLargeStatic(0xFE12, 1, Unaligned);
		DefineTokenLargeStatic(0xFE13, 0, Volatile);
		DefineTokenLargeStatic(0xFE14, 0, Tailcall);
		DefineTokenLargeStatic(0xFE15, 0, Initobj);
		DefineTokenLargeStatic(0xFE16, 4, Constrained);
		DefineTokenLargeStatic(0xFE17, 0, Cpblk);
		DefineTokenLargeStatic(0xFE18, 0, Initblk);
		DefineTokenLargeStatic(0xFE1A, 0, Rethrow);
		DefineTokenLargeStatic(0xFE1C, 4, Sizeof);
		DefineTokenLargeStatic(0xFE1E, 0, Readonly);

		//================================================================================================================
		Signature* SignatureManager::Alloc()
		{
			Signature* signature = new Signature();
			memset(signature, 0, sizeof(Signature));
			return signature;
		}

		SignatureElement* SignatureManager::AllocElement()
		{
			SignatureElement* element = new SignatureElement();
			memset(element, 0, sizeof(SignatureElement));
			return element;
		}

		void SignatureManager::Release(Signature* signature)
		{
			if (signature == NULL)
			{
				return;
			}
			ReleaseElementChain(signature->Front);
			delete signature;
		}

		void SignatureManager::ReleaseElement(SignatureElement* element)
		{
			if (element == NULL)
			{
				return;
			}
			delete element;
		}

		void SignatureManager::ReleaseElementChain(SignatureElement* chain)
		{
			SignatureElement* current = chain;
			while (current != NULL)
			{
				SignatureElement* next = current->Next;
				delete current;
				current = next;
			}
		}

		Signature* SignatureManager::Read(mdSignature signatureToken, IMetaDataImport* metadataImport)
		{
			PCCOR_SIGNATURE corSignature = NULL;
			ULONG corSignatureSize;
			Signature* signature = NULL;
			if (SUCCEEDED(metadataImport->GetSigFromToken(signatureToken, &corSignature, &corSignatureSize)))
			{
				signature = Read(corSignature);
			}
			return signature;
		}

		Signature* SignatureManager::Read(PCCOR_SIGNATURE corSignature)
		{
			Signature* signature = Alloc();
			CorCallingConvention callConvention;
			corSignature += CorSigUncompressData(corSignature, (ULONG*)&callConvention);
			signature->CallingConvention = callConvention;

			ULONG corElementsCount;
			corSignature += CorSigUncompressData(corSignature, &corElementsCount);
			if ((callConvention & CorCallingConvention::IMAGE_CEE_CS_CALLCONV_LOCAL_SIG) != CorCallingConvention::IMAGE_CEE_CS_CALLCONV_LOCAL_SIG &&
				(callConvention & CorCallingConvention::IMAGE_CEE_CS_CALLCONV_FIELD) != CorCallingConvention::IMAGE_CEE_CS_CALLCONV_FIELD)
			{
				//add return type;
				corElementsCount++;
			}
			SignatureElement* current = NULL;
			for (UINT32 i = 0; i < corElementsCount; i++)
			{
				SignatureElement* previous = current;
				current = ReadElement(&corSignature);
				if (signature->Front == NULL)
				{
					signature->Front = current;
				}
				else
				{
					previous->Next = current;
				}
			}
			return signature;
		}

		SignatureElement* SignatureManager::ReadElement(PCCOR_SIGNATURE* corSignatureRef)
		{
			SignatureElement* element = AllocElement();
			PCCOR_SIGNATURE corSignature = *corSignatureRef;
			CorElementType elementType;
			corSignature += CorSigUncompressElementType(corSignature, &elementType);
			element->ElementType = elementType;
			switch (elementType)
			{
			case ELEMENT_TYPE_VOID:
			case ELEMENT_TYPE_BOOLEAN:
			case ELEMENT_TYPE_CHAR:
			case ELEMENT_TYPE_I1:
			case ELEMENT_TYPE_U1:
			case ELEMENT_TYPE_I2:
			case ELEMENT_TYPE_U2:
			case ELEMENT_TYPE_I4:
			case ELEMENT_TYPE_U4:
			case ELEMENT_TYPE_I8:
			case ELEMENT_TYPE_U8:
			case ELEMENT_TYPE_R4:
			case ELEMENT_TYPE_R8:
			case ELEMENT_TYPE_STRING:
			case ELEMENT_TYPE_VAR:
			case ELEMENT_TYPE_MVAR:
			case ELEMENT_TYPE_TYPEDBYREF:
			case ELEMENT_TYPE_I:
			case ELEMENT_TYPE_U:
			case ELEMENT_TYPE_OBJECT:
			case ELEMENT_TYPE_FNPTR:
			case ELEMENT_TYPE_INTERNAL:
			case ELEMENT_TYPE_MAX:
			case ELEMENT_TYPE_MODIFIER:
			case ELEMENT_TYPE_END:
			case ELEMENT_TYPE_SENTINEL:
			{
				//nothing to do more
				break;
			}
			case ELEMENT_TYPE_VALUETYPE:
			case ELEMENT_TYPE_CLASS:
			case ELEMENT_TYPE_CMOD_REQD:
			case ELEMENT_TYPE_CMOD_OPT:
			{
				mdToken token;
				corSignature += CorSigUncompressToken(corSignature, &token);
				element->ClassToken = token;
				break;
			}
			case ELEMENT_TYPE_SZARRAY:
			case ELEMENT_TYPE_PINNED:
			case ELEMENT_TYPE_PTR:
			case ELEMENT_TYPE_BYREF:
			{
				element->ChildElement = ReadElement(&corSignature);
				break;
			}
			case ELEMENT_TYPE_GENERICINST:
			{
				//parse generic class
				element->ChildElement = ReadElement(&corSignature);
				//parse elements
				ULONG argumentsCount = CorSigUncompressData(corSignature);
				SignatureElement* current = NULL;
				for (ULONG i = 0; i < argumentsCount; i++)
				{
					SignatureElement* previous = current;
					current = ReadElement(&corSignature);
					if (element->ArgumentElement == NULL)
					{
						element->ArgumentElement = current;
					}
					else
					{
						previous->Next = current;
					}
				}
				break;
			}
			case ELEMENT_TYPE_ARRAY:
			{
				//TODO
				break;
			}
			}
			*corSignatureRef = corSignature;
			return element;
		}

		UINT32 SignatureManager::GetRawSize(Signature* signature)
		{
			UINT32 size = sizeof(CorCallingConvention);
			size += GetRawSizeRecurcive(signature->Front);
			return size;
		}

		UINT32 SignatureManager::GetRawSizeRecurcive(SignatureElement* element)
		{
			UINT32 size = 0;
			if (element == NULL)
			{
				return size;
			}
			size += sizeof(CorElementType);
			if (element->ClassToken != 0)
			{
				size += sizeof(mdToken);
			}
			size += GetRawSizeRecurcive(element->ArgumentElement);
			size += GetRawSizeRecurcive(element->ChildElement);
			size += GetRawSizeRecurcive(element->Next);
			return size;
		}

		UINT32 SignatureManager::GetChainCount(SignatureElement* chain)
		{
			UINT32 count = 0;
			SignatureElement* current = chain;
			while (current != NULL)
			{
				count++;
				current = current->Next;
			}
			return count;
		}

		mdSignature SignatureManager::Write(Signature* signature, IMetaDataEmit* metadataEmit)
		{
			UINT32 rawSize = GetRawSize(signature);
			BYTE* corSignatureData = new BYTE[rawSize];
			BYTE* corSignatureDataBegin = corSignatureData;

			corSignatureData += CorSigCompressData(signature->CallingConvention, corSignatureData);

			UINT32 actualElementsCount = GetChainCount(signature->Front);
			UINT32 elementsCount = actualElementsCount;
			if (signature->CallingConvention == CorCallingConvention::IMAGE_CEE_CS_CALLCONV_DEFAULT)
			{
				elementsCount--;
			}
			corSignatureData += CorSigCompressData(elementsCount, corSignatureData);

			SignatureElement* current = signature->Front;
			for (UINT32 i = 0; i < actualElementsCount; i++)
			{
				corSignatureData = WriteElement(current, corSignatureData);
				current = current->Next;
			}

			UINT32 realSize = corSignatureData - corSignatureDataBegin;

			BYTE* temp = new BYTE[realSize];
			memcpy(temp, corSignatureDataBegin, realSize);
			delete[] corSignatureDataBegin;
			PCCOR_SIGNATURE corSignature = (PCCOR_SIGNATURE)temp;

			mdSignature corSignatureToken;
			HRESULT result = metadataEmit->GetTokenFromSig(corSignature, realSize, &corSignatureToken);
			return corSignatureToken;
		}

		BYTE* SignatureManager::WriteElement(SignatureElement* element, BYTE* corSignatureData)
		{
			corSignatureData += CorSigCompressElementType(element->ElementType, corSignatureData);
			switch (element->ElementType)
			{
			case ELEMENT_TYPE_VOID:
			case ELEMENT_TYPE_BOOLEAN:
			case ELEMENT_TYPE_CHAR:
			case ELEMENT_TYPE_I1:
			case ELEMENT_TYPE_U1:
			case ELEMENT_TYPE_I2:
			case ELEMENT_TYPE_U2:
			case ELEMENT_TYPE_I4:
			case ELEMENT_TYPE_U4:
			case ELEMENT_TYPE_I8:
			case ELEMENT_TYPE_U8:
			case ELEMENT_TYPE_R4:
			case ELEMENT_TYPE_R8:
			case ELEMENT_TYPE_STRING:
			case ELEMENT_TYPE_VAR:
			case ELEMENT_TYPE_MVAR:
			case ELEMENT_TYPE_TYPEDBYREF:
			case ELEMENT_TYPE_I:
			case ELEMENT_TYPE_U:
			case ELEMENT_TYPE_OBJECT:
			case ELEMENT_TYPE_FNPTR:
			case ELEMENT_TYPE_INTERNAL:
			case ELEMENT_TYPE_MAX:
			case ELEMENT_TYPE_MODIFIER:
			case ELEMENT_TYPE_END:
			case ELEMENT_TYPE_SENTINEL:
			{
				//nothing to do more
				break;
			}
			case ELEMENT_TYPE_VALUETYPE:
			case ELEMENT_TYPE_CLASS:
			case ELEMENT_TYPE_CMOD_REQD:
			case ELEMENT_TYPE_CMOD_OPT:
			{
				corSignatureData += CorSigCompressToken(element->ClassToken, corSignatureData);
				break;
			}
			case ELEMENT_TYPE_SZARRAY:
			case ELEMENT_TYPE_PINNED:
			case ELEMENT_TYPE_PTR:
			case ELEMENT_TYPE_BYREF:
			{
				corSignatureData = WriteElement(element->ChildElement, corSignatureData);
				break;
			}
			case ELEMENT_TYPE_GENERICINST:
			{
				//parse generic class
				corSignatureData = WriteElement(element->ChildElement, corSignatureData);
				//parse elements
				UINT32 argumentsCount = GetChainCount(element->ArgumentElement);
				corSignatureData += CorSigCompressData(argumentsCount, corSignatureData);
				SignatureElement* current = element->ArgumentElement;
				for (ULONG i = 0; i < argumentsCount; i++)
				{
					corSignatureData = WriteElement(current, corSignatureData);
					current = current->Next;
				}
				break;
			}
			case ELEMENT_TYPE_ARRAY:
			{
				//TODO
				break;
			}
			}
			return corSignatureData;
		}

		UINT32 SignatureManager::InsertElement(Signature* signature, SignatureElement* element)
		{
			UINT32 index = 0;
			if (signature->Front == NULL)
			{
				signature->Front = element;
				return index;
			}
			SignatureElement* final = signature->Front;
			while (final->Next != NULL)
			{
				index++;
				final = final->Next;
			}
			final->Next = element;
			index++;
			return index;
		}

		//================================================================================================================

		ExceptionHandler* ExceptionHandlerManager::Alloc()
		{
			ExceptionHandler* handler = new ExceptionHandler();
			memset(handler, 0, sizeof(ExceptionHandler));
			return handler;
		}

		void ExceptionHandlerManager::Release(ExceptionHandler* handler)
		{
			if (handler == NULL)
			{
				return;
			}
			delete handler;
		}

		void ExceptionHandlerManager::ReleaseChain(ExceptionHandler* chain)
		{
			ExceptionHandler* current = chain;
			while (current != NULL)
			{
				ExceptionHandler* next = current->Next;
				delete current;
				current = next;
			}
		}

		ExceptionHandler* ExceptionHandlerManager::ReadChain(const COR_ILMETHOD_SECT* ilSection, Instruction* instructionChain)
		{
			if (ilSection->IsFat())
			{
				COR_ILMETHOD_SECT_EH_FAT* fatSection = (COR_ILMETHOD_SECT_EH_FAT*)&ilSection->Fat;
				return ReadChainFat(fatSection, instructionChain);
			}
			else
			{
				COR_ILMETHOD_SECT_EH_SMALL* smallSection = (COR_ILMETHOD_SECT_EH_SMALL*)&ilSection->Small;
				return ReadChainSmall(smallSection, instructionChain);
			}
		}

		ExceptionHandler* ExceptionHandlerManager::ReadChainSmall(COR_ILMETHOD_SECT_EH_SMALL* ilSection, Instruction* chain)
		{
			UINT32 count = ilSection->DataSize / sizeof(IMAGE_COR_ILMETHOD_SECT_EH_CLAUSE_SMALL);
			ExceptionHandler* front = NULL;
			ExceptionHandler* current = NULL;
			for (UINT32 i = 0; i < count; i++)
			{
				IMAGE_COR_ILMETHOD_SECT_EH_CLAUSE_SMALL ilClause = ilSection->Clauses[i];
				ExceptionHandler* previous = current;
				current = ReadSmall(&ilClause, chain);
				if (front == NULL)
				{
					front = current;
				}
				else
				{
					previous->Next = current;
				}
			}
			return front;
		}

		ExceptionHandler* ExceptionHandlerManager::ReadChainFat(COR_ILMETHOD_SECT_EH_FAT* ilSection, Instruction* chain)
		{
			UINT32 count = ilSection->DataSize / sizeof(IMAGE_COR_ILMETHOD_SECT_EH_CLAUSE_FAT);
			ExceptionHandler* front = NULL;
			ExceptionHandler* current = NULL;
			for (UINT32 i = 0; i < count; i++)
			{
				IMAGE_COR_ILMETHOD_SECT_EH_CLAUSE_FAT ilClause = ilSection->Clauses[i];
				ExceptionHandler* previous = current;
				current = ReadFat(&ilClause, chain);
				if (front == NULL)
				{
					front = current;
				}
				else
				{
					previous->Next = current;
				}
			}
			return front;
		}

		ExceptionHandler* ExceptionHandlerManager::ReadSmall(IMAGE_COR_ILMETHOD_SECT_EH_CLAUSE_SMALL* ilClause, Instruction* instructionChain)
		{
			ExceptionHandler* handler = Alloc();
			handler->Flags = (CorExceptionFlag)ilClause->Flags;
			handler->ClassToken = ilClause->ClassToken;
			handler->TryBegin = InstructionManager::ByOffset(instructionChain, ilClause->TryOffset);
			handler->TryEnd = InstructionManager::ByOffset(instructionChain, ilClause->TryOffset, ilClause->TryLength);
			handler->HandlerBegin = InstructionManager::ByOffset(instructionChain, ilClause->HandlerOffset);
			handler->HandlerEnd = InstructionManager::ByOffset(instructionChain, ilClause->HandlerOffset, ilClause->HandlerLength);
			if (handler->Flags == CorExceptionFlag::COR_ILEXCEPTION_CLAUSE_FILTER)
			{
				handler->Filter = InstructionManager::ByOffset(instructionChain, ilClause->FilterOffset);
			}
			return handler;
		}

		ExceptionHandler* ExceptionHandlerManager::ReadFat(IMAGE_COR_ILMETHOD_SECT_EH_CLAUSE_FAT* ilClause, Instruction* instructionChain)
		{
			ExceptionHandler* handler = Alloc();
			handler->Flags = (CorExceptionFlag)ilClause->Flags;
			handler->ClassToken = ilClause->ClassToken;
			handler->TryBegin = InstructionManager::ByOffset(instructionChain, ilClause->TryOffset);
			handler->TryEnd = InstructionManager::ByOffset(instructionChain, ilClause->TryOffset, ilClause->TryLength);
			handler->HandlerBegin = InstructionManager::ByOffset(instructionChain, ilClause->HandlerOffset);
			handler->HandlerEnd = InstructionManager::ByOffset(instructionChain, ilClause->HandlerOffset, ilClause->HandlerLength);
			if (handler->Flags == CorExceptionFlag::COR_ILEXCEPTION_CLAUSE_FILTER)
			{
				handler->Filter = InstructionManager::ByOffset(instructionChain, ilClause->FilterOffset);
			}
			return handler;
		}

		UINT32 ExceptionHandlerManager::GetChainSize(ExceptionHandler* chain)
		{
			UINT32 count = GetChainCount(chain);
			return COR_ILMETHOD_SECT_EH_FAT::Size(count);
		}

		UINT32 ExceptionHandlerManager::GetChainCount(ExceptionHandler* chain)
		{
			UINT32 count = 0;
			while (chain != NULL)
			{
				count++;
				chain = chain->Next;
			}
			return count;
		}

		UINT32 ExceptionHandlerManager::WriteTo(ExceptionHandler* handler, BYTE* ilClauseData)
		{
			IMAGE_COR_ILMETHOD_SECT_EH_CLAUSE_FAT* ilClause = (IMAGE_COR_ILMETHOD_SECT_EH_CLAUSE_FAT*)ilClauseData;
			ilClause->Flags = handler->Flags;
			ilClause->TryOffset = InstructionManager::GetOffset(handler->TryBegin);
			ilClause->TryLength = InstructionManager::GetRangeSize(handler->TryBegin, handler->TryEnd);
			ilClause->HandlerOffset = InstructionManager::GetOffset(handler->HandlerBegin);
			ilClause->HandlerLength = InstructionManager::GetRangeSize(handler->HandlerBegin, handler->HandlerEnd);
			ilClause->ClassToken = handler->ClassToken;
			if (ilClause->Flags == CorExceptionFlag::COR_ILEXCEPTION_CLAUSE_FILTER)
			{
				ilClause->FilterOffset = InstructionManager::GetOffset(handler->Filter);
			}
			return sizeof(IMAGE_COR_ILMETHOD_SECT_EH_CLAUSE_FAT);
		}

		void ExceptionHandlerManager::WriteChainTo(ExceptionHandler* chain, BYTE* ilSectionData)
		{
			if (chain == NULL)
			{
				return;
			}
			//NOTE: at this moment we do not support building of Small sections
			//... even if section matches Small requirements it will be converted to Fat
			//... it's fine to CLR
			COR_ILMETHOD_SECT_EH_FAT* ilSection = (COR_ILMETHOD_SECT_EH_FAT*)ilSectionData;

			//set Section Kind
			ilSection->Kind = CorILMethodSect::CorILMethod_Sect_EHTable | CorILMethodSect::CorILMethod_Sect_FatFormat;

			//set Clauses
			UINT32 clausesCount = 0;
			BYTE* ilClausesData = (BYTE*)&ilSection->Clauses;
			ExceptionHandler* current = chain;
			while (current != NULL)
			{
				ilClausesData += WriteTo(current, ilClausesData);
				clausesCount++;
				current = current->Next;
			}

			//set DataSize
			UINT32 dataSize = COR_ILMETHOD_SECT_EH_FAT::Size(clausesCount);
			ilSection->SetDataSize(dataSize);
		}

		ExceptionHandler* ExceptionHandlerManager::DefineTryFinally(Method* method, Instruction* tryBegin, Instruction* tryEnd, Instruction* handlerBegin, Instruction* handlerEnd)
		{
			//first of all we need to update Instructions according CLR rules
			Instruction* endfinallyInstruction = InstructionManager::Create(OpCodes::Endfinally);
			handlerEnd = MethodManager::InsertChainAfter(method, handlerEnd, endfinallyInstruction);

			Instruction* leaveInstruction = InstructionManager::Create(OpCodes::Leave_S);
			tryEnd = MethodManager::InsertChainAfter(method, tryEnd, leaveInstruction);
			leaveInstruction->ByteValue = InstructionManager::GetRangeSize(tryEnd->Next, handlerEnd);
			Instruction* offsetInstruction = InstructionManager::ByOffset(leaveInstruction, leaveInstruction->GetSize() + leaveInstruction->ByteValue);

			//now create and apply handler
			ExceptionHandler* handler = Alloc();
			handler->Flags = CorExceptionFlag::COR_ILEXCEPTION_CLAUSE_FINALLY;
			handler->TryBegin = tryBegin;
			handler->TryEnd = tryEnd;
			handler->HandlerBegin = handlerBegin;
			handler->HandlerEnd = handlerEnd;
			InsertHandler(method, handler);

			return handler;
		}

		void ExceptionHandlerManager::InsertHandler(Method* method, ExceptionHandler* handler)
		{
			if (method->FrontHandler == NULL)
			{
				method->FrontHandler = handler;
				return;
			}
			//if handler if finally - put it back, otherwise - put it on top
			if (handler->Flags == CorExceptionFlag::COR_ILEXCEPTION_CLAUSE_FINALLY)
			{
				ExceptionHandler* current = method->FrontHandler;
				while (current->Next != NULL)
				{
					current = current->Next;
				}
				current->Next = handler;
			}
			else
			{
				handler->Next = method->FrontHandler;
				method->FrontHandler = handler;
			}

		}

		//================================================================================================================


		Method* MethodManager::Alloc()
		{
			Method* method = new Method();
			memset(method, 0, sizeof(Method));
			return method;
		}

		void MethodManager::Release(Method* method)
		{
			if (method == NULL)
			{
				return;
			}
			InstructionManager::ReleaseChain(method->FrontInstruction);
			ExceptionHandlerManager::ReleaseChain(method->FrontHandler);
			delete method;
		}

		Method* MethodManager::Read(IMAGE_COR_ILMETHOD* ilMethod)
		{
			Method* method = NULL;
			COR_ILMETHOD_TINY* tinyMethod = (COR_ILMETHOD_TINY*)&ilMethod->Tiny;
			COR_ILMETHOD_FAT* fatMethod = (COR_ILMETHOD_FAT*)&ilMethod->Fat;
			if (tinyMethod->IsTiny())
			{
				method = ReadTiny(tinyMethod);
			}
			else
			{
				method = ReadFat(fatMethod);
			}
			return method;
		}

		Method* MethodManager::ReadTiny(COR_ILMETHOD_TINY* ilMethod)
		{
			Method* method = Alloc();
			method->FrontInstruction = InstructionManager::ReadChain(ilMethod->GetCode(), ilMethod->GetCodeSize());
			method->MaxStack = ilMethod->GetMaxStack();
			method->LocalVarSigTok = 0;
			return method;
		}

		Method* MethodManager::ReadFat(COR_ILMETHOD_FAT* ilMethod)
		{
			Method* method = Alloc();
			method->FrontInstruction = InstructionManager::ReadChain(ilMethod->GetCode(), ilMethod->GetCodeSize());
			method->MaxStack = ilMethod->GetMaxStack();
			method->LocalVarSigTok = ilMethod->GetLocalVarSigTok();
			const COR_ILMETHOD_SECT* ilSection = ilMethod->GetSect();
			while (ilSection != NULL)
			{
				switch (ilSection->Kind())
				{
				case CorILMethodSect::CorILMethod_Sect_EHTable:
					method->FrontHandler = ExceptionHandlerManager::ReadChain(ilSection, method->FrontInstruction);
					break;
				default:
					// we do not handle other types of section as at this moment CLR do not support anything except exceptions
					break;
				}
				ilSection = ilSection->Next();
			}
			return method;
		}

		UINT32 MethodManager::GetSize(Method* method)
		{
			//NOTE: at this moment we do not support building of Tiny method
			//... even if method matches Tiny requirements it will be converted to Fat
			//... it's fine to CLR
			UINT32 size = sizeof(COR_ILMETHOD_FAT);
			UINT32 codeSize = InstructionManager::GetChainSize(method->FrontInstruction);
			size += AlignCodeSize(codeSize);
			size += ExceptionHandlerManager::GetChainSize(method->FrontHandler);
			return size;
		}

		UINT32 MethodManager::AlignCodeSize(UINT32 size)
		{
			//DWORD alignment
			const UINT32 alignment = 4;
			UINT32 rest = size % alignment;
			if (rest != 0)
			{
				size += alignment - rest;
			}
			return size;
		}

		void MethodManager::WriteTo(Method* method, BYTE* methodData)
		{
			//NOTE: at this moment we do not support building of Tiny method
			//... even if method matches Tiny requirements it will be converted to Fat
			//... it's fine to CLR
			IMAGE_COR_ILMETHOD* ilMethod = (IMAGE_COR_ILMETHOD*)methodData;
			COR_ILMETHOD_FAT* ilFatMethod = (COR_ILMETHOD_FAT*)&ilMethod->Fat;

			unsigned int flags = 0;

			//set flags
			flags = flags | CorILMethodFlags::CorILMethod_FatFormat;
			if (method->LocalVarSigTok != 0)
			{
				flags = flags | CorILMethodFlags::CorILMethod_InitLocals;
			}
			if (method->FrontHandler != NULL)
			{
				flags = flags | CorILMethodFlags::CorILMethod_MoreSects;
			}
			ilFatMethod->SetFlags(flags);

			//set Size
			unsigned int headerSize = sizeof(COR_ILMETHOD_FAT) / 4;
			ilFatMethod->SetSize(headerSize);

			//set MaxStack
			ilFatMethod->SetMaxStack(method->MaxStack);

			UINT32 codeSize = InstructionManager::GetChainSize(method->FrontInstruction);
			//set CodeSize
			ilFatMethod->SetCodeSize(codeSize);

			//set LocalVarSigTok
			ilFatMethod->SetLocalVarSigTok(method->LocalVarSigTok);

			//copy Code
			BYTE* targetCode = ilFatMethod->GetCode();
			InstructionManager::WriteChainTo(method->FrontInstruction, targetCode);

			//set ExceptionSection
			BYTE* sectionData = (BYTE*)ilFatMethod->GetSect();
			ExceptionHandlerManager::WriteChainTo(method->FrontHandler, sectionData);
		}

		Instruction* MethodManager::InsertChainBefore(Method* method, Instruction* instruction, Instruction* chain)
		{
			Instruction* chainFront = InstructionManager::InsertChainBefore(instruction, chain);
			method->FrontInstruction = InstructionManager::MoveToFront(chainFront);
			return chainFront;
		}

		Instruction* MethodManager::InsertChainAfter(Method* method, Instruction* instruction, Instruction* chain)
		{
			Instruction* chainFinal = InstructionManager::InsertChainAfter(instruction, chain);
			return chainFinal;
		}

		//================================================================================================================


		Instruction* InstructionManager::Alloc()
		{
			return Alloc(0);
		}

		Instruction* InstructionManager::Alloc(UINT32 valueSize)
		{
			UINT32 size = sizeof(Instruction);
			if (valueSize > sizeof(UINT64))
			{
				size += valueSize - sizeof(UINT64);
			}
			BYTE* data = new BYTE[size];
			memset(data, 0, size);
			Instruction* instruction = (Instruction*)data;
			instruction->ValueSize = valueSize;
			return instruction;
		}

		void InstructionManager::Release(Instruction* instruction)
		{
			if (instruction == NULL)
			{
				return;
			}
			BYTE* data = (BYTE*)instruction;
			delete[] data;
		}

		void InstructionManager::ReleaseChain(Instruction* instruction)
		{
			instruction = MoveToFront(instruction);
			while (instruction != NULL)
			{
				Instruction* temp = instruction;
				instruction = instruction->Next;
				Release(temp);
			}
		}

		Instruction* InstructionManager::Read(BYTE* ilCode)
		{
			OpCode* opCode = OpCodes::Read(ilCode);
			//__ASSERT(opCode != NULL, L"Unknown OpCode");
			ilCode += opCode->TokenSize;
			Instruction* instruction = Create(opCode, ilCode);
			return instruction;
		}

		Instruction* InstructionManager::ReadChain(BYTE* ilCode, UINT32 ilCodeSize)
		{
			if (ilCodeSize == 0)
			{
				return NULL;
			}
			Instruction* current = NULL;
			Instruction* previous = NULL;
			BYTE* ilCodeEnd = ilCode + ilCodeSize;
			while (ilCode < ilCodeEnd)
			{
				previous = current;
				current = Read(ilCode);
				ilCode += current->GetSize();
				current->Previous = previous;
				if (previous != NULL)
				{
					previous->Next = current;
				}
			}
			current = MoveToFront(current);
			return current;
		}

		Instruction* InstructionManager::Create(OpCode* opCode, BYTE* valueData)
		{
			Instruction* instruction = NULL;
			UINT32 valueSize = 0;
			if (opCode->GetValueSize != NULL)
			{
				valueSize = opCode->GetValueSize(valueData);
			}
			else
			{
				valueSize = opCode->ValueSize;
			}
			instruction = Alloc(valueSize);
			instruction->OpCode = opCode;
			memcpy(&instruction->ArrayValue, valueData, valueSize);
			return instruction;
		}

		Instruction* InstructionManager::Create(OpCode* opCode, BYTE value)
		{
			return Create(opCode, (BYTE*)&value);
		}

		Instruction* InstructionManager::Create(OpCode* opCode)
		{
			UINT64 dummy = 0;
			return Create(opCode, (BYTE*)&dummy);
		}

		Instruction* InstructionManager::Create(OpCode* opCode, WORD value)
		{
			return Create(opCode, (BYTE*)&value);
		}

		Instruction* InstructionManager::Create(OpCode* opCode, UINT32 value)
		{
			return Create(opCode, (BYTE*)&value);
		}

		Instruction* InstructionManager::Create(OpCode* opCode, UINT64 value)
		{
			return Create(opCode, (BYTE*)&value);
		}

		UINT32 InstructionManager::WriteTo(Instruction* instruction, BYTE* data)
		{
			OpCode* opCode = instruction->OpCode;
			if (opCode->TokenSize == 1)
			{
				memcpy(data, (BYTE*)&opCode->TokenData[0], sizeof(BYTE));
				data += sizeof(BYTE);
			}
			else
			{
				memcpy(data, (BYTE*)&opCode->TokenData[1], sizeof(BYTE));
				data += sizeof(BYTE);
				memcpy(data, (BYTE*)&opCode->TokenData[0], sizeof(BYTE));
				data += sizeof(BYTE);
			}
			memcpy(data, (BYTE*)&instruction->ArrayValue, instruction->ValueSize);
			return instruction->GetSize();
		}

		UINT32 InstructionManager::WriteChainTo(Instruction* instruction, BYTE* data)
		{
			UINT32 chainSize = 0;
			while (instruction != NULL)
			{
				chainSize += WriteTo(instruction, data + chainSize);
				instruction = instruction->Next;
			}
			return chainSize;
		}

		UINT32 InstructionManager::GetOffset(Instruction* instruction)
		{
			if (instruction == NULL)
			{
				return 0;
			}
			UINT32 offset = 0;
			while (instruction->Previous != NULL)
			{
				instruction = instruction->Previous;
				offset += instruction->GetSize();
			}
			return offset;
		}

		Instruction* InstructionManager::ByOffset(Instruction* chain, UINT32 offset)
		{
			Instruction* target = NULL;
			Instruction* current = MoveToFront(chain);
			while (current != NULL)
			{
				if (offset == 0)
				{
					target = current;
					break;
				}
				offset -= current->GetSize();
				current = current->Next;
			}
			return target;
		}

		Instruction* InstructionManager::ByOffset(Instruction* chain, UINT32 offset, UINT32 length)
		{
			offset += length;
			Instruction* target = NULL;
			Instruction* current = MoveToFront(chain);
			while (current != NULL)
			{
				offset -= current->GetSize();
				if (offset == 0)
				{
					target = current;
					break;
				}
				current = current->Next;
			}
			return target;
		}

		UINT32 InstructionManager::GetRangeSize(Instruction* instructionFrom, Instruction* instructionTo)
		{
			UINT32 size = 0;
			while (instructionFrom != instructionTo && instructionFrom != NULL)
			{
				size += instructionFrom->GetSize();
				instructionFrom = instructionFrom->Next;
			}
			if (instructionFrom != NULL)
			{
				size += instructionFrom->GetSize();
			}
			return size;
		}

		UINT32 InstructionManager::GetChainSize(Instruction* instruction)
		{
			instruction = MoveToFront(instruction);
			UINT32 size = 0;
			while (instruction != NULL)
			{
				size += instruction->GetSize();
				instruction = instruction->Next;
			}
			return size;
		}

		Instruction* InstructionManager::MoveToFront(Instruction* instruction)
		{
			if (instruction == NULL)
			{
				return NULL;
			}
			while (instruction->Previous != NULL)
			{
				instruction = instruction->Previous;
			}
			return instruction;
		}

		Instruction* InstructionManager::MoveToFinal(Instruction* instruction)
		{
			if (instruction == NULL)
			{
				return NULL;
			}
			while (instruction->Next != NULL)
			{
				instruction = instruction->Next;
			}
			return instruction;
		}

		Instruction* InstructionManager::InsertChainBefore(Instruction* instruction, Instruction* chain)
		{
			Instruction* chainFront = InstructionManager::MoveToFront(chain);
			Instruction* chainFinal = InstructionManager::MoveToFinal(chain);

			Instruction* instructionAfter = instruction->Previous;
			Instruction* instructionBefore = instruction;

			if (instructionAfter != NULL)
			{
				instructionAfter->Next = chainFront;
				chainFront->Previous = instructionAfter;
			}
			if (instructionBefore != NULL)
			{
				instructionBefore->Previous = chainFinal;
				chainFinal->Next = instructionBefore;
			}

			return chainFront;
		}

		Instruction* InstructionManager::InsertChainAfter(Instruction* instruction, Instruction* chain)
		{
			Instruction* chainFront = InstructionManager::MoveToFront(chain);
			Instruction* chainFinal = InstructionManager::MoveToFinal(chain);

			Instruction* instructionAfter = instruction;
			Instruction* instructionBefore = instruction->Next;

			if (instructionAfter != NULL)
			{
				instructionAfter->Next = chainFront;
				chainFront->Previous = instructionAfter;
			}
			if (instructionBefore != NULL)
			{
				instructionBefore->Previous = chainFinal;
				chainFinal->Next = instructionBefore;
			}

			return chainFinal;
		}

		Instruction* InstructionManager::LookForward(Instruction* startFrom, OpCode* targetOpCode)
		{
			Instruction* current = startFrom;
			while (current != NULL && current->OpCode != targetOpCode)
			{
				current = current->Next;
			}
			return current;
		}

		Instruction* InstructionManager::LookBackward(Instruction* startFrom, OpCode* targetOpCode)
		{
			Instruction* current = startFrom;
			while (current != NULL && current->OpCode != targetOpCode)
			{
				current = current->Previous;
			}
			return current;
		}

		//================================================================================================================

		ILCodeInjector::ILCodeInjector()
		{
			_metadataEmit = NULL;
			_metadataImport = NULL;
			_methodAlloc = NULL;
			_corProfilerInfo = NULL;
		}

		ILCodeInjector::~ILCodeInjector()
		{
			if (_metadataEmit != NULL)
			{
				_metadataEmit->Release();
			}
			if (_metadataImport != NULL)
			{
				_metadataImport->Release();
			}
			if (_methodAlloc != NULL)
			{
				_methodAlloc->Release();
			}
		}

		HRESULT ILCodeInjector::Initialize(ICorProfilerInfo2* corProfilerInfo, ModuleID moduleId, std::wstring pinvokeModuleName, std::wstring injectedClassName, std::wstring prologMethodName, std::wstring epilogMethodName)
		{
			_moduleId = moduleId;
			_corProfilerInfo = corProfilerInfo;

			HRESULT _initializeResult = S_OK;
			_initializeResult = _corProfilerInfo->GetILFunctionBodyAllocator(moduleId, &_methodAlloc);
			if (FAILED(_initializeResult)) return _initializeResult;

			_initializeResult = _corProfilerInfo->GetModuleMetaData(moduleId, ofRead | ofWrite, IID_IMetaDataEmit, (IUnknown**)&_metadataEmit);
			if (FAILED(_initializeResult)) return _initializeResult;

			_initializeResult = _corProfilerInfo->GetModuleMetaData(_moduleId, ofRead | ofWrite, IID_IMetaDataImport, (IUnknown **)&_metadataImport);
			if (FAILED(_initializeResult)) return _initializeResult;

			mdAssemblyRef mscorlibToken = 0;
			_initializeResult = GetMscorlibToken(_moduleId, &mscorlibToken);
			if (FAILED(_initializeResult)) return _initializeResult;

			mdModuleRef pinvokeModule;
			_initializeResult = _metadataEmit->DefineModuleRef(pinvokeModuleName.c_str(), &pinvokeModule);
			if (FAILED(_initializeResult)) return _initializeResult;

			mdTypeDef sysObjectToken;
			_initializeResult = _metadataEmit->DefineTypeRefByName(mscorlibToken, L"System.Object", &sysObjectToken);
			if (FAILED(_initializeResult)) return _initializeResult;

			mdTypeDef injectedClassToken;
			_initializeResult = _metadataEmit->DefineTypeDef(injectedClassName.c_str(), tdAbstract | tdSealed, sysObjectToken, NULL, &injectedClassToken);
			if (FAILED(_initializeResult)) return _initializeResult;

			//prologMethod ===============================================
			BYTE prologMethodSignature[] = {
				IMAGE_CEE_CS_CALLCONV_DEFAULT, // Callconv
				1,                             // Argument count
				ELEMENT_TYPE_VOID,             // Return type
				ELEMENT_TYPE_STRING            // Argument type: string
			};
			_initializeResult = _metadataEmit->DefineMethod(injectedClassToken, prologMethodName.c_str(), mdPublic | mdStatic | mdPinvokeImpl, prologMethodSignature, sizeof(prologMethodSignature), 0, miIL | miManaged | miPreserveSig, &_prologMethod);
			if (FAILED(_initializeResult)) return _initializeResult;

			_initializeResult = _metadataEmit->DefinePinvokeMap(_prologMethod, 0, prologMethodName.c_str(), pinvokeModule);
			if (FAILED(_initializeResult)) return _initializeResult;

			mdParamDef prologMethodParam;
			_initializeResult = _metadataEmit->DefineParam(_prologMethod, 1, L"arg0", pdIn | pdHasFieldMarshal, 0, NULL, 0, &prologMethodParam);
			if (FAILED(_initializeResult)) return _initializeResult;

			BYTE paramType = NATIVE_TYPE_LPWSTR;
			_initializeResult = _metadataEmit->SetFieldMarshal(prologMethodParam, &paramType, 1);
			if (FAILED(_initializeResult)) return _initializeResult;

			//epilogMethod ===============================================
			BYTE epilogMethodSignature[] = {
				IMAGE_CEE_CS_CALLCONV_DEFAULT, // Callconv
				0,                             // Argument count
				ELEMENT_TYPE_VOID              // Return type
			};
			_initializeResult = _metadataEmit->DefineMethod(injectedClassToken, epilogMethodName.c_str(), mdPublic | mdStatic | mdPinvokeImpl, epilogMethodSignature, sizeof(epilogMethodSignature), 0, miIL | miManaged | miPreserveSig, &_epilogMethod);
			if (FAILED(_initializeResult)) return _initializeResult;

			_initializeResult = _metadataEmit->DefinePinvokeMap(_epilogMethod, 0, epilogMethodName.c_str(), pinvokeModule);
			if (FAILED(_initializeResult)) return _initializeResult;

			return S_OK;
		}

		HRESULT ILCodeInjector::GetMscorlibToken(ModuleID sampleModuleId, mdAssemblyRef* mscorlibToken)
		{
			std::wstring mscorlibName(L"mscorlib");
			HRESULT result = S_OK;
			IMetaDataAssemblyImport* assemblyImport = NULL;
			result = _corProfilerInfo->GetModuleMetaData(_moduleId, ofRead, IID_IMetaDataAssemblyImport, (IUnknown**)&assemblyImport);
			if (FAILED(result)) return result;

			HCORENUM assemblyEnumerator = NULL;
			mdAssemblyRef assemblyRefs[32]{ 0 };
			ULONG assemblyRefsCount = 0;
			result = assemblyImport->EnumAssemblyRefs(&assemblyEnumerator, assemblyRefs, _countof(assemblyRefs), &assemblyRefsCount);
			if (FAILED(result)) return result;

			for (ULONG i = 0; i < assemblyRefsCount; i++)
			{
				wchar_t assemblyNameData[255]{ 0 };
				ULONG assemblyNameLength = 0;
				HRESULT result = assemblyImport->GetAssemblyRefProps(assemblyRefs[i], NULL, NULL, assemblyNameData, _countof(assemblyNameData), &assemblyNameLength, NULL, NULL, NULL, NULL);
				/*BYTE* publicKeyToken = NULL;
				ULONG publicKeyTokenSize = 0;
				wchar_t assemblyNameData[255]{ 0 };
				ULONG assemblyNameLength = 0;
				char* hashValue = NULL;
				ULONG hashLength = 0;
				DWORD flags = 0;
				HRESULT result = assemblyImport->GetAssemblyRefProps(assemblyRefs[i], (const void**)&publicKeyToken, &publicKeyTokenSize, assemblyNameData, _countof(assemblyNameData), &assemblyNameLength, NULL, (const void**)&hashValue, &hashLength, &flags);*/
				if (FAILED(result))
				{
					continue;
				}
				std::wstring assemblyName(assemblyNameData);
				if (mscorlibName.size() == assemblyName.size())
				{
					if (_wcsnicmp(mscorlibName.c_str(), assemblyName.c_str(), mscorlibName.size()) == 0)
					{
						*mscorlibToken = assemblyRefs[i];
						return S_OK;
					}
				}
			}
			assemblyImport->Release();
			return E_FAIL;
		}

		HRESULT ILCodeInjector::Inject(FunctionID functionId)
		{
			ModuleID moduleId;
			ClassID classId;
			mdMethodDef methodToken;
			HRESULT result = _corProfilerInfo->GetFunctionInfo(functionId, &classId, &moduleId, &methodToken);
			if (FAILED(result)) return result;

			IMAGE_COR_ILMETHOD* originalMethod = NULL;
			ULONG originalMethodSize = 0;

			result = _corProfilerInfo->GetILFunctionBody(_moduleId, methodToken, (LPCBYTE*)&originalMethod, &originalMethodSize);
			if (FAILED(result)) return result;

			PCCOR_SIGNATURE corSignature;
			ULONG corSignatureSize;
			result = _metadataImport->GetMethodProps(methodToken, 0, 0, 0, 0, 0, &corSignature, &corSignatureSize, 0, 0);
			if (FAILED(result)) return result;

			Reflection::Emit::Method* method = Reflection::Emit::MethodManager::Read(originalMethod);
			//Reflection::Emit::MethodManager::WriteDebug(method);

			mdFieldDef commandTextFieldToken = 0;
			//find _commandText field token
			{
				std::wstring commandTextFieldName(L"_commandText");

				mdTypeDef typeToken;
				result = _corProfilerInfo->GetClassIDInfo(classId, &_moduleId, &typeToken);
				if (FAILED(result)) return result;

				result = _metadataImport->FindField(typeToken, commandTextFieldName.c_str(), NULL, 0, &commandTextFieldToken);
			}

			//inject one more local variable of method return type if not VOID
			bool saveReturn;
			BYTE returnLocalIndex;
			{
				Reflection::Emit::Signature* argSignature = Reflection::Emit::SignatureManager::Read(corSignature);
				saveReturn = argSignature->Front->ElementType != CorElementType::ELEMENT_TYPE_VOID;
				if (saveReturn)
				{
					Reflection::Emit::Signature* localSignature = Reflection::Emit::SignatureManager::Read(method->LocalVarSigTok, _metadataImport);
					returnLocalIndex = Reflection::Emit::SignatureManager::InsertElement(localSignature, argSignature->Front);
					mdSignature localVarSigTok = Reflection::Emit::SignatureManager::Write(localSignature, _metadataEmit);
					method->LocalVarSigTok = localVarSigTok;
				}
			}
			//insert prolog
			{
				Reflection::Emit::Instruction* loadThisInstruction = Reflection::Emit::InstructionManager::Create(Reflection::Emit::OpCodes::Ldarg_0);
				Reflection::Emit::MethodManager::InsertChainBefore(method, method->FrontInstruction, loadThisInstruction);

				Reflection::Emit::Instruction* loadCommandTextInstruction = Reflection::Emit::InstructionManager::Create(Reflection::Emit::OpCodes::Ldfld, commandTextFieldToken);
				Reflection::Emit::MethodManager::InsertChainAfter(method, loadThisInstruction, loadCommandTextInstruction);

				Reflection::Emit::Instruction* prologCall = Reflection::Emit::InstructionManager::Create(Reflection::Emit::OpCodes::Call, _prologMethod);
				Reflection::Emit::MethodManager::InsertChainAfter(method, loadCommandTextInstruction, prologCall);
			}
			//insert epilog
			{
				//NOTE: method could have many RET instructions (e.g. in SWITCH). TODO: handle this case
				Reflection::Emit::Instruction* finalInstruction = Reflection::Emit::InstructionManager::MoveToFinal(method->FrontInstruction);
				Reflection::Emit::Instruction* returnInstruction = Reflection::Emit::InstructionManager::LookBackward(finalInstruction, Reflection::Emit::OpCodes::Ret);
				Reflection::Emit::Instruction* insertAfterInstruction = NULL;
				if (returnInstruction == NULL)
				{
					returnInstruction = Reflection::Emit::InstructionManager::Create(Reflection::Emit::OpCodes::Ret);
					finalInstruction = Reflection::Emit::MethodManager::InsertChainAfter(method, finalInstruction, returnInstruction);
				}
				insertAfterInstruction = returnInstruction->Previous;

				if (saveReturn)
				{
					Reflection::Emit::Instruction* setLocalInstruction = Reflection::Emit::InstructionManager::Create(Reflection::Emit::OpCodes::Stloc_S, returnLocalIndex);
					insertAfterInstruction = Reflection::Emit::MethodManager::InsertChainAfter(method, insertAfterInstruction, setLocalInstruction);
				}

				Reflection::Emit::Instruction* epilogCall = Reflection::Emit::InstructionManager::Create(Reflection::Emit::OpCodes::Call, _epilogMethod);
				insertAfterInstruction = Reflection::Emit::MethodManager::InsertChainAfter(method, insertAfterInstruction, epilogCall);

				if (saveReturn)
				{
					Reflection::Emit::Instruction* loadLocalInstruction = Reflection::Emit::InstructionManager::Create(Reflection::Emit::OpCodes::Ldloc_S, returnLocalIndex);
					insertAfterInstruction = Reflection::Emit::MethodManager::InsertChainAfter(method, insertAfterInstruction, loadLocalInstruction);
				}

				Reflection::Emit::ExceptionHandlerManager::DefineTryFinally(method, method->FrontInstruction, epilogCall->Previous, epilogCall, epilogCall);
			}

			//Reflection::Emit::MethodManager::WriteDebug(method);

			ULONG copyMethodSize = Reflection::Emit::MethodManager::GetSize(method);
			BYTE* copyMethodData = (BYTE*)_methodAlloc->Alloc(copyMethodSize);
			Reflection::Emit::MethodManager::WriteTo(method, copyMethodData);
			result = _corProfilerInfo->SetILFunctionBody(_moduleId, methodToken, copyMethodData);
			if (FAILED(result)) return result;

			Reflection::Emit::MethodManager::Release(method);
			return S_OK;
		}

		HRESULT ILCodeInjector::FindFieldToken(ClassID classId, std::wstring fieldName, mdFieldDef* fieldToken)
		{
			return S_OK;
		}
	}
}