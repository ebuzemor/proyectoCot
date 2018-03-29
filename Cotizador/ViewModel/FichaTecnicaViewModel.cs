using Cotizador.Common;
using Cotizador.Model;
using Cotizador.View;
using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;

namespace Cotizador.ViewModel
{
    public class FichaTecnicaViewModel : Notificador
    {
        #region Commands
        public RelayCommand BuscarFichaTecnicaCommand { get; set; }
        public RelayCommand DescargarFichaTecnicaCommand { get; set; }
        public RelayCommand ResetFormularioCommand { get; set; }
        public RelayCommand CerrarMensajeCommand { get; set; }
        #endregion

        #region Variables
        private ApiKey _appKey;
        private Usuario _usuario;
        private String _localhost;
        private string _infoFicha;
        private string _txtDescripcion;
        private string _txtResumen;
        private string _txtMensaje;
        private bool _verMensaje;
        private Producto _ftProducto;
        private Image _imagenProducto;
        private string _rutaImagenProducto;

        public ApiKey AppKey { get => _appKey; set { _appKey = value; OnPropertyChanged(); } }
        public Usuario Usuario { get => _usuario; set { _usuario = value; OnPropertyChanged(); } }
        public string Localhost { get => _localhost; set { _localhost = value; OnPropertyChanged(); } }
        public string InfoFicha { get => _infoFicha; set { _infoFicha = value; OnPropertyChanged(); } }
        public string TxtDescripcion { get => _txtDescripcion; set { _txtDescripcion = value; OnPropertyChanged(); } }
        public string TxtResumen { get => _txtResumen; set { _txtResumen = value; OnPropertyChanged(); } }
        public string TxtMensaje { get => _txtMensaje; set { _txtMensaje = value; OnPropertyChanged(); } }
        public bool VerMensaje { get => _verMensaje; set { _verMensaje = value; OnPropertyChanged(); } }
        public Producto FtProducto { get => _ftProducto; set { _ftProducto = value; OnPropertyChanged(); } }
        public Image ImagenProducto { get => _imagenProducto; set { _imagenProducto = value; OnPropertyChanged(); } }
        public string RutaImagenProducto { get => _rutaImagenProducto; set { _rutaImagenProducto = value; OnPropertyChanged(); } }
        #endregion

        #region Constructor
        public FichaTecnicaViewModel()
        {
            BuscarFichaTecnicaCommand = new RelayCommand(BuscarFichaTecnica);
            DescargarFichaTecnicaCommand = new RelayCommand(DescargarFichaTecnica);
            ResetFormularioCommand = new RelayCommand(ResetFormulario);
            CerrarMensajeCommand = new RelayCommand(CerrarMensaje);
        }
        #endregion

        #region Métodos
        private async void BuscarFichaTecnica(object parameter)
        {                        
            //RutaImagenProducto = null;
            var vmBuscarFT = new BuscarFichaTecnicaViewModel
            {
                AppKey = AppKey,
                Usuario = Usuario,
                Localhost = Localhost
            };
            var vwBuscarFT = new BuscarFichaTecnicaView
            {
                DataContext = vmBuscarFT
            };
            var result = await DialogHost.Show(vwBuscarFT, "FichaTecnica");
            if (result.Equals("SelProducto") == true)
            {
                FtProducto = vmBuscarFT.NvoProducto;
                InfoFicha = "Código: " + FtProducto.CodigoInterno + ", Descripción: " + FtProducto.Descripcion + ", Precio Unit.: $" + FtProducto.PrecioPublico;
                //Se busca la ficha tecnica por ClaveProducto
                var rest = new RestClient(Localhost);
                var req = new RestRequest("buscarFichaTecnica/" + FtProducto.ClaveProducto, Method.GET);
                req.AddHeader("Accept", "application/json");
                req.AddHeader("Authorization", "Bearer " + AppKey.Token);
                try
                {
                    IRestResponse<FichaTecnicaJson> resp = rest.Execute<FichaTecnicaJson>(req);
                    if (resp.IsSuccessful == true && resp.StatusCode == HttpStatusCode.OK)
                    {
                        FichaTecnicaJson ListaFicTec = JsonConvert.DeserializeObject<FichaTecnicaJson>(resp.Content);
                        FichaTecnica FicTec = ListaFicTec.ListaFichaTecnica.First();
                        TxtDescripcion = FicTec.Descripcion;
                        TxtResumen = FicTec.Resumen;
                        byte[] imgBytes = Convert.FromBase64String(FicTec.Imagen);                        
                        using (MemoryStream stream = new MemoryStream(imgBytes, 0, imgBytes.Length))
                        {
                            ImageFormat imgFormato = ImageFormat.Bmp;
                            switch (FicTec.Extension)
                            {
                                case "PNG": imgFormato = ImageFormat.Png; break;
                                case "JPG": imgFormato = ImageFormat.Jpeg; break;
                                case "BMP": imgFormato = ImageFormat.Bmp; break;
                                case "GIF": imgFormato = ImageFormat.Gif; break;
                            }
                            stream.Write(imgBytes, 0, imgBytes.Length);
                            ImagenProducto = Image.FromStream(stream, true);
                            ImagenProducto.Save(Path.GetTempPath() + FicTec.ClaveProducto + "." + imgFormato, imgFormato); //En caso de dar problemas, generar nombres de imagenes aleatorios con GUID
                            RutaImagenProducto = Path.GetTempPath() + FicTec.ClaveProducto + "." + imgFormato;
                            stream.Dispose();
                        }
                    }
                }
                catch (Exception ex)
                {
                    TxtMensaje = "Error al abrir imagen, intente de nuevo por favor.";
                    VerMensaje = true;
                    Console.Write(ex.Message);
                }
            }
        }

        private void DescargarFichaTecnica(object parameter)
        {

        }

        private void ResetFormulario(object parameter)
        {
            RutaImagenProducto = null;
            InfoFicha = null;
            TxtDescripcion = null;
            TxtResumen = null;
        }

        private void CerrarMensaje(object parameter) => VerMensaje = false;
        #endregion
    }
}