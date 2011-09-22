----------------------------------------------------------------
-- Copyright (c) 2010-2011 Zipline Games, Inc. 
-- All Rights Reserved. 
-- http://getmoai.com
----------------------------------------------------------------

MOAI_CLOUD_URL = "http://services.moaicloud.com/ideapplication"
MOAISim.openWindow ( "Cloud", 64, 64 )

function findString ( task )
	print ( task:getString() )
end
task = MOAIHttpTask.new ()
task:setCallback ( findString )
task:httpGet ( MOAI_CLOUD_URL ) 




