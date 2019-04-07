using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseballStats.Common
{
    public class Constants
    {
        public static string ApiEndpointUrl
        {
            get
            {
                return @"http://lookup-service-prod.mlb.com/{0}";
            }
        }

        #region Player Search
        public static string ApiPlayerSearchUrl
        {
            get
            {
                //0 is 'Y' or 'N' for active or not. || 1 is name part, either whole or part with '%25' appended
                //If you’re using a single search term 
                //i.e. using ‘cespedes’ instead of ‘yoenis cespedes’, you’ll need to append a ‘%25’ character to your search term. 
                //Without it, the request will return 500. See example.
                //https://appac.github.io/mlb-data-api-docs/#player-data-player-search-get
                return @"/json/named.search_player_all.bam?sport_code='mlb'&active_sw='{0}'&name_part='{1}'";
            }
        }

        public static string funcReplacePlayerSearchValues(string _searchStr, string _active, string _namePartStr)
        {
            string updatedStr = string.Format(_searchStr, _active, _namePartStr);
            return updatedStr;
        }
        #endregion

        #region Player Info
        public static string ApiPlayerInfoUrl
        {
            get
            {
                //0 is player id.
                //https://appac.github.io/mlb-data-api-docs/#player-data-player-info-get
                return @"json/named.player_info.bam?sport_code='mlb'&player_id='{0}'";
            }
        }

        public static string funcReplacePlayerInfoValues(string _infoStr, string _playerId)
        {
            string updatedStr = string.Format(_infoStr, _playerId);
            return updatedStr;
        }
        #endregion

        #region Season Hitting Stats
        public static string ApiSeasonHittingStatsUrl
        {
            get
            {
                //0 is game type:
                //The type of games you want career stats for.
                //'R' - Regular Season
                //'S' - Spring Training
                //'E' - Exhibition
                //'A' - All Star Game
                //'D' - Division Series
                //'F' - First Round(Wild Card)
                //'L' - League Championship
                //'W' - World Series

                //1 is season year 2019 etc
                //2 is player id.
                //https://appac.github.io/mlb-data-api-docs/#stats-data-season-hitting-stats-get
                    return @"json/named.sport_hitting_tm.bam?league_list_id='mlb'&game_type='{0}'&season='{1}'&player_id='{2}'";
            }
        }

        public static string funcReplaceSeasonHittingStatsValues(string _seasonHittingStatsStr, string _gameType, string _seasonYear, string _playerId)
        {
            string updatedStr = string.Format(_seasonHittingStatsStr,  _gameType,  _seasonYear, _playerId);
            return updatedStr;
        }
        #endregion

    }
}