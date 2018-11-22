#include "stdafx.h"
#include "Chronos.DotNet.Agent.h"

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
					ExceptionHandlerClause::ExceptionHandlerClause()
					{
						Flags = CorExceptionFlag::COR_ILEXCEPTION_CLAUSE_NONE;
						ClassToken = 0;
						FilterOffset = 0;
						HandlerLength = 0;
						HandlerOffset = 0;
						TryLength = 0;
						TryOffset = 0;
						Next = null;
					}

					ExceptionHandlerClause::ExceptionHandlerClause(IMAGE_COR_ILMETHOD_SECT_EH_CLAUSE_SMALL clause)
					{
						Flags = (CorExceptionFlag)clause.Flags;
						ClassToken = clause.ClassToken;
						FilterOffset = clause.FilterOffset;
						HandlerLength = clause.HandlerLength;
						HandlerOffset = clause.HandlerOffset;
						TryLength = clause.TryLength;
						TryOffset = clause.TryOffset;
						Next = null;
					}

					ExceptionHandlerClause::ExceptionHandlerClause(IMAGE_COR_ILMETHOD_SECT_EH_CLAUSE_FAT clause)
					{
						Flags = (CorExceptionFlag)clause.Flags;
						ClassToken = clause.ClassToken;
						FilterOffset = clause.FilterOffset;
						HandlerLength = clause.HandlerLength;
						HandlerOffset = clause.HandlerOffset;
						TryLength = clause.TryLength;
						TryOffset = clause.TryOffset;
						Next = null;
					}

					ExceptionHandlerClause::~ExceptionHandlerClause()
					{

					}

					void ExceptionHandlerClause::Copy(BYTE* clauseData)
					{
						IMAGE_COR_ILMETHOD_SECT_EH_CLAUSE_FAT* ilClause = (IMAGE_COR_ILMETHOD_SECT_EH_CLAUSE_FAT*)clauseData;
						ilClause->Flags = Flags;
						ilClause->TryOffset = TryOffset;
						ilClause->TryLength = TryLength;
						ilClause->HandlerOffset = HandlerOffset;
						ilClause->HandlerLength = HandlerLength;
						ilClause->ClassToken = ClassToken;
						ilClause->FilterOffset = FilterOffset;
					}
				}
			}
		}
	}
}