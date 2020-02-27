using System.Threading.Tasks;
using Circa.Actors.Domain.Dtos;
using MongoDB.Bson;
using Solari.Vanth;

namespace Circa.Actors.Application
{
    public interface IPersonApplication
    {
        Task<CommonResponse<ObjectId>> Insert(InsertPersonDto dto);
    }
}