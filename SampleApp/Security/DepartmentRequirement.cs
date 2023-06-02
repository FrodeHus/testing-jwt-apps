using Microsoft.AspNetCore.Authorization;

namespace SampleApp.Security;

public class DepartmentRequirement : IAuthorizationRequirement
{
    public DepartmentRequirement(string departmentName) => Department = departmentName;

    public string Department { get; }
}
