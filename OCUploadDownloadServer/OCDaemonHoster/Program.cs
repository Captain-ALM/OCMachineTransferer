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
			writeLine("Open Computers Daemon Hoster (OCDH) : (C) Captain ALM 2020.");
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
			serverRuntime();
		}

		public static void accessor() {
			writeLine("Accessor Mode!");
			serverRuntime();
		}
		
		public static void serverRuntime() {
			IPEndPoint address = new IPEndPoint(IPAddress.Parse(ipAddress),port);
			writeLine("[INFO] : Address Setup!");
			List<String> wl = new List<String>();
			if (settings.ContainsKey("whitelist")) {
				
				wl.AddRange(settings["whitelist"].Split(",".ToCharArray()));
			}
			OCNetworkListener server = new OCNetworkListener(address, wl);
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
			} else if (prot.Equals("3")) {
				write("[INFO] : Access Mode : ");
				clientIn.sendHandshake("1");
				String protam = clientIn.receiveProtocol();
				if (protam.Equals("1") && ! settings.ContainsKey("writeonly")) {
					writeLine("Send");
					writeLine("[INFO] : Sending : Sending Handshake...");
					clientIn.sendHandshake("1");
					writeLine("[INFO] : Sending : Receiving Path...");
					Int32 sl = clientIn.receiveSmallNumber();
					if (sl != 0) {
						Int32 l = clientIn.receiveNumber(sl);
						if (l != 0) {
							String nom = clientIn.receiveData(l);
							writeLine("[INFO] : Reading : " + nom);
							try {
								String data = loadFile(nom);
								writeLine("[INFO] : Sending : Waiting For Handshake...");
								if (clientIn.receiveHandshake("1")) {
									writeLine("[INFO] : Sending : " + nom);
									clientIn.sendSmallNumber(data.Length.ToString().Length);
									clientIn.sendNumber(data.Length);
									clientIn.sendData(data);
								}
								writeLine("[INFO] : Sending : Waiting For Handshake...");
								clientIn.receiveHandshake("1");
							} catch (IOException e) {
							}
						}
					}
				} else if (protam.Equals("2") && ! settings.ContainsKey("readonly")) {
					writeLine("Receive");
					writeLine("[INFO] : Receiving : Sending Handshake...");
					clientIn.sendHandshake("1");
					writeLine("[INFO] : Receiving : Receiving Path...");
					Int32 sl = clientIn.receiveSmallNumber();
					if (sl != 0) {
						Int32 l = clientIn.receiveNumber(sl);
						if (l != 0) {
							String nom = clientIn.receiveData(l);
							writeLine("[INFO] : Receiving : Sending Handshake...");
							clientIn.sendHandshake("1");
							writeLine("[INFO] : Receiving : " + nom);
							sl = clientIn.receiveSmallNumber();
							if (sl != 0) {
								l = clientIn.receiveNumber(sl);
								if (l != 0) {
									String data = clientIn.receiveData(l);
									writeLine("[INFO] : Writing : " + nom);
									try {
										saveFile(nom,data);
										writeLine("[INFO] : Receiving : Sending Handshake...");
										clientIn.sendHandshake("1");
									} catch (IOException e) {
									}
								}
							}
						}
					}
				} else if (protam.Equals("3") && settings.ContainsKey("creation")) {
					writeLine("File Creation");
					writeLine("[INFO] : Creating : Sending Handshake...");
					clientIn.sendHandshake("1");
					writeLine("[INFO] : Creating : Receiving Path...");
					Int32 sl = clientIn.receiveSmallNumber();
					if (sl != 0) {
						Int32 l = clientIn.receiveNumber(sl);
						if (l != 0) {
							String nom = clientIn.receiveData(l);
							writeLine("[INFO] : Creating : " + nom);
							try {
								createFile(nom);
								writeLine("[INFO] : Creating : Sending Handshake...");
								clientIn.sendHandshake("1");
							} catch (IOException e) {
							}
						}
					}
				} else if (protam.Equals("4") && settings.ContainsKey("deletion")) {
					writeLine("Deletion");
					writeLine("[INFO] : Deleting : Sending Handshake...");
					clientIn.sendHandshake("1");
					writeLine("[INFO] : Deleting : Receiving Path...");
					Int32 sl = clientIn.receiveSmallNumber();
					if (sl != 0) {
						Int32 l = clientIn.receiveNumber(sl);
						if (l != 0) {
							String nom = clientIn.receiveData(l);
							writeLine("[INFO] : Deleting : " + nom);
							try {
								deleteFile(nom);
								writeLine("[INFO] : Deleting : Sending Handshake...");
								clientIn.sendHandshake("1");
							} catch (IOException e) {
							}
						}
					}
				} else if (protam.Equals("5") && settings.ContainsKey("enumeration")) {
					writeLine("Enumeration");
					writeLine("[INFO] : Enumerating : Sending Handshake...");
					clientIn.sendHandshake("1");
					writeLine("[INFO] : Enumerating : Receiving Path...");
					Int32 sl = clientIn.receiveSmallNumber();
					if (sl != 0) {
						Int32 l = clientIn.receiveNumber(sl);
						if (l != 0) {
							String nom = clientIn.receiveData(l);
							writeLine("[INFO] : Enumerating : " + nom);
							try {
								String result = "";
								if (Directory.Exists(nom) || File.Exists(nom)) {
									if (File.GetAttributes(nom).HasFlag(FileAttributes.Directory)) {
										List<String> enr = new List<string>(Directory.GetFileSystemEntries(nom));
										enr.Remove(nom);
										if (enr.Count > 0) {
											if (enr.Count == 1) {
												result = enr[0].ToString();
											} else {
												for (int i=0;i<(enr.Count - 1);i++) {
													result = result + enr[i].ToString() + "\r\n";
												}
												result = result + enr[enr.Count - 1].ToString();
											}
										}
										enr.Clear();
										enr = null;
									} else {
										result = new FileInfo(nom).Length.ToString();
									}
								}
								writeLine("[INFO] : Enumerating : Waiting For Handshake...");
								if (clientIn.receiveHandshake("1")) {
									writeLine("[INFO] : Enumerating : Sending Enumeration...");
									clientIn.sendSmallNumber(result.Length.ToString().Length);
									clientIn.sendNumber(result.Length);
									clientIn.sendData(result);
								}
								writeLine("[INFO] : Enumerating : Waiting For Handshake...");
								clientIn.receiveHandshake("1");
							} catch (IOException e) {
							}
						}
					}
				}else if (protam.Equals("6") && settings.ContainsKey("deletion")) {
					writeLine("Directory Creation");
					writeLine("[INFO] : Creating : Sending Handshake...");
					clientIn.sendHandshake("1");
					writeLine("[INFO] : Creating : Receiving Path...");
					Int32 sl = clientIn.receiveSmallNumber();
					if (sl != 0) {
						Int32 l = clientIn.receiveNumber(sl);
						if (l != 0) {
							String nom = clientIn.receiveData(l);
							writeLine("[INFO] : Creating : " + nom);
							try {
								if (! Directory.Exists(nom)) {Directory.CreateDirectory(nom);}
								writeLine("[INFO] : Creating : Sending Handshake...");
								clientIn.sendHandshake("1");
							} catch (IOException e) {
							}
						}
					}
				} else {
					writeLine("Unknown");
					clientIn.sendHandshake("0");
				}
			}
		}

		public static String loadFile(String target) {
			return System.IO.File.ReadAllText(target, System.Text.Encoding.ASCII);
		}

		public static void saveFile(String target, String contents) {
			try {
				var tp = System.IO.Path.GetDirectoryName(target);
				if (! Directory.Exists(tp)) {Directory.CreateDirectory(tp);}
			} catch (ArgumentException e) {
			}
			System.IO.File.WriteAllText(target,contents,System.Text.Encoding.ASCII);
		}
		
		public static void createFile(String target) {
			try {
				var tp = System.IO.Path.GetDirectoryName(target);
				if (! Directory.Exists(tp)) {Directory.CreateDirectory(tp);}
			} catch (ArgumentException e) {
			}
			using (File.Create(target));
		}
		
		public static void deleteFile(String target) {
			if (File.GetAttributes(target).HasFlag(FileAttributes.Directory)) {
				System.IO.Directory.Delete(target);
			} else {
				System.IO.File.Delete(target);
			}
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
				"OCDH.exe <listening IP Address> <listening Port> [-mode=<MODE>] [-whitelist=<IP Address [Seperated By ,]>] [-target=<target file path>] [-cache] [-enumeration] [-creation] [-deletion] [-writeonly] [-readonly]");
			writeLine("");
			writeLine("-mode=<MODE> : allows to select a Hosting Mode.");
			writeLine("-whitelist=<IP Address [Seperated By ,]> : allows IP Address to connect, if there is no whitelist switch then any IP Address can connect.");
			writeLine("-target=<target file path> : allows to select a file for hosting (File Host Mode Only).");
			writeLine("-cache : caches the target file once (File Host Mode Only).");
			writeLine("-enumeration : allows for file/directory enumeration (File Access Mode Only).");
			writeLine("-creation : allows for file/directory creation (File Access Mode Only).");
			writeLine("-deletion : allows for file/directory deletion (File Access Mode Only).");
			writeLine("-readonly : disallows write access for files (File Access Mode Only).");
			writeLine("-writeonly : disallows read access for files (File Access Mode Only).");
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