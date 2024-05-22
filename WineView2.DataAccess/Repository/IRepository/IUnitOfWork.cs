using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WineView2.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IColorRepository Color { get; }
        void Save();
    }
}
