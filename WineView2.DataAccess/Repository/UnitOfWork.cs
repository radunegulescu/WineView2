using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WineView2.DataAccess.Data;
using WineView2.DataAccess.Repository.IRepository;

namespace WineView2.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public IColorRepository Color { get; private set; }
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Color = new ColorRepository(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
