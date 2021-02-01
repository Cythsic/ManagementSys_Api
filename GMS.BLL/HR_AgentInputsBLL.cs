using GMS.Model;
using System;
using System.Collections.Generic;

namespace GMS.BLL
{
    public class HR_AgentInputsBLL
    {
        public bool ChangeStatus(List<AgentInputs> modelList, object pms)
        {
            try
            {
                foreach (AgentInputs model in modelList)
                {
                    ChangeStatus("cardId= '" + model.CardID + "'", pms);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private bool ChangeStatus(string where, object pms)
        {
            string sql = "UPDATE AgentInputs SET " + pms + " =1 ";
            if (string.IsNullOrWhiteSpace(where))
            {
                return false;
            }

            sql += " where " + where;

            return DapperHelper.Excute(sql, pms) > 0;
        }


    }
}
