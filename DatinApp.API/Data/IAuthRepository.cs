using System.Threading.Tasks;
using DatinApp.API.Models;

namespace DatinApp.API.Controllers
{
    public interface IAuthRepository
    {
        Task<tbl_user> Register(tbl_user user, string password);
        Task<tbl_user> Login(string username, string password);
        Task<bool> UserExist(string username);

    }
}