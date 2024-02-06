using Gamification.UI.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gamification.UI.Models;

namespace Gamification.UI.Services.Interfaces
{
  public interface ITasksServices
  {
	Task<IEnumerable<Tasks>> GetTasks();
	Task<TasksResponse> GetTasksResponse(string username);
	Task<IEnumerable<TasksResponse>> GetResponsePoint(string username);
	Task<bool> CreateResponse(TasksResponse tasksResponse);
	public Task<IEnumerable<LeaderBoader>> GetLeaders(string caseStudy);
  }
}
