using Gamification.UI.Data;
using Gamification.UI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gamification.UI.Models;

namespace Gamification.UI.Services.Implementations
{
	public class TasksService : ITasksServices
	{
		public readonly ApplicationDbContext _db;
		public TasksService(ApplicationDbContext db)
		{
			_db = db;
		}

		public async Task<IEnumerable<Tasks>> GetTasks()
		{
			var data = await _db.Tasks.ToListAsync();
			return data;
		}

		public async Task<TasksResponse> GetTasksResponse(string username)
		{
			throw new System.NotImplementedException();
		}

		public async Task<IEnumerable<TasksResponse>> GetResponsePoint(string username)
		{
			var data = await _db.Responses.Where(q => q.RespondantName == username).ToListAsync();
			return data;
		}

		public async Task<bool> CreateResponse(TasksResponse tasksResponse)
		{
			var d = tasksResponse.Score;

			var score = await _db.Scores.Where(u => u.Username == tasksResponse.RespondantName).FirstOrDefaultAsync();




			if (score == null)
			{
				var setScore = new Scores
				{

					Username = tasksResponse.RespondantName,
					Score = tasksResponse.Score
				};
				await _db.Scores.AddAsync(setScore);
			}
			else
			{
				var pushtoScore = new Scores
				{
					//Id = score.Id,
					Username = tasksResponse.RespondantName,
					Score = tasksResponse.Score + score.Score
				};
				d = score.Score + tasksResponse.Score;
				score.Score = d;
				_db.Entry<Scores>(score).State = EntityState.Modified;
				//pushtoScore.Score = d + score.Score;
				//pushtoScore.Id = score.Id;
				//pushtoScore.Username = tasksResponse.RespondantName;
				//_db.Scores.Update(pushtoScore);
			}


			await _db.Responses.AddAsync(tasksResponse);

			var save = await _db.SaveChangesAsync();
			return save > 0;
		}

		public async Task<IEnumerable<LeaderBoader>> GetLeaders(string caseStudy)
		{
			var data = await _db.LeaderBoaders.OrderByDescending(s => s.Point).Where(c => c.CaseStudy == caseStudy).ToListAsync();
			return data;
		}

		//public async Task<int> GetLeaders()
		//{
		// var data =  _db.Responses.Where(s=>s.RespondantName==username).Sum(q=>q.Score);
		//var data =  _db.Responses.GroupBy(c => c.RespondantName).
		//	  Select(g => new
		//	  {
		//		g.Key,
		//		SUM = g.Sum(s => s.Score)//.Inqueries.Select(t => t.TotalTimeSpent).Sum())
		//	  });
		//var data = _db.Responses.FindAsync().GroupBy(i => i.RespondantName).Sum(c => c.Sum(a => a.Score));


		//var query = from p in _db.Responses
		//	  group p by p.RespondantName
		//into g
		//	  where g.Sum(a=>a.Score)
		//	  orderby g.Key
		//	  select new { g.Key, Sum = g.Sum() };

		//return data;
		//}
	}
}
