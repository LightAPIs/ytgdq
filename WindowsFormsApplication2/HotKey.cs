using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public class HotKey
    {
        private readonly string id;
        private readonly string defaultKeys;
        private string keys;

        //[Flags]
        //public enum KeyModifiers
        //{
        //    None = 0,
        //    Alt = 1,
        //    Control = 2,
        //    Shift = 4,
        //    WindowsKey = 8
        //}

        public HotKey(string _id, string _defaultKeys)
        {
            this.id = _id;
            this.defaultKeys = IsValid(_defaultKeys) ? _defaultKeys : "None";
            this.keys = "None";
        }

        public bool SetKeys(string _keys)
        {
            if (IsValid(_keys))
            {
                this.keys = _keys;
                return true;
            }
            return false;
        }

        public string GetId()
        {
            return this.id;
        }

        public string GetKeys()
        {
            return this.keys;
        }

        public string GetDefaultKeys()
        {
            return this.defaultKeys;
        }

        public string ResetKeys()
        {
            this.keys = this.defaultKeys;
            return this.defaultKeys;
        }

        public Keys TransKeyCode()
        {
            Keys result = Keys.None;
            Regex numRgx = new Regex(@"^[0-9]$");
            string[] splitsKeys = this.keys.Split('+');
            foreach (string k in splitsKeys)
            {
                string tempK = k;
                if (k == "Ctrl")
                {
                    tempK = "Control";
                }
                if (numRgx.IsMatch(k))
                {
                    tempK = "D" + tempK;
                }
                result = result | (Keys)Enum.Parse(typeof(Keys), tempK, true);
            }

            return result;
        }

        public Keys TransOnlyKeyCode()
        {
            string[] splitsKeys = this.keys.Split('+');
            Regex rgx = new Regex(@"^[A-Z0-9]$|^F[1-9][0-2]?$");
            Regex numRgx = new Regex(@"^[0-9]$");
            for (int i = 0; i < splitsKeys.Length; i++)
            {
                if (rgx.IsMatch(splitsKeys[i]))
                {
                    if (numRgx.IsMatch(splitsKeys[i]))
                    {
                        return (Keys)Enum.Parse(typeof(Keys), "D" + splitsKeys[i], true);
                    }
                    else
                    {
                        return (Keys)Enum.Parse(typeof(Keys), splitsKeys[i], true);
                    }
                }
            }
            return Keys.None;
        }

        public int TransKeyModifiers()
        {
            if (this.keys.Contains("Ctrl"))
            {
                return 2;
            }
            else if (this.keys.Contains("Alt"))
            {
                return 1;
            }
            return 0;
        }

        public static bool IsValid(string val)
        {
            bool valid = true;
            if (val == "None")
            {
                return true;
            }
            string[] splitsKeys = val.Split('+');
            Regex rgx = new Regex(@"^[A-Z0-9]$|^F[1-9][0-2]?$");
            foreach (string k in splitsKeys)
            {
                if (k == "Ctrl" || k == "Alt" || rgx.IsMatch(k))
                {
                    continue;
                }
                valid = false;
            }
            return valid;
        }
    }
}
