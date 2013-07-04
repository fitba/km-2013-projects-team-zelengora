using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
    public class ClanakOcjenaClanka
    {
        private int _IdClanak;
        public int IdClanak{
            get { return _IdClanak; }
            set { _IdClanak = value; }
        }

        private int _IdOcjena;
        public int IdOcjena
        {
            get { return _IdOcjena; }
            set { _IdOcjena = value; }
        }
        private int _IdKorisnik;
        public int IdKorisnik
        {
            get { return _IdKorisnik; }
            set { _IdKorisnik = value; }
        }



    }
}
