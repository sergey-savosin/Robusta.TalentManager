﻿using AutoMapper;
using Robusta.TalentManager.Data;
using Robusta.TalentManager.Domain;
using Robusta.TalentManager.WebApi.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Rousta.TalentManager.WebApi.WebHost.Controllers
{
    public class EmployeesController: ApiController
    {
        private readonly IUnitOfWork uow = null;
        private readonly IRepository<Employee> repository = null;
        private readonly IMapper mapper = null;

        public EmployeesController(IUnitOfWork uow, IRepository<Employee> repository, IMapper mapper)
        {
            this.uow = uow;
            this.repository = repository;
            this.mapper = mapper;
        }

        public HttpResponseMessage Get(int id)
        {
            var employee = repository.Find(id);
            if (employee == null)
            {
                var response = Request.CreateResponse(HttpStatusCode.NotFound, "Employee not found");

                throw new HttpResponseException(response);
            }

            return Request.CreateResponse<EmployeeDto>(HttpStatusCode.OK, mapper.Map<Employee, EmployeeDto>(employee));
        }

        public HttpResponseMessage Post(EmployeeDto employeeDto)
        {
            var employee = mapper.Map<EmployeeDto, Employee>(employeeDto);

            repository.Insert(employee);
            uow.Save();

            var response = Request.CreateResponse<Employee>(HttpStatusCode.Created, employee);

            string uri = Url.Link("DefaultApi", new { id = employee.Id });
            response.Headers.Location = new Uri(uri);
            return response;
        }

        protected override void Dispose(bool disposing)
        {
            if (repository != null)
                repository.Dispose();

            if (uow != null)
                uow.Dispose();
                
            base.Dispose(disposing);
        }
    }

}