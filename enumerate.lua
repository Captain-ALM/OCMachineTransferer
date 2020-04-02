local ic = require("internet")
local c = require("component")
local shell = require("shell")
local fs = require("filesystem")

io.write("enumerate Utility (C) Captain ALM 2020 :: BSD 2-Clause\n")

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
local filename = ""
local file_parentpath = ""
local ipaddress = "127.0.0.1"
local port = 100
if #args < 2 then
  io.write("Usage: enumerate <local output file> <remote file/folder name> <ipaddress> <port>\n")
  return
elseif #args == 2 then
  ipaddress = "127.0.0.1"
  port = 100
elseif #args == 3 then
  ipaddress = args[3]
  port = 100
elseif #args >= 4 then
  ipaddress = args[3]
  port = math.floor(math.abs(args[4]))
end

filename = shell.resolve(args[1])
file_parentpath = fs.path(filename)

if fs.exists(file_parentpath) and not fs.isDirectory(file_parentpath) then
  io.stderr:write("Invalid Folder Path!\n")
  return 1
end

if fs.isDirectory(filename) then
  io.stderr:write("File is a directory!\n")
  return 1
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
  shandshake(connection,"5")
  if not rhandshake(connection) then
    connection:close()
    return 1
  end
  print("Sending File Name...\n")
  senddata(connection,args[2])
  shandshake(connection,"1")
  print("Waiting For Data...\n")
  local lld = math.floor(math.abs(recsmallnum(connection)))
  local ld = math.floor(math.abs(recdata(connection,lld)))
  local data = recdata(connection,ld)
  if data == nil then
    print("Message Not Received!\n")
    connection:close()
    return 1
  else
    print("Message Received!\n")
  end
  print("Writing File...\n")
  if not fs.exists(file_parentpath) then
    fs.makeDirectory(file_parentpath)
  end
  local f, reason = io.open(filename, "w")
  if f then
    f:write(data)
    f:flush()
    f:close()
  else
    print("File Write Failed!\n")
    connection:close()
    return 1
  end
  shandshake(connection,"1")
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