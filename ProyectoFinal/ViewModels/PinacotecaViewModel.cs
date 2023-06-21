using ProyectoFinal.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Text.Json.Serialization;
using System.Net.Http.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using GalaSoft.MvvmLight.Command;
using System.Windows;
using System.Xml.Serialization;
using System.Windows.Controls;

namespace ProyectoFinal.ViewModels
{
    public class PinacotecaViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        /// //////////////////////////////////////////////////////////////////////////////////////////////
        public string Error { get; set; } = "";
        int indice;
        public ObservableCollection<DatosPinacoteca> DatosOb { get; set; } = new ObservableCollection<DatosPinacoteca>();
        public DatosPinacoteca datosP { get; set; } = new DatosPinacoteca();
        public string Vista { get; set; } = "ver";
        public ICommand? AgregarCommand { get; set; }
        public ICommand? EditarCommand { get; set; }
        public ICommand? EliminarCommand { get; set; }
        public ICommand? CambiarVistaCommand { get; set; }
        public ICommand? CancelarCommand { get; set; }

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        public void Agregar()
        {
            Error = "";
            if (string.IsNullOrEmpty(datosP.Nombre))
            {
                Error += "Reingrese el nombre\n";
            }
            if (string.IsNullOrEmpty(datosP.Direccion))
            {
                Error += "Reingrese la direccion\n";
            }
            if (string.IsNullOrEmpty(datosP.Ciudad))
            {
                Error += "Reingrese la ciudad\n";
            }
            if (string.IsNullOrEmpty(datosP.MetrosCuadrados))
            {
                Error += "Reingrese la cantidad de metros cuadrados";
            }
            int i = 0;
            foreach (DatosPinacoteca item in DatosOb)
            {
                if (item.Nombre == datosP.Nombre)
                {
                    MessageBox.Show("Ingrese un nombre diferente");
                    return;
                }
                else
                {
                    i++;
                }
            }
            if (Error == "" && DatosOb != null)
            {
                DatosOb.Add(datosP);
                Guardar();
                CambiarVista("ver");
            }


        }

        public void CambiarVista(string vistaACambiar)
        {
            Vista = vistaACambiar;
            if (vistaACambiar == "agregar")
            {
                datosP = new DatosPinacoteca();
            }
            if (vistaACambiar == "editar")
            {
                if (DatosOb != null)
                {
                    indice = DatosOb.IndexOf(datosP);
                }
                DatosPinacoteca copiaDatosPinacoteca = new DatosPinacoteca()
                {
                    Nombre = datosP.Nombre,
                    Direccion = datosP.Direccion,
                    Ciudad = datosP.Ciudad,
                    MetrosCuadrados = datosP.Ciudad
                };
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Vista"));
        }

        public void Cancelar()
        {
            CambiarVista("ver");
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Vista)));

        }

        public void Editar()
        {
            if (DatosOb != null)
            {
                DatosOb[indice] = datosP;
                Guardar();
                CambiarVista("ver");
            }
        }

        public void Eliminar()
        {
            if (datosP != null && DatosOb != null)
            {
                DatosOb.Remove(datosP);
                CambiarVista("ver");
                Guardar();
            }
        }

        public void Guardar()
        {
            var json = JsonConvert.SerializeObject(DatosOb);
            File.WriteAllText("DatosOb.json", json);
        }

        public void Cargar()
        {
            if (File.Exists("DatosOb.json"))
            {
                var json = File.ReadAllText("DatosOb.json");
                var datos = JsonConvert.DeserializeObject<ObservableCollection<DatosPinacoteca>>(json);
                if (datos != null)
                {
                    DatosOb = (ObservableCollection<DatosPinacoteca>)datos;
                }
                else
                {
                    DatosOb = new ObservableCollection<DatosPinacoteca>();
                }
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////

        public PinacotecaViewModel()
        {
            Cargar();
            AgregarCommand = new RelayCommand(Agregar);
            EditarCommand = new RelayCommand(Editar);
            EliminarCommand = new RelayCommand(Eliminar);
            CancelarCommand = new RelayCommand(Cancelar);
            CambiarVistaCommand = new RelayCommand<string>(CambiarVista);
        }











    }
}
