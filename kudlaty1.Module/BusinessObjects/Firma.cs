using DevExpress.CodeParser;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using DevExpress.XtraRichEdit.Import.Html;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Json;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;


namespace kudlaty1.Module.BusinessObjects
{
    // Czynny, Zwolniony, Niezarejestrowany
    public enum StatusVat { Nieznane = -1, Czynny = 1, Zwolniony = 2, Niezarejestrowany = 3 }

    [DefaultClassOptions]
    [XafDefaultProperty(nameof(Nazwa))]
    [NavigationItem("Operacje")]
    public class Firma : BaseObject
    {
        public Firma(Session session) : base(session)
        { }


        StatusVat statusVat;
        string adresWykonywaniaDzialalnosci;
        string adresSiedziby;
        string krs;
        DateTime dataRozpoczeciaDzialanosci;
        string regon;
        string nip;
        string nazwa;
        

        [Size(200)]
        public string Nazwa
        {
            get => nazwa;
            set => SetPropertyValue(nameof(Nazwa), ref nazwa, value);

        }



        [Size(15)]
        [XafDisplayName("NIP")]
        public string Nip
        {
            get => nip;
            set => SetPropertyValue(nameof(Nip), ref nip, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        [XafDisplayName("REGON")]
        public string Regon
        {
            get => regon;
            set => SetPropertyValue(nameof(Regon), ref regon, value);
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        [XafDisplayName("KRS")]
        public string Krs
        {
            get => krs;
            set => SetPropertyValue(nameof(Krs), ref krs, value);
        }
        [XafDisplayName("Data rozpoczecia dzialalosci")]
        public DateTime DataRozpoczeciaDzialanosci
        {
            get => dataRozpoczeciaDzialanosci;
            set => SetPropertyValue(nameof(DataRozpoczeciaDzialanosci), ref dataRozpoczeciaDzialanosci, value);
        }

        [Size(SizeAttribute.Unlimited)]
        public string AdresSiedziby
        {
            get => adresSiedziby;
            set => SetPropertyValue(nameof(AdresSiedziby), ref adresSiedziby, value);
        }


        [Size(SizeAttribute.Unlimited)]
        public string AdresWykonywaniaDzialalnosci
        {
            get => adresWykonywaniaDzialalnosci;
            set => SetPropertyValue(nameof(AdresWykonywaniaDzialalnosci), ref adresWykonywaniaDzialalnosci, value);
        }

        [XafDisplayName("Status VAT")]
        public StatusVat StatusVat
        {
            get => statusVat;
            set => SetPropertyValue(nameof(StatusVat), ref statusVat, value);
        }

        [Action(ToolTip = "Pobiera informacje o firmie z danych Ministerstwa Finansow", Caption = "Pobierz Dane z MF")]
        public  async void wypelnijDane()
        {
            OdpowiedzMF? odpowiedzMF;
            var ministerstwo = new Ministerstwo();
            string adres;

            if (ministerstwo.sprawdzNIP(Nip)) adres = "https://wl-api.mf.gov.pl/api/search/nip/" + Nip;
            else if (ministerstwo.sprawdzREGON(Regon)) adres = "https://wl-api.mf.gov.pl/api/search/regon/" + Regon;
            else 
            {

                Nazwa = "Podaj poprawny NIP lub REGON";
                return;
            }
            adres = adres + "?date=" + DateTime.Now.ToString("yyyy-MM-dd");

            try
            {
                var klient = new HttpClient();
                var dane_tmp = await klient.GetStringAsync(new Uri(adres));
                odpowiedzMF = JsonSerializer.Deserialize<OdpowiedzMF>(dane_tmp);
                Nazwa = odpowiedzMF.result.subject.name;
                Nip = odpowiedzMF.result.subject.nip;
                Krs = odpowiedzMF.result.subject.krs;
                Regon = odpowiedzMF.result.subject.regon;
                AdresSiedziby = odpowiedzMF.result.subject.residenceAddress;
                AdresWykonywaniaDzialalnosci = odpowiedzMF.result.subject.workingAdress;
                dataRozpoczeciaDzialanosci = DateTime.Parse(odpowiedzMF.result.subject.registrationLegalDate);
                if (odpowiedzMF.result.subject.statusVat=="Czynny") StatusVat = StatusVat.Czynny;
                else if(odpowiedzMF.result.subject.statusVat == "Zwolniony") StatusVat = StatusVat.Zwolniony;
                else if(odpowiedzMF.result.subject.statusVat == "Niezarejestrowany") StatusVat = StatusVat.Niezarejestrowany;

            }
            catch (Exception ex)
            {
                Nazwa = "Wystapil blad pobierania danych " + ex.Message;

            }

        }






            /* Jak to robia ludzie kudlaci

            [Action(ToolTip = "Pobiera informacje o firmie z danych Ministerstwa Finansow", Caption = "Pobierz Dane z MF")]
            public async void pobierzDane()
            {
                var klient = new HttpClient();
                try {
                    if (Nip == null) Nip = "";
                    if (Regon == null) Regon = "";
                    var adres = new Uri("https://wl-api.mf.gov.pl/api/search/nip/" + Nip.ToString() + "?date=2023-01-08");

                    if(Nip=="" & Regon!="")
                    {
                        adres = new Uri("https://wl-api.mf.gov.pl/api/search/regon/" + Regon.ToString() + "?date=2023-01-08");
                    }
                    if (Nip != "" || Regon != "")
                    {
                        var dane_tmp = await klient.GetStringAsync(adres);
                        var dane = JsonObject.Parse(dane_tmp);
                        //this.nazwa=dane["Subject"]["Name"].ToString();
                        if (dane["result"]["subject"]["name"] != null) Nazwa = dane["result"]["subject"]["name"].ToString();
                        else Nazwa = "Brak nazwy";
                        if (dane["result"]["subject"]["regon"] != null) Regon = dane["result"]["subject"]["regon"].ToString();
                        else Regon = "";
                        if (dane["result"]["subject"]["nip"] != null) Nip = dane["result"]["subject"]["nip"].ToString();
                        else Nip = "";
                        if (dane["result"]["subject"]["krs"] != null) Krs = dane["result"]["subject"]["krs"].ToString();
                        else Krs = "";
                        if (dane["result"]["subject"]["residenceAddress"] != null) AdresSiedziby = dane["result"]["subject"]["residenceAddress"].ToString();
                        else AdresSiedziby = "";
                        if (dane["result"]["subject"]["workingAddress"] != null) AdresWykonywaniaDzialalnosci = dane["result"]["subject"]["workingAddress"].ToString();
                        else AdresWykonywaniaDzialalnosci = "";
                        if (dane["result"]["subject"]["registrationLegalDate"] != null) DataRozpoczeciaDzialanosci = DateTime.Parse(dane["result"]["subject"]["registrationLegalDate"].ToString());
                        // Status VAT traktuje inaczej ;)

                        // Czynny, Zwolniony, Niezarejestrowany
                        if (dane["result"]["subject"]["statusVat"].ToString() == "Czynny") StatusVat = StatusVat.Czynny;
                        if (dane["result"]["subject"]["statusVat"].ToString() == "Zwolniony") StatusVat = StatusVat.Zwolniony;
                        if (dane["result"]["subject"]["statusVat"].ToString() == "Niezarejestrowany") StatusVat = StatusVat.Niezarejestrowany;
                    }
                    else Nazwa = "Wypelnij NIP lub REGON";

                }
                catch (Exception ex)
                {
                    Nazwa = "Bledny NIP";
                    Krs = "";
                    Regon = "";
                    Nip = "";
                    AdresSiedziby = "";
                    AdresWykonywaniaDzialalnosci = "";
                    DataRozpoczeciaDzialanosci = DateTime.Parse("1970-01-01");
                    StatusVat = StatusVat.Nieznane;
                }
            }
            // Koniec przykladu wg ludzi kudlatych
            */


        }





}
