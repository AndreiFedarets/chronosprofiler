// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently,
// but are changed infrequently

#pragma once

#pragma comment(lib, "delayimp.lib")
#pragma comment(lib, "Chronos.Agent.lib")

#ifndef STRICT
#define STRICT
#endif

#include "targetver.h"

#define _ATL_APARTMENT_THREADED

#define _ATL_NO_AUTOMATIC_NAMESPACE

#define _ATL_CSTRING_EXPLICIT_CONSTRUCTORS	// some CString constructors will be explicit

#define ATL_NO_ASSERT_ON_DESTROY_NONEXISTENT_WINDOW

#define CHRONOS_DOTNET_EXPORT_API

#include "resource.h"
#include <atlbase.h>
#include <atlcom.h>
#include <atlctl.h>
