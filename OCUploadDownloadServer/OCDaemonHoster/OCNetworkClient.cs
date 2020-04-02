/*
 * Created by SharpDevelop.
 * User: Alfred
 * Date: 28/12/2019
 * Time: 18:34
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace captainalm.network.oc
{
	public sealed class OCNetworkClient {
		private Socket sock;
		private IPEndPoint remoteAddress;
		private IPEndPoint localAddress;
		private Boolean connected;

		internal OCNetworkClient(Socket socketIn) {
			sock = socketIn;
			if (sock != null) {
				if (sock.Connected) {
					remoteAddress = (IPEndPoint) sock.RemoteEndPoint;
					localAddress = (IPEndPoint) sock.LocalEndPoint;
					connected = true;
				} else {
					connected = false;
				}
			} else {
				connected = false;
			}
		}

		public IPEndPoint getRemoteAddress() {
			return remoteAddress;
		}

		public IPEndPoint getLocalAddress() {
			return localAddress;
		}

		public Socket getSocket() {
			return sock;
		}

		public Boolean sendHandshake(String chIn) {
			if (chIn == null) {
				return false;
			}
			if (connected && chIn.Length == 1) {
				try {
					var bts = System.Text.Encoding.ASCII.GetBytes(chIn.Substring(0,1));
					sock.Send(bts,bts.Length, SocketFlags.None);
					return true;
				} catch (SocketException e) {
					connected = false;
				}
			}
			return false;
		}

		public String receiveProtocol() {
			if (connected) {
				try {
					var bts = new Byte[1];
					sock.Receive(bts,1,SocketFlags.None);
					String prot = System.Text.Encoding.ASCII.GetString(bts);
					return prot;
				} catch (SocketException e) {
					connected = false;
				}
			}
			return "";
		}

		public Boolean receiveHandshake(String chIn) {
			if (chIn == null) {
				return false;
			}
			if (connected && chIn.Length == 1) {
				try {
					var bts = new Byte[1];
					sock.Receive(bts,1,SocketFlags.None);
					Boolean test = System.Text.Encoding.ASCII.GetString(bts).Equals(chIn.Substring(0,1));
					return test;
				} catch (SocketException e) {
					connected = false;
				}
			}
			return false;
		}

		public Boolean sendData(String data) {
			if (data == null) {
				return false;
			}
			if (connected && data.Length > 0) {
				try {
					var bts = System.Text.Encoding.ASCII.GetBytes(data);
					sock.Send(bts,bts.Length, SocketFlags.None);
					return true;
				} catch (SocketException e) {
					connected = false;
				}
			}
			return false;
		}

		public String receiveData() {
			String toret = "";
			if (connected) {
				try {
					int lout = 0;
					while (sock.Available < 1 && lout < 50) {
						try {
							Thread.Sleep(100);
						} catch (ThreadInterruptedException e) {
							break;
						}
						lout++;
					}
					int len = sock.Available;
					byte[] bufferIn = new byte[len];
					int res = sock.Receive(bufferIn,len,SocketFlags.None);
					if (res == 0 && len != 0) {
						connected = false;
					} else {
						connected = true;
					}
					toret = System.Text.Encoding.ASCII.GetString(bufferIn);
				} catch (SocketException e) {
					connected = false;
					toret = "";
				}
			}
			return toret;
		}
		
		public Boolean sendSmallNumber(Int32 numIn) {
			if (numIn > -1 && numIn < 10 && connected) {
				String numStr = numIn.ToString();
				try {
					var bts = System.Text.Encoding.ASCII.GetBytes(numStr.Substring(0, 1));
					sock.Send(bts,bts.Length,SocketFlags.None);
					return true;
				} catch (SocketException e) {
					connected = false;
				}
			}
			return false;
		}
		
		public Boolean sendNumber(Int32 numIn) {
			if (connected) {
				String numStr = numIn.ToString();
				try {
					var bts = System.Text.Encoding.ASCII.GetBytes(numStr);
					sock.Send(bts,bts.Length,SocketFlags.None);
					return true;
				} catch (SocketException e) {
					connected = false;
				}
			}
			return false;
		}
		
		public Int32 receiveSmallNumber() {
			if (connected) {
				try {
					var bts = new Byte[1];
					sock.Receive(bts,1,SocketFlags.None);
					String sn = System.Text.Encoding.ASCII.GetString(bts);
					return Int32.Parse(sn);
				} catch (SocketException e) {
					connected = false;
				} catch (FormatException e) {
				} catch (OverflowException e) {
				}
			}
			return 0;
		}
		
		public Int32 receiveNumber(Int32 lIn) {
			Int32 toret = 0;
			if (connected) {
				try {
					int len = lIn;
					byte[] bufferIn = new byte[len];
					int pos = 0;
					while (pos < len) {
						int res = sock.Receive(bufferIn, pos, len - pos,SocketFlags.None);
						if (res == -1) {
							connected = false;
							break;
						} else {
							pos += res;
							connected = true;
						}
					}
					toret = Int32.Parse(System.Text.Encoding.ASCII.GetString(bufferIn));
				} catch (SocketException e) {
					connected = false;
					toret = 0;
				} catch (FormatException e) {
					toret = 0;
				}catch (OverflowException e) {
					toret = 0;
				}
			}
			return toret;
		}
		
		public String receiveData(Int32 lIn) {
			String toret = "";
			if (connected) {
				try {
					int len = lIn;
					byte[] bufferIn = new byte[len];
					int pos = 0;
					while (pos < len) {
						int res = sock.Receive(bufferIn, pos, len - pos,SocketFlags.None);
						if (res == -1) {
							connected = false;
							break;
						} else {
							pos += res;
							connected = true;
						}
					}
					toret = System.Text.Encoding.ASCII.GetString(bufferIn);
				} catch (SocketException e) {
					connected = false;
					toret = "";
				} catch (FormatException e) {
					toret = "";
				}catch (OverflowException e) {
					toret = "";
				}
			}
			return toret;
		}

		public void invokeConnectionCheck() {
			try {
				byte[] bufferIn = new byte[1];
				int res = sock.Receive(bufferIn,1,SocketFlags.None);
				if (res == 0) {
					connected = false;
				} else {
					connected = true;
				}
			} catch (SocketException e) {
				connected = false;
			}
		}

		public Boolean getIsConnected() {
			return connected;
		}

		public void shutdown() {
			if (connected) {
				try {
					sock.Shutdown(SocketShutdown.Both);
				} catch (SocketException e) {
				}
			}
		}

		public void close() {
			if (connected) {
				try {
					sock.Close();
				} catch (SocketException e) {
				}
			}
			connected = false;
			sock = null;
		}
	}
}
