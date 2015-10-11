using System.ServiceProcess;
using System.Timers;
using WhoIsBetter.DataAccess;
using System.Linq;
using System;
using System.Messaging;
using WhoIsBetter.Entity;

namespace WhoIsBetter.WinService
{
    public partial class Scheduler : ServiceBase
    {
        Timer timer = null;
        //TorneoDA torneoDA = null;
        //ContendorDA contendorDA = null;
        //FavoritoDA favoritoDA = null;

        WSTorneo.TorneoServiceClient proxyTorneo = null;
        WSContendor.ContendorServiceClient proxyContendor = null;
        WSFavorito.FavoritoServiceClient proxyFavorito = null;

        public Scheduler()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            timer = new Timer();
            timer.Interval = 5000; //every 5 sec
            timer.Elapsed += new ElapsedEventHandler(timer_Tick);
            timer.Enabled = true;

            //torneoDA = new TorneoDA();
            //contendorDA = new ContendorDA();
            //favoritoDA = new FavoritoDA();

            proxyTorneo = new WSTorneo.TorneoServiceClient();
            proxyContendor = new WSContendor.ContendorServiceClient();
            proxyFavorito = new WSFavorito.FavoritoServiceClient();

            Logger.WriteErrorLog("WhoIsBetter Scheduler service started");
        }

        protected override void OnStop()
        {
            timer.Enabled = false;
            Logger.WriteErrorLog("WhoIsBetter Scheduler service stopped");
        }

        protected void timer_Tick(object sender, ElapsedEventArgs e)
        {
            try
            {
                var torneos = proxyTorneo.ListarTorneos(null).Where(w => !(new int[] { 3, 4 }).Contains(w.IDEstado)).ToList();

                Logger.WriteErrorLog("Torneos procesados: " + torneos.Count);

                torneos.ForEach(torneo =>
                {
                    Logger.WriteErrorLog(
                        "Torneo: "+ torneo.ID +
                        " estado: " + torneo.IDEstado + 
                        " FI: " + torneo.FechaInicio +
                        " FF: " + torneo.FechaFin +
                        " HOY: " + DateTime.Today
                    );

                    if (torneo.IDEstado == 1 && torneo.FechaInicio == DateTime.Today)
                    {
                        if (proxyContendor.ListarContendores(torneo.ID).Count < torneo.NumeroContendores)
                            torneo.IDEstado = 4; //4 = cancelado
                        else
                            torneo.IDEstado = 2; //2 = en curso

                        proxyTorneo.ActualizarTorneo(torneo);
                    }
                    else if (torneo.IDEstado == 2 && torneo.FechaFin == DateTime.Today)
                    {
                        torneo.IDEstado = 3; //3 = finalizado

                        proxyTorneo.ActualizarTorneo(torneo);

                        //string rutaCola = @".\private$\DSWarrior2";

                        //if (!MessageQueue.Exists(rutaCola))
                        //{
                        //    MessageQueue.Create(rutaCola);
                        //}

                        //var cola = new MessageQueue(rutaCola);
                        //cola.Formatter = new XmlMessageFormatter(new Type[] { typeof(Favorito) });

                        //var totalMessages = cola.GetAllMessages().Count();

                        //for (int i = 0; i < totalMessages; i++)
                        //{
                        //    var mensaje = cola.Receive();
                        //    var favorito = (Favorito)mensaje.Body;
                            
                        //    proxyFavorito.CrearFavorito(favorito);
                        //}
                    }
                });

                Logger.WriteErrorLog("Timer ticked has been done successfully");
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog(ex);
            }

        }
    }
}
