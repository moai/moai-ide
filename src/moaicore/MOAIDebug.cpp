// Copyright (c) 2010-2011 Zipline Games, Inc. All Rights Reserved.
// http://getmoai.com

#include "pch.h"
#include <moaicore/MOAIDebug.h>
#ifdef WIN32
#include <winsock.h>
#endif

//================================================================//
// MOAIDebug
//================================================================//

//----------------------------------------------------------------//
int MOAIDebug::mSocketID = -1;

//----------------------------------------------------------------//
void MOAIDebug::Callback(lua_State *L, lua_Debug *ar)
{
	lua_getinfo(L, "nSl", ar);
	int ev = ar->event;
	const char *name = ar->name;
	const char *namewhat = ar->namewhat;
	const char *what = ar->what;
	const char *source = ar->source;
	const char *short_src = ar->short_src;
	int currentline = ar->currentline;
	int nups = ar->nups;
	int linedefined = ar->linedefined;
	int lastlinedefined = ar->lastlinedefined;
	// debugging information is now filled in.
	// compare with set breakpoints etc...

	return;
}

//----------------------------------------------------------------//
void MOAIDebug::HookLua(lua_State* L, const char* target, int port)
{
	MOAIDebug::mSocketID = -1;

#ifdef WIN32
	// Initalize WinSock if required.
	WORD wVersionRequested;
    WSADATA wsaData;
	wVersionRequested = MAKEWORD(2, 2);
    int err = WSAStartup(wVersionRequested, &wsaData);
	if (err != 0)
	{
		printf("debug: Unable to initalize WinSock! [winsock error %i]", WSAGetLastError());
		return;
	}
#endif

	// Attempt to connect to the IDE.  There needs to be
	// a command-line toggle for this so that it doesn't attempt
	// to do it if there's no IDE available.
	MOAIDebug::mSocketID = socket(PF_INET, SOCK_STREAM, IPPROTO_TCP);
	if (MOAIDebug::mSocketID == -1)
	{
		printf("debug: Unable to create socket for debugging! [winsock error %i]", WSAGetLastError());
		return;
	}

	// Resolve the port and target name into a socket address.
	struct sockaddr_in addr;
	memset(&addr, 0, sizeof(addr));
    addr.sin_family = AF_INET;
    addr.sin_port = htons(port);
    int res = inet_pton(AF_INET, target, &addr.sin_addr);
	if (res < 0)
	{
		printf("debug: Unable to connect socket for debugging (invalid address family)! [winsock error %i]", WSAGetLastError());
		closesocket(MOAIDebug::mSocketID);
		MOAIDebug::mSocketID = -1;
		return;
	}
	else if (res == 0)
	{
		printf("debug: Unable to connect socket for debugging (invalid IP address)! [winsock error %i]", WSAGetLastError());
		closesocket(MOAIDebug::mSocketID);
		MOAIDebug::mSocketID = -1;
		return;
	}

	// Connect to the actual IDE.
	if (connect(MOAIDebug::mSocketID, (struct sockaddr *)&addr, sizeof(addr)) == -1)
	{
		printf("debug: Unable to connect socket to requested endpoint! [winsock error %i]", WSAGetLastError());
		closesocket(MOAIDebug::mSocketID);
		MOAIDebug::mSocketID = -1;
		return;
	}

	// Send an attachment call to the IDE (for testing).
	char buffer[256];
	memset(&buffer, 0, sizeof(buffer));
	strcpy(buffer, "{\"ID\":\"excp_internal\",\"ExceptionMessage\":\"This is a test message.\",\"FunctionName\":\"Main\",\"LineNumber\":12,\"FileName\":\"Main.lua\"}\0\0");
	sendto(MOAIDebug::mSocketID, buffer, 129, 0, (struct sockaddr *)&addr, sizeof(addr));

	// Initalize the Lua hooks.
	//lua_sethook(L, MOAIDebug::Callback, LUA_MASKCALL | LUA_MASKRET | LUA_MASKLINE | LUA_MASKCOUNT, 1);

	return;
}