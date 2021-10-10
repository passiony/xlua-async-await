using System.IO;
using UnityEngine;
using XLua;

public class XluaManager : MonoBehaviour
{
    const string luaScriptsFolder = "LuaScripts";
    const string gameMainScriptName = "GameMain";
    LuaEnv luaEnv = null;

    public bool HasGameStart
    {
        get;
        protected set;
    }
    
    void Awake()
    {
        luaEnv = new LuaEnv();
        HasGameStart = false;
        if (luaEnv != null)
        {
            luaEnv.AddLoader(CustomLoader);
        }
        else
        {
            Debug.LogError("InitLuaEnv null!!!");
        }
    }

    void Start()
    {
        StartGame();
    }
    
    void StartGame()
    {
        if (luaEnv != null)
        {
            LoadScript(gameMainScriptName);
            SafeDoString("GameMain.Start()");
            HasGameStart = true;
        }
    }

    public void SafeDoString(string scriptContent)
    {
        if (luaEnv != null)
        {
            try
            {
                luaEnv.DoString(scriptContent);
            }
            catch (System.Exception ex)
            {
                string msg = string.Format("xLua exception : {0}\n {1}", ex.Message, ex.StackTrace);
                Debug.LogError(msg, null);
            }
        }
    }

    void LoadScript(string scriptName)
    {
        SafeDoString(string.Format("require('{0}')", scriptName));
    }

    public static byte[] CustomLoader(ref string filepath)
    {
        string scriptPath = string.Empty;
        filepath = filepath.Replace(".", "/") + ".lua";
        scriptPath = Path.Combine(Application.dataPath, luaScriptsFolder, filepath);

        return File.ReadAllBytes(scriptPath);
    }

    private void Update()
    {
        if (luaEnv != null)
        {
            luaEnv.Tick();
        }
    }

    private void OnApplicationQuit()
    {
        if (luaEnv != null && HasGameStart)
        {
            SafeDoString("GameMain.OnApplicationQuit()");
        }
    }

     void OnDestroy()
    {
        if (luaEnv != null)
        {
            try
            {
                luaEnv.Dispose();
                luaEnv = null;
            }
            catch (System.Exception ex)
            {
                string msg = string.Format("xLua exception : {0}\n {1}", ex.Message, ex.StackTrace);
                Debug.LogError(msg, null);
            }
        }
    }
}
