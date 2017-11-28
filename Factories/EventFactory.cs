using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Options;
using ExcitedEmu.Models;
using Dapper;
using System.Linq;
namespace ExcitedEmu.Factories {
    public class EventFactory : IFactory<Event>{
        private readonly IOptions<MySqlOptions> MySqlConfig;
        public EventFactory(IOptions<MySqlOptions> config)
        {
            MySqlConfig = config;
        }
        internal IDbConnection Connection {
            get {
                return new MySqlConnection(MySqlConfig.Value.ConnectionString);
            }
        }
        public List<JoinResult> GetEvents() {
            using(IDbConnection dbConnection = Connection)
            {
                using(IDbCommand command = dbConnection.CreateCommand())
                {
                    string query = $"SELECT events.title, events.date, events.duration, users.first_name as coordinator, users.idusers as coordinatorID, events.participants, events.idevents, events.goal FROM users LEFT OUTER JOIN events ON users.idusers = events.users_idusers where idevents > 0 order by date";
                    dbConnection.Open();
                    return dbConnection.Query<JoinResult>(query).ToList();
                }
            }
        }
        public List<JoinResult> MyEvents(int userID) {
            using(IDbConnection dbConnection = Connection)
            {
                using(IDbCommand command = dbConnection.CreateCommand())
                {
                    
                    string query = $"SELECT events.title, events.date, events.duration, events.participants, events.idevents, participants.progress FROM events LEFT OUTER JOIN participants ON events.idevents = participants.events_idevents where participants.users_idusers = {userID}";
                    dbConnection.Open();
                    return dbConnection.Query<JoinResult>(query).ToList();
                }
            }
        }
        public JoinResult GetEvent(int EventID) {
            using(IDbConnection dbConnection = Connection)
            {
                using(IDbCommand command = dbConnection.CreateCommand())
                {
                    string query = $"SELECT events.title, events.date, events.duration, events.description, users.first_name as coordinator, events.goal FROM users LEFT OUTER JOIN events ON users.idusers = events.users_idusers where idevents = '{EventID}'";
                    dbConnection.Open();
                    return dbConnection.Query<JoinResult>(query).First();
                }
            }
        }
        public List<Participant> GetParticipants(int EventID){
            using(IDbConnection dbConnection = Connection)
            {
                using(IDbCommand command = dbConnection.CreateCommand())
                {
                    string query = $"SELECT users.first_name as name FROM users LEFT OUTER JOIN participants ON users.idusers = participants.users_idusers where events_idevents = '{EventID}'";
                    dbConnection.Open();
                    return dbConnection.Query<Participant>(query).ToList();
                }
            }
        }
        public List<EActivity> GetEActivities(int EventID){
            using(IDbConnection dbConnection = Connection)
            {
                using(IDbCommand command = dbConnection.CreateCommand())
                {
                    string query = $"SELECT users.first_name as participant, activities.progress, activities.title  FROM users LEFT OUTER JOIN activities ON users.idusers = activities.users_idusers where events_idevents = '{EventID}'";
                    dbConnection.Open();
                    return dbConnection.Query<EActivity>(query).ToList();
                }
            }
        }
        public void NewActivity(Activity Activity){
            using (IDbConnection dbConnection = Connection){
                string query = $"INSERT INTO activities (title, description, date, progress, users_idusers, events_idevents) VALUES (@title, @description, @date, @progress, @users_idusers, @events_idevents)";
                dbConnection.Execute(query, Activity);
                query = $"UPDATE participants SET progress = progress + @progress WHERE events_idevents = @events_idevents AND users_idusers = @users_idusers";
                dbConnection.Execute(query,Activity);
            }
        }
        public int AddEvent(Event Event, int userID){
            using (IDbConnection dbConnection = Connection){
                Event.duration = Event.duration.ToString() + " "+ Event.timeMod.ToString();
                string datetime = Event.date.ToString("yyyy/MM/dd ") + Event.time.ToString("HH:mm:ss");
                string query = $"INSERT INTO events (title, description, date, duration, users_idusers, participants, goal) VALUES (@title, @description, '{datetime}' , @duration, '{userID}', @participants, @goal)";
                dbConnection.Execute(query, Event);
                query = $"SELECT * FROM events WHERE title = @title";
                Event result = dbConnection.Query<Event>(query, Event).First();
                return result.idevents;
            }
        }
        public void JoinEvent(int EventID, int userID){
            using (IDbConnection dbConnection = Connection){
                string query = $"SELECT * FROM participants WHERE events_idevents = '{EventID}' and users_idusers = '{userID}'";
                if (dbConnection.Query<Participants>(query).ToList().Count == 0)
                {
                    query = $"SELECT * FROM events WHERE idevents = '{EventID}'";
                    Event result = dbConnection.Query<Event>(query).First();
                    int goal = result.goal;
                    query = $"INSERT INTO participants (events_idevents, users_idusers, goal) VALUES ('{EventID}','{userID}','{goal}')";
                    dbConnection.Execute(query);
                    query = $"UPDATE events SET participants = participants + 1 WHERE idevents = '{EventID}'";
                    dbConnection.Execute(query);
                }
            }
        }

        public void LeaveEvent(int EventID,int userID){
            using (IDbConnection dbConnection = Connection){
                string query = $"DELETE FROM participants WHERE events_idevents = '{EventID}' and users_idusers = '{userID}'";
                dbConnection.Execute(query);
                query = $"DELETE FROM activities WHERE events_idevents = '{EventID}' and users_idusers = '{userID}'";
                dbConnection.Execute(query);
                query = $"UPDATE events SET participants = participants - 1 WHERE idevents = '{EventID}'";
                dbConnection.Execute(query);
            }
        }
        public void DeleteEvent(int EventID){
            using (IDbConnection dbConnection = Connection){
                string query = $"DELETE FROM participants WHERE events_idevents = '{EventID}'";
                dbConnection.Execute(query);
                query = $"DELETE FROM activities WHERE events_idevents = '{EventID}'";
                dbConnection.Execute(query);
                query = $"DELETE FROM events WHERE idevents = '{EventID}'";
                dbConnection.Execute(query);
            }
        }
    }
}
