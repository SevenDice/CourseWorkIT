using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lab_4
{
    public partial class AGRODataContext
    {
        public AGRODataContext(string path) :
        base(ConnectionStringFactory.Create(path))
        {
            OnCreated();
        }
    }
}