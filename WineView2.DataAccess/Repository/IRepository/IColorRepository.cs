using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WineView2.Models;

namespace WineView2.DataAccess.Repository.IRepository
{
    public interface IColorRepository : IRepository<Color>
    {
        void Update(Color obj);
    }
}
