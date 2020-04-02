local ic = require("internet")
local c = require("component")
local shell = require("shell")
local fs = require("filesystem")

io.write("delete Utility (C) Captain ALM 2020 :: BSD 2-Clause\n")

if not c.isAvailable("internet") then
  io.stderr:write("Internet Card Required!\n")
  return 2
end

local i = c.internet

if(i == nil or ic == nil) then
  io.stderr:write("Internet Component/API Required!\n")
  return 2
end
if(not i.isTcpEnabled()) then
  io.stderr:write("TCP Support Required!\n")
  return 2
end
local args, options = shell.parse(...)
local ipaddress = "127.0.0.1"
local port = 100
if #args < 1 then
  io.write("Usage: delete <remote file/folder name> <ipaddress> <port>\n")
  return
elseif #args == 1 then
  ipaddress = "127.0.0.1"
  port = 100
elseif #args == 2 then
  ipaddress = args[2]
  port = 100
elseif #args >= 3 then
  ipaddress = args[2]
  port = math.floor(math.abs(args[3]))
end

function catch(block)
   return block[1]
end

function try(block)
   status, result = pcall(block[1])
   if not status then
      block[2](result)
   end
   return result
end

local function shandshake(con,val)
  print("Sending Handshake...\n")
  con:write(val)
  return nil
 end

local function rhandshake(con)
  local ret = nil
  try {
    function()
	  ret = con:read(1)
	end,
	catch {
	  function(ex)
	    ret = nil
	  end
	}
  }
  if ret == nil then
    print("Handshake Not Received!\n")
    return false
  else
    if ret == "" or ret == "0" then
	  print("Handshake Not Received!\n")
	  return false
	else
	  print("Handshake Received...\n")
	  return true
	end
  end
end

local function senddata(con,data)
  local datlen = tostring(string.len(data))
  con:write(tostring(string.len(datlen)))
  con:write(datlen)
  con:write(data)
end

local function recsmallnum(con)
  local ret = 0
  try {
    function()
	  ret = con:read(1)
	end,
	catch {
	  function(ex)
	    ret = 0
      end
	}
  }
  return ret
 end
 
local function recdata(con,datlen)
  local ret = ""
  try {
    function()
	  ret = tostring(con:read(datlen))
	end,
	catch {
	  function(ex)
	    ret = ""
      end
	}
  }
  if (ret == "" and datlen > 0) then
    return nil
  else
    return ret
  end
end

local function executer(connection)
  connection:setTimeout(5)
  shandshake(connection,"3")
  if not rhandshake(connection) then
    connection:close()
    return 1
  end
  shandshake(connection,"4")
  if not rhandshake(connection) then
    connection:close()
    return 1
  end
  print("Sending Path...\n")
  senddata(connection,args[1])
  if not rhandshake(connection) then
    connection:close()
    return 1
  end
end
 
print("Opening Connection!\n")
local connection = ic.open(ipaddress, port)

if connection then
  try {
      function()
	    executer(connection)
	  end,
    catch {
      function(ex)
	    io.stderr:write("Error Caught: "..ex..".\n")
      end
    }
  }
  ::ci::
  print("Terminating...\n")
  connection:close()
  return
else
  io.stderr:write("Connection Failed!\n")
  return 1
end