// Copyright (c) 2010-2011 Zipline Games, Inc. All Rights Reserved.
// http://getmoai.com

#ifndef	MOAIHARNESS_H
#define	MOAIHARNESS_H

#include <string>

//================================================================//
// MOAIHarness
//================================================================//
/**	@name	MOAIHarness
	@text	Internal debugging and hooking class.
*/
class MOAIHarness :
	public USGlobalClass < MOAIHarness, USLuaObject > {
private:

	// socket information
	static int		mSocketID;
	static struct sockaddr_in mSocketAddr;

	// pausing and waiting
	static bool     mEnginePaused;
	static void		Pause();

	// callbacks
	static void		Callback(lua_State *L, lua_Debug *ar);

	// message sending
	static void     SendWait();
	static void     SendMessage(std::string data);

	// message receiving
	static void		ReceiveContinue();
	static void		ReceiveBreak();
	static void		ReceiveMessage();

public:

	// hook function
	static void		HookLua(lua_State* L, const char* target, int port);

};

#endif
