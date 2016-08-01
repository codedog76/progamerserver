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
                MySqlDataReader dbReader = dbConnection.getMySqlDataReader("SELECT achievement.achievement_id, achievement.achievement_title, achievement.achievement_description, achievement.achievement_total FROM progamer.achievement;");
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
                MySqlDataReader dbReader1 = dbConnection1.getMySqlDataReader("SELECT userachievement.userachievement_id, userachievement.user_student_number, userachievement.achievement_id, userachievement.userachievement_progress FROM progamer.userachievement WHERE userachievement.user_student_number='" + user_student_number+"';");
                if (dbReader1.HasRows)
                {
                    UserAchievementList outgoing_userachievement_list = new UserAchievementList();
                    while (dbReader1.Read())
                    {
                        UserAchievement outgoing_userachievement = new UserAchievement();
                        outgoing_userachievement.userachievement_id = dbReader1.GetInt32(0);
                        outgoing_userachievement.user_student_number = dbReader1.GetString(1);
                        outgoing_userachievement.achievement_id = dbReader1.GetInt32(2);
                        outgoing_userachievement.userachievement_progress = dbReader1.GetInt32(3);
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

        [Route("updateuserachievement")]
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
                foreach (UserAchievement incoming_userachievement in userachievement_list)
                {
                    int userachievement_id = incoming_userachievement.userachievement_id;
                    string user_student_number = incoming_userachievement.user_student_number;
                    int achievement_id = incoming_userachievement.achievement_id;
                    int userachievement_progress = incoming_userachievement.userachievement_progress;
                    MySQLConnection dbConnection = new MySQLConnection();
                    MySqlDataReader dbReader = dbConnection.getMySqlDataReader("UPDATE progamer.userachievement SET userachievement.userachievement_progress=" + userachievement_progress + " WHERE userachievement_progress.user_student_number='" + user_student_number + "' AND userachievement_progress.achievement_id='" + achievement_id + "';");
                    dbConnection.closeConnection();
                }
                response.response_valid = true;
                response.response_message = "UserAchievement updated";
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
                MySqlDataReader dbReader = dbConnection.getMySqlDataReader("SELECT user.user_student_number, user.user_nickname, user.user_avatar, user.user_is_private FROM progamer.user WHERE user.user_student_number='" + user_student_number + "' LIMIT 1;");
                dbReader.Read();
                if (dbReader.HasRows)
                {
                    User outgoing_user = new User();
                    outgoing_user.user_student_number = dbReader.GetString(0);
                    outgoing_user.user_nickname = dbReader.GetString(1);
                    outgoing_user.user_avatar = dbReader.GetInt32(2);
                    outgoing_user.user_is_private = dbReader.GetInt32(3);
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
