using System.Threading.Tasks;
using Circa.Actors.Domain;
using Solari.Callisto.Abstractions.CQR;
using Solari.Vanth;

namespace Circa.Actors.Infra
{
    public interface IPersonRepository
    {
        Task<CommonResponse<Person>> AddPerson(ICallistoInsert<Person> operation);
    }
}