using Ferma.Core.Entites;
using Ferma.Core.IUnitOfWork;
using Ferma.Data.UnitOfWork;
using Ferma.Service.Services.Interfaces.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferma.Service.Services.Implementations.User
{
    public class UserTermIndexServices : IUserTermIndexServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserTermIndexServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<UserTerm>> UserTerms()
        {
            var terms = await _unitOfWork.UserTermRepository.GetAllAsync(x => !x.IsDelete);
            return terms.ToList();
        }
    }
}
