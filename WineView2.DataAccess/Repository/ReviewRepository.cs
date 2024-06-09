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
    public class ReviewRepository : Repository<Review>, IReviewRepository
    {
        private ApplicationDbContext _db;

        public ReviewRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Review obj)
        {
            _db.Reviews.Update(obj);
        }
    }
}
