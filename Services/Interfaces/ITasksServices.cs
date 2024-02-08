using Tulip.Data;
using Tulip.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tulip.Services.Interfaces
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
