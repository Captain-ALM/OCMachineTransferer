local ic = require("internet")
local c = require("component")
local shell = require("shell")
local fs = require("filesystem")
local os = require("os")

if not c.isAvailable("internet") then
  return 2
end

local i = c.internet

if(i == nil or ic == nil) then
  return 2
end
if(not i.isTcpEnabled()) then
  return 2
end
local args, options = shell.parse(...)
local filename = ""
local file_parentpath = ""
local ipaddress = "127.0.0.1"
local port = 100
if #args == 0 then
  io.write("Usage: download <filename> <ipaddress> <port>")
  return
elseif #args == 1 then
  ipaddress = "127.0.0.1"
  port = 100
elseif #args == 2 then
  ipaddress = args[2]
  port = 100
elseif #args == 3 then
  ipaddress = args[2]
  port = args[3] + 0
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
elseif not fs.exists(filename) and (fs.get(filename) == nil or fs.get(filename).isReadOnly()) then
  io.stderr:write("File is read-only!\n")
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

local function readc(con)
  local herr = false
  local data = ""
  while not herr do
    try {
	  function()
	    local tmp = ""
		tmp = con:read(1)
		if tmp ~= nil then
		  if tmp ~= "" then
		    data = data..tmp
			con:setTimeout(0)
		  end
		end
	  end,
	  catch {
	    function(ex)
		  herr = true
		end
	  }
	}
  end
  if data == "" then
    data = nil
  end
  con:setTimeout(5)
  return data
end

local function shandshake(con)
  print("Sending Handshake...\n")
  con:write("1")
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

local function rmsg(con)
  local ret = readc(con)
  if ret == nil then
    print("Message Not Received!\n")
    return nil
  else
	print("Message Received!\n")
	return ret
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
      shandshake(connection)
	  print("Waiting For Data...\n")
      local data = rmsg(connection)
      if data == nil then
        connection:close()
	    return 1
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
      shandshake(connection)
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