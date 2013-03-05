using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KMWeb
{
    public class Clanak
    {
        string _sadrzaj;
        string _naslov;

        public string Sadrzaj
        {
            get { return this._sadrzaj; }
            set { this._sadrzaj = value; }
        }
        public string Naslov
        {
            get { return this._naslov; }
            set { this._naslov = value; }
        }
    }
}