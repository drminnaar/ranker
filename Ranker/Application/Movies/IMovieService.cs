using System.Collections.Generic;
using System.Threading.Tasks;
using Ranker.Application.Movies.Models;

namespace Ranker.Application.Movies
{
    public interface IMovieService
    {
        Task<MovieDetail> CreateMovie(MovieForCreate movieForCreate);
        Task DeleteMovie(long movieId);
        Task<MovieDetail> GetMovie(long movieId);
        Task<MovieForPatch?> GetMovieForPatch(long movieId);
        Task<IPagedCollection<MovieDetail>> GetMovies(MovieQuery query);
        Task PatchMovie(long movieId, MovieForPatch movieForPatch);
        Task UpdateMovie(long movieId, MovieForUpdate movieForUpdate);
    }
}
