namespace Tulip.Services.Interfaces
{
    public interface ISAPBuilder
    {
        public ISAPBuilder SetUsername(string username);
        public ISAPBuilder SetCaseStudy(string caseStudy);
        public ISAPBuilder SetClientId(int clientId);
        public ISAPBuilder SetApplicationServer(string server);
        public Task<ISAP> Build();
    }
}