using ProgamerWebAPI.Models;
using ProgamerWebAPI.Controllers;
using System;
using System.Linq;
using System.Web.Http;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

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
                MySQLConnection dbConnection1 = new MySQLConnection();
                MySqlDataReader dbReader1 = dbConnection1.getMySqlDataReader("INSERT IGNORE INTO progamer.user (user.user_student_number, user.user_nickname, user.user_password, user.user_avatar, user.user_is_private) VALUES ('" + user_student_number + "', '" + user_nickname + "', '" + user_password + "', " + user_avatar + ", " + user_is_private + ");");
                dbConnection1.closeConnection();
                if (dbReader1.RecordsAffected == 1)
                {
                    MySQLConnection dbConnection2 = new MySQLConnection();
                    MySqlDataReader dbReader2 = dbConnection2.getMySqlDataReader("INSERT INTO progamer.userlevel (userlevel.user_student_number, userlevel.level_id) SELECT '" + user_student_number + "', level.level_id FROM progamer.level;");
                    dbConnection2.closeConnection();
                    MySQLConnection dbConnection3 = new MySQLConnection();
                    MySqlDataReader dbReader3 = dbConnection3.getMySqlDataReader("INSERT INTO progamer.userlevel (userlevel.user_student_number, userlevel.level_id) SELECT '" + user_student_number + "', level.level_id FROM progamer.level;");
                    dbConnection3.closeConnection();
                    if (dbReader2.RecordsAffected != 0 && dbReader3.RecordsAffected != 0)
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
                        response.response_message = "Failed to add userlevel data with level count " + dbReader2.RecordsAffected;
                        return Ok(response);
                    }
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
                MySqlDataReader dbReader = dbConnection.getMySqlDataReader("SELECT user.user_student_number, user.user_nickname, user.user_avatar, user.user_is_private, user.user_type FROM progamer.user WHERE user.user_student_number='" + user_student_number + "' AND user.user_password='" + user_password + "' limit 1;");
                dbReader.Read();
                if (dbReader.HasRows)
                {
                    User outgoing_user = new User();
                    outgoing_user.user_student_number = dbReader.GetString(0);
                    outgoing_user.user_nickname = dbReader.GetString(1);
                    outgoing_user.user_avatar = dbReader.GetInt32(2);
                    outgoing_user.user_is_private = dbReader.GetInt32(3);
                    outgoing_user.user_type = dbReader.GetString(4);
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
                string user_type = incoming_user.user_type;
                MySQLConnection dbConnection = new MySQLConnection();
                if (user_type.Equals("admin"))
                {
                    MySqlDataReader dbReader = dbConnection.getMySqlDataReader("SELECT user.user_student_number, user.user_nickname, user.user_avatar, IFNULL(list1.user_total_score, 0), IFNULL(list1.user_total_attempts, 0), IFNULL(list1.user_total_time, 0) FROM (SELECT userlevel.user_student_number, SUM(userlevel.userlevel_score) AS user_total_score, SUM(userlevel.userlevel_attempts) AS user_total_attempts, SUM(userlevel.userlevel_time) AS user_total_time FROM progamer.userlevel WHERE userlevel.userlevel_completed=1 GROUP BY userlevel.user_student_number) AS list1 RIGHT JOIN progamer.user ON list1.user_student_number=user.user_student_number WHERE user.user_type='student';");
                    if (dbReader.HasRows)
                    {
                        UserList outgoing_user_list = new UserList();
                        while (dbReader.Read())
                        {
                            User outgoing_user = new User();
                            outgoing_user.user_student_number = dbReader.GetString(0);
                            outgoing_user.user_nickname = dbReader.GetString(1);
                            outgoing_user.user_avatar = dbReader.GetInt32(2);
                            outgoing_user.user_total_score = dbReader.GetInt32(3);
                            outgoing_user.user_total_attempts = dbReader.GetInt32(4);
                            outgoing_user.user_total_time = dbReader.GetInt32(5);
                            outgoing_user_list.user_list.Add(outgoing_user);
                        }
                        outgoing_user_list.response_valid = true;
                        outgoing_user_list.response_message = "Valid users found";
                        dbConnection.closeConnection();
                        return Ok(outgoing_user_list);
                    }
                    else
                    {
                        BooleanResponse response = new BooleanResponse();
                        response.response_valid = false;
                        response.response_message = user_student_number + " doesn't exist";
                        dbConnection.closeConnection();
                        return Ok(response);
                    }
                }
                else
                {
                    MySqlDataReader dbReader = dbConnection.getMySqlDataReader("SELECT user.user_student_number, user.user_nickname, user.user_avatar, userlevel_data.user_total_score, userlevel_data.user_total_attempts, userlevel_data.user_total_time FROM (SELECT list1.user_student_number, list1.user_total_score, list1.user_total_attempts, list1.user_total_time FROM (SELECT userlevel.user_student_number, SUM(userlevel.userlevel_score) AS user_total_score, SUM(userlevel.userlevel_attempts) AS user_total_attempts, SUM(userlevel.userlevel_time) AS user_total_time FROM progamer.userlevel WHERE userlevel.userlevel_completed=1 AND userlevel.level_id<=(SELECT MAX(userlevel.level_id) FROM progamer.userlevel WHERE userlevel.user_student_number='" + user_student_number + "' AND userlevel.userlevel_completed=1) group by userlevel.user_student_number) AS list1 INNER JOIN (SELECT userlevel.user_student_number FROM progamer.userlevel WHERE userlevel.userlevel_completed=1 AND userlevel.level_id=(SELECT MAX(userlevel.level_id) FROM progamer.userlevel WHERE userlevel.user_student_number='" + user_student_number + "' AND userlevel.userlevel_completed=1) group by userlevel.user_student_number) list2 ON list1.user_student_number=list2.user_student_number) AS userlevel_data INNER JOIN progamer.user ON userlevel_data.user_student_number=user.user_student_number WHERE user_type='student';");
                    if (dbReader.HasRows)
                    {
                        UserList outgoing_user_list = new UserList();
                        while (dbReader.Read())
                        {
                            User outgoing_user = new User();
                            outgoing_user.user_student_number = dbReader.GetString(0);
                            outgoing_user.user_nickname = dbReader.GetString(1);
                            outgoing_user.user_avatar = dbReader.GetInt32(2);
                            outgoing_user.user_total_score = dbReader.GetInt32(3);
                            outgoing_user.user_total_attempts = dbReader.GetInt32(4);
                            outgoing_user.user_total_time = dbReader.GetInt32(5);
                            outgoing_user_list.user_list.Add(outgoing_user);
                        }
                        outgoing_user_list.response_valid = true;
                        outgoing_user_list.response_message = "Valid users found";
                        dbConnection.closeConnection();
                        return Ok(outgoing_user_list);
                    }
                    else
                    {
                        BooleanResponse response = new BooleanResponse();
                        response.response_valid = false;
                        response.response_message = user_student_number + " doesn't exist";
                        dbConnection.closeConnection();
                        return Ok(response);
                    }
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

        [Route("studentdata")]
        public IHttpActionResult PostStudentData([FromBody]User incoming_user)
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
                MySqlDataReader dbReader = dbConnection.getMySqlDataReader("SELECT user.user_student_number, user.user_nickname, IFNULL(list1.user_levels_completed, 0), IFNULL(list1.user_total_score, 0), IFNULL(list1.user_total_attempts, 0), IFNULL(list1.user_total_time, 0), IFNULL(list1.user_average_score, 0), IFNULL(list1.user_average_attempts, 0), IFNULL(list1.user_average_time, 0) FROM (SELECT userlevel.user_student_number, MAX(userlevel.level_id) AS user_levels_completed, SUM(userlevel.userlevel_score) AS user_total_score, SUM(userlevel.userlevel_attempts) AS user_total_attempts, SUM(userlevel.userlevel_time) AS user_total_time, AVG(userlevel.userlevel_score) AS user_average_score, AVG(userlevel.userlevel_attempts) AS user_average_attempts, AVG(userlevel.userlevel_time) AS user_average_time FROM progamer.userlevel WHERE userlevel.userlevel_completed=1 GROUP BY userlevel.user_student_number) AS list1 RIGHT JOIN progamer.user ON list1.user_student_number=user.user_student_number WHERE user.user_type='student';");
                if (dbReader.HasRows)
                {
                    UserList outgoing_user_list = new UserList();
                    while (dbReader.Read())
                    {
                        User outgoing_user = new User();
                        outgoing_user.user_student_number = dbReader.GetString(0);
                        outgoing_user.user_nickname = dbReader.GetString(1);
                        outgoing_user.user_levels_completed = dbReader.GetInt32(2);
                        outgoing_user.user_total_score = dbReader.GetInt32(3);
                        outgoing_user.user_total_attempts = dbReader.GetInt32(4);
                        outgoing_user.user_total_time = dbReader.GetInt32(5);
                        outgoing_user.user_average_score = dbReader.GetDouble(6);
                        outgoing_user.user_average_attempts = dbReader.GetDouble(7);
                        outgoing_user.user_average_time = dbReader.GetDouble(8);
                        outgoing_user_list.user_list.Add(outgoing_user);
                    }
                    outgoing_user_list.response_valid = true;
                    outgoing_user_list.response_message = "Valid users found";
                    dbConnection.closeConnection();
                    return Ok(outgoing_user_list);
                }
                else
                {
                    BooleanResponse response = new BooleanResponse();
                    response.response_valid = false;
                    response.response_message = user_student_number + " doesn't exist";
                    dbConnection.closeConnection();
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
        public IHttpActionResult PostLevels([FromBody]User incoming_user)
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
                MySQLConnection dbConnection0 = new MySQLConnection();
                MySqlDataReader dbReader0 = dbConnection0.getMySqlDataReader("INSERT INTO progamer.userlevel (userlevel.user_student_number, userlevel.level_id) SELECT * FROM (SELECT '" + user_student_number + "', level.level_id FROM progamer.level) as tmp WHERE NOT EXISTS (SELECT * FROM progamer.userlevel WHERE userlevel.user_student_number='" + user_student_number + "' AND tmp.level_id=userlevel.level_id);");
                dbConnection0.closeConnection();
                MySQLConnection dbConnection1 = new MySQLConnection();
                MySqlDataReader dbReader1 = dbConnection1.getMySqlDataReader("SELECT userlevel.user_student_number, level.level_id, level.level_number, level.level_title, level.level_description, userlevel.userlevel_completed, userlevel.userlevel_score, userlevel.userlevel_attempts, userlevel.userlevel_time FROM progamer.level LEFT JOIN progamer.userlevel ON level.level_id=userlevel.level_id AND userlevel.user_student_number='" + user_student_number + "';");
                if (dbReader1.HasRows)
                {
                    LevelList outgoing_level_list = new LevelList();
                    while (dbReader1.Read())
                    {
                        Level outgoing_level = new Level();
                        outgoing_level.level_user_student_number_id = dbReader1.GetString(0);
                        outgoing_level.level_id = dbReader1.GetInt32(1);
                        outgoing_level.level_number = dbReader1.GetInt32(2);
                        outgoing_level.level_title = dbReader1.GetString(3);
                        outgoing_level.level_description = dbReader1.GetString(4);
                        outgoing_level.level_completed = dbReader1.GetInt32(5); ;
                        outgoing_level.level_score = dbReader1.GetInt32(6);
                        outgoing_level.level_attempts = dbReader1.GetInt32(7);
                        outgoing_level.level_time = dbReader1.GetInt32(8);
                        MySQLConnection dbConnection2 = new MySQLConnection();
                        MySqlDataReader dbReader2 = dbConnection2.getMySqlDataReader("SELECT puzzle.puzzle_id, puzzle.puzzle_level_id, puzzle.puzzle_instructions, puzzle.puzzle_data FROM progamer.puzzle WHERE puzzle.puzzle_level_id=" + outgoing_level.level_id + " ORDER BY RAND() LIMIT 5;");
                        if (dbReader2.HasRows)
                        {
                            while (dbReader2.Read())
                            {
                                Puzzle outgoing_puzzle = new Puzzle();
                                outgoing_puzzle.puzzle_id = dbReader2.GetInt32(0);
                                outgoing_puzzle.puzzle_level_id = dbReader2.GetInt32(1);
                                outgoing_puzzle.puzzle_instructions = dbReader2.GetString(2);
                                outgoing_puzzle.puzzle_data = dbReader2.GetString(3);
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

        [Route("adminlevels")]
        public IHttpActionResult PostAdminLevels([FromBody]User incoming_user)
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
                MySqlDataReader dbReader = dbConnection.getMySqlDataReader("SELECT level.level_id, level.level_number, level.level_title FROM progamer.level;");
                if (dbReader.HasRows)
                {
                    LevelList outgoing_level_list = new LevelList();
                    while (dbReader.Read())
                    {
                        Level outgoing_level = new Level();
                        outgoing_level.level_id = dbReader.GetInt32(0);
                        outgoing_level.level_number = dbReader.GetInt32(1);
                        outgoing_level.level_title = dbReader.GetString(2);
                        outgoing_level_list.level_list.Add(outgoing_level);
                    }
                    dbConnection.closeConnection();
                    outgoing_level_list.response_valid = true;
                    outgoing_level_list.response_message = "Valid levels found";
                    return Ok(outgoing_level_list);
                }
                else
                {
                    dbConnection.closeConnection();
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

        [Route("adminpuzzles")]
        public IHttpActionResult PostAdminPuzzles([FromBody]Level incoming_level)
        {
            try
            {
                if (incoming_level.Equals(null))
                {
                    BooleanResponse response = new BooleanResponse();
                    response.response_valid = false;
                    response.response_message = "Invalid input";
                    return Ok(response);
                }
                int level_id = incoming_level.level_id;
                MySQLConnection dbConnection = new MySQLConnection();
                MySqlDataReader dbReader = dbConnection.getMySqlDataReader("SELECT puzzle.puzzle_id, puzzle.puzzle_instructions, puzzle.puzzle_data FROM progamer.puzzle WHERE puzzle.puzzle_level_id="+level_id+";");
                if (dbReader.HasRows)
                {
                    PuzzleList outgoing_puzzle_list = new PuzzleList();
                    while (dbReader.Read())
                    {
                        Puzzle outgoing_puzzle = new Puzzle();
                        outgoing_puzzle.puzzle_id = dbReader.GetInt32(0);
                        outgoing_puzzle.puzzle_instructions = dbReader.GetString(1);
                        outgoing_puzzle.puzzle_data = dbReader.GetString(2);
                        outgoing_puzzle_list.puzzle_list.Add(outgoing_puzzle);
                    }
                    dbConnection.closeConnection();
                    outgoing_puzzle_list.response_valid = true;
                    outgoing_puzzle_list.response_message = "Valid puzzles found";
                    return Ok(outgoing_puzzle_list);
                }
                else
                {
                    dbConnection.closeConnection();
                    BooleanResponse response = new BooleanResponse();
                    response.response_valid = false;
                    response.response_message = "No puzzles found";
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

        [Route("averagelevel")]
        public IHttpActionResult PostAverageLevel([FromBody]Level incoming_level)
        {
            try
            {
                if (incoming_level.Equals(null))
                {
                    BooleanResponse response = new BooleanResponse();
                    response.response_valid = false;
                    response.response_message = "Invalid input";
                    return Ok(response);
                }
                int level_id = incoming_level.level_id;
                MySQLConnection dbConnection = new MySQLConnection();
                MySqlDataReader dbReader = dbConnection.getMySqlDataReader("SELECT AVG(userlevel.userlevel_score) AS avg_score, AVG(userlevel.userlevel_attempts) AS avg_attempts, AVG(userlevel.userlevel_time) AS avg_time FROM progamer.userlevel INNER JOIN progamer.user ON userlevel.user_student_number=user.user_student_number WHERE userlevel.level_id=" + level_id + " AND userlevel.userlevel_score<>0 AND userlevel.userlevel_attempts<>0 AND userlevel.userlevel_time<>0 AND user.user_type='student';");
                dbReader.Read();
                if (dbReader.HasRows)
                {
                    Level outgoing_level = new Level();
                    outgoing_level.level_score = dbReader.GetInt32(0);
                    outgoing_level.level_attempts = dbReader.GetInt32(1);
                    outgoing_level.level_time = dbReader.GetInt32(2);
                    outgoing_level.response_valid = true;
                    outgoing_level.response_message = "Valid average level data found";
                    dbConnection.closeConnection();
                    return Ok(outgoing_level);
                }
                else
                {
                    dbConnection.closeConnection();
                    BooleanResponse response = new BooleanResponse();
                    response.response_valid = false;
                    response.response_message = level_id + " doesn't exist";
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

        [Route("achievements")]
        public IHttpActionResult PostAchievements([FromBody]User incoming_user)
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
                MySqlDataReader dbReader = dbConnection.getMySqlDataReader("SELECT achievement.achievement_id, achievement.achievement_title, achievement.achievement_description, achievement.achievement_total, achievement.achievement_target FROM progamer.achievement;");
                if (dbReader.HasRows)
                {
                    AchievementList outgoing_achievement_list = new AchievementList();
                    while (dbReader.Read())
                    {
                        Achievement outgoing_achievement = new Achievement();
                        outgoing_achievement.achievement_id = dbReader.GetInt32(0);
                        outgoing_achievement.achievement_title = dbReader.GetString(1);
                        outgoing_achievement.achievement_description = dbReader.GetString(2);
                        outgoing_achievement.achievement_total = dbReader.GetInt32(3);
                        outgoing_achievement.achievement_target = dbReader.GetString(4);
                        outgoing_achievement_list.achievement_list.Add(outgoing_achievement);
                    }
                    dbConnection.closeConnection();
                    outgoing_achievement_list.response_valid = true;
                    outgoing_achievement_list.response_message = "Valid achievements found";
                    return Ok(outgoing_achievement_list);
                }
                else
                {
                    dbConnection.closeConnection();
                    BooleanResponse response = new BooleanResponse();
                    response.response_valid = false;
                    response.response_message = "Achievements don't exist";
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

        [Route("userachievements")]
        public IHttpActionResult PostUserAchievements([FromBody]User incoming_user)
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
                MySQLConnection dbConnection0 = new MySQLConnection();
                MySqlDataReader dbReader0 = dbConnection0.getMySqlDataReader("INSERT INTO progamer.userachievement (userachievement.user_student_number, userachievement.achievement_id) SELECT * FROM (SELECT '" + user_student_number + "', achievement.achievement_id FROM progamer.achievement) as tmp WHERE NOT EXISTS (SELECT * FROM progamer.userachievement WHERE userachievement.user_student_number='" + user_student_number + "' AND tmp.achievement_id=userachievement.achievement_id);");
                dbConnection0.closeConnection();
                MySQLConnection dbConnection1 = new MySQLConnection();
                MySqlDataReader dbReader1 = dbConnection1.getMySqlDataReader("SELECT userachievement.userachievement_id, userachievement.user_student_number, userachievement.achievement_id, userachievement.userachievement_notified, achievement.achievement_title, achievement.achievement_description, achievement.achievement_total, userachievement.userachievement_progress, userachievement.userachievement_completed, IFNULL(userachievement.userachievement_date_completed, '') AS userachievement_date_completed FROM progamer.userachievement INNER JOIN progamer.achievement ON userachievement.achievement_id=achievement.achievement_id AND userachievement.user_student_number='" + user_student_number + "';");
                if (dbReader1.HasRows)
                {
                    UserAchievementList outgoing_userachievement_list = new UserAchievementList();
                    while (dbReader1.Read())
                    {
                        UserAchievement outgoing_userachievement = new UserAchievement();
                        outgoing_userachievement.userachievement_id = dbReader1.GetInt32(0);
                        outgoing_userachievement.user_student_number = dbReader1.GetString(1);
                        outgoing_userachievement.achievement_id = dbReader1.GetInt32(2);
                        outgoing_userachievement.userachievement_notified = dbReader1.GetInt32(3);
                        outgoing_userachievement.achievement_title = dbReader1.GetString(4);
                        outgoing_userachievement.achievement_description = dbReader1.GetString(5);
                        outgoing_userachievement.achievement_total = dbReader1.GetInt32(6);
                        outgoing_userachievement.userachievement_progress = dbReader1.GetInt32(7);
                        outgoing_userachievement.userachievement_completed = dbReader1.GetInt32(8);
                        outgoing_userachievement.userachievement_date_completed = dbReader1.GetString(9);

                        outgoing_userachievement_list.userachievement_list.Add(outgoing_userachievement);
                    }
                    dbConnection1.closeConnection();
                    outgoing_userachievement_list.response_valid = true;
                    outgoing_userachievement_list.response_message = "Valid userachievements found";
                    return Ok(outgoing_userachievement_list);
                }
                else
                {
                    dbConnection1.closeConnection();
                    BooleanResponse response = new BooleanResponse();
                    response.response_valid = false;
                    response.response_message = "Userachievements don't exist";
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

        [Route("updateuserachievements")]
        public IHttpActionResult PostUpdateUserLevel([FromBody]UserAchievementList incoming_userachievement_list)
        {
            try
            {
                BooleanResponse response = new BooleanResponse();
                if (incoming_userachievement_list.Equals(null))
                {
                    response.response_valid = false;
                    response.response_message = "Invalid input";
                    return Ok(response);
                }
                List<UserAchievement> userachievement_list = incoming_userachievement_list.userachievement_list;
                UserAchievementList outgoing_userachievement_list = new UserAchievementList();
                foreach (UserAchievement incoming_userachievement in userachievement_list)
                {
                    int userachievement_id = incoming_userachievement.userachievement_id;
                    int userachievement_progress = incoming_userachievement.userachievement_progress;
                    int userachievement_completed = incoming_userachievement.userachievement_completed;
                    int userachievement_notified = incoming_userachievement.userachievement_notified;
                    string userachievement_date_completed = "";
                    if (userachievement_completed == 1)
                        userachievement_date_completed = ", userachievement.userachievement_date_completed=NOW()";
                    MySQLConnection dbConnection = new MySQLConnection();
                    MySqlDataReader dbReader = dbConnection.getMySqlDataReader("UPDATE progamer.userachievement SET userachievement.userachievement_progress=" + userachievement_progress + ", userachievement.userachievement_completed=" + userachievement_completed + ", userachievement.userachievement_notified=" + userachievement_notified + userachievement_date_completed + " WHERE userachievement.userachievement_id=" + userachievement_id + ";");
                    dbConnection.closeConnection();

                    MySQLConnection dbConnection2 = new MySQLConnection();
                    MySqlDataReader dbReader2 = dbConnection2.getMySqlDataReader("SELECT userachievement_id, userachievement_date_completed FROM progamer.userachievement WHERE userachievement_id=" + userachievement_id + " LIMIT 1;");
                    dbConnection2.closeConnection();
                    UserAchievement outgoing_user_achievement = new UserAchievement();
                    outgoing_user_achievement.userachievement_id = dbReader2.GetInt32(0);
                    outgoing_user_achievement.userachievement_date_completed = dbReader2.GetString(1);
                    outgoing_userachievement_list.userachievement_list.Add(outgoing_user_achievement);
                }
                outgoing_userachievement_list.response_valid = true;
                outgoing_userachievement_list.response_message = "UserAchievements updated";
                return Ok(outgoing_userachievement_list);
            }
            catch (MySqlException ex)
            {
                BooleanResponse response = new BooleanResponse();
                response.response_valid = true;
                response.response_message = ex.Message;
                return Ok(response);
            }
        }

        [Route("user")]
        public IHttpActionResult PostUser([FromBody]User incoming_user)
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
                MySqlDataReader dbReader = dbConnection.getMySqlDataReader("SELECT user.user_student_number, user.user_nickname, user.user_avatar, user.user_is_private, IFNULL(totals.user_total_score, 0) AS user_total_score, IFNULL(totals.user_total_attempts, 0) AS user_total_attempts, IFNULL(totals.user_total_time, 0) AS user_total_time, IFNULL(averages.user_average_score, 0) AS user_average_score, IFNULL(averages.user_average_attempts, 0) AS user_average_attempts, IFNULL(averages.user_average_time, 0) AS user_average_time FROM (SELECT SUM(userlevel.userlevel_score) AS user_total_score, SUM(userlevel.userlevel_attempts) AS user_total_attempts, SUM(userlevel.userlevel_time) AS user_total_time FROM progamer.userlevel WHERE userlevel.user_student_number='" + user_student_number + "' AND userlevel.userlevel_completed='1') AS totals, (SELECT AVG(userlevel.userlevel_score) AS user_average_score, AVG(userlevel.userlevel_attempts) AS user_average_attempts, AVG(userlevel.userlevel_time) AS user_average_time FROM progamer.userlevel WHERE userlevel.user_student_number='" + user_student_number + "' AND userlevel.userlevel_completed='1') AS averages, progamer.user WHERE user.user_student_number='" + user_student_number + "' LIMIT 1;");
                dbReader.Read();
                if (dbReader.HasRows)
                {
                    User outgoing_user = new User();
                    outgoing_user.user_student_number = dbReader.GetString(0);
                    outgoing_user.user_nickname = dbReader.GetString(1);
                    outgoing_user.user_avatar = dbReader.GetInt32(2);
                    outgoing_user.user_is_private = dbReader.GetInt32(3);
                    outgoing_user.user_total_score = dbReader.GetInt32(4);
                    outgoing_user.user_total_attempts = dbReader.GetInt32(5);
                    outgoing_user.user_total_time = dbReader.GetInt32(6);
                    outgoing_user.user_average_score = dbReader.GetInt32(7);
                    outgoing_user.user_average_attempts = dbReader.GetInt32(8);
                    outgoing_user.user_average_time = dbReader.GetInt32(9);
                    outgoing_user.response_valid = true;
                    outgoing_user.response_message = "Valid user found";
                    dbConnection.closeConnection();
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

        [Route("average")]
        public IHttpActionResult PostAverage([FromBody]User incoming_user)
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
                MySqlDataReader dbReader = dbConnection.getMySqlDataReader("SELECT totals.user_total_score, totals.user_total_attempts, totals.user_total_time, averages.user_average_score, averages.user_average_attempts, averages.user_average_time FROM (SELECT SUM(userlevel.userlevel_score) AS user_total_score, SUM(userlevel.userlevel_attempts) AS user_total_attempts, SUM(userlevel.userlevel_time) AS user_total_time FROM progamer.userlevel INNER JOIN progamer.user ON userlevel.user_student_number=user.user_student_number WHERE userlevel.userlevel_completed='1' AND user.user_type='student') AS totals, (SELECT AVG(userlevel.userlevel_score) AS user_average_score, AVG(userlevel.userlevel_attempts) AS user_average_attempts, AVG(userlevel.userlevel_time) AS user_average_time FROM progamer.userlevel INNER JOIN progamer.user ON userlevel.user_student_number=user.user_student_number WHERE userlevel.userlevel_completed='1' AND user.user_type='student') AS averages LIMIT 1;");
                dbReader.Read();
                if (dbReader.HasRows)
                {
                    User outgoing_user = new User();
                    outgoing_user.user_total_score = dbReader.GetInt32(0);
                    outgoing_user.user_total_attempts = dbReader.GetInt32(1);
                    outgoing_user.user_total_time = dbReader.GetInt32(2);
                    outgoing_user.user_average_score = dbReader.GetInt32(3);
                    outgoing_user.user_average_attempts = dbReader.GetInt32(4);
                    outgoing_user.user_average_time = dbReader.GetInt32(5);
                    outgoing_user.response_valid = true;
                    outgoing_user.response_message = "Valid user found";
                    dbConnection.closeConnection();
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

        [Route("puzzles")]
        public IHttpActionResult PostPuzzles([FromBody]Level incoming_level)
        {
            try
            {
                if (incoming_level.Equals(null))
                {
                    BooleanResponse response = new BooleanResponse();
                    response.response_valid = false;
                    response.response_message = "Invalid input";
                    return Ok(response);
                }
                int level_id = incoming_level.level_id;
                MySQLConnection dbConnection = new MySQLConnection();
                MySqlDataReader dbReader = dbConnection.getMySqlDataReader("SELECT puzzle.puzzle_id, puzzle.puzzle_level_id, puzzle.puzzle_instructions, puzzle.puzzle_data FROM progamer.puzzle WHERE puzzle.puzzle_level_id=" + level_id + " ORDER BY RAND() LIMIT 5;");
                if (dbReader.HasRows)
                {
                    Level puzzle_list = new Level();
                    while (dbReader.Read())
                    {
                        Puzzle outgoing_puzzle = new Puzzle();
                        outgoing_puzzle.puzzle_id = dbReader.GetInt32(0);
                        outgoing_puzzle.puzzle_level_id = dbReader.GetInt32(1);
                        outgoing_puzzle.puzzle_instructions = dbReader.GetString(2);
                        outgoing_puzzle.puzzle_data = dbReader.GetString(3);
                        puzzle_list.puzzle_list.Add(outgoing_puzzle);
                    }
                    dbConnection.closeConnection();
                    puzzle_list.response_valid = true;
                    puzzle_list.response_message = "Valid puzzles found";
                    return Ok(puzzle_list);
                }
                else
                {
                    BooleanResponse response = new BooleanResponse();
                    response.response_valid = false;
                    response.response_message = "Puzzles don't exist";
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

        [Route("updateuser")]
        public IHttpActionResult PostUpdateUser([FromBody]User incoming_user)
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
                int user_avatar = incoming_user.user_avatar;
                int user_is_private = incoming_user.user_is_private;
                MySQLConnection dbConnection = new MySQLConnection();
                MySqlDataReader dbReader = dbConnection.getMySqlDataReader("UPDATE progamer.user SET user.user_nickname='" + user_nickname + "', user.user_avatar=" + user_avatar + ", user.user_is_private=" + user_is_private + " WHERE user_student_number='" + user_student_number + "';");
                dbConnection.closeConnection();
                if (dbReader.RecordsAffected == 1)
                {
                    BooleanResponse response = new BooleanResponse();
                    response.response_valid = true;
                    response.response_message = user_student_number + " updated";
                    return Ok(response);

                }
                else
                {
                    BooleanResponse response = new BooleanResponse();
                    response.response_valid = false;
                    response.response_message = user_student_number + " not found";
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

        [Route("updateuserlevel")]
        public IHttpActionResult PostUpdateUserLevel([FromBody]LevelList incoming_levels)
        {
            try
            {
                BooleanResponse response = new BooleanResponse();
                if (incoming_levels.Equals(null))
                {
                    response.response_valid = false;
                    response.response_message = "Invalid input";
                    return Ok(response);
                }
                List<Level> level_list = incoming_levels.level_list;
                foreach (Level incoming_level in level_list)
                {
                    string user_student_number = incoming_level.level_user_student_number_id;
                    int level_id = incoming_level.level_id;
                    int level_completed = incoming_level.level_completed;
                    int level_score = incoming_level.level_score;
                    int level_attempts = incoming_level.level_attempts;
                    int level_time = incoming_level.level_time;
                    MySQLConnection dbConnection = new MySQLConnection();
                    MySqlDataReader dbReader = dbConnection.getMySqlDataReader("UPDATE progamer.userlevel SET userlevel.userlevel_completed=" + level_completed + ", userlevel.userlevel_score=" + level_score + ", userlevel.userlevel_attempts=" + level_attempts + ", userlevel.userlevel_time=" + level_time + " WHERE userlevel.user_student_number='" + user_student_number + "' AND userlevel.level_id='" + level_id + "';");
                    dbConnection.closeConnection();
                }
                response.response_valid = true;
                response.response_message = "Userlevel updated";
                return Ok(response);
            }
            catch (MySqlException ex)
            {
                BooleanResponse response = new BooleanResponse();
                response.response_valid = true;
                response.response_message = ex.Message;
                return Ok(response);
            }
        }
    }
}
