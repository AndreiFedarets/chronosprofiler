#include "stdafx.h"
#include "Chronos.DotNet.Agent.h"

#define DefineTokenShortStatic(token, argSize, opCode) OpCode* OpCodes::opCode = OpCodes::Define(#opCode, token, sizeof(__byte), argSize, null);
#define DefineTokenLargeStatic(token, argSize, opCode) OpCode* OpCodes::opCode = OpCodes::Define(#opCode, token, sizeof(__ushort), argSize, null);
#define DefineTokenShortDynamic(token, getArgSize, opCode) OpCode* OpCodes::opCode = OpCodes::Define(#opCode, token, sizeof(__byte), -1, getArgSize);

namespace Chronos
{
	namespace Agent
	{
		namespace DotNet
		{
			namespace Reflection
			{
				namespace Emit
				{
					__byte GetSwitchValueSize(__byte* code)
					{
						__uint count = *code;
						return count * sizeof(__uint);
					};

					OpCode* OpCodes::Read(__byte* data)
					{
						__ushort token = (__byte)*data;
						if (token == 0xFE) //2-bytes OpCode first byte
						{
							token = (token << 8) | (__ushort)*(data + 1);
						}
						__map<__ushort, OpCode*>::iterator i = Items->find(token);
						OpCode* opCode = null;
						if (i != Items->end())
						{
							opCode = i->second;
						}
						return opCode;
					}


					OpCode* OpCodes::Define(char* name, __ushort token, __ushort tokenSize, __byte valueSize, GetOpCodeValueSize getValueSize)
					{
						__map<__ushort, OpCode*>::iterator i = Items->find(token);
						OpCode* opCode = null;
						if (i == Items->end())
						{
							opCode = new OpCode();
							opCode->Name = name;
							opCode->Token = token;
							opCode->TokenSize = tokenSize;
							opCode->ValueSize = valueSize;
							opCode->GetValueSize = getValueSize;
							Items->insert(std::pair<__ushort, OpCode*>(opCode->Token, opCode));
						}
						else
						{
							opCode = i->second;
						}
						return opCode;
					}
					__map<__ushort, OpCode*>* OpCodes::Items = new __map<__ushort, OpCode*>();

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
				}
			}
		}
	}
}