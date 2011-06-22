// Copyright (c) 2010-2011 Zipline Games, Inc. All Rights Reserved.
// http://getmoai.com

#include <aku/AKU-luaext.h>
#include <moaicore/moaicore.h>

#include <moaiext-harness/MOAIHarness.h>

//================================================================//
// AKU-harness
//================================================================//

//----------------------------------------------------------------//
void AKUExtLoadHarness () {

	//... bind harness to lua state
	MOAIHarness::HookLua(AKUGetLuaState(), "127.0.0.1", 7018);
}
