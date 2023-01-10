using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace kudlaty1.Module.BusinessObjects
{
    public class Subject
    {
        private string _workingAdress;
        private string _residenceAddress;
        private string _krs;
        private string _regon;
        private string _registrationLegalDate;
        private string _nip;
        private string _name;
        
        public string workingAdress { get=>_workingAdress; set { _workingAdress = value ?? ""; } }
        public string residenceAddress { get => _residenceAddress; set { _residenceAddress = value ?? String.Empty; } }
        public string krs { get => _krs; set { _krs = value ?? "Brak wpisu w KRS"; } }
        public string regon { get => _regon; set { _regon = value ?? String.Empty; } }
        public string registrationLegalDate { get; set; }
        public string nip { get; set; }
        public string name { get; set; }

        public string statusVat { get; set; }
    }

    public class Result
    {
        public Subject subject { get; set; }
        public string requestDateTime { get; set; }
        public string requestId { get; set; }
    }

    public class OdpowiedzMF
    {
        private string _code;
        public string code { get => _code; set { _code = value ?? "ok"; } }
        public string? message { get; set; }
        public Result result { get; set; }

    }

    public class Ministerstwo
    { 

        //zostawiam stary konstruktor, moze sie przyda ;)
        public Ministerstwo() { }
       
        public bool sprawdzREGON(string REGON)
        {
            //UWAGA - ignorowane sa REGONY 14-to cyfrowe!!!!!
            int suma = 0;
            int[] wagi = { 8, 9, 2, 3, 4, 5, 6, 7 };
            if (REGON.Length != 9) return false;
            for (int i = 0; i < 9; i++)
            {
                if (!Char.IsDigit(REGON[i])) return false;
                if (i < 8)
                {
                    suma = suma + (int)Char.GetNumericValue(REGON[i]) * wagi[i];
                }
            }
            if (suma % 11 == (int)Char.GetNumericValue(REGON[8])) return true;
            return false;
        }

        public bool sprawdzNIP(string NIP)
        {
            int suma = 0;
            int[] wagi = { 6, 5, 7, 2, 3, 4, 5, 6, 7 };
            if (NIP.Length != 10) return false;
            for(int i=0; i < 10; i++)
            {
                if (!Char.IsDigit(NIP[i])) return false;
                if(i<9)
                {
                    suma = suma + (int)Char.GetNumericValue(NIP[i]) * wagi[i];
                }
            }
            if (suma%11 == (int)Char.GetNumericValue(NIP[9])) return true;

            return false;
        }

    }
}
