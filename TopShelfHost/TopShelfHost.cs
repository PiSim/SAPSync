using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;
using SAPSync;
using SyncService;

namespace TopShelfHost
{
    public class TopShelfHost
    {
        public static void Main(string[] args)
        {
            StartHost(new SyncManager());
        }

        public static void StartHost(ISyncManager syncManager)
        {
            var rc = HostFactory.New(x =>
            {
                x.Service(() => new SyncService.SyncService(syncManager));

                x.RunAsLocalService();
                x.StartAutomatically();
                x.UseNLog();
                x.SetDescription("Topshelf Host");
            });

            rc.Run();
        }
    }
}
