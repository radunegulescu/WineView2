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
        public IApplicationUserRepository ApplicationUser { get; private set; }
        public IShoppingCartRepository ShoppingCart { get; private set; }
        public IOrderHeaderRepository OrderHeader { get; private set; }
        public IOrderDetailRepository OrderDetail { get; private set; }
        public IColorRepository Color { get; private set; }
        public IWineRepository Wine { get; private set; }
        public IWineryRepository Winery { get; private set; }
        public IStyleRepository Style { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            ApplicationUser = new ApplicationUserRepository(_db);
            ShoppingCart = new ShoppingCartRepository(_db);
            OrderHeader = new OrderHeaderRepository(_db);
            OrderDetail = new OrderDetailRepository(_db);
            Color = new ColorRepository(_db);
            Wine = new WineRepository(_db); 
            Winery = new WineryRepository(_db);
            Style = new StyleRepository(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
