using UnityEngine;
using System.Collections;
using System.IO;
using System.Diagnostics;

namespace NetBattle
{
    public class GameLog : LogWriter<GameLog>
    {
        /// <summary>
        /// 必须在逻辑帧Update里调用才能获取得到
        /// </summary>
        /// <param name="log">Log.</param>
        public string[] OnlyShowFile = { };
        public string[] HideShowFile = { };
        public string[] OnlyShowMethod = { };
        public string[] HideShowMethod = { };
        public string[] HasSomeInfo = { };
        public float[] clamp = { };
        string directPath = "";
        public GameLog()
        {
            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
                return;
            string path = Application.dataPath + directPath + "/LogWriter/WriterConf.txt";
            FileInfo info = new FileInfo(path);
            if (info.Exists)
            {
                StreamReader reader = info.OpenText();
                string strinfo = reader.ReadToEnd();
                reader.Close();
                JSON.JSONNode node = JsonRead.Parse(strinfo);
                OnlyShowFile = NodeAsStrings(node, "OnlyShowFile");
                HideShowFile = NodeAsStrings(node, "HideShowFile");
                OnlyShowMethod = NodeAsStrings(node, "OnlyShowMethod");
                HideShowMethod = NodeAsStrings(node, "HideShowMethod");
                HasSomeInfo = NodeAsStrings(node, "HasSomeInfo");

                clamp = NodeAsFloats(node, "KeyClamp");

            }
            else
            {
                UnityEngine.Debug.Log(" error Path:" + path);
            }
        }
        public float[] NodeAsFloats(JSON.JSONNode node, string key)
        {
            float[] floats = new float[] { -1, -1 };
            for (int i = 0; i < node[key].Count; i++)
            {
                floats[i] = node[key][i].AsFloat;
            }
            return floats;
        }
        public string[] NodeAsStrings(JSON.JSONNode node, string key)
        {
            string[] strs = new string[node[key].Count];
            for (int i = 0; i < node[key].Count; i++)
            {
                strs[i] = node[key][i];
            }
            return strs;
        }
        void Print(string log)
        {
            //return;
            /*int frames = LockStepManager.Instance.GameIndexFrames;
            if ((clamp[0] == -1 && clamp[1] == -1) || (frames > clamp[0] && frames < clamp[1]))
            {
                AddLog(LockStepManager.Instance.GameIndexFrames + ":" + log);
            }*/
        }
        void Print(Vector3 log)
        {
            //return;
            Print(LogVector3(log));
        }
        public void Print(params object[] objects)
        {
            //return;
            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
                return;
            System.Diagnostics.StackFrame sf = new System.Diagnostics.StackFrame(1);
            System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(sf);
            string info = st.ToString().Replace("at ", "");

            string[] splitSt = info.Split('.');
            string file = "";
            string method = "";
            for (int i = 0; i < splitSt.Length; i++)
            {
                if (i == splitSt.Length - 1 || splitSt[i].Contains("(") || splitSt[i].Contains(")"))
                    method += splitSt[i];
                else
                    file += splitSt[i];
            }

            bool onlyShowFile = (OnlyShowFile.Length > 0 && !HasValue(OnlyShowFile, file));
            bool hideShowFile = (HasValue(HideShowFile, file));
            bool onlyShowMethod = (OnlyShowMethod.Length > 0 && !HasValue(OnlyShowMethod, method));
            bool hideShowMethod = (HasValue(HideShowMethod, method));

            if (onlyShowFile || hideShowFile || onlyShowMethod || hideShowMethod)
            {
                return;
            }
            string value = "";
            value += GetStackTrace(1);
            for (int i = 0; i < objects.Length; i++)
            {
                if (objects[i] is Vector3)
                    value += LogVector3((Vector3)objects[i]);
                else
                    value += objects[i].ToString();
            }
            bool hasSomeInfo = (HasSomeInfo.Length > 0 && !HasValue(HasSomeInfo, value));
            if (hasSomeInfo)
                return;
            Print(value);
        }
        public string GetStackTrace(int index)
        {
            string info = "[";
            while (true)
            {
                index++;
                System.Diagnostics.StackFrame sf = new System.Diagnostics.StackFrame(index);
                System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(sf);


                if (sf.GetMethod() == null || st.ToString().Contains("unknown method") || index > 10)
                {
                    info += "]";
                    return info;
                }

                string ststr = st.ToString();
                string fileMethod = sf.GetMethod().Name;
                string fileName = sf.GetMethod().ReflectedType.Name;

                info += fileName + ":" + fileMethod + "|";
            }
        }
        public static string LogVector3(Vector3 value)
        {
            return "(" + value.x + "," + value.y + "," + value.z + ")";
        }
        public bool HasValue(string[] value, string target)
        {
            for (int i = 0; i < value.Length; i++)
            {
                if (target.Contains(value[i]))
                    return true;
            }
            return false;
        }
        public override void Close()
        {
            base.Close();
            instance = null;
        }
    }
}

