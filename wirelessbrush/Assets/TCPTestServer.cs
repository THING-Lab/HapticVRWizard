using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class TCPTestServer : MonoBehaviour {  	
	#region private members 	
	/// <summary> 	
	/// TCPListener to listen for incomming TCP connection 	
	/// requests. 	
	/// </summary> 	
	private TcpListener tcpListener; 
	/// <summary> 
	/// Background thread for TcpServer workload. 	
	/// </summary> 	
	private Thread tcpListenerThread;  	
	/// <summary> 	
	/// Create handle to connected tcp client. 	
	/// </summary> 	
	private TcpClient connectedTcpClient; 	
	#endregion 	
	
	public GameObject objectToAccess;
	private bool haveMessage = false;
	private string clientMessage;
	// public ToolState latestJSON;

	// Use this for initialization
	void Start () { 		
		// Start TcpServer background thread 		
		tcpListenerThread = new Thread (new ThreadStart(ListenForIncommingRequests)); 		
		tcpListenerThread.IsBackground = true; 		
		tcpListenerThread.Start(); 	
	}  	
	
	// Update is called once per frame
	void Update () { 		
		if (Input.GetKeyDown(KeyCode.Space)) {             
			SendMessage();         
		} 	

		if (haveMessage)
		{
			// Global.latestJSON = JsonUtility.FromJson<ToolState>(clientMessage);
			// Debug.Log(Global.latestJSON.tool);
			Debug.Log("Tool is " + Global.latestJSON.tool + " and D1 state is " + Global.latestJSON.D1);
			// objectToAccess.GetComponent<moveKyleTcp>().moveFromTcp(float.Parse(clientMessage));	
			haveMessage = false;
		}
	}  	
	
	/// <summary> 	
	/// Runs in background TcpServerThread; Handles incomming TcpClient requests 	
	/// </summary> 	
	private void ListenForIncommingRequests () { 		
		try { 			
			// Create listener on localhost port 8052. 			
			// tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 8052); // this line came with the example but it only listens for local clients
			tcpListener = new TcpListener(IPAddress.Any, 8052); // this listens to all traffic to port 8052
			// send TCP messages from terminal with `$ netcat localhost 8052`
			tcpListener.Start();              
			Debug.Log("Server is listening");              
			Byte[] bytes = new Byte[1024];  		

			while (true) {
                // ISSUE: I BELIEVE THIS ONLY CREATES ONE THREAD FOR ALL LISTENERS :?
                using (connectedTcpClient = tcpListener.AcceptTcpClient()) {
                Debug.Log("client connected");
                    // Get a stream object for reading
                    using (NetworkStream stream = connectedTcpClient.GetStream()) {
                        StreamReader reader = new StreamReader(stream, Encoding.ASCII);
                        while(true) {
                            string clientMessage = reader.ReadLine();
							Global.latestJSON = JsonUtility.FromJson<ToolState>(clientMessage);
                            // ct = JsonUtility.FromJson<CameraTransform>(json);
                            haveMessage = true;
                        }               
                    }               
                }           
            } 

			// while (true) { 				
			// 	using (connectedTcpClient = tcpListener.AcceptTcpClient()) {
			// 		// Get a stream object for reading
			// 		using (NetworkStream stream = connectedTcpClient.GetStream()) {
			// 			int length;
			// 			// Read incomming stream into byte arrary.
			// 			while ((length = stream.Read(bytes, 0, bytes.Length)) != 0) {
			// 				var incommingData = new byte[length];
			// 				Array.Copy(bytes, 0, incommingData, 0, length);
			// 				// Convert byte array to string message.
			// 				clientMessage = Encoding.ASCII.GetString(incommingData); 							
			// 				Debug.Log("client message received as: " + clientMessage);
			// 				haveMessage = true;
			// 			} 					
			// 		} 				
			// 	} 			
			// } 		
		} 		
		catch (SocketException socketException) { 			
			Debug.Log("SocketException " + socketException.ToString()); 		
		}     
	}  	
	/// <summary> 	
	/// Send message to client using socket connection. 	
	/// </summary> 	
	private void SendMessage() { 		
		if (connectedTcpClient == null) {             
			return;         
		}  		
		
		try { 			
			// Get a stream object for writing. 			
			NetworkStream stream = connectedTcpClient.GetStream(); 			
			if (stream.CanWrite) {                 
				string serverMessage = "This is a message from your server."; 			
				// Convert string message to byte array.                 
				byte[] serverMessageAsByteArray = Encoding.ASCII.GetBytes(serverMessage); 				
				// Write byte array to socketConnection stream.               
				stream.Write(serverMessageAsByteArray, 0, serverMessageAsByteArray.Length);               
				Debug.Log("Server sent his message - should be received by client");           
			}       
		} 		
		catch (SocketException socketException) {             
			Debug.Log("Socket exception: " + socketException);         
		} 	
	} 
}

public class ToolState {
	public string tool;
	public int D0;
	public int D1;
	public int D2;
	public int D5;
	public int D6;
	public int D7;
	public int D8;
	
	public ToolState() {
		// Debug.Log("doing something"); // nothing
	}
		

}

public class Global {
	public static ToolState latestJSON;
}