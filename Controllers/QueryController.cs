using System;
using System.ComponentModel;
using Dictionary.Models;
using Dictionary.Models.DTOs;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Dictionary.Controllers
{
    public class QueryController : ApiController
    {
        private readonly ApplicationDbContext _dbContext = new ApplicationDbContext();

        // GET api/Query/Translate
        [HttpPost]
        [Route("api/Query/Translate")]
        public async Task<TranslateResultDTO> Translate([Required][FromBody] TranslateRequestDTO request, CancellationToken cts)
        {
            var collection =  await _dbContext.DictionaryEntries
                .Where(e => 
                    request.DictionaryIds.Contains(e.DictionaryId) && 
                    e.Word.ToLower().Contains(request.Keyword.ToLower()))
                .GroupBy(r => r.Dictionary)
                .ToListAsync(cts);

            return new TranslateResultDTO
            {
                Keyword = request.Keyword,
                Results = collection.Select(group => new DictionaryQueryResultDTO
                {
                    DictionaryId = group.Key.Id,
                    DictionaryName = group.Key.Name,
                    Results = group.Select(e => new DictionaryEntryDTO
                    {
                        Id = e.Id,
                        Word = e.Word,
                        Meaning = e.Meaning,
                        WordDifference = StringsDifferenceScore(e.Word.ToLower(), request.Keyword.ToLower()),
                    }).OrderBy(e => e.WordDifference).ToList(),
                }).ToList(),
            };
        }

        // GET api/Query/Languages
        [HttpGet]
        [Route("api/Query/Languages")]
        public async Task<LanguageCollectionDTO> GetLanguages(CancellationToken cts)
        {
            var collection =  await _dbContext.Languages.ToListAsync(cts);

            return new LanguageCollectionDTO
            {
                Languages = collection.Select(l => new LanguageDTO
                {
                    Id = l.Id,
                    Code = l.Code,
                    Name = l.Name,
                    SourceDictionaries = l.SourceDictionaries.Select(d => new DictionaryDTO
                    {
                        Id = d.Id,
                        Name = d.Name,
                        Description = d.Description,
                    }).ToList(),
                }).ToList(),
            };
        }

        public static int StringsDifferenceScore(string firstStr, string secondStr)
        {
            if (firstStr.Length == 0)
            {
                return secondStr.Length;
            }

            if (secondStr.Length == 0)
            {
                return firstStr.Length;
            }

            var d = new int[firstStr.Length + 1, secondStr.Length + 1];
            for (var i = 0; i <= firstStr.Length; i++)
            {
                d[i, 0] = i;
            }

            for (var j = 0; j <= secondStr.Length; j++)
            {
                d[0, j] = j;
            }

            for (var i = 1; i <= firstStr.Length; i++)
            {
                for (var j = 1; j <= secondStr.Length; j++)
                { 
                    var cost = (secondStr[j - 1] == firstStr[i - 1]) ? 0 : 1; 
                    d[i, j] = Math.Min(d[i - 1, j] + 1, Math.Min(d[i, j - 1] + 1, d[i - 1, j - 1] + cost)); 
                } 
            } 

            return d[firstStr.Length, secondStr.Length]; 
        } 
    }
}