/*
 * Created by SharpDevelop.
 * User: Alfred
 * Date: 28/12/2019
 * Time: 17:53
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;

using captainalm.network.oc;

namespace OCDaemonHoster
{
	public class Program {
		public static String ipAddress;
		public static int port;
		public static int version = 0;

		public static Dictionary<String, String> settings = new Dictionary<String, String>();
		public static String cache;
		public static List<String> addrsv4;
		public static List<String> addrsv6;

		public static void Main(String[] args) {
			writeLine("Open Computers Daemon Hoster (OCDH) : (C) Captain ALM 2019.");
			writeLine("License: BSD 2-Clause.");
			addrsv4 = getInterfaceAddresses(4);
			addrsv6 = getInterfaceAddresses(6);
			if (args != null) {
				if (args.Length < 2) {
					printUsage();
					Environment.Exit(1);
				} else {
					decryptArgs(args);
					if (settings.ContainsKey("mode")) {
						if (settings["mode"].ToLower().Equals("h")) {
							hoster();
						} else if (settings["mode"].ToLower().Equals("a")) {
							accessor();
						}
					}
				}
			} else {
				printUsage();
				Environment.Exit(1);
			}
			Environment.Exit(0);
		}

		public static void hoster() {
			writeLine("Hosting Mode!");
			IPEndPoint address = new IPEndPoint(IPAddress.Parse(ipAddress),port);
			writeLine("[INFO] : Address Setup!");
			if (settings.ContainsKey("target")) {
				writeLine("[INFO] : Target File : " + settings["target"]);
			}
			cache = "";
			if (settings.ContainsKey("cache") && settings.ContainsKey("target")) {
				try {
					cache = loadFile(settings["target"]);
					writeLine("[INFO] : File Cached!");
				} catch (IOException e) {
				}
			}
			OCNetworkListener server = new OCNetworkListener(address);
			writeLine("[INFO] : Listener Started!");
			writeLine("[INFO] : Listener 'Address:Port' : " + server.getListeningAddress().Address.ToString()
			          + ":" + server.getListeningAddress().Port);
			writeLine("[INFO] : Open Addresses 'Address:Port' :");
			if (version == 4 && ipAddress.Equals(IPAddress.Any.ToString())) {
				foreach (String c in addrsv4) {
					writeLine(c + ":" + port);
				}
			} else if (version == 6 && ipAddress.Equals(IPAddress.IPv6Any.ToString())) {
				foreach (String c in addrsv6) {
					writeLine(c + ":" + port);
				}
			} else if (version == 0 && ipAddress.Equals(IPAddress.IPv6Any.ToString())) {
				List<String> addrsT = new List<String>();
				addrsT.AddRange(addrsv4);
				addrsT.AddRange(addrsv6);
				foreach (String c in addrsT) {
					writeLine(c + ":" + port);
				}
				addrsT.Clear();
				addrsT = null;
			} else {
				writeLine(ipAddress + ":" + port);
			}
			Boolean exec = true;
			while (exec) {
				if (server.getIsThereAcceptedClient()) {
					OCNetworkClient client = server.getAcceptedClient();
					writeLine("[INFO] : Client Accepted!");
					writeLine("[INFO] : Client 'Address:Port' : " + client.getRemoteAddress().Address.ToString()
					          + ":" + client.getRemoteAddress().Port);
					handleProtocol(client);
					server.returnAcceptedClient(client);
					writeLine("[INFO] : Client Disposed!");
				}
				try {
					Thread.Sleep(100);
				} catch (ThreadInterruptedException e) {
					break;
				}
			}
			server.close();
			server = null;
		}

		public static void accessor() {
			throw new NotImplementedException("Method not Implemented.");
		}

		public static void handleProtocol(OCNetworkClient clientIn) {
			String prot = clientIn.receiveProtocol();
			if (prot.Equals("1")) {
				writeLine("[INFO] : Sending...");
				String data = "";
				if (settings.ContainsKey("target") && !settings.ContainsKey("cache")) {
					writeLine("[INFO] : Sending : Loading Data...");
					try {
						data = loadFile(settings["target"]);
					} catch (IOException e) {
						data = "";
					}
				} else if (settings.ContainsKey("cache")) {
					writeLine("[INFO] : Sending : Retrieving Data...");
					data = cache;
				}
				writeLine("[INFO] : Sending : Sending Handshake...");
				clientIn.sendHandshake("1");
				writeLine("[INFO] : Sending : Waiting For Handshake...");
				Boolean hand1Succ = clientIn.receiveHandshake("1");
				if (hand1Succ) {
					writeLine("[INFO] : Sending : Sending Data...");
					clientIn.sendData(data);
					writeLine("[INFO] : Sending : Waiting For Handshake...");
					clientIn.receiveHandshake("1");
				}
			} else if (prot.Equals("2")) {
				writeLine("[INFO] : Receiving...");
				writeLine("[INFO] : Receiving : Sending Handshake...");
				clientIn.sendHandshake("1");
				writeLine("[INFO] : Receiving : Waiting For Data...");
				String data = clientIn.receiveData();
				writeLine("[INFO] : Receiving : Processing Data...");
				if (data.Contains("\r") && !data.Contains("\n")) {
					data = data.Replace("\r", "\r\n");
				}
				if (data.Contains("\n") && !data.Contains("\r")) {
					data = data.Replace("\n", "\r\n");
				}
				if (settings.ContainsKey("cache")) {
					writeLine("[INFO] : Receiving : Caching Data...");
					cache = data;
				}
				if (settings.ContainsKey("target")) {
					writeLine("[INFO] : Receiving : Saving Data...");
					try {
						saveFile(settings["target"], data);
					} catch (IOException e) {
					}
				}
				writeLine("[INFO] : Receiving : Sending Handshake...");
				clientIn.sendHandshake("1");
			}
		}

		public static String loadFile(String target) {
			return System.IO.File.ReadAllText(target, System.Text.Encoding.ASCII);
		}

		public static void saveFile(String target, String contents) {
			System.IO.File.WriteAllText(target,contents,System.Text.Encoding.ASCII);
		}

		public static void decryptArgs(String[] args) {
			try {
				port = Int32.Parse(args[1]);
			} catch (FormatException e) {
				port = 0;
			}
			for (int i = 2; i < args.Length; i++) {
				String carg = args[i];
				Boolean hasEquals = carg.Contains("=");
				Boolean isSwitch = carg.StartsWith("-");
				String cSwitch = "";
				String cValue = "";
				if (isSwitch && !hasEquals) {
					cSwitch = carg.Substring(1).ToLower();
				} else if (isSwitch && hasEquals) {
					cSwitch = carg.Substring(1, carg.IndexOf("=") - 1).ToLower();
					cValue = carg.Substring(carg.IndexOf("=") + 1);
				}
				if (!settings.ContainsKey(cSwitch)) {
					settings.Add(cSwitch, cValue);
				}
			}
			try {
				var ip = IPAddress.Parse(args[0]);
				if (ip.AddressFamily == AddressFamily.InterNetworkV6 && ! args[0].Equals(IPAddress.IPv6Any.ToString())) {
					version = 6;
				} else if (ip.AddressFamily == AddressFamily.InterNetworkV6 && args[0].Equals(IPAddress.IPv6Any.ToString()) && Environment.OSVersion.Version.Major >= 6) {
					version = 0;
				} else if (ip.AddressFamily == AddressFamily.InterNetworkV6 && args[0].Equals(IPAddress.IPv6Any.ToString()) && Environment.OSVersion.Version.Major < 6) {
					version = 6;
				} else {
					version = 4;
				}
			} catch (FormatException ex) {
				if (Environment.OSVersion.Version.Major >= 6) {
					version = 0;
				} else {
					version = 4;
				}
			}
			try {
				ipAddress = verifyInterface(args[0], version);
			} catch (SocketException ex) {
				ipAddress = IPAddress.IPv6Any.ToString();
			}
		}

		public static List<String> getInterfaceAddresses(int ver) {
			List<String> toret = new List<String>();
			var allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
			foreach (NetworkInterface netc in allNetworkInterfaces) {
				if (netc.OperationalStatus == OperationalStatus.Up) {
					var ipInfo = netc.GetIPProperties().UnicastAddresses;
					foreach (UnicastIPAddressInformation cadd in ipInfo) {
						String sadd = "";
						if (cadd.Address.AddressFamily == AddressFamily.InterNetwork && (ver == 4 || ver == 0)) {
							sadd = cadd.Address.ToString();
						} else if (cadd.Address.AddressFamily == AddressFamily.InterNetworkV6 && (ver == 6 || ver == 0)) {
							sadd =  cadd.Address.ToString();
						}
						if (sadd != "") {
							if (sadd.Contains("%")) {
								sadd = sadd.Substring(0, sadd.IndexOf("%"));
							}
							toret.Add(sadd);
						}
					}
				}
			}
			return toret;
		}

		public static String verifyInterface(String inF, int ver) {
			Boolean isContained = false;
			List<String> addrsT = new List<String>();
			if (ver == 4 || ver == 0) {
				addrsT.AddRange(addrsv4);
			} else if (ver == 6 ||  ver == 0) {
				addrsT.AddRange(addrsv6);
			}
			foreach (String c in addrsT) {
				if (c.Equals(inF)) {
					isContained = true;
					break;
				}
			}
			if (! isContained) {
				if (inF.Equals(IPAddress.Any.ToString()) && ver == 4) {
					return IPAddress.Any.ToString();
				}
			}
			addrsT.Clear();
			addrsT = null;
			if (isContained) {
				return inF;
			} else {
				return IPAddress.IPv6Any.ToString();
			}
		}

		public static void printUsage() {
			writeLine("");
			writeLine("Usage:");
			writeLine(
				"OCDH.exe <listening IP Address> <listening Port> [-mode=<MODE>] [-target=<target file path>] [-cache] [-enumeration] [-creation] [-deletion]");
			writeLine("");
			writeLine("-mode=<MODE> : allows to select a Hosting Mode.");
			writeLine("-target=<target file path> : allows to select a file for hosting (File Host Mode Only).");
			writeLine("-cache : caches the target file once (File Host Mode Only).");
			writeLine("-enumeration : allows for file/directory enumeration (File Access Mode Only).");
			writeLine("-creation : allows for file/directory creation (File Access Mode Only).");
			writeLine("-deletion : allows for file/directory deletion (File Access Mode Only).");
			writeLine("");
			writeLine("MODE:");
			writeLine("H : File Host Mode, Hosts a single file for access.");
			writeLine("A : File Access Mode, Allows file system access.");
		}

		public static void write(String stringIn) {
			Console.Out.Write(stringIn);
		}

		public static void writeLine(String stringIn) {
			Console.Out.WriteLine(stringIn);
		}

		public static void writeError(String stringIn) {
			Console.Error.Write(stringIn);
		}

		public static void writeErrorLine(String stringIn) {
			Console.Error.WriteLine(stringIn);
		}
	}
}