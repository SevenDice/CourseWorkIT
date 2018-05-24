using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lab_4
{
    public class Auto
    {
        private static Auto Auth = new Auto();
        bool key = false;
        public static Auto GetAuth()
        {
            return Auth;
        }

        public bool GetKey()
        {
            return key;
        }

        public void SetKey(bool t)
        {
            key = t;
        }
    }
}