using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Socket
using System;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;

// Model (暫停 Live2D 相關引用)
// using Live2D;
// using Live2D.Cubism.Core;
// using Live2D.Cubism.Framework;

// Windows setting
using System.Runtime.InteropServices;

/* 
 * Author: Kennard Wang 
 * GitHiub: https://github.com/KennardWang
*/

public class Momose : MonoBehaviour
{
    // Window setting, reference: https://blog.csdn.net/qq_39097425/article/details/81664448
    //[DllImport("user32.dll")]
    //static extern IntPtr SetWindowLong(IntPtr hwnd, int _nIndex, int dwNewLong);
    //[DllImport("user32.dll")]
    //static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
    //[DllImport("user32.dll")]
    //static extern IntPtr GetForegroundWindow();


    // Player setting
    //const uint SHOWWINDOW = 0x0040;
    //const int STYLE = -16;
    //const int BORDER = 1;
    //const int TOP = -1;

    //int GUIwidth = 480;
    //int GUIheight = 360;
    //int GUIposX = 1450;
    //int GUIposY = 650;


    // Socket connect, reference: https://blog.csdn.net/u012234115/article/details/46481845
    Socket clientSocket;
    Socket serverSocket;

    // Receive Data
    byte[] recData = new byte[1024];

    // Model parameter, reference: https://docs.live2d.com/cubism-sdk-tutorials/about-parameterupdating-of-model/?locale=ja
    // private CubismModel Model;
    // private CubismParameter parameter;  // do not use array, otherwise it won't update
    private float t1;  // time controller for breath
    private float t2;  // time controller for hands
    private float angleX;  // head angle
    private float angleY;  // head angle
    private float angleZ;  // head angle
    private float eyeOpenLeft;
    private float eyeOpenRight;
    private float eyeBallX;
    private float eyeBallY;
    private float eyebrowLeft;
    private float eyebrowRight;
    private float mouthOpen;
    private float mouthWidth;

    // Public read-only properties for external access
    public float AngleX => angleX;
    public float AngleY => angleY;
    public float AngleZ => angleZ;
    public float EyeOpenLeft => eyeOpenLeft;
    public float EyeOpenRight => eyeOpenRight;
    public float EyeBallX => eyeBallX;
    public float EyeBallY => eyeBallY;
    public float EyebrowLeft => eyebrowLeft;
    public float EyebrowRight => eyebrowRight;
    public float MouthOpen => mouthOpen;
    public float MouthWidth => mouthWidth;

    // Debug settings
    private bool enableFaceParamsDebug = true;
    private float faceParamsDebugIntervalSec = 0.2f;
    private float faceParamsDebugTimer = 0f;

    void init()
    {
        // Initialize IP and port
        const string IP = "127.0.0.1";
        int PORT = PlayerPrefs.GetInt("port", 14514);  // 使用默认端口14514
        
        // 确保端口号有效
        if (PORT <= 0 || PORT > 65535)
        {
            PORT = 14514;  // 如果端口无效，使用默认端口
            PlayerPrefs.SetInt("port", PORT);
            PlayerPrefs.Save();
        }
        
        Debug.Log($"[Momose] 使用端口: {PORT}");

        // Socket initialization
        serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(IP), PORT);
        serverSocket.Bind(ipEndPoint);
        serverSocket.Listen(100);

        Debug.Log($"[Momose] Socket服务器已启动，监听端口 {PORT}");

        // Start a new thread to update parameters
        Thread connect = new Thread(new ThreadStart(paraUpdate));
        connect.Start();

        // Model initialization (停用)
        // Model = this.FindCubismModel();
        t1 = t2 = 0.0f;
        eyeOpenLeft = eyeOpenRight = 1.0f;
        eyeBallX = eyeBallY = 0.0f;
        mouthWidth = 1.0f;
        mouthOpen = 0.0f;
        eyebrowLeft = eyebrowRight = -1.0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Screen.SetResolution(GUIwidth, GUIheight, false);  // set resolution
        //StartCoroutine("displaySetting");

        init();
    }

    // Update is called once per frame, please use LateUpdate() instead of Update(), otherwise it will stuck
    void LateUpdate()
    {
        // 停用 Live2D 更新邏輯
        //（保留方法以避免行為改變與外部呼叫錯誤）

        // Debug face parameters at an interval
        if (enableFaceParamsDebug)
        {
            faceParamsDebugTimer += Time.deltaTime;
            if (faceParamsDebugTimer >= faceParamsDebugIntervalSec)
            {
                faceParamsDebugTimer = 0f;
                Debug.Log(
                    $"[FaceParams] " +
                    $"angleX={angleX:F3}, angleY={angleY:F3}, angleZ={angleZ:F3} | " +
                    $"eyeOpenLeft={eyeOpenLeft:F3}, eyeOpenRight={eyeOpenRight:F3} | " +
                    $"eyeBallX={eyeBallX:F3}, eyeBallY={eyeBallY:F3} | " +
                    $"eyebrowLeft={eyebrowLeft:F3}, eyebrowRight={eyebrowRight:F3} | " +
                    $"mouthWidth={mouthWidth:F3}, mouthOpen={mouthOpen:F3}");
            }
        }
    }

    void SocketConnect()
    {
        try
        {
            if (clientSocket != null) { clientSocket.Close(); }

            Debug.Log("[Momose] 等待Python客户端连接...");
            clientSocket = serverSocket.Accept();
            Debug.Log("[Momose] Python客户端已连接！");
        }
        catch (Exception e)
        {
            Debug.LogError($"[Momose] Socket连接错误: {e.Message}");
        }
    }

    void paraUpdate()
    {
        string buff = "";
        string[] para;  // parameters

        SocketConnect();

        while (true)
        {
            try
            {
                recData = new byte[1024];
                int len = clientSocket.Receive(recData);

                // Client sends data by group, check if no data comes, then enter next group(loop)
                if (len == 0)
                {
                    Debug.Log("[Momose] 客户端断开连接，重新连接...");
                    SocketConnect();
                    continue;
                }

                buff = Encoding.ASCII.GetString(recData, 0, len);  // store data in buffer as string type
                para = buff.Split(' '); // get 11 parameters

                // 确保有足够的数据
                if (para.Length >= 11)
                {
                    angleZ = Convert.ToSingle(para[0]);  // roll
                    angleY = Convert.ToSingle(para[1]);  // pitch
                    angleX = Convert.ToSingle(para[2]);  // yaw

                    // eye openness
                    eyeOpenLeft = Convert.ToSingle(para[3]);
                    eyeOpenRight = Convert.ToSingle(para[4]);

                    // eyeballs
                    eyeBallX = Convert.ToSingle(para[5]);
                    eyeBallY = Convert.ToSingle(para[6]);

                    // eyebrows
                    eyebrowLeft = Convert.ToSingle(para[7]);
                    eyebrowRight = Convert.ToSingle(para[8]);

                    // mouth
                    mouthWidth = Convert.ToSingle(para[9]);
                    mouthOpen = Convert.ToSingle(para[10]);
                }
                else
                {
                    Debug.LogWarning($"[Momose] 接收到的数据格式不正确，期望11个参数，实际收到{para.Length}个");
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"[Momose] 数据接收错误: {e.Message}");
                try
                {
                    SocketConnect();  // 尝试重新连接
                }
                catch (Exception reconnectEx)
                {
                    Debug.LogError($"[Momose] 重新连接失败: {reconnectEx.Message}");
                    Thread.Sleep(1000);  // 等待1秒后重试
                }
            }
        }
    }

    /*
    IEnumerator displaySetting()
    {
        yield return new WaitForSeconds(0.1f); // wait for 0.1 second
        SetWindowLong(GetForegroundWindow(), STYLE, BORDER); // pop up window
        bool result = SetWindowPos(GetForegroundWindow(), TOP, GUIposX, GUIposY, GUIwidth, GUIheight, SHOWWINDOW); // set position and display at top
    }
    */
}
