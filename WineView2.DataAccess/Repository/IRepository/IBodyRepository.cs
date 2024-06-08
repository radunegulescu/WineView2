using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WineView2.Models;

namespace WineView2.DataAccess.Repository.IRepository
{
    public interface IBodyRepository : IRepository<Body>
    {
        void Update(Body obj);
    }
}
