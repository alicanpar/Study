using Study.DataAccess.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Study.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IProductCategoryRepository ProductCategory { get; }
        ICompanyRepository Company { get; }
        IProductRepository Products { get; }
        void Save();
    }
}
