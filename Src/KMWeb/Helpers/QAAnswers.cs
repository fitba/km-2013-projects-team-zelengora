using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KMWeb.QA
{
    public class QAAnswers
    {
        int _id;
        string _odgovor;
        int _idPitanje;
        int _idKorisnik;
        string _korisnickoIme;
        string _datum;
        int _ukupnaOcjena;

        public int Id
        {
            get { return this._id; }
            set { this._id = value; }
        }

        public string Odgovor
        {
            get { return this._odgovor; }
            set { this._odgovor = value; }
        }

        public int IdPitanje
        {
            get { return this._idPitanje; }
            set { this._idPitanje = value; }
        }

        public string Datum
        {
            get { return this._datum; }
            set { this._datum = value; }
        }

        public int IdKorisnik
        {
            get { return this._idKorisnik; }
            set { this._idKorisnik = value; }
        }

        public int UkupnaOcjena
        {
            get { return this._ukupnaOcjena; }
            set { this._ukupnaOcjena = value; }
        }

        public string KorisnickoIme
        {
            get { return this._korisnickoIme; }
            set { this._korisnickoIme = value; }
        }


    }
}