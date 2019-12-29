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
	public class OCNetworkClient {
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
