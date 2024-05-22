using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WineView2.DataAccess.Data;
using WineView2.DataAccess.Repository.IRepository;
using WineView2.Models;

namespace WineView2.DataAccess.Repository
{
    public class ColorRepository : Repository<Color>, IColorRepository
    {
        private ApplicationDbContext _db;
        public ColorRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Color obj)
        {
            _db.Colors.Update(obj);
        }
    }
}
