using GwcltdApp.Data.Infrastructure;
using GwcltdApp.Data.Repositories;
using GwcltdApp.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using GwcltdApp.Web.Infrastructure.Extensions;

namespace GwcltdApp.Web.Infrastructure.Core
{
    public class ApiControllerBaseExtended : ApiController
    {
        protected List<Type> _requiredRepositories;

        protected readonly IDataRepositoryFactory _dataRepositoryFactory;
        protected IEntityBaseRepository<Error> _errorsRepository;
        protected IEntityBaseRepository<Option> _optionsRepository;
        protected IEntityBaseRepository<Production> _productionsRepository;
        protected IEntityBaseRepository<WSystem> _wsystemsRepository;
        protected IEntityBaseRepository<OptionType> _optiontypesRepository;
        protected IUnitOfWork _unitOfWork;

        private HttpRequestMessage RequestMessage;

        public ApiControllerBaseExtended(IDataRepositoryFactory dataRepositoryFactory, IUnitOfWork unitOfWork)
        {
            _dataRepositoryFactory = dataRepositoryFactory;
            _unitOfWork = unitOfWork;
        }

        protected HttpResponseMessage CreateHttpResponse(HttpRequestMessage request, List<Type> repos, Func<HttpResponseMessage> function)
        {
            HttpResponseMessage response = null;

            try
            {
                RequestMessage = request;
                InitRepositories(repos);
                response = function.Invoke();
            }
            catch (DbUpdateException ex)
            {
                LogError(ex);
                response = request.CreateResponse(HttpStatusCode.BadRequest, ex.InnerException.Message);
            }
            catch (Exception ex)
            {
                LogError(ex);
                response = request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

            return response;
        }
        
        private void InitRepositories(List<Type> entities)
        {
            _errorsRepository = _dataRepositoryFactory.GetDataRepository<Error>(RequestMessage);

            if (entities.Any(e => e.FullName == typeof(Option).FullName))
            {
                _optionsRepository = _dataRepositoryFactory.GetDataRepository<Option>(RequestMessage);
            }

            if (entities.Any(e => e.FullName == typeof(Production).FullName))
            {
                _productionsRepository = _dataRepositoryFactory.GetDataRepository<Production>(RequestMessage);
            }

            if (entities.Any(e => e.FullName == typeof(OptionType).FullName))
            {
                _optiontypesRepository = _dataRepositoryFactory.GetDataRepository<OptionType>(RequestMessage);
            }

            if (entities.Any(e => e.FullName == typeof(WSystem).FullName))
            {
                _wsystemsRepository = _dataRepositoryFactory.GetDataRepository<WSystem>(RequestMessage);
            }

            if (entities.Any(e => e.FullName == typeof(User).FullName))
            {
                _usersRepository = _dataRepositoryFactory.GetDataRepository<User>(RequestMessage);
            }
        }

        private void LogError(Exception ex)
        {
            try
            {
                Error _error = new Error()
                {
                    Message = ex.Message,
                    StackTrace = ex.StackTrace,
                    DateCreated = DateTime.Now
                };

                _errorsRepository.Add(_error);
                _unitOfWork.Commit();
            }
            catch { }
        }
    }
}