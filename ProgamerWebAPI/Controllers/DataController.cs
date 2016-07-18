using ProgamerWebAPI.Models;
using ProgamerWebAPI.Controllers;
using System;
using System.Linq;
using System.Web.Http;
using MySql.Data.MySqlClient;

namespace ProgamerWebAPI.Controllers
{
    [RoutePrefix("api/data")]
    public class DataController : ApiController
    {
        [Route("test")]
        public IHttpActionResult GetStatus()
        {
            try
            {
                MySQLConnection dbConnection = new MySQLConnection();
                MySqlDataReader dbReader = dbConnection.getMySqlDataReader("SELECT user.user_student_number, user.user_nickname, user.user_avatar, user.user_is_private FROM progamer.user WHERE user.user_student_number='test' AND user.user_password='test' limit 1;");
                dbReader.Read();
                if (dbReader.HasRows)
                {
                    User outgoing_user = new User();
                    outgoing_user.user_student_number = dbReader.GetString(0);
                    outgoing_user.user_nickname = dbReader.GetString(1);
                    outgoing_user.user_avatar = dbReader.GetInt32(2);
                    outgoing_user.user_is_private = dbReader.GetInt32(3);
                    dbConnection.closeConnection();
                    outgoing_user.response_valid = true;
                    outgoing_user.response_message = "Valid user found";
                    return Ok(outgoing_user);
                }
                else
                {
                    dbConnection.closeConnection();
                    BooleanResponse response = new BooleanResponse();
                    response.response_valid = false;
                    response.response_message = "test doesn't exist";
                    return Ok(response);
                }
            }
            catch (MySqlException ex)
            {
                BooleanResponse response = new BooleanResponse();
                response.response_valid = false;
                response.response_message = ex.Message;
                return Ok(response);
            }
        }

        [Route("register")]
        public IHttpActionResult PostRegister([FromBody]User incoming_user)
        {            
            try
            {
                if (incoming_user.Equals(null))
                {
                    BooleanResponse response = new BooleanResponse();
                    response.response_valid = false;
                    response.response_message = "Invalid input";
                    return Ok(response);
                }
                string user_student_number = incoming_user.user_student_number;
                string user_nickname = incoming_user.user_nickname;
                string user_password = incoming_user.user_password;
                int user_avatar = incoming_user.user_avatar;
                int user_is_private = incoming_user.user_is_private;
                MySQLConnection dbConnection = new MySQLConnection();
                MySqlDataReader dbReader = dbConnection.getMySqlDataReader("INSERT IGNORE INTO progamer.user (user.user_student_number, user.user_nickname, user.user_password, user.user_avatar, user.user_is_private) VALUES ('" + user_student_number + "', '" + user_nickname + "', '" + user_password + "', " + user_avatar + ", " + user_is_private + ");");
                dbConnection.closeConnection();
                if (dbReader.RecordsAffected == 1)
                {
                    BooleanResponse response = new BooleanResponse();
                    response.response_valid = true;
                    response.response_message = user_student_number + " registered succesfully";
                    return Ok(response);
                }
                else
                {
                    BooleanResponse response = new BooleanResponse();
                    response.response_valid = false;
                    response.response_message = user_student_number + " already exists";
                    return Ok(response);
                }
            }
            catch (MySqlException ex)
            {
                BooleanResponse response = new BooleanResponse();
                response.response_valid = false;
                response.response_message = ex.Message;
                return Ok(response);
            }
        }

        [Route("login")]
        public IHttpActionResult PostLogin([FromBody]User incoming_user)
        {            
            try
            {
                if (incoming_user.Equals(null))
                {
                    BooleanResponse response = new BooleanResponse();
                    response.response_valid = false;
                    response.response_message = "Invalid input";
                    return Ok(response);
                }
                string user_student_number = incoming_user.user_student_number;
                string user_password = incoming_user.user_password;
                MySQLConnection dbConnection = new MySQLConnection();
                MySqlDataReader dbReader = dbConnection.getMySqlDataReader("SELECT user.user_student_number, user.user_nickname, user.user_avatar, user.user_is_private FROM progamer.user WHERE user.user_student_number='" + user_student_number + "' AND user.user_password='" + user_password + "' limit 1;");
                dbReader.Read();
                if (dbReader.HasRows)
                {
                    User outgoing_user = new User();
                    outgoing_user.user_student_number = dbReader.GetString(0);
                    outgoing_user.user_nickname = dbReader.GetString(1);
                    outgoing_user.user_avatar = dbReader.GetInt32(2);
                    outgoing_user.user_is_private = dbReader.GetInt32(3);
                    dbConnection.closeConnection();
                    outgoing_user.response_valid = true;
                    outgoing_user.response_message = "Valid user found";
                    return Ok(outgoing_user);
                }
                else
                {
                    dbConnection.closeConnection();
                    BooleanResponse response = new BooleanResponse();
                    response.response_valid = false;
                    response.response_message = user_student_number + " doesn't exist";
                    return Ok(response);
                }
            }
            catch (MySqlException ex)
            {
                BooleanResponse response = new BooleanResponse();
                response.response_valid = false;
                response.response_message = ex.Message;
                return Ok(response);
            }
        }

        [Route("leaderboard")]
        public IHttpActionResult PostLeaderboard([FromBody]User incoming_user)
        {            
            try
            {
                if (incoming_user.Equals(null))
                {
                    BooleanResponse response = new BooleanResponse();
                    response.response_valid = false;
                    response.response_message = "Invalid input";
                    return Ok(response);
                }
                string user_student_number = incoming_user.user_student_number;
                MySQLConnection dbConnection = new MySQLConnection();
                MySqlDataReader dbReader = dbConnection.getMySqlDataReader("SELECT list1.user_student_number, user.user_nickname, user.user_avatar, list1.user_overall_score, list1.user_overall_attempts, list1.user_overall_time FROM (SELECT userlevel.user_student_number, SUM(userlevel.userlevel_score) AS user_overall_score, SUM(userlevel.userlevel_attempts) AS user_overall_attempts, SUM(userlevel.userlevel_time) AS user_overall_time FROM progamer.userlevel INNER JOIN progamer.level ON userlevel.level_id=level.level_id WHERE level.level_number<=(SELECT MAX(level.level_number) FROM progamer.userlevel INNER JOIN progamer.level ON userlevel.level_id=level.level_id AND userlevel.user_student_number='" + user_student_number + "') group by userlevel.user_student_number) AS list1 INNER JOIN (SELECT userlevel.user_student_number FROM progamer.userlevel INNER JOIN progamer.level ON userlevel.level_id=level.level_id WHERE level.level_number=(SELECT MAX(level.level_number) FROM progamer.userlevel INNER JOIN progamer.level ON userlevel.level_id=level.level_id AND userlevel.user_student_number='" + user_student_number + "')) AS list2 ON list1.user_student_number=list2.user_student_number INNER JOIN progamer.user ON user.user_student_number=list1.user_student_number;");
                if (dbReader.HasRows)
                {
                    UserList outgoing_user_list = new UserList();
                    while (dbReader.Read())
                    {
                        User outgoing_user = new User();
                        outgoing_user.user_student_number = dbReader.GetString(0);
                        outgoing_user.user_nickname = dbReader.GetString(1);
                        outgoing_user.user_avatar = dbReader.GetInt32(2);
                        outgoing_user.user_overall_score = dbReader.GetInt32(3);
                        outgoing_user.user_overall_attempts = dbReader.GetInt32(4);
                        outgoing_user.user_overall_time = dbReader.GetInt32(5);
                        outgoing_user_list.user_list.Add(outgoing_user);
                    }
                    dbConnection.closeConnection();
                    outgoing_user_list.response_valid = true;
                    outgoing_user_list.response_message = "Valid users found";
                    return Ok(outgoing_user_list);
                }
                else
                {
                    dbConnection.closeConnection();
                    BooleanResponse response = new BooleanResponse();
                    response.response_valid = false;
                    response.response_message = user_student_number + " doesn't exist";
                    return Ok(response);
                }
            }
            catch (MySqlException ex)
            {
                BooleanResponse response = new BooleanResponse();
                response.response_valid = false;
                response.response_message = ex.Message;
                return Ok(response);
            }
        }

        [Route("levels")]
        public IHttpActionResult GetLevels()
        {           
            try
            {
                MySQLConnection dbConnection1 = new MySQLConnection();
                MySqlDataReader dbReader1 = dbConnection1.getMySqlDataReader("SELECT level.level_id, level.level_number, level.level_title, level.level_description FROM progamer.level;");
                if (dbReader1.HasRows)
                {
                    LevelList outgoing_level_list = new LevelList();
                    while (dbReader1.Read())
                    {
                        Level outgoing_level = new Level();
                        outgoing_level.level_id = dbReader1.GetInt32(0);
                        outgoing_level.level_number = dbReader1.GetInt32(1);
                        outgoing_level.level_title = dbReader1.GetString(2);
                        outgoing_level.level_description = dbReader1.GetString(3);
                        MySQLConnection dbConnection2 = new MySQLConnection();
                        MySqlDataReader dbReader2 = dbConnection2.getMySqlDataReader("SELECT puzzle.puzzle_id, puzzle.level_id, puzzle.puzzle_type, puzzle.puzzle_instructions, puzzle.puzzle_expected_output, puzzle.puzzle_data FROM progamer.puzzle WHERE puzzle.level_id=" + outgoing_level.level_id + " ORDER BY RAND() LIMIT 5;");
                        if (dbReader2.HasRows)
                        {
                            while (dbReader2.Read())
                            {
                                Puzzle outgoing_puzzle = new Puzzle(); 61404
                                outgoing_puzzle.puzzle_id = dbReader2.GetInt32(0);
                                outgoing_puzzle.puzzle_level_id = dbReader2.GetInt32(1);
                                outgoing_puzzle.puzzle_type = dbReader2.GetString(2);
                                outgoing_puzzle.puzzle_instructions = dbReader2.GetString(3);
                                outgoing_puzzle.puzzle_expected_outcome = dbReader2.GetString(4);
                                outgoing_puzzle.puzzle_data = dbReader2.GetString(5);
                                outgoing_level.puzzle_list.Add(outgoing_puzzle);
                                
                            }
                        }
                        dbConnection2.closeConnection();
                        outgoing_level_list.level_list.Add(outgoing_level);
                    }
                    dbConnection1.closeConnection();
                    outgoing_level_list.response_valid = true;
                    outgoing_level_list.response_message = "Valid levels found";
                    return Ok(outgoing_level_list);
                }
                else
                {
                    dbConnection1.closeConnection();
                    BooleanResponse response = new BooleanResponse();
                    response.response_valid = false;
                    response.response_message = "Levels don't exist";
                    return Ok(response);
                }
            }
            catch (MySqlException ex)
            {
                BooleanResponse response = new BooleanResponse();
                response.response_valid = false;
                response.response_message = ex.Message;
                return Ok(response);
            }
        }
    }
}
