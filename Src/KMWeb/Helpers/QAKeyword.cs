using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KMWeb.QA
{
    public class QAKeyword
    {
        int _id;
        string _key;

        public int Id
        {
            get { return this._id; }
            set { this._id = value; }
        }

        public string Key
        {
            get { return this._key; }
            set { this._key = value; }
        }

       
    }
}