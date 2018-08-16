using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using reportservice.Model;

namespace reportservice.Service.Interface {
    public interface IGenealogyService {
        Task<(object, HttpStatusCode)> getGenealogyOpAsync(long startDate, long endDate, string op, string codigo, string fieldFilter);        
    }
}