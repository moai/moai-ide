// Copyright (c) 2010-2011 Zipline Games, Inc. All Rights Reserved.
// http://getmoai.com

#include "pch.h"
#include <moaicore/MOAIDebug.h>
#include <libjson.h>
#ifdef WIN32
#include <winsock.h>
#endif

//================================================================//
// MOAIDebug
//================================================================//

//----------------------------------------------------------------//
int MOAIDebug::mSocketID = -1;
bool MOAIDebug::mEnginePaused = false;
struct sockaddr_in MOAIDebug::mSocketAddr;

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
	MOAIDebug::mEnginePaused = false;

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
	memset(&MOAIDebug::mSocketAddr, 0, sizeof(MOAIDebug::mSocketAddr));
    MOAIDebug::mSocketAddr.sin_family = AF_INET;
    MOAIDebug::mSocketAddr.sin_port = htons(port);
    int res = inet_pton(AF_INET, target, &MOAIDebug::mSocketAddr.sin_addr);
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
	if (connect(MOAIDebug::mSocketID, (struct sockaddr *)&MOAIDebug::mSocketAddr, sizeof(MOAIDebug::mSocketAddr)) == -1)
	{
		printf("debug: Unable to connect socket to requested endpoint! [winsock error %i]", WSAGetLastError());
		closesocket(MOAIDebug::mSocketID);
		MOAIDebug::mSocketID = -1;
		return;
	}

	// Initalize the Lua hooks.
	lua_sethook(L, MOAIDebug::Callback, LUA_MASKCALL | LUA_MASKRET | LUA_MASKLINE | LUA_MASKCOUNT, 1);

	// At this point the debug hooks are initalized, but we don't want to proceed until the IDE tells
	// us to continue (in case it wants to set up initial breakpoints).
	MOAIDebug::SendWait();
	MOAIDebug::Pause();

	// After we have done the wait-and-pause cycle, we are ready to give control back to the engine.
	return;
}

//----------------------------------------------------------------//
void MOAIDebug::Pause()
{
	MOAIDebug::mEnginePaused = true;
	while (MOAIDebug::mEnginePaused)
	{
		// Receive and handle IDE messages until we get either continue
		// or break.
		MOAIDebug::ReceiveMessage();
	}
}

//----------------------------------------------------------------//
void MOAIDebug::SendWait()
{
	// Sends a wait signal to the IDE.
	std::string wait;
	wait = "{\"ID\":\"wait\"}";
	MOAIDebug::SendMessage(wait);
}

//----------------------------------------------------------------//
void MOAIDebug::SendMessage(std::string data)
{
	// Add the terminators.
	data += "\0\0";

	// Send the data in 256-byte chunks.
	for (int i = 0; i < data.length(); i += 256)
	{
		// Create a buffer.
		char buffer[256];
		const char* raw = data.c_str();
		memset(&buffer, 0, sizeof(buffer));

		// Copy our data.
		int size = 0;
		int a = 0;
		for (a = 0; a < 256; a++)
		{
			buffer[a] = raw[i + a];
			if (raw[i + a] == '\0')
				break; // NULL terminator.
		}
		size = (a == 256) ? 256 : a + 1;
		size = (size < 2) ? 2 : size;

		// Send the burst of data.
		sendto(MOAIDebug::mSocketID, buffer, size, 0, (struct sockaddr *)&MOAIDebug::mSocketAddr, sizeof(MOAIDebug::mSocketAddr));
	}
}

//----------------------------------------------------------------//
void MOAIDebug::ReceiveMessage()
{
	// Receive a single message from the TCP socket and then
	// delegate the data to one of the ReceiveX functions.
	std::string json = "";
	while (true)
	{
		// Get the data.
		char buffer[256];
		memset(&buffer, 0, sizeof(buffer));
		int bytes = recv(MOAIDebug::mSocketID, &buffer[0], 256, 0);
		if (bytes == 0 || bytes == SOCKET_ERROR)
			break;

		// Add to the std::string buffer.
		for (int i = 0; i < bytes; i += 1)
			json += buffer[i];

		// Check to see if the last two bytes received were NULL terminators.
		if (json[json.length() - 1] == '\0' &&
			json[json.length() - 2] == '\0')
			break;
	}

	// We have received a message.  Parse the JSON to work out
	// exactly what type of message it is.
	JSONNode node = libjson::parse(json);
}