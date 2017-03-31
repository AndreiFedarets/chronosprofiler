#define THREAD_LOGGER_FAST_DICTIONARY
#define CALLSTACK_RAW_EVENTS
#define HIGH_RESOLUTION_TIMER
#define THREAD_LOGGER_TLS
#define LOCK_FREE_UNIT_MANAGER
#define FUNCTION_EVENT_DIRECT_CALL
//#define HIGH_RESOLUTION_SLEEP

#define __FREEOBJ(variable) if (variable != null) { delete variable; variable = null; }
#define __FREEARR(variable) if (variable != null) { delete[] variable; variable = null; }

#define __short __int16
#define __int __int32
#define __long __int64
#define __byte unsigned __int8
#define __ushort unsigned __int16
#define __uint unsigned __int32
#define __ulong unsigned __int64
#define __wchar wchar_t
#define __bool bool
#define null 0


#pragma once
#include <string>
#include <vector>
#include <queue>
#include <map>
#include <list>
#include <cor.h>
#include <corprof.h>

 