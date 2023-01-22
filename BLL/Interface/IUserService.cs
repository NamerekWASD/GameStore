using BLL.DTO;

namespace BLL.Interface
{
	public interface ICompanyService
	{
		CompanyDTO GetCompany(int id);
		CompanyDTO AddCompany(CompanyDTO Company);
		CompanyDTO EditCompany(int id, CompanyDTO Company);
		CompanyDTO DeleteCompany(int id);
	}
}