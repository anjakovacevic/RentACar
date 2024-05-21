using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekat
{
    public class RentACarContext : DbContext
    {
        public DbSet<Cars> Cars { get; set; }
        public DbSet<Locations> Locations { get; set; }

        // singleton pattern with thread safety
        private static RentACarContext instance;
        private static readonly object padlock = new object();
        public static RentACarContext Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                        instance = new RentACarContext();
                    return instance;
                }
            }
        }
        private RentACarContext() { }
    }
}
