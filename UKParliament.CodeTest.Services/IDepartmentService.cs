using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Services.Helpers;

namespace UKParliament.CodeTest.Services;
public interface IDepartmentService
{
    Task<ServiceResult<List<Department>>> GetAllDepartmentsAsync();
}
