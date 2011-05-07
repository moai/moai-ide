// Copyright (c) 2010-2011 Zipline Games, Inc. All Rights Reserved.
// http://getmoai.com

#ifndef	MOAIDEBUG_H
#define	MOAIDEBUG_H

//================================================================//
// MOAIDebug
//================================================================//
/**	@name	MOAIDebug
	@text	Internal debugging and hooking class.
*/
class MOAIDebug :
	public USGlobalClass < MOAIDebug, USLuaObject > {
private:

	// socket information
	static int		mSocketID;

	// callbacks
	static void		Callback(lua_State *L, lua_Debug *ar);

public:

	// hook function
	static void		HookLua(lua_State* L, const char* target, int port);

};

#endif
