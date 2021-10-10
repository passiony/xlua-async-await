require("framework.System")
logger = require("framework.Logger")

local LuaAsyncTest = require "exmples/LuaAsyncTest";

GameMain = {}

local function Start()
	-- luaide 调试
	-- breakInfoFun, xpcallFun = require("LuaDebug")("localhost", 7003)

    print("GameMain.Start")
    LuaAsyncTest.Run()
end

local function OnApplicationQuit()
end

GameMain.Start = Start
GameMain.OnApplicationQuit = OnApplicationQuit

return GameMain