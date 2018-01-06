﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Projekt1_UlewiczNienajadloRekawek
{
    public partial class UNRGlowne : Form
    {
        int UNRLiczbaPowtorzen;
        Int32 UNRMaxRozmiarTablic;
        Int32 UNRDolnaGranica;
        Int32 UNRGornaGranica;
        //int UNRSeed = 0; // inicjator wartości Random(bez wstawiania tych samych wartości ze względu na Timestamp)
        List<Int32> UNRTablicaSortowania = new List<Int32>();
        List<Int32> UNRTablicaPoSortowaniu = new List<Int32>();
        List<UNRCzasy> UNRTablicaPomiarowa = new List<UNRCzasy>();
        List<UNRCzasy> UNRTablicaOstateczna = new List<UNRCzasy>();
        
        
        public UNRGlowne()
        {
            InitializeComponent();
            UNRDgvPoSortowaniu.Visible = false;
            UNRDgvPrzedSortowaniem.Visible = false;
           

        }

        private void UNRBtnWynikiFormaTabelaryczna_Click(object sender, EventArgs e)
        {
            //int start, stop;
            UNRConversionTB();
            UNRLosowanie();
            //int UNROstatecznyRozmiar;
            double UNROstatecznyCzas = 0;
            Stopwatch UNRwatch = new Stopwatch();
            for (int UNRAktRozmiar = 0; UNRAktRozmiar < UNRMaxRozmiarTablic; UNRAktRozmiar++) //generowanie aktualnego rozmiaru tablicy
            {

                for (int UNRProba = 0; UNRProba < UNRLiczbaPowtorzen; UNRProba++) {         //generowanie liczby tablic o danym rozmiarze

                    UNRUtils.UNRLosowanie(UNRDolnaGranica, UNRGornaGranica, UNRAktRozmiar, UNRTablicaSortowania);
                    UNRCzasy UNRczas_sortowania_ob = new UNRCzasy();
                    if (UNRShell.Checked == true) 
                    { 
                    UNRShell UNRShell_ob = new UNRShell(UNRTablicaSortowania);
                    UNRDgvPrzedSortowaniem.Visible = false;
                    UNRwatch.Reset();
                    UNRwatch.Start();
                    UNRTablicaPoSortowaniu = UNRShell_ob.Sortuj();
                    UNRwatch.Stop();
                    UNRczas_sortowania_ob.UNRUstawRozmiar(UNRAktRozmiar);
                    UNRczas_sortowania_ob.UNRUstawCzasPomiaru(1000000 * UNRwatch.ElapsedTicks / Stopwatch.Frequency); //pomiar czasu w mikrosekundach
                    UNRTablicaPomiarowa.Add(UNRczas_sortowania_ob);
                    }
                    if (UNRGrzebieniowe.Checked == true)
                    {
                        UNRGrzebieniowe UNRGrzebieniowe_ob = new UNRGrzebieniowe(UNRTablicaSortowania);
                    UNRDgvPrzedSortowaniem.Visible = false;
                    UNRwatch.Reset();
                    UNRwatch.Start();
                    UNRTablicaPoSortowaniu = UNRGrzebieniowe_ob.Sortuj();
                    UNRwatch.Stop();
                    UNRczas_sortowania_ob.UNRUstawRozmiar(UNRAktRozmiar);
                    UNRczas_sortowania_ob.UNRUstawCzasPomiaru(1000000 * UNRwatch.ElapsedTicks / Stopwatch.Frequency); //pomiar czasu w mikrosekundach
                    UNRTablicaPomiarowa.Add(UNRczas_sortowania_ob);
                    }
                
                }

            }

           // UNRCzasy UNRCzasy_ostateczne = new UNRCzasy();

            for (int UNRi = 1; UNRi < UNRTablicaPomiarowa.Count; UNRi++)
            {

                UNRCzasy UNRCzasy_ostateczne = new UNRCzasy();

                if (UNRTablicaPomiarowa[UNRi].UNRGetRozmiar() == UNRTablicaPomiarowa[UNRi - 1].UNRGetRozmiar())
                {

                    UNROstatecznyCzas = UNROstatecznyCzas + UNRTablicaPomiarowa[UNRi].UNRGetCzasPomiaru();

                }
                else
                {

                    UNROstatecznyCzas = UNROstatecznyCzas / UNRLiczbaPowtorzen;
                    UNRCzasy_ostateczne.UNRUstawCzasPomiaru(UNROstatecznyCzas);
                    UNRCzasy_ostateczne.UNRUstawRozmiar(UNRTablicaPomiarowa[UNRi].UNRGetRozmiar());
                    UNRTablicaOstateczna.Add(UNRCzasy_ostateczne);
                    UNROstatecznyCzas = 0;
                }
            }



            //var UNRListToSee = new BindingList<Int32>(UNRTablicaOstateczna);
            DataTable UNRtable = UNRUtils.UNRConvertListToDataTableCzasy(UNRTablicaOstateczna);
            UNRDgvPoSortowaniu.DataSource = UNRtable;
            UNRDgvPoSortowaniu.Visible = true;

                MessageBox.Show("ok");
                

            //if (UNRShell.Checked == true)
            //{


            //    UNRShell Shell = new UNRShell(UNRTablicaSortowania);
            //    UNRDgvPrzedSortowaniem.Visible = false;
            //    watch.Start();
            //    UNRTablicaPoSortowaniu = Shell.Sortuj();
            //    watch.Stop();
            //    var UNRListToSee = new BindingList<Int32>(UNRTablicaPoSortowaniu);
            //    DataTable table = UNRUtils.UNRConvertListToDataTable(UNRTablicaPoSortowaniu);
            //    UNRDgvPoSortowaniu.DataSource = table;
            //    UNRDgvPoSortowaniu.Visible = true;


            //}
            

        }

        private void UNRBtnWizualizacjaTablicyPrzedSortowaniem_Click(object sender, EventArgs e)
        {
                
                UNRLosowanie();
               // UNRRandomArray UNR_PokazTablice = new UNRRandomArray(Int32.TryParse(UNRTBoxDolnaGranicaPrzedzialu.Text, out UNRDolnaGranica))
                //UNRTablicaSortowania
                var UNRListToSee = new BindingList<Int32>(UNRTablicaSortowania);
                DataTable table = UNRUtils.UNRConvertListToDataTable(UNRTablicaSortowania);
                UNRDgvPrzedSortowaniem.DataSource = table;
                //UNRDgvPrzedSortowaniem.DataSource = UNRListToSee;
                UNRDgvPrzedSortowaniem.Visible = true;
           
             
        }

        private void UNRLosowanie() {

            UNRConversionTB();
            //Int32.TryParse(UNRTBoxMinimalnaProba.Text, out UNRLiczbaPowtorzen);
            //Int32.TryParse(UNRTBoxMaxRozmiarTablic.Text, out UNRMaxRozmiarTablic);
            //Int32.TryParse(UNRTBoxDolnaGranicaPrzedzialu.Text, out UNRDolnaGranica);
            //Int32.TryParse(UNRTBoxGornaGranicaPrzedzialu.Text, out UNRGornaGranica);

            //Utwórz losową tablicę
            UNRTablicaSortowania = UNRUtils.UNRLosowanie(UNRDolnaGranica, UNRGornaGranica, UNRMaxRozmiarTablic, UNRTablicaSortowania);

            //Wybierz rodzaj obiektu jakim ma być losowana tablica
            //if (UNRShell.Checked == true)
            //{


            //    UNRShell Shell = new UNRShell(UNRTablicaSortowania);

            //}

            //else if (UNRGrzebieniowe.Checked == true)
            //{

            //    UNRGrzebieniowe Grzebieniowe = new UNRGrzebieniowe(UNRTablicaSortowania);

            //};
        
        
        }

         private void UNRBtnWizualizacjaTablicyPoSortowaniem_Click(object sender, EventArgs e)
         {
             UNRConversionTB();

             if (UNRShell.Checked == true)
             {


                 UNRShell Shell = new UNRShell(UNRTablicaSortowania);
                 UNRDgvPrzedSortowaniem.Visible = false;               
                 UNRTablicaPoSortowaniu = Shell.Sortuj();
                 var UNRListToSee = new BindingList<Int32>(UNRTablicaPoSortowaniu);
                 DataTable table = UNRUtils.UNRConvertListToDataTable(UNRTablicaPoSortowaniu);
                 UNRDgvPoSortowaniu.DataSource = table;
                 UNRDgvPoSortowaniu.Visible = true;
                 

             }

             else if (UNRGrzebieniowe.Checked == true)
             {

                 UNRGrzebieniowe Grzebieniowe = new UNRGrzebieniowe(UNRTablicaSortowania);
                 UNRDgvPrzedSortowaniem.Visible = false;

                 UNRTablicaPoSortowaniu = Grzebieniowe.Sortuj();
                 var UNRListToSee = new BindingList<Int32>(UNRTablicaPoSortowaniu);
                 DataTable table = UNRUtils.UNRConvertListToDataTable(UNRTablicaPoSortowaniu);
                 UNRDgvPoSortowaniu.DataSource = table;
                 UNRDgvPoSortowaniu.Visible = true;
             };
        
         }

         private bool UNRConversionTB() {

             Int32.TryParse(UNRTBoxMinimalnaProba.Text, out UNRLiczbaPowtorzen);
             Int32.TryParse(UNRTBoxMaxRozmiarTablic.Text, out UNRMaxRozmiarTablic);
             Int32.TryParse(UNRTBoxDolnaGranicaPrzedzialu.Text, out UNRDolnaGranica);
             Int32.TryParse(UNRTBoxGornaGranicaPrzedzialu.Text, out UNRGornaGranica);

             return true;
         }
    }
}
