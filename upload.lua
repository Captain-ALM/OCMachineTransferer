local ic = require("internet")
local c = require("component")
local shell = require("shell")
local fs = require("filesystem")
local os = require("os")

io.write("upload Utility (C) Captain ALM 2020 :: BSD 2-Clause\n")

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
if #args == 0 then
  io.write("Usage: upload <filename> <ipaddress> <port>")
  return
elseif #args == 1 then
  ipaddress = "127.0.0.1"
  port = 100
elseif #args == 2 then
  ipaddress = args[2]
  port = 100
elseif #args == 3 then
  ipaddress = args[2]
  port = math.floor(math.abs(args[3]))
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

local function shandshake(con)
  print("Sending Handshake...\n")
  con:write("2")
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

local function rmsgs(con)
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
    print("Message Failed!\n")
    return false
  else
    if ret == "" or ret == "0" then
	  print("Message Failed!\n")
	  return false
	else
	  print("Message Succeeded!\n")
	  return true
	end
  end
end

print("Opening Connection!\n")
local connection = ic.open(ipaddress, port)

if connection then
  try {
    function()
      connection:setTimeout(5)
      shandshake(connection)
      if not rhandshake(connection) then
        connection:close()
        return 1
      end
      print("Reading File...\n")
      local f, reason = io.open(filename, "r")
      local data = ""
      if f then
        data = f:read("*a")
        f:close()
      else
        print("File Read Failed!\n")
    	connection:close()
    	return 1
      end
      print("Sending Data...\n")
      connection:write(data)
      rmsgs(connection)
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