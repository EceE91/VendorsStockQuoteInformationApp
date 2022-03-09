using System.Collections.Generic;
using System.Threading.Tasks;
using AspNetCoreAngularApp.AspNetCoreAngularApp.Core.Models;

namespace AspNetCoreAngularApp.AspNetCoreAngularApp.Core.Interfaces
{
    public interface IRepositoryBase<T> where T : ModelBase
    {
        /// <summary>
        /// Gets all records
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<T>> GetAllAsync();
    }
}