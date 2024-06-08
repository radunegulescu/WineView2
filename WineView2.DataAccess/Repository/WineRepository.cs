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
    public class WineRepository : Repository<Wine>, IWineRepository
    {
        private ApplicationDbContext _db;

        public WineRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Wine obj)
        {
            var objFromDb = _db.Wines.FirstOrDefault(p => p.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = obj.Name;
                objFromDb.Price = obj.Price;
                objFromDb.Price5 = obj.Price5;
                objFromDb.Price10 = obj.Price10;
                objFromDb.ColorId = obj.ColorId;
                objFromDb.WineryId = obj.WineryId;  
                objFromDb.StyleId = obj.StyleId;
                /*                objFromDb.Volume = obj.Volume;
                                objFromDb.Grapes = obj.Grapes;
                                objFromDb.ClasifierId = obj.ClasifierId;
                                objFromDb.IsInClasifier = obj.IsInClasifier;*/

                if (obj.ImageUrl != null)
                {
                    objFromDb.ImageUrl = obj.ImageUrl;
                }
            }
            _db.Wines.Update(objFromDb);
        }
    }
}
