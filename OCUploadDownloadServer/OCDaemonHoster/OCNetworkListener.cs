/*
 * Created by SharpDevelop.
 * User: Alfred
 * Date: 28/12/2019
 * Time: 19:08
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;

namespace captainalm.network.oc
{
	public class OCNetworkListener {
		private Socket sSock;
		private Thread lThread;
		private Boolean listening;
		private OCNetworkClient acceptedClient;
		private Boolean cWaiting;
		private Boolean cExists;
		private Object slockcl = new Object();
		private IPEndPoint listeningAddress;

		public OCNetworkListener(IPEndPoint addressIn) {
			lThread = new Thread(new ThreadStart(this.run));
			lThread.IsBackground = true;
			listening = false;
			listeningAddress = addressIn;
			try {
				if (Environment.OSVersion.Version.Major >= 6 && addressIn.Address.Equals(IPAddress.IPv6Any)) {
					sSock = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
					sSock.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, false);
				} else {
					sSock = new Socket(addressIn.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
				}
				sSock.ReceiveBufferSize = Int16.MaxValue;
				sSock.SendBufferSize = Int16.MaxValue;
				sSock.ReceiveTimeout = 5000;
				sSock.SendTimeout = 5000;
			} catch (SocketException e) {
				sSock = null;
			}
			try {
				if (sSock == null) {
					throw new SocketException(0);
				}
				sSock.Bind(addressIn);
				sSock.Listen(1);
				listening = true;
			} catch (SocketException e) {
			}
			if (listening) {
				lThread.Start();
			}
		}

		public OCNetworkClient getAcceptedClient() {
			OCNetworkClient toret = null;
			lock (slockcl) {
				if (cWaiting && acceptedClient != null) {
					cExists = true;
					cWaiting = false;
					toret = acceptedClient;
				}
			}
			return toret;
		}

		public void returnAcceptedClient(OCNetworkClient toRet) {
			lock (slockcl) {
				if (cExists && toRet == acceptedClient) {
					toRet.shutdown();
					toRet.close();
					toRet = null;
					cExists = false;
				}
			}
		}

		public IPEndPoint getListeningAddress() {
			return listeningAddress;
		}

		public Boolean getIsThereGottenClient() {
			return cExists;
		}

		public Boolean getIsThereAcceptedClient() {
			return cWaiting;
		}

		public Boolean getIsListening() {
			return listening;
		}

		public void close() {
			if (acceptedClient != null && (cWaiting == true)) {
				this.getAcceptedClient();
			}
			if (acceptedClient != null && (cExists == true)) {
				this.returnAcceptedClient(acceptedClient);
			}
			if (listening) {
				sSock.Close();
			}
			listening = false;
			while (lThread.IsAlive) {
				try {
					Thread.Sleep(100);
				} catch (ThreadInterruptedException e) {
					break;
				}
			}
			sSock = null;
		}
		
		protected void run() {
			while (listening) {
				while (cExists) {
					try {
						Thread.Sleep(100);
					} catch (ThreadInterruptedException e) {
						break;
					}
				}
				try {
					Socket sa = sSock.Accept();
					sa.ReceiveBufferSize = Int16.MaxValue;
					sa.SendBufferSize = Int16.MaxValue;
					sa.ReceiveTimeout = 5000;
					sa.SendTimeout = 5000;
					acceptedClient = new OCNetworkClient(sa);
					cWaiting = true;
					while (cWaiting) {
						try {
							Thread.Sleep(100);
						} catch (ThreadInterruptedException e) {
							break;
						}
					}
				} catch (SocketException e) {
				}
			}
		}
	}
}
