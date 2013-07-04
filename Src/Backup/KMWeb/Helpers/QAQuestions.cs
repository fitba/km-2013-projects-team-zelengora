using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KMWeb.QA
{
    public class QAQuestions
    {
        int _id;
        string _naslovPitanja;
        string _pitanje;
        string _datum;
        int _pregleda;
        string _korisnickoIme;
        int _idKorisnik;
        int _idKategorija;
        int _brojOdgovora;
        int _brojOcjena;
        int _ukupnaOcjena;
        List<QAKeyword> _kljucnaRijec = new List<QAKeyword>();

        public int Id { 
            get { return this._id; } 
            set { this._id = value; } 
        }

        public string NaslovPitanja { 
            get { return this._naslovPitanja; }
            set { this._naslovPitanja = value; }
        }

        public string Pitanje {
            get { return this._pitanje; }
            set { this._pitanje = value; }
        }

        public string Datum {
            get { return this._datum; }
            set { this._datum = value; }
        }

        public int Pregleda {
            get { return this._pregleda; }
            set {this._pregleda=value;}
        }
        public string KorisnickoIme
        {
            get { return this._korisnickoIme; }
            set { this._korisnickoIme = value; }
        }

        public int IdKorisnik
        {
            get { return this._idKorisnik; }
            set { this._idKorisnik = value; }
        }

        public int IdKategorija
        {
            get { return this._idKategorija; }
            set { this._idKategorija = value; }
        }

        public int BrojOdgovora
        {
            get { return this._brojOdgovora; }
            set { this._brojOdgovora = value; }
        }

        public int BrojOcjena
        {
            get { return this._brojOcjena; }
            set { this._brojOcjena = value; }
        }

        public int UkupnaOcjena
        {
            get { return this._ukupnaOcjena; }
            set { this._ukupnaOcjena = value; }
        }

        public List<QAKeyword> KljucnaRijec
        {
            get { return this._kljucnaRijec; }
            set { this._kljucnaRijec = value; }
        }
    }
}