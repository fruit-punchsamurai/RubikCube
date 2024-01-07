using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Threading;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Runtime.InteropServices.WindowsRuntime;

public class PythonListener : MonoBehaviour
{
    Thread thread;
    Thread pythonThread;
    Process process = new Process();
    public int connectionPort = 25001;
    TcpListener server;
    TcpClient client;
    bool running;
    public string data = "";
    public string scrambleStep = "";
    public string solveStep = "";

   



    void Start()
    {
        // Receive on a separate thread so Unity doesn't freeze waiting for data
        //ThreadStart ts = new ThreadStart(GetData);
        //thread = new Thread(ts);
        //thread.Start();
    }

    void OnDestroy()
    {
        // Clean up resources (close client and server) when the MonoBehaviour is being destroyed
        running = false;

        SendResponseBeforeClosing();
        // Close the client and server in the main Unity thread

        server?.Stop();
        client?.Close();
        process.Close();
        thread?.Join();
        pythonThread?.Join();// Wait for the thread to finish


        // Send a response message before closing the connection
    }

    void SendResponseBeforeClosing()
    {
        // You can add any additional logic for sending a response before closing the connection
        if (client != null && client.Connected)
        {
            NetworkStream nwStream = client.GetStream();
            string responseMessage = "Closing connection.";
            byte[] responseBytes = Encoding.UTF8.GetBytes(responseMessage);
            nwStream.Write(responseBytes, 0, responseBytes.Length);
        }
    }

    void GetData()
    {
        try
        {
            // Create the server
            server = new TcpListener(IPAddress.Any, connectionPort);
            server.Start();

            // Create a client to get the data stream
            client = server.AcceptTcpClient();

            // Start listening
            running = true;
            while (running)
            {
                running = Connection();
            }
            client?.Close();
            server?.Stop();
            process.Close();
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError($"Error in GetData: {ex.Message}");
        }
    }


    bool Connection()
    {
        // Read data from the network stream
        NetworkStream nwStream = client.GetStream();
        byte[] buffer = new byte[client.ReceiveBufferSize];
        int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);

        // Decode the bytes into a string
        string dataReceived = Encoding.UTF8.GetString(buffer, 0, bytesRead);
        if (dataReceived != null && dataReceived != "")
        {
            data = dataReceived;
            print("yes");
            print(data);

            if (data[0] == 'z')
            {
                data = data.Substring(1);
                scrambleStep = data;
                data = "";
                string response = "Scramble step received";
                byte[] responseByte = Encoding.UTF8.GetBytes(response);
                nwStream.Write(responseByte, 0, responseByte.Length);
                SendScrambleSteps();
                return true;
            }
            else if (data[0] == 'x')
            {
                data = data.Substring(1);
                solveStep = data;
                print(solveStep);
                data = "";
                string response = "solution step received";
                byte[] responseByte = Encoding.UTF8.GetBytes(response);
                nwStream.Write(responseByte, 0, responseByte.Length);
                return false;
            }
        }
        return false;
    }

  

    void Update()
    {

    
    }


    public List<string> StringToList(string solution)
    {
        List<string> solutionList = new List<string>(solution.Split(new string[] { " " }, System.StringSplitOptions.RemoveEmptyEntries));
        return solutionList;
    }

    public void SendScrambleSteps()
    {
        print('1');
        //scrambleStep = "B U2 F2 B2 U' B2 U L' D' F' B D2 B2 R2 U2 D B2 L2 D B2 L2";
        if (scrambleStep != null && scrambleStep != "" && !CubeState.keyMove && !CubeState.autoRotateDrag && !CubeState.drag)
        {
            print(scrambleStep);
            keyboardControl.scrambleSteps = StringToList(scrambleStep);
        }
    }

    public void SendSolutionSteps()
    {
        print('2');
        
        //solveStep = "R L' B' U R' U B L F L2 F' D2 B2 U' D2 F2 L2 D2 F2 L2";
        if (solveStep != null && solveStep != "" && !CubeState.keyMove && !CubeState.autoRotateDrag && !CubeState.drag)
        {
            print(solveStep);
            keyboardControl.solveSteps = StringToList(solveStep);
            solveStep = "";
        }
    }

    

    public void launchPython()
    {
        ThreadStart ts = new ThreadStart(GetData);
        thread = new Thread(ts);
        thread.Start();
        ThreadStart pythonTS = new ThreadStart(launchCamera);
        pythonThread = new Thread(pythonTS);
        pythonThread.Start();
    }
    public void launchCamera()
    {
        string pythonInterpreterPath = "C:\\Users\\nimesh\\anaconda3\\python.exe";

        // Arguments to pass to the Python script
        string pythonArguments = "C:\\testprograms\\RubikCube\\Assets\\Python\\my.py";

        // Create a new process start info
        ProcessStartInfo startInfo = new ProcessStartInfo();
        startInfo.FileName = pythonInterpreterPath;
        startInfo.Arguments = pythonArguments;
        startInfo.RedirectStandardOutput = true;
        startInfo.RedirectStandardError = true;
        startInfo.UseShellExecute = false;
        startInfo.CreateNoWindow = true;

        // Start the process
        
        process.StartInfo = startInfo;
        process.Start();
        //process.BeginOutputReadLine();
        //process.BeginErrorReadLine();
        // Wait for the process to exit
        process.WaitForExit();

    }
}
